//
//  PD controller to track a target position by applying forces to the rigid body in this gameobj
//

using UnityEngine;
using System.Collections;

public class PhysicsTracker : MonoBehaviour 
{
   public float PGain = 1.0f;
   public float DGain = .1f;
   public float StopDist = .01f; //stop applying forces when within this dist of the goal
   public Transform GoalTrans; //optional, can also be driven in code

   private Rigidbody _rigidBody;
   private Vector3 _overrideTargetPos = Vector3.zero;
   private bool _useOverride = false;

   public void UseOverride(bool b) { _useOverride = b; }
   public void SetOverrideTargetPos(Vector3 tp) { _overrideTargetPos = tp; }

	void Start () 
   {
      _rigidBody = gameObject.GetComponent<Rigidbody>();
	}

   Vector3 GetTargetPos()
   {
      if (_useOverride)
         return _overrideTargetPos;
      else
         return (GoalTrans != null) ? GoalTrans.position : Vector3.zero;
   }

	void Update () 
   {
      if (_rigidBody == null)
         return;

      Vector3 curPos = this.transform.position;
      Vector3 targetPos = GetTargetPos();

      //basic PD controller:
      //f=pGain*(targetPos-curPos) + dGain*(targetVel-curVel)
      Vector3 dp = targetPos - curPos;
      if (dp.sqrMagnitude > StopDist*StopDist)
      {
         Vector3 deltaVel = -1.0f*_rigidBody.velocity;
         Vector3 force = (PGain * dp) + (DGain*deltaVel);

         _rigidBody.AddForce(force);
      }
	}
}
