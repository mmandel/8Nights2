//
//  This game object will move with the torch carrier's head
//
//  Expected to have a rigid body on the same game object, so it can be used to detect when the players head enters a trigger
//  Also doing path following logic here.
//

using UnityEngine;
using System.Collections;

public class Nights2TorchPlayer : MonoBehaviour 
{
    [Tooltip("When the torch carrier starts following the path, this is size of the 'deadzone' they have initially")]
    public float InitialPathDeadzone = .5f;
    [Tooltip("This is how close the player needs to stay to the path to be considered following it")]
    public float NearPathThresh = .25f;
    [Tooltip("How many seconds does the player need to be off the path before we fail them?")]
    public float OutsidePathFailureSecs = .5f;
    

    [Space(10)]

    [Tooltip("Set this flag true to draw some debug stuff for the path the torch carrier is walking down")]
    public bool DebugPathFollowing = true;
    public GameObject DebugSnapToPath = null;

    private bool _wasDebugPathOn = false;
    private bool _deadzoneActive = false;
    private Vector3 _deadzoneCenter = Vector3.zero;
    private float _outsidePathStartTime = -1.0f;

    void Start()
    {
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

    void OnNights2StateChanged(object sender, Nights2Mgr.StateChangedEventArgs e)
    {
        //OK, starting to follow a path
        if (e.NewState == Nights2Mgr.Nights2State.SeekingBeacon)
        {
            //record their position to allow for a small "dead zone" before they are required to stay super close
            _deadzoneActive = true;
            _deadzoneCenter = GetPlayerPosOnGround();
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

            //make sure player stays on the path
            if (!_deadzoneActive && (curPath != null))
            {
                Vector3 closestPtOnPath = curPath.ClosestPointOnPath(curPlayerPos);
                closestPtOnPath.y = 0.0f; //project to ground, just in case

                //are we too far off the path?
                float distToPath = (closestPtOnPath - curPlayerPos).magnitude;
                if (distToPath > NearPathThresh)
                {
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
                    Vector3 closestPt = p.ClosestPointOnPath(transform.position);
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
