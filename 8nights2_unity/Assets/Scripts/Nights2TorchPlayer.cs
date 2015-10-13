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
    [Tooltip("Set this flag true to draw some debug stuff for the path the torch carrier is walking down")]
    public bool DebugPathFollowing = true;
    public GameObject DebugSnapToPath = null;

    bool IsFollowingPath()
    {
        return (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.SeekingBeacon) ||
               (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.NearBeacon);
    }

	void Update () 
    {
        //keep it attached to the torch carrier's head
        transform.parent = Nights2CamMgr.Instance.GetHeadTrans();
        transform.localPosition = Vector3.zero;

        if (DebugPathFollowing && (DebugSnapToPath != null) && IsFollowingPath())
        {
            DebugSnapToPath.SetActive(true);
            Nights2Path p = Nights2Mgr.Instance.CurrentTorchPath();
            if (p != null)
            {
                Vector3 closestPt = p.ClosestPointOnPath(transform.position);
                DebugSnapToPath.transform.position = closestPt;
            }
        }
        else if (DebugSnapToPath != null)
        {
            DebugSnapToPath.SetActive(false);
        }

	}

    void OnDrawGizmos()
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
    }
}
