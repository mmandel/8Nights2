//
//  A spot we are leading the torch carrier to, and is activated by the lantern carrier
//  It is spawned by the Nights2SpotMgr.  It can also be made "active", which turns its physics sensor on
//

using UnityEngine;
using System.Collections;

public class Nights2Spot : MonoBehaviour 
{

   public EightNightsMgr.GroupID LightGroup = EightNightsMgr.GroupID.Spot1;

   [ScriptButton("Debug SHOW", "OnDebugShow")]
   public bool DummyDebugShow;
    [ScriptButton("Debug HIDE", "OnDebugHide")]
   public bool DummyDebugHide;

   private bool _isSpotActive = false;
   private bool _lanternArrived = false;
   private Collider _collider = null;
   private GameObject _spawned = null;

   private Nights2SpotDebug _debugViz = null;
   private bool _debugOverride = false;

   public Vector3 GetPos()
   {
      return transform.position;
   }

   public void SetSpawned(GameObject go)
   {
       _spawned = go;
       _collider = (go != null) ? go.GetComponent<Collider>() : null;

       _debugViz = _spawned.GetComponentInChildren<Nights2SpotDebug>();
       if (_debugViz != null)
           _debugViz.gameObject.SetActive(Nights2SpotMgr.Instance.ShowSpotDebugSpheres);
   }

   void Awake()
   {
   }


	void Start () 
   {
       MakeActive(false);
	}

    void Update()
    {
       if (!Nights2SpotMgr.Instance.IsOverridingSpots() && (LightMgr.Instance != null) && !LightMgr.Instance.TestLights)
          LightMgr.Instance.SetLight(LightGroup, EightNightsMgr.LightID.Light1, _isSpotActive ? 1.0f : 0.0f);

        if ((_debugViz != null) && !_debugOverride)
            _debugViz.gameObject.SetActive(Nights2SpotMgr.Instance.ShowSpotDebugSpheres);
    }

   public void MakeActive(bool b)
   {
      _isSpotActive = b;
      _lanternArrived = false;

      //turn physical light on/off
      if (LightMgr.Instance != null)
          LightMgr.Instance.SetLight(LightGroup, EightNightsMgr.LightID.Light1, b ? 1.0f : 0.0f);

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

   public void OnDebugShow(string propPath)
   {
      MakeActive(true);
      if (_debugViz != null)
         _debugViz.gameObject.SetActive(true);
      _debugOverride = true;
   }

   public void OnDebugHide(string propPath)
   {
      MakeActive(false);
      if (_debugViz != null)
         _debugViz.gameObject.SetActive(false);
      _debugOverride = false;
   }
}
