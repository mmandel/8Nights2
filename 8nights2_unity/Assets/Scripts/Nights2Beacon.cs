//
//  A single "candle" in the installation
//  A physical spotlight and audio stem will be associated with one of these
//

using UnityEngine;
using System.Collections;

public class Nights2Beacon : MonoBehaviour 
{

    public string IsLitBool = "on";
    public string IsHiddenBool = "hidden";
    public string IsNextBool = "is_next";
    public string PlayerCloseBool = "is_close";

    private bool _isNextBeacon = false;
    private bool _isLit = false;
    private int _beaconIdx = -1;
    private Animator _animator = null;
    private bool _playerIsNear = false;

    public bool IsLit() { return _isLit; } 
    public void SetLit(bool b)
    {
        if (_isLit != b)
        {
            _isLit = b;

            SetAnimatorBool(IsLitBool, b);
            //TODO: events?
        }
    }

    public bool IsNext() { return _isNextBeacon; }
    public void SetIsNext(bool b)
    {
        if (_isNextBeacon != b)
        {
            _isNextBeacon = b;
            //TODO: events?
        }
    }

    public int BeaconIdx() { return _beaconIdx; }
    public void SetBeaconIdx(int idx) { _beaconIdx = idx; }

	void Start () 
    {
        _animator = gameObject.GetComponent<Animator>();

        SetIsNext(false);
        SetLit(false);
	}

    void SetAnimatorBool(string boolName, bool val)
    {
        if (_animator != null)
            _animator.SetBool(boolName, val);
    }

    bool IsHiddenInWorld()
    {
        Nights2TorchPlayer.PortalState curPortalState = Nights2TorchPlayer.Instance.GetPortalState();
        Nights2Mgr.Nights2State curState = Nights2Mgr.Instance.GetState();
        bool showIt = (IsLit() && (curState == Nights2Mgr.Nights2State.BeaconLit)) || (_isNextBeacon && (curPortalState == Nights2TorchPlayer.PortalState.ThroughExitPortal));
        return !showIt;
    }

    bool ShouldDuckAudio()
    {
        return IsHiddenInWorld();
    }

	void Update () 
    {
        Nights2Mgr.Nights2State curState = Nights2Mgr.Instance.GetState();

        SetAnimatorBool(IsHiddenBool, IsHiddenInWorld());
        SetAnimatorBool(IsNextBool, _isNextBeacon && IsInSeekingState());
        SetAnimatorBool(PlayerCloseBool, _playerIsNear);
        if (_playerIsNear && (curState == Nights2Mgr.Nights2State.SeekingBeacon) && !Nights2TorchPlayer.Instance.IsPortalShowing())
            Nights2Mgr.Instance.SetState(Nights2Mgr.Nights2State.NearBeacon);
	}

    bool IsInSeekingState()
    {
        return (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.SeekingBeacon) ||
                             (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.NearBeacon);
    }

    void OnTriggerEnter(Collider other)
    {
        if (_isLit)
            return;

        //see if the torch is lighting us
        if ((other != null) && other.GetComponent<Nights2Torch>() != null)
        {
            //transision to seeking beacon state when torch is lit by shamash
            if (IsNext() && IsInSeekingState())
            {
               TriggerTorchLitBeacon();
            }
        }
    }

    public void TriggerTorchLitBeacon()
    {
       Debug.Assert(IsNext() && IsInSeekingState());

       Debug.Log("TORCH LIT BEACON!!");

       SetIsNext(false);
       SetLit(true);
       Nights2Mgr.Instance.SetState(Nights2Mgr.Nights2State.BeaconLit);
    }


    //Nights2NearShamash will call this when the player is close
    public void NotifyPlayerNearby()
    {
        if (!_isNextBeacon)
            return;

        Debug.Log("PLAYER NEAR NEXT BEACON!");

        _playerIsNear = true;
    }
    public void NotifyPlayerNotNearby()
    {
        _playerIsNear = false;
        //Debug.Log("PLAYER EXIT NEAR NEXT BEACON!");
    }
}
