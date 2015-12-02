//
//  Manages overall state progression
//

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Nights2Mgr : MonoBehaviour 
{

    public bool UseDebugPathOrder = false; //set true for my half-size paths I can test in my apartment 
    public Nights2Path[] CandlePathOrder = new Nights2Path[0]; //the order we cycle through candles, will be random if this array isnt long enough
    public Nights2Path[] DebugPathOrder = new Nights2Path[0]; //use these if UseDebugPathOrder is true
    public GameObject[] Candles = new GameObject[0]; //expected to have Nights2Beacon com on them

    [Space(10)]

    public Nights2Beacon[] ClockwiseCandleOrder = new Nights2Beacon[0]; //order to light things up in clockwise order

    public float BeaconLitSuccessTime = 3.0f; //how long we stay in the BeaconLit state before auto advancing to the next stage of the installation
    public float BeaconLitBlastTime = 1.5f; //how long do all the beacons light the color of the newly lit beacon?

    public GameObject VRRig; //this gets teleported between worlds, all the VR stuff is a child of this
    public GameObject RoomWorld; 
    public GameObject[] AltWorlds = new GameObject[0];

    public enum WorldID
    {
        RoomWorld,
        AltWorld1,
        AltWorld2
    }

    public event StateChangedHandler OnStateChanged;
    public class StateChangedEventArgs : EventArgs
    {
        public StateChangedEventArgs(Nights2State oldState, Nights2State newState) { OldState = oldState; NewState = newState; }
        public Nights2State OldState;
        public Nights2State NewState;
    }
    public delegate void StateChangedHandler(object sender, StateChangedEventArgs e);

    private Nights2State _curState = Nights2State.GettingReady;

    private List<Nights2Beacon> _unlitBeacons = new List<Nights2Beacon>();
    private List<Nights2Beacon> _litBeacons = new List<Nights2Beacon>();
    private Nights2Beacon _nextBeacon = null; //the next beacon to be lit by the torch carrier
    private Dictionary<Nights2Beacon, Nights2Path> _beaconToPathMap = new Dictionary<Nights2Beacon, Nights2Path>();
    private bool _isPathEditting = false;
    private int _curAltWorldIdx = 0;
    private float _stateActivateTime = 0.0f;
    private WorldID _curWorld = WorldID.RoomWorld;

    private float _turnStartTime = -1.0f;
    private float _turnEndTime = -1.0f;

    //how many candles were lit the last launch of the app
    private int _prevLaunchCandleProgress = -1;


    private LightAction _curLightOverride = LightAction.None;
    private float _lightActionStartTime = -1.0f;
    private TurnOnInSequenceParams _turnOnSeqParams = null;
    private TurnAllOnParams _turnOnAllParams = null;
    private GradientCycleParams _gradCycleParams = null;

    private bool _torchHasMagic = false;

    public enum Nights2State
    {
        GettingReady,      //participant is putting on the headset
        SeekingShamash,    //i.e dark hallway, need to light torch with shamash
        NearShamash,       //close to shamash
        SeekingBeacon,     //torch lit, following lantern carrier
        NearBeacon,        //near the beacon
        FlameExtinguished, //failure, gotta re-light torch
        BeaconLit,          //success!
        AllBeaconsLit       //final success state
    };

    public enum LightAction
    {
       None,
       TurnAllOn, //turn on for x seconds, optional color override
       GradientCycle, //cycle through a gradient at the given spead with option fmod level meter to control brightness
       TurnOnInSequence, //turn on one at a time using counter clockwise sequence, option to specify delay between each, and final hold time
    }

    public class TurnAllOnParams
    {
       public TurnAllOnParams(float duration, bool shouldOverrideColor, Color overrideColor)
       {
          Duration = duration;
          ShouldOverrideColor = shouldOverrideColor;
          OverrideColor = overrideColor;
       }

       public float Duration;
       public bool ShouldOverrideColor;
       public Color OverrideColor;
    };

    public class GradientCycleParams
    {
       public GradientCycleParams(float duration, Gradient gradient, float cycleSpeed, FModLevelMeter levelMeter = null, float levelMeterGain = 1.0f)
       {
          Duration = duration;
          ColorGradient = gradient;
          LevelMeter = levelMeter;
          LevelMeterGain = levelMeterGain;
          CycleSpeed = cycleSpeed;
       }

       public float Duration;
       public Gradient ColorGradient;
       public FModLevelMeter LevelMeter;
       public float LevelMeterGain;
       public float CycleSpeed; //cycles per second
    }

    public class TurnOnInSequenceParams
    {
       public TurnOnInSequenceParams(float holdDuration, float nextLightInterval, Nights2Beacon[] lightOrder)
       {
          HoldDuration = holdDuration;
          NextLightInterval = nextLightInterval;
          LightOrder = lightOrder;
       }

       public float HoldDuration;
       public float NextLightInterval;
       public Nights2Beacon[] LightOrder;
    }

    //trigger effect on lights that turns them on in sequence
    public void FXTurnOnInSequence(TurnOnInSequenceParams paramDef)
    {
       _curLightOverride = LightAction.TurnOnInSequence;
       _turnOnSeqParams = paramDef;
       _lightActionStartTime = Time.time;
    }

    //trigger effect on lights that turns them on in sequence
    public void FXGradientCycle(GradientCycleParams paramDef)
    {
       _curLightOverride = LightAction.GradientCycle;
       _gradCycleParams = paramDef;
       _lightActionStartTime = Time.time;
    }

    //trigger effect on lights that turns them on in sequence
    public void FXTurnOnAll(TurnAllOnParams paramDef)
    {
       _curLightOverride = LightAction.TurnAllOn;
       _turnOnAllParams = paramDef;
       _lightActionStartTime = Time.time;
    }

    public static Nights2Mgr Instance { get; private set; }

    public bool TorchHasMagic() { return _torchHasMagic; }
    public void SetTorchHasMagic(bool b) { _torchHasMagic = b; }

    public bool IsOverridingLights()
    {
       return _curLightOverride != LightAction.None;
    }

    public int NumCandlesLit()
    {
        return _litBeacons.Count;
    }

    public GameObject CurAltWorld()
    {
        return AltWorlds[_curAltWorldIdx];
    }

    public WorldID CurAltWorldID()
    {
        return (_curAltWorldIdx == 0) ? WorldID.AltWorld1 : WorldID.AltWorld2;
    }

    public bool InAltWorld1()
    {
        return (_curWorld == WorldID.AltWorld1);
    }

    public bool InAltWorld2()
    {
        return (_curWorld == WorldID.AltWorld2);
    }

    public void NotifyInWorld(WorldID newWorld)
    {
        WorldID prevWorld = _curWorld;

        _curWorld = newWorld;
        if(newWorld == WorldID.AltWorld1)
            Nights2AudioMgr.Instance.ActivateBackingLoop(Nights2AudioMgr.BackingLoops.kCaveWorldTheme);
        else if(newWorld == WorldID.AltWorld2)
            Nights2AudioMgr.Instance.ActivateBackingLoop(Nights2AudioMgr.BackingLoops.kOpenWorldTheme);
        else if (newWorld == WorldID.RoomWorld)
        {
            //came through return portal
            if ((prevWorld == WorldID.AltWorld1) || (prevWorld == WorldID.AltWorld2))
            {
                Nights2AudioMgr.Instance.ActivateBackingLoop(Nights2AudioMgr.BackingLoops.kBeaconAmbience);
            }
        }
    }

    public Nights2State GetState() { return _curState; }

    public float GetTurnElapsedTime()
    {
       if (_turnStartTime < 0.0f)
          return 0.0f;
       else if (_turnEndTime < 0.0f)
          return Time.time - _turnStartTime;
       else //between rounds, display last turns time
          return _turnEndTime - _turnStartTime;
    }

    public void SetState(Nights2State s)
    {
        if (_curState != s)
        {
            Nights2State prevState = _curState;
            _curState = s;

            Debug.Log("STATE CHANGE from '" + prevState.ToString() + "' to '" + _curState.ToString() +"'");

            _stateActivateTime = Time.time;

            //if just starting to seek new beacon, pick one
            if (((prevState == Nights2State.SeekingShamash) || (prevState == Nights2State.NearShamash)) &&
                (_curState == Nights2State.SeekingBeacon))
            {
                LightCurrentPath(true);
            }

            //reset magic state on torch
            if ((_curState == Nights2State.SeekingShamash) || (_curState == Nights2State.GettingReady) || (_curState == Nights2State.BeaconLit))
               _torchHasMagic = false;           

            //keep track of when this turn started, so we can display a timer
            if (prevState == Nights2State.GettingReady)
            {
               _turnStartTime = Time.time;
               _turnEndTime = -1.0f;
            }
            else if (_curState == Nights2State.GettingReady)
            {
               _turnEndTime = Time.time;
            }

            //pick beacon when starting to seek shamash so it can sync up with the color of the new candle
            if(_curState == Nights2State.SeekingShamash)
            {
                PickNextBeacon();
            }

            //turn off path
            if ((_curState != Nights2State.SeekingBeacon) && (_curState != Nights2State.NearBeacon))
                LightCurrentPath(false);

            //when torch is not lit, we play shamash ambience
            if ((_curState == Nights2State.SeekingShamash) || (_curState == Nights2State.GettingReady) || (_curState == Nights2State.NearShamash) || (_curState == Nights2State.BeaconLit))
            {
                Nights2AudioMgr.Instance.ActivateBackingLoop(Nights2AudioMgr.BackingLoops.kShamashAmbience);
            }

            if (_curState == Nights2State.SeekingBeacon)
            {
               Nights2AudioMgr.Instance.ActivateBackingLoop(Nights2AudioMgr.BackingLoops.kBeaconAmbience);
            }

            //update tracking lists if a beacon is lit
            if (_curState == Nights2State.BeaconLit)
            {
               Debug.Assert(_nextBeacon != null);

               if (Nights2AudioMgr.Instance.BeaconLitOneOff != null)
                   Nights2AudioMgr.Instance.BeaconLitOneOff.Play();

               //start blasting all the beacons the same color
               FXTurnOnAll(new TurnAllOnParams(BeaconLitBlastTime, true, _nextBeacon.CandleColor));

               //update state of next beacon
               _nextBeacon.SetLit(true);
               _nextBeacon.SetIsNext(false);

               //update bookkeeping
               if (_unlitBeacons.Contains(_nextBeacon))
                  _unlitBeacons.Remove(_nextBeacon);
               if (!_litBeacons.Contains(_nextBeacon))
                  _litBeacons.Add(_nextBeacon);

               //pick alt world for next beacon
               _curAltWorldIdx = NumCandlesLit() % AltWorlds.Length;

               SaveProgression();
            }

            if (OnStateChanged != null)
                OnStateChanged(this, new StateChangedEventArgs(prevState, s));
        }
    }

    public void SetIsPathEditting(bool b) { _isPathEditting = b; }

    //paths register here so mgr knows which paths lead to which beacons
    public void RegisterPath(Nights2Path path, Nights2Beacon leadsToBeacon)
    {
        if ((path == null) || (leadsToBeacon == null))
            return;

        //ignore if not in the current progression
        //we allow multiple progressions, make sure we have the right one
        bool found = false;
        for (int i = 0; i < CandlePathOrderLength(); i++)
        {
            if (GetPath(i) == path)
            {
                found = true;
                break;
            }
        }

        if (!found)
            return;

        _beaconToPathMap[leadsToBeacon] = path;
    }

    //returns the path the torch carrier is currently following
    public Nights2Path CurrentTorchPath()
    {
        if (_nextBeacon == null)
            return null;

        if (_beaconToPathMap.ContainsKey(_nextBeacon))
            return _beaconToPathMap[_nextBeacon];

        return null;
    }

    void Awake()
    {
        Instance = this;
    }

	void Start () 
    {
        ResetInstallation();
        LoadProgression();
	}

    public void ResetInstallation()
    {
        Nights2AudioMgr.Instance.ActivateBackingLoop(Nights2AudioMgr.BackingLoops.kShamashAmbience);
        _torchHasMagic = false;
        SetState(Nights2State.GettingReady);

        ResetBeacons();
    }

    public Nights2Beacon GetBeacon(int idx)
    {
        if ((idx >= 0) && (idx < Candles.Length))
        {
            return Candles[idx].GetComponent<Nights2Beacon>();
        }

        return null;
    }

    public Nights2Beacon NextBeacon()
    {
        return _nextBeacon;
    }

    const string kCandleProgreshKey = "num candles lit";

    void SaveProgression()
    {
       PlayerPrefs.SetInt(kCandleProgreshKey, NumCandlesLit());
    }

    void LoadProgression()
    {
       _prevLaunchCandleProgress = PlayerPrefs.GetInt(kCandleProgreshKey, -1);
    }

    public int PrevLaunchCandleProgress()
    {
       return _prevLaunchCandleProgress;
    }

    void ResetBeacons()
    {
        _unlitBeacons.Clear();
        _litBeacons.Clear();

        //build unlit list and initialize beacons to off
        int idx = 0;
        foreach (GameObject g in Candles)
        {
            Nights2Beacon b = g.GetComponent<Nights2Beacon>();
            if (b != null)
            {
                b.SetIsNext(false);
                b.SetLit(false);
                b.SetBeaconIdx(idx);

                _unlitBeacons.Add(b);
            }

            idx++;
        }
    }

    int CandlePathOrderLength()
    {
        return UseDebugPathOrder ? DebugPathOrder.Length : CandlePathOrder.Length;
    }

    Nights2Path GetPath(int idx)
    {
        return UseDebugPathOrder ? DebugPathOrder[idx] : CandlePathOrder[idx];
    }

    void PickNextBeacon()
    {
        if (_unlitBeacons.Count == 0)
        {
            Debug.LogError("Can't pick next beacon, there are no unlit ones left!");
            return;
        }

        Nights2Beacon b = null;
        //first beacon, just use the one we're configured for
        if (_litBeacons.Count < CandlePathOrderLength())
        {
            b = GetPath(_litBeacons.Count).LeadsToBeacon;
        }

        //pick randomly
        if (b == null)
        {
            int idxToPick = UnityEngine.Random.Range(0, _unlitBeacons.Count);
            b = _unlitBeacons[idxToPick];
        }

        //old one no longer next
        if (_nextBeacon != null)
            _nextBeacon.SetIsNext(false);

        //new one is
        b.SetIsNext(true);

        _nextBeacon = b;
    }

    void LightCurrentPath(bool lightOn)
    {
        int curPathIdx = -1;
        Nights2Path path = CurrentTorchPath();
        if (path != null)
        {
            for (int i = 0; i < CandlePathOrderLength(); i++)
            {
                if (GetPath(i) == path)
                {
                    curPathIdx = i;
                    break;
                }
            }
        }
        //first turn em all off
        for (int i = 0; i < 8; i++)
        {
            EightNightsMgr.GroupID g = (EightNightsMgr.GroupID)((int)EightNightsMgr.GroupID.Path1 + i);
            if(!lightOn || (i != curPathIdx))
                LightMgr.Instance.SetLight(g, EightNightsMgr.LightID.Light1, 0.0f);
        }

        //turn on the one we want
        if (lightOn && (curPathIdx >= 0))
        {
            EightNightsMgr.GroupID g = (EightNightsMgr.GroupID)((int)EightNightsMgr.GroupID.Path1 + curPathIdx);
            LightMgr.Instance.SetLight(g, EightNightsMgr.LightID.Light1, 1.0f);
        }
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        //use spacebar to toggle between getting ready and initial gameplay state
        if (Input.GetKeyDown(KeyCode.Space) ||
            ((Nights2InputMgr.Instance.TorchInfo().GetRedButtonDown() || Nights2InputMgr.Instance.LanternInfo().GetRedButtonDown()) && !_isPathEditting)) //red button on torch too!
        {
            if (GetState() == Nights2State.GettingReady) //activate seeking shamash state to get into the experience
                SetState(Nights2State.SeekingShamash);
            else //otherwise swap lantern and torch
               Nights2CamMgr.Instance.SwapControllers();
        }

        //gamepad cheat to toggle between end turn and initial state
        if (Input.GetButtonDown("A"))
        {
            if (GetState() == Nights2State.GettingReady) //activate seeking shamash state to get into the experience
                SetState(Nights2State.SeekingShamash);
            else
                SetState(Nights2State.GettingReady);
        }

        //'r' key resets everything
        if (Input.GetKeyDown(KeyCode.R))
            ResetInstallation();

        if (Input.GetButtonDown("RB"))
            CheatToNextState();

        if (Input.GetButtonDown("A"))
        {

        }

        if (_curState == Nights2State.BeaconLit)
        {
            //stay in beacon lit state for a bit, then auto advance to next thing
            float elapsed = Time.time - _stateActivateTime;
            if (elapsed > BeaconLitSuccessTime)
            {
                if (_unlitBeacons.Count == 0) //we're done, all beacons are lit!
                    SetState(Nights2State.AllBeaconsLit);
                else //light up!
                    SetState(Nights2State.SeekingShamash);
            }
        }
        
        //special FX on lights
        UpdateLightFX();

        //figure out if we should be in ducked mode
        bool shouldAudioDuck = true;
        //if ((_curState == Nights2State.SeekingShamash) || (_curState == Nights2State.GettingReady) || (_curState == Nights2State.BeaconLit) || (_curState == Nights2State.AllBeaconsLit))
        //   shouldAudioDuck = false;
        if (_curWorld == WorldID.RoomWorld)
           shouldAudioDuck = false;
        Nights2AudioMgr.Instance.SetDuckedMode(shouldAudioDuck);
	}

   void UpdateLightFX()
   {
      if (_curLightOverride == LightAction.None)
         return;

      float u = 0.0f;
      float actionElapsed = Time.time - _lightActionStartTime;

      switch (_curLightOverride)
      {
         //turn on all lights together
         case LightAction.TurnAllOn:
            u = Mathf.Clamp01(actionElapsed / _turnOnAllParams.Duration);

            //fade in for a bit
            const float kFadeInTime = .35f;
            float blendIntensityU = Mathf.Clamp01(actionElapsed / kFadeInTime);

            for (int i = 0; i < 8; i++)
            {
               EightNightsMgr.GroupID candleGroup = (EightNightsMgr.GroupID)(EightNightsMgr.GroupID.Candle1 + i);
               LightMgr.Instance.SetAllLightsInGroup(candleGroup, blendIntensityU, _turnOnAllParams.ShouldOverrideColor ? _turnOnAllParams.OverrideColor : LightMgr.Instance.GetDefaultColor(candleGroup));
            }
         break;

         case LightAction.GradientCycle:
            u = Mathf.Clamp01(actionElapsed / _gradCycleParams.Duration);

            //sample the color gradient
            float gradientSample = (actionElapsed * _gradCycleParams.CycleSpeed) % 1.0f;
            Color gradColor = _gradCycleParams.ColorGradient.Evaluate(gradientSample);

            float intensity = 1.0f;
            if (_gradCycleParams.LevelMeter != null)
               intensity = Mathf.Clamp01(_gradCycleParams.LevelMeter.CurLevel * _gradCycleParams.LevelMeterGain);

            for (int i = 0; i < 8; i++)
            {
               EightNightsMgr.GroupID candleGroup = (EightNightsMgr.GroupID)(EightNightsMgr.GroupID.Candle1 + i);
               LightMgr.Instance.SetAllLightsInGroup(candleGroup, intensity, gradColor);
            }
         break;

         case LightAction.TurnOnInSequence:
            float turnOnTime = (_turnOnSeqParams.LightOrder.Length * _turnOnSeqParams.NextLightInterval);
            float totalTime = turnOnTime + _turnOnSeqParams.HoldDuration;

            u = Mathf.Clamp01(actionElapsed / totalTime);

            float turnOnU = Mathf.Clamp01(actionElapsed / turnOnTime);
            int onIdxThresh = (int)(turnOnU * _turnOnSeqParams.LightOrder.Length);
            for (int i = 0; i < _turnOnSeqParams.LightOrder.Length; i++)
            {
               Nights2Beacon candle = _turnOnSeqParams.LightOrder[i];
               EightNightsMgr.GroupID candleGroup = Nights2AudioMgr.Instance.GetGroupForBeacon(candle);
               LightMgr.Instance.SetAllLightsInGroup(candleGroup, (i <= onIdxThresh) ? 1.0f : 0.0f, LightMgr.Instance.GetDefaultColor(candleGroup));
            }            

         break;

         default: break;
      }

      if (Mathf.Approximately(u, 1.0f)) //done?
      {
         _curLightOverride = LightAction.None;
      }
      
   }

   public void CheatCandlesLitTo(int desiredCandlesLit)
   {
      //OK, this is probably not full proof
      Nights2TorchPlayer.Instance.CheatPortalState(Nights2TorchPlayer.PortalState.NoProgress);
      Nights2TorchPlayer.Instance.CheateTreasureState(Nights2TorchPlayer.TreasureState.NoProgress);
      
      //reset all beacons to unlit
      ResetBeacons();

      desiredCandlesLit = Mathf.Min(Candles.Length,desiredCandlesLit);
      for (int i = 0; i < desiredCandlesLit; i++)
      {
         Nights2Beacon b = GetPath(i).LeadsToBeacon;
         _unlitBeacons.Remove(b);
         _litBeacons.Add(b);

         b.SetLit(true);
         b.SetIsNext(false);
      }

      //start shamash sequence
      SetState(Nights2State.SeekingShamash);

      SaveProgression();
      _curAltWorldIdx = NumCandlesLit() % AltWorlds.Length;
   }

   public void CheatToNextState()
   {
      if (GetState() == Nights2State.GettingReady)
         SetState(Nights2State.SeekingShamash);
      else if (GetState() == Nights2State.SeekingShamash)
         Nights2Shamash.Instance.NotifyPlayerNearby();
      else if (GetState() == Nights2State.NearShamash)
      {
         SetState(Nights2State.SeekingBeacon);
         Nights2Shamash.Instance.NotifyPlayerNotNearby();
         for (int i = 0; i < Candles.Length; i++)
             Candles[i].GetComponent<Nights2Beacon>().NotifyPlayerNotNearby();
      }
      else if (GetState() == Nights2State.SeekingBeacon)
      {
          if (Nights2TorchPlayer.Instance.GetPortalState() == Nights2TorchPlayer.PortalState.NoProgress)
              Nights2TorchPlayer.Instance.CheatPortalState(Nights2TorchPlayer.PortalState.ShowingEntrancePortal);
          else if (Nights2TorchPlayer.Instance.GetPortalState() == Nights2TorchPlayer.PortalState.ShowingEntrancePortal)
              Nights2TorchPlayer.Instance.CheatPortalState(Nights2TorchPlayer.PortalState.ThroughEntrancePortal);
          else if (Nights2TorchPlayer.Instance.GetPortalState() == Nights2TorchPlayer.PortalState.ThroughEntrancePortal)
          {
              if (Nights2TorchPlayer.Instance.GetTreasureState() == Nights2TorchPlayer.TreasureState.WaitingForTreasureReveal)
                  Nights2TorchPlayer.Instance.CheateTreasureState(Nights2TorchPlayer.TreasureState.TreasureReveal);
              else if (Nights2TorchPlayer.Instance.GetTreasureState() == Nights2TorchPlayer.TreasureState.TreasureReveal)
                  Nights2TorchPlayer.Instance.CheateTreasureState(Nights2TorchPlayer.TreasureState.TreasureCompleted);
              else if (Nights2TorchPlayer.Instance.GetTreasureState() == Nights2TorchPlayer.TreasureState.TreasureCompleted)
                  Nights2TorchPlayer.Instance.CheatPortalState(Nights2TorchPlayer.PortalState.ShowingExitPortal);
          }
          else if (Nights2TorchPlayer.Instance.GetPortalState() == Nights2TorchPlayer.PortalState.ShowingExitPortal)
          {
              Nights2TorchPlayer.Instance.CheatPortalState(Nights2TorchPlayer.PortalState.ThroughExitPortal);
              SetState(Nights2State.NearBeacon);
              _nextBeacon.NotifyPlayerNearby();
          }
          else if (Nights2TorchPlayer.Instance.GetPortalState() == Nights2TorchPlayer.PortalState.ThroughExitPortal)
          {
              SetState(Nights2State.NearBeacon);
              _nextBeacon.NotifyPlayerNearby();
          }
      }
      else if (GetState() == Nights2State.FlameExtinguished)
         SetState(Nights2State.SeekingBeacon);
      else if (GetState() == Nights2State.NearBeacon)
      {
         _nextBeacon.TriggerTorchLitBeacon();
      }
      else if (GetState() == Nights2State.BeaconLit)
      {
         _nextBeacon.NotifyPlayerNotNearby();
         SetState(Nights2State.SeekingShamash);
      }
   }
}
