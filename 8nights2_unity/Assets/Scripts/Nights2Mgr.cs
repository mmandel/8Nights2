//
//  Manages overall state progression
//

using UnityEngine;
using System.Collections;
using System;

public class Nights2Mgr : MonoBehaviour 
{

    public event StateChangedHandler OnStateChanged;
    public class StateChangedEventArgs : EventArgs
    {
        public StateChangedEventArgs(Nights2State oldState, Nights2State newState) { OldState = oldState; NewState = newState; }
        public Nights2State OldState;
        public Nights2State NewState;
    }
    public delegate void StateChangedHandler(object sender, StateChangedEventArgs e);

    private Nights2State _curState = Nights2State.GettingReady;

    public enum Nights2State
    {
        GettingReady,      //participant is putting on the headset
        SeekingShamash,    //i.e dark hallway, need to light torch with shamash
        NearShamash,       //close to shamash
        SeekingBeacon,     //torch lit, following lantern carrier
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

            if (OnStateChanged != null)
                OnStateChanged(this, new StateChangedEventArgs(prevState, s));
        }
    }

    void Awake()
    {
        Instance = this;
    }

	void Start () 
    {
        SetState(Nights2State.GettingReady);
	}
	
	void Update () 
    {
        //use spacebar to toggle between getting ready and initial gameplay state
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GetState() != Nights2State.GettingReady)
                SetState(Nights2State.GettingReady);
            else
                SetState(Nights2State.SeekingShamash);
        }
	}
}
