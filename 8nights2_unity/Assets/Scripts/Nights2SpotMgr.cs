//
//  This takes a list of transes and spawns a "spot" prefab (with a Nights2Spot script on it) to handle detecting when the lantern carrier reveals the spot
//  The rest of the code goes through this manager to make spots "current" and forward messages along from events that occur on that spot
//

using UnityEngine;
using System.Collections;
using System;

public class Nights2SpotMgr : MonoBehaviour 
{
   public GameObject SpotPrefab = null; //expected to be a prefab with the Nightsa2Spot script on it
   public Transform[] SpotLocations = new Transform[0];

   public event SpotEventHandler OnLanternArrived;
   public class SpotEventArgs : EventArgs
   {
      public SpotEventArgs(Nights2Spot s) { Spot = s;  }
      public Nights2Spot Spot;
   }
   public delegate void SpotEventHandler(object sender, SpotEventArgs e);

   private Nights2Spot[] _spots = null;
   private Nights2Spot _activeSpot = null;

   public static Nights2SpotMgr Instance { get; private set; }

   void Awake()
   {
      Instance = this;
   }

	void Start () 
   {
      //spawn into each location
      _spots = new Nights2Spot[SpotLocations.Length];
      for (int i = 0; i < _spots.Length; i++)
      {
         GameObject spawnedObj = Instantiate(SpotPrefab) as GameObject;
         Debug.Assert(spawnedObj != null);

         spawnedObj.transform.parent = SpotLocations[i];
         spawnedObj.transform.localPosition = Vector3.zero;
         spawnedObj.transform.localRotation = Quaternion.identity;

         Nights2Spot spot = spawnedObj.GetComponent<Nights2Spot>();
         Debug.Assert(spot != null);
         spot.MakeActive(false);
         _spots[i] = spot;
      }

      _activeSpot = null;
	}

   public void MakeSpotActive(Nights2Spot s)
   {
      if (s == _activeSpot)
         return;

      if (_activeSpot != null)
         _activeSpot.MakeActive(false);

      _activeSpot = s;
      if (_activeSpot != null)
         _activeSpot.MakeActive(true);
   }

   public Nights2Spot ActiveSpot()
   {
      return _activeSpot;
   }

   //given a trans, find the corresponding spawned spot
   public Nights2Spot FindSpotForLocation(Transform loc)
   {
      for (int i = 0; i < SpotLocations.Length; i++)
      {
         if (loc == SpotLocations[i])
            return _spots[i];
      }
      return null;
   }

   public void NotifyLanternArrived(Nights2Spot spot)
   {
      if (OnLanternArrived != null)
         OnLanternArrived(this, new SpotEventArgs(spot));
   }

   void OnDrawGizmosSelected()
   {
      for (int i = 0; i < SpotLocations.Length; i++)
      {
         const float kSphereRadius = .1f;
         Gizmos.color = Color.blue;
         Gizmos.DrawSphere(SpotLocations[i].position, kSphereRadius);

         drawString(SpotLocations[i].gameObject.name, SpotLocations[i].position, Color.yellow);
      }
   }

   static void drawString(string text, Vector3 worldPos, Color? colour = null)
   {
      #if UNITY_EDITOR
      UnityEditor.Handles.BeginGUI();
      Color oldColor = GUI.color;
      if (colour.HasValue) GUI.color = colour.Value;
      var view = UnityEditor.SceneView.currentDrawingSceneView;
      Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
      Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
      GUIStyle s = new GUIStyle();
      s.fontSize = 15;
      GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text, s);
      GUI.color = oldColor;
      UnityEditor.Handles.EndGUI();
      #endif
   }
}
