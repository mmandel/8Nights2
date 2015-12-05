﻿//
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
    [Tooltip("Treasure prefab to spawn when in alt world")]
    public GameObject TreasurePrefab;
    [Tooltip("How close does lantern need to be to treasure spot to reveal it")]
    public float TreasureRevealDist = .5f;
    public FMOD_StudioEventEmitter TreasureRevealSound;

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
    private Nights2Portal _portalFX = null;
    private bool _portalShowing = false;
    private bool _waitTillBehindPortal = false;
    private Nights2Treasure _treasure;  //spawned from time to time

    private PortalState _curPortalState = PortalState.NoProgress;
    public enum PortalState
    {
        NoProgress,
        ShowingEntrancePortal,
        ThroughEntrancePortal,
        ShowingExitPortal,
        ThroughExitPortal
    }

    private TreasureState _curTreasureState = TreasureState.NoProgress;
    public enum TreasureState
    {
        NoProgress,
        WaitingForTreasureReveal,
        TreasureReveal,
        TreasureWaitForCollection,
        TreasureCompleted
    }

    public static Nights2TorchPlayer Instance { get; private set; }

    public PortalState GetPortalState() { return _curPortalState; }
    public TreasureState GetTreasureState() { return _curTreasureState; }

    public void CheatPortalState(PortalState s) { SetPortalState(s); }
    public void CheateTreasureState(TreasureState s) 
    {
       if (s == TreasureState.TreasureWaitForCollection)
          _treasure.ForceOpen();
       else if (s == TreasureState.TreasureCompleted)
          _treasure.ForceCollect();
        SetTreasureState(s); 
    }

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

    void TeleportToWorld(GameObject world, Nights2Mgr.WorldID worldID)
    {
        Debug.Assert((world != null) && (Nights2Mgr.Instance.VRRig.transform != null));

        //just parent the VR rig to the new world, that's about it
        Transform vrTrans = Nights2Mgr.Instance.VRRig.transform;

        vrTrans.parent = world.transform;
        vrTrans.localPosition = Vector3.zero;
        vrTrans.localRotation = Quaternion.identity;

        Nights2Mgr.Instance.NotifyInWorld(worldID);
    }

    void OnNights2StateChanged(object sender, Nights2Mgr.StateChangedEventArgs e)
    {
        //OK, starting to follow a path
        if (e.NewState == Nights2Mgr.Nights2State.SeekingBeacon)
        {
            //reset portal state
            SetPortalState(PortalState.NoProgress);
            //reset treasure state too
            SetTreasureState(TreasureState.NoProgress);

            ShowPortal(false);
        }
        else
        {
            if (e.NewState != Nights2Mgr.Nights2State.NearBeacon) //we want portal state to still be "through exit portal"
                SetPortalState(PortalState.NoProgress);
            TeleportToWorld(Nights2Mgr.Instance.RoomWorld, Nights2Mgr.WorldID.RoomWorld);
        }
    }

    //get the player's current position projected onto the ground
    Vector3 GetPlayerPosOnGround()
    {
        Vector3 r = this.transform.position;
        r.y = 0.0f;
        return r;
    }

    Vector3 GetLanternPosOnGround()
    {
        Vector3 r = Nights2CamMgr.Instance.GetLanternParent().transform.position;
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

    Nights2Spot GetTreasureSpot()
    {
        Nights2Path curPath = Nights2Mgr.Instance.CurrentTorchPath();
        Debug.Assert(curPath != null);
        Nights2Spot treasureSpot = curPath.GetTreasureSpot();
        Debug.Assert(treasureSpot != null);
        return treasureSpot;
    }

    Vector3 GetTreasureSpotOnGround()
    {
        Nights2Spot spot = GetTreasureSpot();
        Vector3 pos = spot.GetPos();
        pos.y = 0.0f;
        return pos;
    }

    void SetTreasureState(TreasureState s)
    {
        if (s != _curTreasureState)
        {
            TreasureState prevState = _curTreasureState;

            _curTreasureState = s;

            Nights2Spot treasureSpot = GetTreasureSpot();

            if (_curTreasureState == TreasureState.TreasureReveal)
            {
               //ping from treasure spot when its revealed
               if (Nights2SpotMgr.Instance != null)
                  Nights2SpotMgr.Instance.TriggerSpotFX(Nights2SpotMgr.LightAction.Ping, 1.5f, treasureSpot, .5f);

               if (TreasureRevealSound != null)
                  TreasureRevealSound.Play();
            }

            if (_curTreasureState == TreasureState.TreasureCompleted)
            {
               if (Nights2SpotMgr.Instance != null)
                  Nights2SpotMgr.Instance.TriggerSpotFX(Nights2SpotMgr.LightAction.TurnAllOn, 2.0f);
            }


            //light up spot
            if ((treasureSpot != null) &&
                (_curTreasureState != TreasureState.NoProgress) && (_curTreasureState != TreasureState.TreasureCompleted))
            {
                Nights2SpotMgr.Instance.MakeSpotActive(treasureSpot);
            }

            if (_curTreasureState == TreasureState.WaitingForTreasureReveal)
            {
                //TODO: light up spot!
            }
            else if (_curTreasureState == TreasureState.TreasureReveal)
            {
                //spawn treasure at spot
                Debug.Assert(TreasurePrefab != null);
                GameObject spawnedTreasure = Instantiate(TreasurePrefab) as GameObject;
                Debug.Assert(spawnedTreasure != null);
                _treasure = spawnedTreasure.GetComponent<Nights2Treasure>();
                Debug.Assert(_treasure != null);

                Vector3 treasurePos = GetTreasureSpotOnGround();
                _treasure.gameObject.transform.position = treasurePos;
                //face the torch carrier
                _treasure.gameObject.transform.LookAt(GetPlayerPosOnGround());
            }
            else if (_curTreasureState == TreasureState.TreasureCompleted)
            {
                _treasure = null; //don't track it anymore, it will delete itself
            }

        }
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
                TeleportToWorld(Nights2Mgr.Instance.RoomWorld, Nights2Mgr.WorldID.RoomWorld);
            }
            else if (s == PortalState.ShowingEntrancePortal)
            {
                _waitTillBehindPortal = true;
                Nights2Path curPath = Nights2Mgr.Instance.CurrentTorchPath();
                Debug.Assert(curPath != null);

                Vector3 portalPos = curPath.GetPortalPos(Nights2Path.PortalType.EntrancePortal);
                Vector3 portalDir = curPath.GetPortalDir(Nights2Path.PortalType.EntrancePortal);

                //ping from entrance portal spot when its revealed
                if (Nights2SpotMgr.Instance != null)
                   Nights2SpotMgr.Instance.TriggerSpotFX(Nights2SpotMgr.LightAction.Ping, 1.5f, curPath.GetPortalSpot(Nights2Path.PortalType.EntrancePortal), .5f);

                //put dest trans in the alt world, so camera is positioned correctly on portal
                Transform destTrans = (_portalFX != null) ? _portalFX.PortalDestTrans : null;
                if (destTrans != null)
                {
                    destTrans.position = PortalDestOffset + portalPos + Nights2Mgr.Instance.RoomWorld.transform.position + (Nights2Mgr.Instance.CurAltWorld().transform.position - Nights2Mgr.Instance.RoomWorld.transform.position);
                    destTrans.rotation = Quaternion.LookRotation(portalDir);
                }

                PositionPortal(portalPos);
                AlignPortal(portalDir);
                
                ShowPortal(true);
            }
            else if (s == PortalState.ShowingExitPortal)
            {
                _waitTillBehindPortal = true;
                Nights2Path curPath = Nights2Mgr.Instance.CurrentTorchPath();
                Debug.Assert(curPath != null);

                Vector3 portalPos = curPath.GetPortalPos(Nights2Path.PortalType.ExitPortal);
                Vector3 portalDir = curPath.GetPortalDir(Nights2Path.PortalType.ExitPortal);

                //ping from exit portal spot when its revealed
                if (Nights2SpotMgr.Instance != null)
                   Nights2SpotMgr.Instance.TriggerSpotFX(Nights2SpotMgr.LightAction.Ping, 1.5f, curPath.GetPortalSpot(Nights2Path.PortalType.ExitPortal), .5f);

                //put dest trans in the alt world, so camera is positioned correctly on portal
                Transform destTrans = (_portalFX != null) ? _portalFX.PortalDestTrans : null;
                if (destTrans != null)
                {
                    destTrans.position = PortalDestOffset + portalPos + (Nights2Mgr.Instance.RoomWorld.transform.position - Nights2Mgr.Instance.CurAltWorld().transform.position);
                    destTrans.rotation = Quaternion.LookRotation(portalDir);
                }

                PositionPortal(portalPos);
                AlignPortal(portalDir);

                ShowPortal(true);
            }
            else if ((s == PortalState.ThroughEntrancePortal) || (s == PortalState.ThroughExitPortal))
            {
                ShowPortal(false, true);

                if (s == PortalState.ThroughEntrancePortal)
                {
                    TeleportToWorld(Nights2Mgr.Instance.CurAltWorld(), Nights2Mgr.Instance.CurAltWorldID());
                }
                else
                {
                    TeleportToWorld( Nights2Mgr.Instance.RoomWorld, Nights2Mgr.WorldID.RoomWorld);
                }                
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
        else if ((distToPortal > .15f) && (distToPath <= .5f * PortalWidth))//reset when we're behind the portal again
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

            int curPathSegment = -1;
            Vector3 closestPtOnPath = Vector3.zero;
            if (curPath != null)
                closestPtOnPath = curPath.ClosestPointOnPath(curPlayerPos, out curPathSegment);

            //update portal processing
            float distToEntrance = float.MaxValue;
            float distToExit = float.MaxValue;
            float relativeDistForward = float.MaxValue;
            float relativeDistSide = float.MaxValue;            
            switch (_curPortalState)
            {
                case PortalState.NoProgress:

                    //light up location of entrance portal
                    Nights2Spot entranceSpot = curPath.GetPortalSpot(Nights2Path.PortalType.EntrancePortal);
                    if (entranceSpot != null)
                        Nights2SpotMgr.Instance.MakeSpotActive(entranceSpot);

                    //OK, see when we're close to the entrance portal and show it
                    distToEntrance = curPath.DistToPortal(Nights2Path.PortalType.EntrancePortal, GetLanternPosOnGround());
                    //Debug.Log("Dist to entrance (not showing): " + distToEntrance);
                    if ((distToEntrance >= 0.0f) && (distToEntrance <= ShowPortalDistThresh))
                    {
                        Debug.Log("   SHOWING ENTRANCE PORTAL!");
                        SetPortalState(PortalState.ShowingEntrancePortal);
                    }
                    break;

                case PortalState.ShowingEntrancePortal:

                    //detect when player walks through entrance portal                   
                    curPath.GetPortalRelativePos(Nights2Path.PortalType.EntrancePortal, GetPlayerPosOnGround(), out relativeDistForward, out relativeDistSide);
                    //Debug.Log("Portal relative: " + relativeDistForward + " " + relativeDistSide);
                    //Debug.Log("Dist to entrance (showing): " + distToEntrance);
                    if (isThroughPortal(relativeDistForward, Mathf.Abs(relativeDistSide)))
                    {
                        Debug.Log("   THROUGH ENTRANCE PORTAL!");
                        SetPortalState(PortalState.ThroughEntrancePortal);
                    }
 
                    break;

                case PortalState.ThroughEntrancePortal:

                    if (_curTreasureState == TreasureState.TreasureCompleted)
                    {
                        //light up location of exit portal
                        Nights2Spot exitSpot = curPath.GetPortalSpot(Nights2Path.PortalType.ExitPortal);
                        if (exitSpot != null)
                            Nights2SpotMgr.Instance.MakeSpotActive(exitSpot);

                        //OK, see when we're close to the exit portal and show it                    
                        distToExit = curPath.DistToPortal(Nights2Path.PortalType.ExitPortal, GetLanternPosOnGround());
                        //Debug.Log("Dist to exit (not showing): " + distToExit);
                        if ((distToExit >= 0.0f) && (distToExit <= ShowPortalDistThresh))
                        {
                            Debug.Log("   SHOWING ENTRANCE PORTAL!");
                            SetPortalState(PortalState.ShowingExitPortal);
                        }
                    }
                    else
                    {
                        //update treasure state
                        switch (_curTreasureState)
                        {
                            case TreasureState.NoProgress: 
                                //k, let's get started
                                SetTreasureState(TreasureState.WaitingForTreasureReveal); 
                                break;
                            case TreasureState.WaitingForTreasureReveal:
                                //is the lantern carrier close enough to the spot, then reveal!
                                Vector3 treasurePos = GetTreasureSpotOnGround();
                                Vector3 lanternPos = GetLanternPosOnGround();
                                float distToTreasure = (treasurePos - lanternPos).magnitude;
                                if (distToTreasure <= TreasureRevealDist)
                                {
                                    SetTreasureState(TreasureState.TreasureReveal);
                                }
                                break;
                            case TreasureState.TreasureReveal:
                                //wait for chest to be unlocked
                                if (_treasure.IsUnlocked())
                                   SetTreasureState(TreasureState.TreasureWaitForCollection);
                                break;
                            case TreasureState.TreasureWaitForCollection:
                                //wait for player to take the magic
                                if (_treasure.IsCollected())
                                   SetTreasureState(TreasureState.TreasureCompleted);
                                break;
                            default: break;
                        }                    
                    }
                    break;

                case PortalState.ShowingExitPortal:

                    //detect when player walks through exit portal
                    curPath.GetPortalRelativePos(Nights2Path.PortalType.ExitPortal, GetPlayerPosOnGround(), out relativeDistForward, out relativeDistSide);
                    //Debug.Log("Dist to exit (showing): " + distToExit);
                    //Debug.Log("Portal relative: " + relativeDistForward + " " + relativeDistSide);
                    if (isThroughPortal(relativeDistForward, Mathf.Abs(relativeDistSide)))
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
