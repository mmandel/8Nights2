//
//  A spot we are leading the torch carrier to, and is activated by the lantern carrier
//  It is spawned by the Nights2SpotMgr.  It can also be made "active", which turns its physics sensor on
//

using UnityEngine;
using System.Collections;

public class Nights2Spot : MonoBehaviour 
{

   private bool _isSpotActive = false;
   private bool _lanternArrived = false;
   private Collider _collider = null;
   private GameObject _spawned = null;

   public Vector3 GetPos()
   {
      return transform.position;
   }

   public void SetSpawned(GameObject go)
   {
       _spawned = go;
       _collider = (go != null) ? go.GetComponent<Collider>() : null;
   }

   void Awake()
   {
      MakeActive(false);
   }


	void Start () 
   {

	}

   public void MakeActive(bool b)
   {
      _isSpotActive = b;
      _lanternArrived = false;

      if (_collider != null)
         _collider.enabled = b;
   }

   public bool IsSpotActive()
   {
      return _isSpotActive;
   }


   void OnTriggerEnter(Collider other)
   {
      if (!IsSpotActive() || !_lanternArrived) 
         return;

      //see if the lantern carrier is near
      if ((other != null) && (other == _collider) && (other.GetComponent<Nights2Lantern>() != null))
      {
         //Debug.Log("Lantern is NEAR active spot!!");

         _lanternArrived = true;
         Nights2SpotMgr.Instance.NotifyLanternArrived(this);
      }
   }

   //debug draw when selected
   void OnDrawGizmosSelected()
   {
      const float kSphereRadius = .1f;
      Gizmos.color = Color.blue;
      Gizmos.DrawSphere(transform.position, kSphereRadius);      
   }

}
