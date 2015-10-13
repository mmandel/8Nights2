//
//  Manages overall state progression
//

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Nights2Mgr : MonoBehaviour 
{

    public GameObject StartCandle = null; //optional start candle
    public GameObject[] Candles = new GameObject[0]; //expected to have Nights2Beacon com on them

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

    public enum Nights2State
    {
        GettingReady,      //participant is putting on the headset
        SeekingShamash,    //i.e dark hallway, need to light torch with shamash
        NearShamash,       //close to shamash
        SeekingBeacon,     //torch lit, following lantern carrier
        NearBeacon,        //near the beacon
        FlameExtinguished, //failure, gotta re-light torch
        BeaconLit          //success!
    };

    public static Nights2Mgr Instance { get; private set; }

    public Nights2State GetState() { return _curState; }

    public void SetState(Nights2State s)
    {
        if (_curState != s)
        {
            Nights2State prevState = _curState;
            _curState = s;

            //if just starting to seek new beacon, pick one
            if (((prevState == Nights2State.SeekingShamash) || (prevState == Nights2State.NearShamash)) &&
                (_curState == Nights2State.SeekingBeacon))
            {
                PickNextBeacon();
            }
            //update tracking lists if a beacon is lit
            if (_curState == Nights2State.BeaconLit)
            {
                Debug.Assert(_nextBeacon != null);

                //update state of next beacon
                _nextBeacon.SetLit(true);
                _nextBeacon.SetIsNext(false);

                //update bookkeeping
                if(_unlitBeacons.Contains(_nextBeacon))
                    _unlitBeacons.Remove(_nextBeacon);
                if(!_litBeacons.Contains(_nextBeacon))
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

    void PickNextBeacon()
    {
        if (_unlitBeacons.Count == 0)
        {
            Debug.LogError("Can't pick next beacon, there are no unlit ones left!");
            return;
        }

        Nights2Beacon b = null;
        //first beacon, just use the one we're configured for
        if ((_litBeacons.Count == 0) && (StartCandle != null))
        {
            b = StartCandle.GetComponent<Nights2Beacon>();
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
	
	void Update () 
    {
        //use spacebar to toggle between getting ready and initial gameplay state
        if (Input.GetKeyDown(KeyCode.Space) ||
            (Nights2InputMgr.Instance.TorchInfo().GetRedButtonDown() && !_isPathEditting)) //red button on torch too!
        {
            if (GetState() != Nights2State.GettingReady)
                SetState(Nights2State.GettingReady);
            else
                SetState(Nights2State.SeekingShamash);
        }
	}
}
