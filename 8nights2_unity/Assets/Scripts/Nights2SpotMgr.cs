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
   public bool ShowSpotDebugSpheres = false;
   public Nights2Spot[] Spots = new Nights2Spot[0];

   public event SpotEventHandler OnLanternArrived;
   public class SpotEventArgs : EventArgs
   {
      public SpotEventArgs(Nights2Spot s) { Spot = s;  }
      public Nights2Spot Spot;
   }
   public delegate void SpotEventHandler(object sender, SpotEventArgs e);

   private GameObject[] _spawned = null; //spawned for each Spot
   private Nights2Spot _activeSpot = null;
   private float _overrideStartTime = -1.0f;
   private float _overrideTime = 1.0f; //how long to override for
   private LightAction _overrideAction = LightAction.TurnAllOn;

   public enum LightAction
   {
      TurnAllOn,
      Ping
   }

   public static Nights2SpotMgr Instance { get; private set; }

   public Nights2Spot FindClosestSpotTo(Vector3 p)
   {
      float closestDist = float.MaxValue;
      Nights2Spot closestSpot = null;
      for (int i = 0; i < Spots.Length; i++)
      {
         float curDist = (Spots[i].GetPos() - p).sqrMagnitude;
         if (curDist < closestDist)
         {
            closestDist = curDist;
            closestSpot = Spots[i];
         }
      }

      return closestSpot;
   }

   public void TriggerSpotFX(LightAction l, float overrideTime = 1.0f)
   {
      _overrideStartTime = Time.time;
      _overrideTime = overrideTime; //how long to override for
      _overrideAction = l;
   }   

   void Awake()
   {
      Instance = this;
   }

	void Start () 
   {
      //spawn into each location
       _spawned = new GameObject[Spots.Length];
       for (int i = 0; i < _spawned.Length; i++)
      {
         GameObject spawnedObj = Instantiate(SpotPrefab) as GameObject;
         Debug.Assert(spawnedObj != null);

         Nights2Spot spot = Spots[i];
         Debug.Assert(spot != null);
         spot.SetSpawned(spawnedObj);
         spot.MakeActive(false);

         spawnedObj.transform.parent = spot.transform;
         spawnedObj.transform.localPosition = Vector3.zero;
         spawnedObj.transform.localRotation = Quaternion.identity;

         _spawned[i] = spawnedObj;
      }

      _activeSpot = null;
	}

   void Update()
   {
      //override behavior of spot lights?
      if (_overrideStartTime > 0.0f)
      {
         float elapsed = Time.time - _overrideStartTime;
         float u = Mathf.Clamp01(elapsed / _overrideTime);

         switch (_overrideAction)
         {
            case LightAction.TurnAllOn:
               for (int i = 0; i < Spots.Length; i++)
               {
                  Nights2Spot s = Spots[i];
                  if (s != null)
                     s.MakeActive(true);
               }
               break;
            case LightAction.Ping:
               //TODO!
               break;
            default: break;
         }

         if (Mathf.Approximately(u, 1.0f))
         {
            _overrideStartTime = -1.0f;

            //restore state of spots
            for (int i = 0; i < Spots.Length; i++)
            {
               Nights2Spot s = Spots[i];
               if (s != null)
               {
                  s.MakeActive(false);
               }
            }

            if (ActiveSpot() != null)
               ActiveSpot().MakeActive(true);
         }
      }
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


   public void NotifyLanternArrived(Nights2Spot spot)
   {
      if (OnLanternArrived != null)
         OnLanternArrived(this, new SpotEventArgs(spot));
   }

   void OnDrawGizmosSelected()
   {
      for (int i = 0; i < Spots.Length; i++)
      {
         const float kSphereRadius = .1f;
         Gizmos.color = Color.blue;
         Gizmos.DrawSphere(Spots[i].GetPos(), kSphereRadius);

         drawString(Spots[i].gameObject.name, Spots[i].GetPos(), Color.yellow);
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
      s.normal.textColor = Color.white;
      GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text, s);
      GUI.color = oldColor;
      UnityEditor.Handles.EndGUI();
      #endif
   }
}
