﻿//
// Handles activiting various stems in response to gameplay events in the Nights2 (2015) installation 
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Nights2AudioMgr : MonoBehaviour
{
    public bool Enable8ChannelMode = true;
   public bool ShowTestUI = true;

   [Space(10)]
   public EightNightsMusicPlayerFMod MusicPlayer;

   [Space(10)]

   public FMOD_StudioEventEmitter ShamashAmbience;
   public FMOD_StudioEventEmitter BeaconAmbience;
   public FMOD_StudioEventEmitter CaveWorldTheme;
   public FMOD_StudioEventEmitter OpenWorldTheme;

   [Space(10)] 

   public FMOD_StudioEventEmitter BeaconLitOneOff;

   [Space(10)]
   public float DuckedStemsLevel = .25f;
   public float DuckStemsTime = 1.0f;
   public float UnduckStemsTime = .25f;

   [Space(10)]
   public MusicTestData MusicTester = new MusicTestData();

   [Header("Tuning Values")]
   public int BaseBPM = 84;
   public float StemAttackTime = 2.0f;
   public float StemReleaseTime = 1.0f;


   public static Nights2AudioMgr Instance { get; private set; }

   private GroupStateData[] _groupState = null;
   private EightNightsMgr.GroupID[] _allGroupNames;
   private bool _hadFirstUpdate = false;

   private bool  _isDucked = false;
   private float _duckedStemsFader = 1.0f;
   private float _duckingStartTime = -1.0f;

   //backing loop's state
   public enum StemLoopState
   {
      Off,
      Attacking,
      Sustaining,
      Releasing
   }

   public class GroupStateData
   {
      public EightNightsMgr.GroupID Group;
      public StemLoopState LoopState = StemLoopState.Off;
      public bool UseTriggerCheat = false;
      public KeyCode TriggerCheat = KeyCode.Alpha1;
      public float MasterFader = 1.0f;
      public int CandleIdx = -1;


      public void CaptureTimestamp() { _timeStamp = Time.time; }
      public void SetTimestamp(float t) { _timeStamp = t; }
      public float Timestamp() { return _timeStamp; }
      private float _timeStamp = -1.0f;
   }


   [System.Serializable]
   public class MusicTestData
   {
      public bool EnableTestMode = false;
      [Space(10)]
      [Header("Backing Loop")]
      [Range(0.0f, 1.0f)]
      public float BackingLoopVolume = 1.0f;
      [Header("Candle Stems")]
      [Range(0.0f, 1.0f)]
      public float Candle1Volume = 0.0f;
      [Range(0.0f, 1.0f)]
      public float Candle2Volume = 0.0f;
      [Range(0.0f, 1.0f)]
      public float Candle3Volume = 0.0f;
      [Range(0.0f, 1.0f)]
      public float Candle4Volume = 0.0f;
      [Range(0.0f, 1.0f)]
      public float Candle5Volume = 0.0f;
      [Range(0.0f, 1.0f)]
      public float Candle6Volume = 0.0f;
      [Range(0.0f, 1.0f)]
      public float Candle7Volume = 0.0f;
      [Range(0.0f, 1.0f)]
      public float Candle8Volume = 0.0f;
   }

   void Awake()
   {
      Instance = this;

      //force surround sound on/off
      FMOD_StudioSystem.MandelForceSurround = Enable8ChannelMode;

      //initialize group data for all 8 candles
      _allGroupNames = new EightNightsMgr.GroupID[] { EightNightsMgr.GroupID.Candle1, EightNightsMgr.GroupID.Candle2, EightNightsMgr.GroupID.Candle3, EightNightsMgr.GroupID.Candle4, EightNightsMgr.GroupID.Candle5, EightNightsMgr.GroupID.Candle6, EightNightsMgr.GroupID.Candle7, EightNightsMgr.GroupID.Candle8 };
      _groupState = new GroupStateData[_allGroupNames.Length];
      int i = 0;
      foreach (EightNightsMgr.GroupID g in _allGroupNames)
      {
         GroupStateData newData = new GroupStateData();
         newData.LoopState = StemLoopState.Off;
         newData.Group = g;
         newData.CandleIdx = g - EightNightsMgr.GroupID.Candle1;
         newData.UseTriggerCheat = true;
         _groupState[i] = newData;
         i++;
      }

      //setup cheats
      GetStateForGroup(EightNightsMgr.GroupID.Candle1).TriggerCheat = KeyCode.Alpha1;
      GetStateForGroup(EightNightsMgr.GroupID.Candle2).TriggerCheat = KeyCode.Alpha2;
      GetStateForGroup(EightNightsMgr.GroupID.Candle3).TriggerCheat = KeyCode.Alpha3;
      GetStateForGroup(EightNightsMgr.GroupID.Candle4).TriggerCheat = KeyCode.Alpha4;
      GetStateForGroup(EightNightsMgr.GroupID.Candle5).TriggerCheat = KeyCode.Alpha5;
      GetStateForGroup(EightNightsMgr.GroupID.Candle6).TriggerCheat = KeyCode.Alpha6;
      GetStateForGroup(EightNightsMgr.GroupID.Candle7).TriggerCheat = KeyCode.Alpha7;
      GetStateForGroup(EightNightsMgr.GroupID.Candle8).TriggerCheat = KeyCode.Alpha8;
   }


   public float GetGroupVolume(EightNightsMgr.GroupID g)
   {
      if (MusicPlayer != null)
      {
         return MusicPlayer.GetVolumeForGroup(g);
      }
      return 0.0f;
   }

   //duck stems to lower volume level?
   public void SetDuckedMode(bool b)
   {
      if (_isDucked != b)
      {
         _isDucked = b;
         _duckingStartTime = Time.time;
      }
   }

   public enum BackingLoops
   {
       kShamashAmbience,
       kBeaconAmbience,
       kCaveWorldTheme,
       kOpenWorldTheme
   };

   public void ActivateBackingLoop(BackingLoops l)
   {
       ShamashAmbience.Stop();
       BeaconAmbience.Stop();
       CaveWorldTheme.Stop();
       OpenWorldTheme.Stop();

       switch (l)
       {
           case BackingLoops.kBeaconAmbience: BeaconAmbience.Play(); break;
           case BackingLoops.kShamashAmbience: ShamashAmbience.Play(); break;
           case BackingLoops.kCaveWorldTheme: CaveWorldTheme.Play(); break;
           case BackingLoops.kOpenWorldTheme: OpenWorldTheme.Play(); break;
           default: break;
       }
   }


   void Start()
   {
      _hadFirstUpdate = false;
      SetTempo(BaseBPM);
   }

   public float GetElapsedSecs()
   {
      return MusicPlayer.GetElapsedSecs();
   }

   void SetTempo(int bpm)
   {
      BeatClock.Instance.bpm = BaseBPM;
   }


   void ResetAllStems(bool instant = false)
   {
      Nights2Mgr.Instance.ResetInstallation();

      foreach (GroupStateData g in _groupState)
      {
         if (instant)
         {
            g.LoopState = StemLoopState.Off;
         }
         else
         {
            if ((g.LoopState != StemLoopState.Off) && (g.LoopState != StemLoopState.Releasing))
            {
               g.LoopState = StemLoopState.Releasing;
               g.CaptureTimestamp();
            }
         }
      }
   }

   void ResetAllStemTimestamps()
   {
      foreach (GroupStateData g in _groupState)
      {
         g.CaptureTimestamp();
      }
   }

   void OnGUI()
   {

      // Show elapsed time regardless of whether debug mode is on, we always want to see that
      Vector3 startPos = new Vector2(10, Screen.height - 50);
      float elapsedSecs = Nights2Mgr.Instance.GetTurnElapsedTime();
      int minutes = (int)(elapsedSecs / 60.0f);
      int secs = (int)(elapsedSecs % 60.0f);
      string secsStr = (secs < 10) ? "0" + secs : secs.ToString();
      GUI.Label(new Rect(startPos.x, startPos.y, 170, 25), "Timer:    " + minutes + ":" + secsStr);

      if (!ShowTestUI || (_allGroupNames == null) || !_hadFirstUpdate)
         return;

      startPos.x = 10;
      startPos.y = 10;
      float buttonVSpacing = 30;

      // Progression Trigger
      Vector2 groupSize = new Vector2(100, buttonVSpacing * 8 + 30);
      GUI.Box(new Rect(startPos.x, startPos.y, groupSize.x, groupSize.y), "Progression");

      Color origGUIColor = GUI.color;

      Color disabledGUIColor = origGUIColor;
      disabledGUIColor.a = .5f;

      int numCandlesLit = Nights2Mgr.Instance.NumCandlesLit();
      for (int i = 0; i < _allGroupNames.Length; i++)
      {
         int curCandle = i + 1;

         bool isButDisabled = false;
         if (curCandle <= numCandlesLit) //should be transparent to indicate its already been progressed past here
         {
            GUI.color = disabledGUIColor;
            isButDisabled = true;
         }
         else
            GUI.color = origGUIColor;

         string butString = "Light " + curCandle;

         //if num lit == 0, put a * next to the further the progression got from the last launch of the app 
         if ((numCandlesLit == 0) && (curCandle == Nights2Mgr.Instance.PrevLaunchCandleProgress()))
         {
            butString = "* " + butString;
         }

         if (GUI.Button(new Rect(startPos.x + 10, startPos.y + curCandle * buttonVSpacing, groupSize.x - 20, 20), butString))
         {
            //TriggerGroup(_allGroupNames[i]);
            if (!isButDisabled)
               Nights2Mgr.Instance.CheatCandlesLitTo(curCandle);
         }
      }

      GUI.color = origGUIColor;


      //kill all button
      startPos.y += (_allGroupNames.Length * buttonVSpacing) + 40;
      //startPos.x -= 55;
      if (GUI.Button(new Rect(startPos.x, startPos.y, groupSize.x - 20, 20), "Reset All"))
      {
         ResetAllStems();
      }

      // text fields for stem tuning params
      /*startPos.x = Screen.width * .5f - 150;
      startPos.y = 10;
      groupSize = new Vector2(200, buttonVSpacing * 2 + 30);
      GUI.Box(new Rect(startPos.x, startPos.y, groupSize.x, groupSize.y), "Stem Behavior");
      //Attack Time
      startPos.y += buttonVSpacing;
      GUI.Label(new Rect(startPos.x + 10, startPos.y, groupSize.x - 50, 20), "Attack Time: ");
      string attackStr = StemAttackTime.ToString();
      string newAttackStr = GUI.TextField(new Rect(startPos.x + 10 + groupSize.x - 70, startPos.y, 50, 20), attackStr);
      if (!newAttackStr.Equals(attackStr))
      {
         float newAttack = 0.0f;
         if (float.TryParse(newAttackStr, out newAttack))
         {
            StemAttackTime = newAttack;
            ResetAllStemTimestamps();
         }
      }
      //Release
      startPos.y += buttonVSpacing;
      GUI.Label(new Rect(startPos.x + 10, startPos.y, groupSize.x - 50, 20), "Release Time: ");
      string releaseStr = StemReleaseTime.ToString();
      string newReleaseStr = GUI.TextField(new Rect(startPos.x + 10 + groupSize.x - 70, startPos.y, 50, 20), releaseStr);
      if (!newReleaseStr.Equals(releaseStr))
      {
         float newRelease = 0.0f;
         if (float.TryParse(newReleaseStr, out newRelease))
         {
            StemReleaseTime = newRelease;
            ResetAllStemTimestamps();
         }
      }*/
      //tweak sync (HACK!)
     /* startPos.y += buttonVSpacing;
      String signStr = (BeatClock.Instance.LatencyMs >= 0) ? "+" : "-";
      GUI.Label(new Rect(startPos.x + 10, startPos.y, groupSize.x - 50, 20), "Sync: " + signStr + (BeatClock.Instance.LatencyMs / 1000));
      if (GUI.Button(new Rect(startPos.x + 10 + groupSize.x - 85, startPos.y, 30, 20), "<"))
      {
         BeatClock.Instance.LatencyMs -= 1000;
      }

      if (GUI.Button(new Rect(startPos.x + 10 + groupSize.x - 45, startPos.y, 30, 20), ">"))
      {
         BeatClock.Instance.LatencyMs += 1000;
      }*/

      // Test level sliders
      startPos.x = Screen.width - 300;
      startPos.y = 10;

      GUI.Box(new Rect(startPos.x - 30, startPos.y, 310, 350), "Stem Levels");

      startPos.y += buttonVSpacing;

      MusicTester.EnableTestMode = GUI.Toggle(new Rect(startPos.x, startPos.y, 200, 30), MusicTester.EnableTestMode, "Override");

      startPos.y += buttonVSpacing;

      Color curGUIColor = origGUIColor;
      if (!MusicTester.EnableTestMode)
         curGUIColor.a = .5f;
      GUI.color = curGUIColor;

      //backing loop
      Rect backingRect = new Rect(startPos.x, startPos.y, 170, 25);
      GUI.Label(backingRect, "Backing: ");
      backingRect.x += 100;
      float backingVol = MusicPlayer.GetBackingLoopVolume();
      float newBackingVol = GUI.HorizontalSlider(backingRect, backingVol, 0.0f, 1.0f);
      if (MusicTester.EnableTestMode) //only sync slider value back if in test mode
         MusicPlayer.SetBackingLoopVolume(newBackingVol);

      startPos.y += buttonVSpacing;

      //peak loop
      Rect peakRect = new Rect(startPos.x, startPos.y, 170, 25);
      GUI.Label(peakRect, "Peak State: ");
      peakRect.x += 100;
      float peakVol = MusicPlayer.GetPeakLoopVolume();
      float newPeakVol = GUI.HorizontalSlider(peakRect, peakVol, 0.0f, 1.0f);
      if (MusicTester.EnableTestMode) //only sync slider value back if in test mode
         MusicPlayer.SetPeakLoopVolume(newPeakVol);

      //loops for each group
      foreach (EightNightsMgr.GroupID g in _allGroupNames)
      {
         startPos.y += buttonVSpacing;

         Rect sliderRect = new Rect(startPos.x, startPos.y, 170, 25);
         int stemIdx = MusicPlayer.GetSubstitutionIdxForGroup(g);
         string groupName = g.ToString() + " (" + (stemIdx + 1) + "): ";
         GUI.Label(sliderRect, groupName);

         sliderRect.x += 100;

         float curV = MusicPlayer.GetVolumeForGroup(g);
         float sliderVol = GUI.HorizontalSlider(sliderRect, curV, 0.0f, 1.0f);
         if (MusicTester.EnableTestMode) //only sync slider value back if in test mode
            MusicPlayer.SetVolumeForGroup(g, sliderVol);
      }

      GUI.color = origGUIColor;

      // Show cur MBT in bottom left corner
      /*startPos.x = 10;
      startPos.y = Screen.height - 30;
      //string MBTStr = "Beat Time: " + (BeatClock.Instance.curMeasure + 1) + ":" + (BeatClock.Instance.curBeat + 1) + ":" + BeatClock.Instance.curTick;
      //GUI.Label(new Rect(startPos.x, startPos.y, 170, 25), MBTStr);
      // Show elapsed time
      startPos.y -= 20;
      //float elapsedSecs = BeatClock.Instance.elapsedSecs;
      float elapsedSecs = Nights2Mgr.Instance.GetTurnElapsedTime();
      int minutes = (int)(elapsedSecs / 60.0f);
      int secs = (int)(elapsedSecs % 60.0f);
      string secsStr = (secs < 10) ? "0" + secs : secs.ToString();
      GUI.Label(new Rect(startPos.x, startPos.y, 170, 25), "Timer:    " + minutes + ":" + secsStr);*/

      //Restart Song Button
      //TODO: this code doesn't work yet, so disabling for now...
      /*   startPos.x += 135;
         startPos.y += 10;
         if(GUI.Button(new Rect(startPos.x, startPos.y, 100, 25), "Restart Song"))
         {
            MusicPlayer.Restart();
         }
      */

      //advance progression in bottom center
      startPos.x = (Screen.width / 2) - 175;
      startPos.y = Screen.height - 30;
      //display current state
      string stateStr = Nights2Mgr.Instance.GetState().ToString();
      if ((Nights2TorchPlayer.Instance.GetTreasureState() != Nights2TorchPlayer.TreasureState.NoProgress) && (Nights2TorchPlayer.Instance.GetTreasureState() != Nights2TorchPlayer.TreasureState.TreasureCompleted))
          stateStr = Nights2TorchPlayer.Instance.GetTreasureState().ToString();
      else if ((Nights2TorchPlayer.Instance.GetPortalState() != Nights2TorchPlayer.PortalState.NoProgress) && (Nights2TorchPlayer.Instance.GetPortalState() != Nights2TorchPlayer.PortalState.ThroughExitPortal)) 
          stateStr = Nights2TorchPlayer.Instance.GetPortalState().ToString();
      GUI.Label(new Rect(startPos.x, startPos.y, 225, 25), "State:  " + stateStr);
      //display next button
      startPos.x += 190;
      if (GUI.Button(new Rect(startPos.x, startPos.y, 30, 20), ">"))
      {
         Nights2Mgr.Instance.CheatToNextState();
      }
      //end a player's turn
      startPos.y -= 25;
      startPos.x -= 100;
      if (GUI.Button(new Rect(startPos.x, startPos.y, 65, 20), "End Turn"))
      {
         Nights2Mgr.Instance.SetState(Nights2Mgr.Nights2State.GettingReady);
      }

      //simulate failing to walk along path
      /*startPos.x += 100;
      if ((Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.SeekingBeacon) &&
         GUI.Button(new Rect(startPos.x, startPos.y, 50, 20), "Fail"))
      {
         Nights2Mgr.Instance.SetState(Nights2Mgr.Nights2State.FlameExtinguished);
      }*/

      //Test lights in bottom right
      if (LightMgr.Instance != null)
      {
         startPos.x = Screen.width - 150;
         startPos.y = Screen.height - 30;
         LightMgr.Instance.TestLights = GUI.Toggle(new Rect(startPos.x, startPos.y, groupSize.x + 50, 20), LightMgr.Instance.TestLights, "Test Lights");
      }

      //current lit up spot also in bottom right
      startPos.y -=  30;
      GUI.Label(new Rect(startPos.x, startPos.y, 225, 25), "Lit Spot:  " + ((Nights2SpotMgr.Instance.ActiveSpot() != null) ? Nights2SpotMgr.Instance.ActiveSpot().gameObject.name : "[none]"));
   }

   public GroupStateData GetStateForGroup(EightNightsMgr.GroupID group)
   {
      foreach (GroupStateData d in _groupState)
      {
         if (d.Group == group)
            return d;
      }
      return null;
   }

   public Nights2Beacon GetBeaconForGroup(EightNightsMgr.GroupID group)
   {
      GroupStateData stateData = GetStateForGroup(group);
      if (stateData != null)
      {
         Nights2Beacon b = Nights2Mgr.Instance.GetBeacon(stateData.CandleIdx);
         return b;
      }

      return null;
   }

   public EightNightsMgr.GroupID GetGroupForBeacon(Nights2Beacon beacon)
   {
      foreach (GroupStateData d in _groupState)
      {
         if (Nights2Mgr.Instance.GetBeacon(d.CandleIdx) == beacon)
            return d.Group;
      }

      Debug.LogWarning("Couldn't find group for beacon '" + beacon.gameObject.name + "'.  Defaulting to 'Candle1'.  Probably a bug!");
      return EightNightsMgr.GroupID.Candle1;
   }

   public FMOD_StudioEventEmitter GetNarrationForGroup(EightNightsMgr.GroupID group)
   {
      return MusicPlayer.GetNarrationForGroup(group);
   }

   public float GetNarrationTimeForGroup(EightNightsMgr.GroupID group)
   {
      return MusicPlayer.GetNarrationTimeForGroup(group);
   }

   public void TriggerGroup(EightNightsMgr.GroupID group)
   {
      //we are going to exclusively sync with the state of each candle
      //so lets trigger the candle instead of the audio directly
      //toggle the light state, and the stem will respond next update
      Nights2Beacon b = GetBeaconForGroup(group);
      if (b != null)
      {
         b.SetLit(!b.IsLit());
      }

      /*GroupStateData stateData = GetStateForGroup(group);
      if (stateData != null)
      {
         stateData.CaptureTimestamp(); //reset decay timers

         //If track already on, then we toggle it off
         if (stateData.LoopState == StemLoopState.Sustaining)
         {
            stateData.LoopState = StemLoopState.Releasing;
         }
      }*/
   }

   bool AllGroupStemsSustaining()
   {
      bool nowInPeak = true;
      foreach (GroupStateData d in _groupState)
      {
         if (d.LoopState != StemLoopState.Sustaining)
         {
            nowInPeak = false;
            break;
         }
      }
      return nowInPeak;
   }


   void Update()
   {
      if (!_hadFirstUpdate)
      {
         MusicPlayer.SetBackingLoopVolume(1.0f);
         MusicPlayer.SetPeakLoopVolume(0.0f);
      }
      _hadFirstUpdate = true;

      //toggle debug UI with a key
      if (Input.GetKeyDown(KeyCode.D) || Input.GetButtonDown("LB"))
      {
         ShowTestUI = !ShowTestUI;
      }

      //handle ducking audio
      if (_duckingStartTime >= 0.0f)
      {
         float elapsed = (Time.time - _duckingStartTime);
         float transishTime = _isDucked ? DuckStemsTime : UnduckStemsTime;
         float u = Mathf.Clamp01(elapsed / transishTime);
         u = _isDucked ? 1.0f - u : u;

         _duckedStemsFader = Mathf.Lerp(DuckedStemsLevel, 1.0f, u);

         if (Mathf.Approximately(u, 1.0f)) //done?
         {
            _duckingStartTime = -1.0f;
         }
      }
      else
      {
         _duckedStemsFader = _isDucked ? DuckedStemsLevel : 1.0f;
      }


      /*if (Input.GetKeyDown(KeyCode.KeypadPlus))
         BeatClock.Instance.LatencyMs += 1000;
      if (Input.GetKeyDown(KeyCode.KeypadMinus))
         BeatClock.Instance.LatencyMs -= 1000;*/

      //keyboard cheats
      foreach (GroupStateData d in _groupState)
      {
         if (d.UseTriggerCheat && Input.GetKeyDown(d.TriggerCheat))
            TriggerGroup(d.Group);
      }


      //test mode for overridding stem levels
      if (MusicTester.EnableTestMode && !ShowTestUI)
      {
         MusicPlayer.SetBackingLoopVolume(MusicTester.BackingLoopVolume);

         MusicPlayer.SetVolumeForGroup(EightNightsMgr.GroupID.Candle1, MusicTester.Candle1Volume);
         MusicPlayer.SetVolumeForGroup(EightNightsMgr.GroupID.Candle2, MusicTester.Candle2Volume);
         MusicPlayer.SetVolumeForGroup(EightNightsMgr.GroupID.Candle3, MusicTester.Candle3Volume);
         MusicPlayer.SetVolumeForGroup(EightNightsMgr.GroupID.Candle4, MusicTester.Candle4Volume);
         MusicPlayer.SetVolumeForGroup(EightNightsMgr.GroupID.Candle5, MusicTester.Candle5Volume);
         MusicPlayer.SetVolumeForGroup(EightNightsMgr.GroupID.Candle6, MusicTester.Candle6Volume);
         MusicPlayer.SetVolumeForGroup(EightNightsMgr.GroupID.Candle7, MusicTester.Candle7Volume);
         MusicPlayer.SetVolumeForGroup(EightNightsMgr.GroupID.Candle8, MusicTester.Candle8Volume);
      }

      //update state of all the audio levels
      if (!MusicTester.EnableTestMode)
      {

         foreach (GroupStateData d in _groupState)
         {
            
            //start attacking or releasing based on candle state
            Nights2Beacon b =  Nights2Mgr.Instance.GetBeacon(d.CandleIdx);
            bool isLit = false;
            if (b != null)
               isLit = b.IsLit();

            //turn on stem if we're lit but not yet attacking or sustaining
            if (isLit && ((d.LoopState != StemLoopState.Attacking) && (d.LoopState != StemLoopState.Sustaining)))
            {
               d.LoopState = StemLoopState.Attacking;
               d.CaptureTimestamp();
            }

            //turn off stem if we're not lit, but current sustaining
            if (!isLit && (d.LoopState == StemLoopState.Sustaining))
            {
               d.LoopState = StemLoopState.Releasing;
               d.CaptureTimestamp();
            }


            if (d.LoopState == StemLoopState.Off)
            {
               MusicPlayer.SetVolumeForGroup(d.Group, 0.0f);
            }
            else if (d.LoopState == StemLoopState.Attacking)
            {
               float u = Mathf.Clamp01((Time.time - d.Timestamp()) / StemAttackTime);
               MusicPlayer.SetVolumeForGroup(d.Group, _duckedStemsFader * d.MasterFader * u);

               if (Mathf.Approximately(u, 1.0f))
               {
                  d.CaptureTimestamp();
                  d.LoopState = StemLoopState.Sustaining;
               }
            }
            else if (d.LoopState == StemLoopState.Sustaining)
            {
                MusicPlayer.SetVolumeForGroup(d.Group, _duckedStemsFader*d.MasterFader * 1.0f);
            }
            else if (d.LoopState == StemLoopState.Releasing)
            {
               float u = Mathf.Clamp01((Time.time - d.Timestamp()) / StemReleaseTime);
               MusicPlayer.SetVolumeForGroup(d.Group, _duckedStemsFader*d.MasterFader * (1.0f - u));

               if (Mathf.Approximately(u, 1.0f))
               {
                  d.CaptureTimestamp();
                  d.LoopState = StemLoopState.Off;
               }
            }
         }
      }
   }
}
