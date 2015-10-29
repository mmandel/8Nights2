//
// Handles activiting various stems in response to gameplay events in the Nights2 (2015) installation 
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Nights2AudioMgr : MonoBehaviour
{
   public bool ShowTestUI = true;

   [Space(10)]
   public EightNightsMusicPlayerFMod MusicPlayer;
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
      if (!ShowTestUI || (_allGroupNames == null) || !_hadFirstUpdate)
         return;

      Vector2 startPos = new Vector2(10, 10);
      float buttonVSpacing = 30;

      // Candle Triggers
      Vector2 groupSize = new Vector2(100, buttonVSpacing * 8 + 30);
      GUI.Box(new Rect(startPos.x, startPos.y, groupSize.x, groupSize.y), "Candle Triggers");

      for (int i = 0; i < _allGroupNames.Length; i++)
      {
         int curCandle = i + 1;
         if (GUI.Button(new Rect(startPos.x + 10, startPos.y + curCandle * buttonVSpacing, groupSize.x - 20, 20), "Candle " + curCandle))
         {
            TriggerGroup(_allGroupNames[i]);
         }
      }


      //kill all button
      startPos.y += (_allGroupNames.Length * buttonVSpacing) + 40;
      //startPos.x -= 55;
      if (GUI.Button(new Rect(startPos.x, startPos.y, groupSize.x - 20, 20), "Reset All"))
      {
         ResetAllStems();
      }

      // text fields for stem tuning params
      startPos.x = Screen.width * .5f - 150;
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
      }
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

      Color origGUIColor = GUI.color;
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
         GUI.Label(sliderRect, g.ToString() + ": ");

         sliderRect.x += 100;

         float curV = MusicPlayer.GetVolumeForGroup(g);
         float sliderVol = GUI.HorizontalSlider(sliderRect, curV, 0.0f, 1.0f);
         if (MusicTester.EnableTestMode) //only sync slider value back if in test mode
            MusicPlayer.SetVolumeForGroup(g, sliderVol);
      }

      GUI.color = origGUIColor;

      // Show cur MBT in bottom left corner
      startPos.x = 10;
      startPos.y = Screen.height - 30;
      string MBTStr = "Beat Time: " + (BeatClock.Instance.curMeasure + 1) + ":" + (BeatClock.Instance.curBeat + 1) + ":" + BeatClock.Instance.curTick;
      GUI.Label(new Rect(startPos.x, startPos.y, 170, 25), MBTStr);
      // Show elapsed time
      startPos.y -= 20;
      int minutes = (int)(BeatClock.Instance.elapsedSecs / 60.0f);
      int secs = (int)(BeatClock.Instance.elapsedSecs % 60.0f);
      string secsStr = (secs < 10) ? "0" + secs : secs.ToString();
      GUI.Label(new Rect(startPos.x, startPos.y, 170, 25), "Elapsed:    " + minutes + ":" + secsStr);

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
      startPos.x = (Screen.width / 2) - 150;
      startPos.y = Screen.height - 30;
      //display current state
      GUI.Label(new Rect(startPos.x, startPos.y, 170, 25), "State:  " +  Nights2Mgr.Instance.GetState().ToString());
      //display next button
      startPos.x += 160;
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
      startPos.x += 100;
      if ((Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.SeekingBeacon) &&
         GUI.Button(new Rect(startPos.x, startPos.y, 50, 20), "Fail"))
      {
         Nights2Mgr.Instance.SetState(Nights2Mgr.Nights2State.FlameExtinguished);
      }

      //Test lights in bottom right
      if (LightMgr.Instance != null)
      {
         startPos.x = Screen.width - 150;
         startPos.y = Screen.height - 30;
         LightMgr.Instance.TestLights = GUI.Toggle(new Rect(startPos.x, startPos.y, groupSize.x + 50, 20), LightMgr.Instance.TestLights, "Test Lights");
      }
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


   public void TriggerGroup(EightNightsMgr.GroupID group)
   {
      //we are going to exclusively sync with the state of each candle
      //so lets trigger the candle instead of the audio directly
      GroupStateData stateData = GetStateForGroup(group);
      if (stateData != null)
      {
         //toggle the light state, and the stem will respond next update
         Nights2Beacon b = Nights2Mgr.Instance.GetBeacon(stateData.CandleIdx);
         if (b != null)
         {
            b.SetLit(!b.IsLit());
         }
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
      if (Input.GetKeyDown(KeyCode.D))
      {
         ShowTestUI = !ShowTestUI;
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
               MusicPlayer.SetVolumeForGroup(d.Group, d.MasterFader * u);

               if (Mathf.Approximately(u, 1.0f))
               {
                  d.CaptureTimestamp();
                  d.LoopState = StemLoopState.Sustaining;
               }
            }
            else if (d.LoopState == StemLoopState.Sustaining)
            {
               MusicPlayer.SetVolumeForGroup(d.Group, d.MasterFader * 1.0f);
            }
            else if (d.LoopState == StemLoopState.Releasing)
            {
               float u = Mathf.Clamp01((Time.time - d.Timestamp()) / StemReleaseTime);
               MusicPlayer.SetVolumeForGroup(d.Group, d.MasterFader * (1.0f - u));

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
