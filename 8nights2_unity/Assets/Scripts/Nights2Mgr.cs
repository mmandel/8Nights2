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

    public float BeaconLitSuccessTime = 3.0f; //how long we stay in the BeaconLit state before auto advancing to the next stage of the installation

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

    public static Nights2Mgr Instance { get; private set; }

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

            //pick beacon when starting to seek shamash so it can sync up with the color of the new candle
            if(_curState == Nights2State.SeekingShamash)
            {
                PickNextBeacon();
            }

            //turn off path
            if ((_curState != Nights2State.SeekingBeacon) && (_curState != Nights2State.NearBeacon))
                LightCurrentPath(false);

            if (_curState == Nights2State.SeekingShamash)
            {
                Nights2AudioMgr.Instance.ActivateBackingLoop(Nights2AudioMgr.BackingLoops.kShamashAmbience);
            }

            //update tracking lists if a beacon is lit
            if (_curState == Nights2State.BeaconLit)
            {
               Debug.Assert(_nextBeacon != null);

               if (Nights2AudioMgr.Instance.BeaconLitOneOff != null)
                   Nights2AudioMgr.Instance.BeaconLitOneOff.Play();

               //update state of next beacon
               _nextBeacon.SetLit(true);
               _nextBeacon.SetIsNext(false);

               //pick alt world for next beacon
               _curAltWorldIdx = (_curAltWorldIdx + 1) % AltWorlds.Length;

               //update bookkeeping
               if (_unlitBeacons.Contains(_nextBeacon))
                  _unlitBeacons.Remove(_nextBeacon);
               if (!_litBeacons.Contains(_nextBeacon))
                  _litBeacons.Add(_nextBeacon);
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
	}

    public void ResetInstallation()
    {
        Nights2AudioMgr.Instance.ActivateBackingLoop(Nights2AudioMgr.BackingLoops.kShamashAmbience);

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
