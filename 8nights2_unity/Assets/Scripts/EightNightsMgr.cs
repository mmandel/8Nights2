﻿//
//  Central manager for the installation.  Handles configuration of all the lights and sends out events as states change
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EightNightsMgr : MonoBehaviour 
{
   [Range(0.0f, 1.0f)]
   public float MasterFader = 1.0f;
   public bool TestLights = false;
   public bool ProceduralButtonFX = true;
   public LightGroupConfig[] LightGroups = new LightGroupConfig[1];
   

   //events
   public event LightHandler OnLightChanged; //send out whenever a physical light is updated
   public event LightTriggeredHandler OnLightEffectTriggered; //send out whenver an effect is triggered on a light
   public event ButtonTriggeredHandler OnButtonTriggered; //send out whenver a button is triggered for a group
   public class LightEventArgs : EventArgs
   {
      public LightEventArgs(GroupID g, LightID l, LightTypes lt, LightData d) { Group = g; Light = l; LightType = lt;  Data = d; }
      public GroupID Group;
      public LightID Light;
      public LightTypes LightType;
      public LightData Data;
   }
   public delegate void LightHandler(object sender, LightEventArgs e);

   public class LightTriggeredEventArgs : EventArgs
   {
      public LightTriggeredEventArgs(GroupID g, LightID l, float w) { Group = g; Light = l; Weight = w; }
      public GroupID Group;
      public LightID Light;
      public float Weight = 1.0f;
   }
   public delegate void LightTriggeredHandler(object sender, LightTriggeredEventArgs e);

   public class ButtonTriggeredEventArgs : EventArgs
   {
      public ButtonTriggeredEventArgs(GroupID g) { Group = g;}
      public GroupID Group;
   }
   public delegate void ButtonTriggeredHandler(object sender, ButtonTriggeredEventArgs e);


   //identifiers for each group of lights
   public enum GroupID
   {
      RiftGroup1 = 0,
      RiftGroup2,
      RiftGroup3,
      RiftGroup4,
      RoomGroup1,
      RoomGroup2,
      RoomGroup3,
      RoomGroup4
   }

   //identifiers for each light within a group (up to 8)
   public enum LightID
   {
      Light1 = 0,
      Light2,
      Light3,
      Light4,
      Light5,
      Light6,
      Light7,
      Light8
   }

   public enum LightTypes
   {
      Hue, //WiFi controlled, RGB lights
      LightJams, //DMX controlled, dimmable lights
      LightJams_Par64, //HMX controllered, dimmable + RGB
   }

   //some overrides that map all the face buttons of the gamepad to a button in the room in case I need to
   //sub in a controller for the button press
   public enum CheatOverrideMode
   {
      None = 0,
      RoomButton1,
      RoomButton2,
      RoomButton3,
      RoomButton4,
      RiftButton
   }

   [System.Serializable]
   public class LightGroupConfig
   {
      [HideInInspector]
      public string GroupName; //editor hack that is filled in automatically so array looks pretty in editor

      public bool Enabled = true;
      public GroupID Group = GroupID.RiftGroup1;
      public Color DefaultColor = Color.white;
      public LightConfig[] Lights = new LightConfig[1];

      public bool IsButtonPressed() { return _isButtonPressed; }
      public void SetIsButtonPressed(bool pressed) { _isButtonPressed = pressed; } 

      private bool _isButtonPressed = false;

   }
   
   [System.Serializable]
   public class LightConfig
   {
      public bool Enabled = true;
      public LightTypes LightType = LightTypes.LightJams;
      [Tooltip("Either the LightJams channel or index of light configured in the HueMessenger")]
      public int Channel;

      public void SetGroupConfig(LightGroupConfig gConfig) { _groupConfig = gConfig; }
      [System.NonSerialized]
      private LightGroupConfig _groupConfig = null;

      //state to implement transition time for LightJams
      private float _fromIntensity = 0.0f;
      private float _toIntensity = 0.0f;
      private float _lastIntensity = 0.0f;
      private Color _fromColor = Color.white;
      private Color _toColor = Color.white;
      private Color _lastColor = Color.white;    
      private float _transitionTime = 0.0f;
      private float _timeStamp = -1.0f;
      private int _debugNum = 0;


      public void Set(Color color, float intensity, float transitionTime = 0.0f)
      {
         if (!Enabled || !_groupConfig.Enabled)
            return;

         intensity *= EightNightsMgr.Instance.MasterFader;

         if (LightType == LightTypes.Hue)
         {
            HueMessenger.Instance.SetState(HueMessenger.Instance.FindLightWithChannel(Channel), (intensity > 0), intensity, color, transitionTime);
         }
         else if ((LightType == LightTypes.LightJams) || (LightType == LightTypes.LightJams_Par64))
         {
            if (transitionTime > 0)
            {
               _transitionTime = transitionTime;
               _timeStamp = Time.time;
               _fromIntensity = _lastIntensity;
               _toIntensity = intensity;
               _fromColor = _lastColor;
               _toColor = color;
            }
            else
            {
                if (LightType == LightTypes.LightJams)
                    LightJamsMgr.Instance.SendToLightJams(Channel, intensity);
                else if(LightType == LightTypes.LightJams_Par64)
                    Par64Value.SetPar64Color(Channel, intensity, color);

               _timeStamp = -1.0f;
               _lastIntensity = intensity;
               _lastColor = color;
            }
         }
      }

      public void Update()
      {
         //handle transition blend
         if (_timeStamp > 0.0f)
         {
            float u = Mathf.Clamp01((Time.time - _timeStamp) / _transitionTime);
            _lastIntensity = Mathf.Lerp(_fromIntensity, _toIntensity, u);
            _lastColor = Color.Lerp(_fromColor, _toColor, u);
            if (LightType == LightTypes.LightJams)
                LightJamsMgr.Instance.SendToLightJams(Channel, _lastIntensity);
            else if (LightType == LightTypes.LightJams_Par64)
                Par64Value.SetPar64Color(Channel, _lastIntensity, _lastColor);

            if (Mathf.Approximately(u, 1.0f)) //done?
               _timeStamp = -1.0f;
         }
      }

      public void SetDebugNum(int d) { _debugNum = d; }
      public int DebugNum() { return _debugNum; }
   }

   public class LightData
   {
      public LightData() { }
      public LightData(Color c, float i) { LightColor = c; LightIntensity = i; }
      public Color LightColor = Color.white;
      public float LightIntensity = 1.0f;
   }

   class BlendHueData
   {
      public BlendHueData(GroupID gID, LightID lID, LightData fromData, LightData toData, float transitionTime)
      {
         _gID = gID;
         _lID = lID;
         _fromData = fromData;
         _toData = toData;
         _transitionTime = transitionTime;

         _timeStamp = Time.time;
      }

      public GroupID GetGroupID() { return _gID; }
      public LightID GetLightID() { return _lID; }

      public void Update()
      {
         if (_timeStamp > 0.0f)
         {
            float u = Mathf.Clamp01((Time.time - _timeStamp) / _transitionTime);
            LightData blendedData = new LightData(Color.Lerp(_fromData.LightColor, _toData.LightColor, u), Mathf.Lerp(_fromData.LightIntensity, _toData.LightIntensity, u));

            EightNightsMgr.Instance.SendHueEvent(_gID, _lID, blendedData);

            if (Mathf.Approximately(u, 1.0f))
               _timeStamp = -1.0f;
         }
      }

      public bool IsDone()
      {
         return _timeStamp < 0.0f;
      }

      GroupID _gID;
      LightID _lID; 
      LightData _fromData;
      LightData _toData;
      float _transitionTime;

      float _timeStamp;

   }

   public static EightNightsMgr Instance { get; private set; }

   List<BlendHueData> _eventBlends = new List<BlendHueData>();
   private CheatOverrideMode _cheatOverride = CheatOverrideMode.None;
   private KeyCode[] _cheatOverrideKeys = new KeyCode[] { KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V, KeyCode.B, KeyCode.N };

   public bool HasCheatOverride()
   {
      return _cheatOverride != CheatOverrideMode.None;
   }

   public bool CheatDownForGroup(GroupID group)
   {
      if (!HasCheatOverride())
         return false;

      if (Input.GetButtonDown("A") || Input.GetButtonDown("B") || Input.GetButtonDown("X") || Input.GetButtonDown("Y"))
      {
         if (_cheatOverride == CheatOverrideMode.RoomButton1)
            return group == GroupID.RoomGroup1;
         else if (_cheatOverride == CheatOverrideMode.RoomButton2)
            return group == GroupID.RoomGroup2;
         else if (_cheatOverride == CheatOverrideMode.RoomButton3)
            return group == GroupID.RoomGroup3;
         else if (_cheatOverride == CheatOverrideMode.RoomButton4)
            return group == GroupID.RoomGroup4;
         else if (_cheatOverride == CheatOverrideMode.RiftButton)
            return group.ToString().Contains("Rift");
      }

      return false;
   }

   public bool CheatStateForGroup(GroupID group)
   {
      if (!HasCheatOverride())
         return false;

      if (Input.GetButton("A") || Input.GetButton("B") || Input.GetButton("X") || Input.GetButton("Y"))
      {
         if (_cheatOverride == CheatOverrideMode.RoomButton1)
            return group == GroupID.RoomGroup1;
         else if (_cheatOverride == CheatOverrideMode.RoomButton2)
            return group == GroupID.RoomGroup2;
         else if (_cheatOverride == CheatOverrideMode.RoomButton3)
            return group == GroupID.RoomGroup3;
         else if (_cheatOverride == CheatOverrideMode.RoomButton4)
            return group == GroupID.RoomGroup4;
         else if (_cheatOverride == CheatOverrideMode.RiftButton)
            return group.ToString().Contains("Rift");
      }

      return false;
   }

   void Awake()
   {
      Instance = this;
   }

	void Start () 
   {
      //subscribe to updates from HueMessenger, which get forwarded to in-game effects triggers
      HueMessenger.Instance.OnLightChanged += OnHueLightChanged;

      //subscribe to updates from LightJamsMgr, which get forwarded to in-game effects triggers
      LightJamsMgr.Instance.OnLightChanged += OnLightJamsLightChanged;

      if (ButtonSoundMgr.Instance != null)
         ButtonSoundMgr.Instance.OnCrescendoEnd += OnCrescendoEnd;

      //generate the configuration for the HueMessenger based on our configuration
      HueMessenger.Instance.Lights = new HueMessenger.Light[NumLightsOfType(LightTypes.Hue)];
      int hueLightIdx = 0;
      for (int i = 0; i < LightGroups.Length; i++)
      {
         LightGroupConfig lg = LightGroups[i];
         for (int j = 0; j < lg.Lights.Length; j++)
         {
            LightConfig lc = lg.Lights[j];

            lc.SetGroupConfig(lg);

            if (lc.LightType == LightTypes.Hue)
            {
               HueMessenger.Light newLight = new HueMessenger.Light();
               newLight.on = false;
               newLight.color = lg.DefaultColor;
               newLight.id = lc.Channel;
               newLight.fade = 1.0f;
               HueMessenger.Instance.Lights[hueLightIdx] = newLight;
               hueLightIdx++;
            }
         }
      }

      //start with lights all off
      SetAllLights(0.0f, Color.white, 0.0f);
	}

   //a way to set the value of any light in the system, regardless of type (Hue, LightJams, whatevs)
   public void SetLight(GroupID gID, LightID lID, float intensity, Color color = default(Color), float transitionTime = 0.0f)
   {
      LightConfig lc = FindLightConfig(gID, lID);
      if (lc != null)
         lc.Set(color, intensity, transitionTime);
   }

   public void SetAllLights(float intensity, Color color = default(Color), float transitionTime = 0.0f)
   {
      for (int i = 0; i < LightGroups.Length; i++)
      {
         LightGroupConfig lg = LightGroups[i];
         for (int j = 0; j < lg.Lights.Length; j++)
         {
            LightConfig lc = lg.Lights[j];
            lc.Set(color, intensity, transitionTime);
         }
      }
   }

   public void SetAllLightsInGroup(GroupID group, float intensity, Color color = default(Color), float transitionTime = 0.0f)
   {
         LightGroupConfig lg = FindGroupConfig(group);
         if(lg != null)
         {
            for (int j = 0; j < lg.Lights.Length; j++)
            {
               LightConfig lc = lg.Lights[j];
               lc.Set(color, intensity, transitionTime);
            }
         }
   }
   
   //get latency for the given light.  Meaning this is how long it takes to set a value, and have it appear on the lights.
   public float GetLatency(GroupID gID, LightID lID)
   {
         LightConfig lc = FindLightConfig(gID, lID);
         if (lc != null)
         {
            if (lc.LightType == LightTypes.Hue)
               return HueMessenger.Instance.GetCurLatency();
            else
               return LightJamsMgr.Instance.GetCurLatency();
         }
         return 0.0f;
   }

   public bool IsHueLight(GroupID g, LightID l)
   {
      LightConfig lConfig = FindLightConfig(g, l);
      if (lConfig != null)
         return lConfig.LightType == LightTypes.Hue;
      return false;
   }

   public bool IsLightJamsLight(GroupID g, LightID l)
   {
      LightConfig lConfig = FindLightConfig(g, l);
      if (lConfig != null)
         return lConfig.LightType == LightTypes.LightJams;
      return false;
   }

   public Color GetDefaultColor(GroupID g)
   {
      LightGroupConfig lg = FindGroupConfig(g);
      if (lg != null)
         return lg.DefaultColor;
      return Color.white;
   }

   public void SendLightTriggeredEvent(GroupID g, LightID l, float weight)
   {
      if (OnLightEffectTriggered != null)
         OnLightEffectTriggered(this, new LightTriggeredEventArgs(g, l, weight));
   }

   public void SendButtonTriggeredEvent(GroupID g)
   {
      if (OnButtonTriggered != null)
         OnButtonTriggered(this, new ButtonTriggeredEventArgs(g));
   }

   //finds the GroupID and LightID of a light with the given channel #
   //returns false if it doesnt find a compatible light in any of the groups
   bool FindLight(int channel, LightTypes lightType, ref GroupID gID, ref LightID lID)
   {
      for (int i = 0; i < LightGroups.Length; i++)
      {
         LightGroupConfig g = LightGroups[i];

         for (int j = 0; j < g.Lights.Length; j++)
         {
            LightConfig l = g.Lights[j];

            if ((l.Channel == channel) && (l.LightType == lightType))
            {
               gID = g.Group;
               lID = (LightID)j;
               return true;
            }
         }
      }

      return false;
   }

   LightGroupConfig FindGroupConfig(GroupID gId)
   {
      for (int i = 0; i < LightGroups.Length; i++)
      {
         LightGroupConfig g = LightGroups[i];
         if (g.Group == gId)
            return g;
      }

      return null;
   }

   LightConfig FindLightConfig(GroupID gId, LightID lId)
   {
      for (int i = 0; i < LightGroups.Length; i++)
      {
         LightGroupConfig g = LightGroups[i];
         if (g.Group == gId)
         {
            for (int j = 0; j < g.Lights.Length; j++)
            {
               if (j == (int)lId)
                  return g.Lights[j];
            }
         }
      }

      return null;
   }

   int NumLightsOfType(LightTypes t)
   {
      int count = 0;
      for (int i = 0; i < LightGroups.Length; i++)
      {
         LightGroupConfig lg = LightGroups[i];
         for (int j = 0; j < lg.Lights.Length; j++)
         {
            LightConfig lc = lg.Lights[j];
            if (lc.LightType == t)
               count++;
         }
      }
      return count;
   }

   void OnHueLightChanged(object sender, HueMessenger.HueEventArgs e)
   {
      if (OnLightChanged != null)
      {
         GroupID gID = GroupID.RiftGroup1;
         LightID lID = LightID.Light1;
         if (FindLight(e.LightID, LightTypes.Hue, ref gID, ref lID))
         {
            LightData newData = new LightData(e.LightColor, e.LightFade);

            //simulate fades
            if (e.TransitionTime > 0.0)
            {
               LightData fromData = new LightData(e.PrevColor, e.PrevFade);
               BlendHueData newBlendData = new BlendHueData(gID, lID, fromData, newData, e.TransitionTime);

               foreach (BlendHueData h in _eventBlends)
               {
                  if ((h.GetGroupID() == gID) && (h.GetLightID() == lID))
                  {
                     _eventBlends.Remove(h);
                     break;
                  }
               }

               _eventBlends.Add(newBlendData);
            }
            else
            {
               SendHueEvent(gID, lID, newData);
            }
         }
      }
   }

   public void SendHueEvent(GroupID gID, LightID lID, LightData newData)
   {
      if(OnLightChanged != null)
         OnLightChanged(this, new LightEventArgs(gID, lID, LightTypes.Hue, newData));
   }

   public void SetButtonPressedState(GroupID group, bool pressed)
   {
      LightGroupConfig lg = FindGroupConfig(group);
      if (lg != null)
      {
         lg.SetIsButtonPressed(pressed);
      }
   }
   

   void OnLightJamsLightChanged(object sender, LightJamsMgr.LJEventArgs e)
   {
      if (OnLightChanged != null)
      {
         GroupID gID = GroupID.RiftGroup1;
         LightID lID = LightID.Light1;
         if (FindLight(e.Channel, LightTypes.LightJams, ref gID, ref lID))
         {
            LightGroupConfig config = FindGroupConfig(gID);
            OnLightChanged(this, new LightEventArgs(gID, lID, LightTypes.LightJams, new LightData(config.DefaultColor, e.Intensity)));
         }
      }
   }

   void OnCrescendoEnd(object sender, ButtonSoundMgr.ButtonCrescendoEventArgs e)
   {
      //turn off lights at end of crescendo to allow for music FX to come through
      if (ProceduralButtonFX)
      {
         SetAllLightsInGroup(e.Group, 0.0f, GetDefaultColor(e.Group), .25f);
      }
   }

	void Update () 
   {
      if (Input.GetKeyDown(KeyCode.Escape))
         Application.Quit();

      //check all the cheat overrides to see if we want to activate one
      for (int i = 0; i < _cheatOverrideKeys.Length; i++)
      {
         KeyCode cheat = _cheatOverrideKeys[i];
         if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(cheat))
         {
            _cheatOverride = (CheatOverrideMode)i;
            Debug.Log("Activated Cheat Override: " + _cheatOverride.ToString());
         }
      }

      //update light configs to handle things like transitioning
      for (int i = 0; i < LightGroups.Length; i++)
      {
         LightGroupConfig lg = LightGroups[i];
         for (int j = 0; j < lg.Lights.Length; j++)
         {
            LightConfig lc = lg.Lights[j];
            lc.Update();
         }
      }

      List<BlendHueData> deleteMe = new List<BlendHueData>();
      foreach (BlendHueData b in _eventBlends)
      {
         b.Update();
         if (b.IsDone())
            deleteMe.Add(b);
      }
      foreach (BlendHueData b in deleteMe)
         _eventBlends.Remove(b);

      //toggle test mode with 't' key
      /*if (Input.GetKeyDown(KeyCode.T))
      {
         TestLights = !TestLights;
         //start with lights all off
         SetAllLights(0.0f, Color.white, 0.0f);
      }*/

      //run test patterns through all the lights
      if (TestLights)
      {
         Color[] testColors = new Color[] { Color.red, Color.red, Color.green, Color.green, Color.blue, Color.blue, Color.yellow, Color.yellow };
         float[] testIntensities = new float[] { 1.0f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f };

         for (int i = 0; i < LightGroups.Length; i++)
         {
            LightGroupConfig lg = LightGroups[i];
            for (int j = 0; j < lg.Lights.Length; j++)
            {
               LightConfig lc = lg.Lights[j];

               float latency = GetLatency(lg.Group, (LightID)j);

               int testNum = ((int)(Time.time - latency));

               if (testNum != lc.DebugNum())
               {
                  Color testColor = testColors[testNum % testColors.Length];
                  float testIntensity = testIntensities[testNum % testIntensities.Length];
                  lc.Set(testColor, testIntensity, 1.0f);

                  lc.SetDebugNum(testNum);
               }
            }
         }
      }
      else
      {
         if (ProceduralButtonFX)
         {
            for (int i = 0; i < LightGroups.Length; i++)
            {
               LightGroupConfig lg = LightGroups[i];
               bool isCresendoing = ButtonSoundMgr.Instance.IsGroupCrescendoing(lg.Group);
               bool isReversing = ButtonSoundMgr.Instance.IsGroupCrescendoingReversed(lg.Group);
               bool isButtonPressed = lg.IsButtonPressed(); 

               float cProgress = ButtonSoundMgr.Instance.GetCrescendoProgressForGroup(lg.Group);
               if (isReversing)
                  cProgress = 1.0f - cProgress;
               for (int j = 0; j < lg.Lights.Length; j++)
               {
                  LightConfig lc = lg.Lights[j];
                  if ((j == 0) && isButtonPressed && isCresendoing) //first light is alway on when you have button pressed in
                     SetLight(lg.Group, (LightID)j, 1.0f);
                  else if(isCresendoing) //set light to crescendo progress
                     SetLight(lg.Group, (LightID)j, cProgress);
               }
            }
         }
      }
	}
}
