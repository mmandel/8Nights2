//
//  This game object will move with the torch carrier's head
//
//  Expected to have a rigid body on the same game object, so it can be used to detect when the players head enters a trigger
//  Also doing path following logic here.
//

using UnityEngine;
using System.Collections;
using System;

public class Nights2TorchPlayer : MonoBehaviour 
{
    [Tooltip("When the torch carrier starts following the path, this is size of the 'deadzone' they have initially")]
    public float InitialPathDeadzone = .5f;
    [Tooltip("This is how close the player needs to stay to the path to be considered following it")]
    public float NearPathThresh = .25f;
    [Tooltip("How wide is the portal?  This defines where the player needs to be to pass through it")]
    public float PortalWidth = 1.0f;
    public float PassPortalMax = 1.0f;
    [Tooltip("How many seconds does the player need to be off the path before we fail them?")]
    public float OutsidePathFailureSecs = .5f;

    [Space(10)]

    [Tooltip("An object to enable/disable and position as torch carrier successfully navigates the path")]
    public GameObject PortalObj = null;
    [Tooltip("The distance thresh from portal location where we should start showing it")]
    public float ShowPortalDistThresh = .05f;
    public Vector3 PortalDestOffset = Vector3.zero;

    [Space(10)]

    [Tooltip("Set this flag true to draw some debug stuff for the path the torch carrier is walking down")]
    public bool DebugPathFollowing = true;
    public GameObject DebugSnapToPath = null;

    public event PortalChangedHandler OnPortalStateChanged;
    public class PortalChangedEventArgs : EventArgs
    {
        public PortalChangedEventArgs(PortalState oldState, PortalState newState) { OldState = oldState; NewState = newState; }
        public PortalState OldState;
        public PortalState NewState;
    }
    public delegate void PortalChangedHandler(object sender, PortalChangedEventArgs e);

    private bool _wasDebugPathOn = false;
    private bool _deadzoneActive = false;
    private Vector3 _deadzoneCenter = Vector3.zero;
    private float _outsidePathStartTime = -1.0f;
    private Nights2Portal _portalFX = null;
    private bool _portalShowing = false;
    private bool _waitTillBehindPortal = false; 

    private PortalState _curPortalState = PortalState.NoProgress;
    public enum PortalState
    {
        NoProgress,
        ShowingEntrancePortal,
        ThroughEntrancePortal,
        ShowingExitPortal,
        ThroughExitPortal
    }

    public static Nights2TorchPlayer Instance { get; private set; }

    public PortalState GetPortalState() { return _curPortalState; }

    public void CheatPortalState(PortalState s) { SetPortalState(s); }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (PortalObj != null)
            _portalFX = PortalObj.GetComponent<Nights2Portal>();

        //subscribe to state changed events
        if (Nights2Mgr.Instance != null)
            Nights2Mgr.Instance.OnStateChanged += OnNights2StateChanged;
    }

    void OnDestroy()
    {
        //unsubscribe
        if (Nights2Mgr.Instance != null)
            Nights2Mgr.Instance.OnStateChanged -= OnNights2StateChanged;
    }

    void TeleportToWorld(GameObject world)
    {
        Debug.Assert((world != null) && (Nights2Mgr.Instance.VRRig.transform != null));

        //just parent the VR rig to the new world, that's about it
        Transform vrTrans = Nights2Mgr.Instance.VRRig.transform;

        vrTrans.parent = world.transform;
        vrTrans.localPosition = Vector3.zero;
        vrTrans.localRotation = Quaternion.identity;
    }

    void OnNights2StateChanged(object sender, Nights2Mgr.StateChangedEventArgs e)
    {
        //OK, starting to follow a path
        if (e.NewState == Nights2Mgr.Nights2State.SeekingBeacon)
        {
            //record their position to allow for a small "dead zone" before they are required to stay super close
            _deadzoneActive = true;
            _deadzoneCenter = GetPlayerPosOnGround();

            //reset portal state
            SetPortalState(PortalState.NoProgress);

            ShowPortal(false);
        }
        else
        {
            SetPortalState(PortalState.NoProgress);
            TeleportToWorld(Nights2Mgr.Instance.RoomWorld);
        }
    }

    //get the player's current position projected onto the ground
    Vector3 GetPlayerPosOnGround()
    {
        Vector3 r = this.transform.position;
        r.y = 0.0f;
        return r;
    }

    bool IsFollowingPath()
    {
        if (Nights2Mgr.Instance == null)
            return false;

        return (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.SeekingBeacon) ||
               (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.NearBeacon);
    }

    void SetPortalState(PortalState s)
    {
        if (s != _curPortalState)
        {
            PortalState prevState = _curPortalState;

            _curPortalState = s;

            if (s == PortalState.NoProgress)
            {
                ShowPortal(false); //just in case
                TeleportToWorld(Nights2Mgr.Instance.RoomWorld);
            }
            else if (s == PortalState.ShowingEntrancePortal)
            {
                _waitTillBehindPortal = false;
                Nights2Path curPath = Nights2Mgr.Instance.CurrentTorchPath();
                Debug.Assert(curPath != null);

                Vector3 portalPos = curPath.GetPortalPos(Nights2Path.PortalType.EntrancePortal);
                Vector3 portalDir = curPath.GetPortalDir(Nights2Path.PortalType.EntrancePortal);

                //put dest trans in the alt world, so camera is positioned correctly on portal
                Transform destTrans = (_portalFX != null) ? _portalFX.PortalDestTrans : null;
                if (destTrans != null)
                {
                    destTrans.position = PortalDestOffset + portalPos + Nights2Mgr.Instance.RoomWorld.transform.position + (Nights2Mgr.Instance.AltWorld.transform.position - Nights2Mgr.Instance.RoomWorld.transform.position);
                    destTrans.rotation = Quaternion.LookRotation(portalDir);
                }

                PositionPortal(portalPos);
                AlignPortal(portalDir);
                
                ShowPortal(true);
            }
            else if (s == PortalState.ShowingExitPortal)
            {
                _waitTillBehindPortal = false;
                Nights2Path curPath = Nights2Mgr.Instance.CurrentTorchPath();
                Debug.Assert(curPath != null);

                Vector3 portalPos = curPath.GetPortalPos(Nights2Path.PortalType.ExitPortal);
                Vector3 portalDir = curPath.GetPortalDir(Nights2Path.PortalType.ExitPortal);

                //put dest trans in the alt world, so camera is positioned correctly on portal
                Transform destTrans = (_portalFX != null) ? _portalFX.PortalDestTrans : null;
                if (destTrans != null)
                {
                    destTrans.position = PortalDestOffset + portalPos + (Nights2Mgr.Instance.RoomWorld.transform.position - Nights2Mgr.Instance.AltWorld.transform.position);
                    destTrans.rotation = Quaternion.LookRotation(portalDir);
                }

                PositionPortal(portalPos);
                AlignPortal(portalDir);

                ShowPortal(true);
            }
            else if ((s == PortalState.ThroughEntrancePortal) || (s == PortalState.ThroughExitPortal))
            {
                ShowPortal(false, true);

                TeleportToWorld((s == PortalState.ThroughEntrancePortal) ? Nights2Mgr.Instance.AltWorld : Nights2Mgr.Instance.RoomWorld);
            }

            if (OnPortalStateChanged != null)
                OnPortalStateChanged(this, new PortalChangedEventArgs(prevState, _curPortalState));
        }
    }

    public bool IsPortalShowing()
    {
        return _portalShowing;
    }

    void ShowPortal(bool b, bool wasActivated = false)
    {
        if (b == _portalShowing)
            return;

        _portalShowing = b;

        if (PortalObj == null)
            return;

        if (_portalFX != null)
        {
            if (b)
                _portalFX.TriggerShowPortal();
            else
            {
                if (wasActivated)
                    _portalFX.TriggerActivatedPortal();
                else
                    _portalFX.TriggerCancelPortal();
            }
        }
        else
            PortalObj.SetActive(b);
    }

    void PositionPortal(Vector3 pt)
    {
        if (PortalObj == null)
            return;

        PortalObj.transform.position = pt;
    }

    void AlignPortal(Vector3 alignDir)
    {
        if (PortalObj == null)
            return;

        PortalObj.transform.rotation = Quaternion.LookRotation(alignDir);
    }

    bool isThroughPortal(float distToPortal, float distToPath)
    {
        if (distToPortal < 0.0f)//negative dist means we're past it
        {
            if ((distToPath <= .5f * PortalWidth) && !_waitTillBehindPortal && (distToPortal >= -PassPortalMax))
            {
                return true;
            }
            else //ooo, went wide, dont go through, wait till behind
                _waitTillBehindPortal = true;
        }
        else if (distToPortal > .02f) //reset when we're behind the portal again
            _waitTillBehindPortal = false;

        return false;
    }


	void Update () 
    {
        //keep it attached to the torch carrier's head
        transform.parent = Nights2CamMgr.Instance.GetHeadTrans();
        transform.localPosition = Vector3.zero;

        if (IsFollowingPath())
        {
            Vector3 curPlayerPos = GetPlayerPosOnGround();
            Nights2Path curPath = Nights2Mgr.Instance.CurrentTorchPath();

            //see if we're outside initial deadzone now
            if (_deadzoneActive)
            {
                if ((curPlayerPos - _deadzoneCenter).magnitude >= InitialPathDeadzone)
                {
                    _deadzoneActive = false;
                }
            }

            int curPathSegment = -1;
            Vector3 closestPtOnPath = Vector3.zero;
            if (curPath != null)
                closestPtOnPath = curPath.ClosestPointOnPath(curPlayerPos, out curPathSegment);

            //make sure player stays on the path
            closestPtOnPath.y = 0.0f; //project to ground, just in case
            float distToPath = (closestPtOnPath - curPlayerPos).magnitude;
            if (!_deadzoneActive && (curPath != null) && !curPath.IsEditting())
            {
                //are we too far off the path?
                if (distToPath > NearPathThresh)
                {
                    //reset timer if portal is open, dont care in that case
                    if ((_curPortalState == PortalState.ShowingEntrancePortal) || (_curPortalState == PortalState.ShowingExitPortal))
                        _outsidePathStartTime = Time.time;

                    if (_outsidePathStartTime < 0.0f) //just fell off the path this frame, start timer
                    {
                        _outsidePathStartTime = Time.time;
                    }
                    else
                    {
                        //if we're off the path long enough, we fail the player
                        float timeOffPath = (Time.time - _outsidePathStartTime);
                        if (timeOffPath >= OutsidePathFailureSecs)
                        {
                            _outsidePathStartTime = -1.0f;
                            Nights2Mgr.Instance.SetState(Nights2Mgr.Nights2State.FlameExtinguished);
                        }
                    }
                }
            }

            //update portal processing
            float distToEntrance = float.MaxValue;
            float distToExit = float.MaxValue;
            switch (_curPortalState)
            {
                case PortalState.NoProgress:

                    //OK, see when we're close to the entrance portal and show it
                    distToEntrance = curPath.DistToPortal(Nights2Path.PortalType.EntrancePortal, GetPlayerPosOnGround());
                    //Debug.Log("Dist to entrance (not showing): " + distToEntrance);
                    if ((distToEntrance >= 0.0f) && (distToEntrance <= ShowPortalDistThresh))
                    {
                        Debug.Log("   SHOWING ENTRANCE PORTAL!");
                        SetPortalState(PortalState.ShowingEntrancePortal);
                    }
                    break;

                case PortalState.ShowingEntrancePortal:

                    //detect when player walks through entrance portal                   
                    distToEntrance = curPath.DistToPortal(Nights2Path.PortalType.EntrancePortal, GetPlayerPosOnGround());
                    //Debug.Log("Dist to entrance (showing): " + distToEntrance);
                    if (isThroughPortal(distToEntrance, distToPath))
                    {
                        Debug.Log("   THROUGH ENTRANCE PORTAL!");
                        SetPortalState(PortalState.ThroughEntrancePortal);
                    }
                    else if(distToEntrance > .02f) //reset when we're behind the portal again
                        _waitTillBehindPortal = false;
 
                    break;

                case PortalState.ThroughEntrancePortal:

                    //OK, see when we're close to the exit portal and show it                    
                    distToExit = curPath.DistToPortal(Nights2Path.PortalType.ExitPortal, GetPlayerPosOnGround());
                    //Debug.Log("Dist to exit (not showing): " + distToExit);
                    if ((distToExit >= 0.0f) && (distToExit <= ShowPortalDistThresh))
                    {
                        Debug.Log("   SHOWING ENTRANCE PORTAL!");
                        SetPortalState(PortalState.ShowingExitPortal);
                    }
                    break;

                case PortalState.ShowingExitPortal:

                    //detect when player walks through exit portal
                    distToExit = curPath.DistToPortal(Nights2Path.PortalType.ExitPortal, GetPlayerPosOnGround());
                    //Debug.Log("Dist to exit (showing): " + distToExit);
                    if (isThroughPortal(distToExit, distToPath))
                    {
                        Debug.Log("   THROUGH EXIT PORTAL!");
                        SetPortalState(PortalState.ThroughExitPortal);
                    }
                    break;

                default: break;
            }
        }
        else //not followinbg path
        {
            ShowPortal(false);
        }

        //visual debugging stuff
        if (DebugPathFollowing && IsFollowingPath())
        {
            Nights2Path p = Nights2Mgr.Instance.CurrentTorchPath();
            if (p != null)
            {
                p.ShowPreview(true);
                if (DebugSnapToPath != null)
                {
                    DebugSnapToPath.SetActive(true);
                    int curPathSegment = -1;
                    Vector3 closestPt = p.ClosestPointOnPath(transform.position, out curPathSegment);
                    DebugSnapToPath.transform.position = closestPt;
                }
            }
        }
        else
        {
            //turn off preview when debugging is turned off
            Nights2Path p = Nights2Mgr.Instance.CurrentTorchPath();
            if(_wasDebugPathOn && (p != null))
                p.ShowPreview(false);

            if (DebugSnapToPath != null)
                DebugSnapToPath.SetActive(false);
        }

        _wasDebugPathOn = DebugPathFollowing;
	}

    /*void OnDrawGizmos()
    {
        if (DebugPathFollowing && IsFollowingPath())
        {
            Nights2Path curPath = Nights2Mgr.Instance.CurrentTorchPath();
            if (curPath != null)
            {
                Vector3 closestPt = curPath.ClosestPointOnPath(transform.position);
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(closestPt, .25f);
            }
        }
    }*/
}
