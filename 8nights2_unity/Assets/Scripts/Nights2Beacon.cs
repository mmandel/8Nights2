//
//  A single "candle" in the installation
//  A physical spotlight and audio stem will be associated with one of these
//

using UnityEngine;
using System.Collections;

public class Nights2Beacon : MonoBehaviour 
{
    [Header("Color")]
    public Color CandleColor = Color.cyan;
    public ColorEntry[] DriveColors = new ColorEntry[0];

    [System.Serializable]
    public class ColorEntry
    {
       public Renderer Mesh;
       public string ColorPropName = "_Color";
    }

    [Header("Animator")]

    public string IsLitBool = "on";
    public string IsHiddenBool = "hidden";
    public string IsNextBool = "is_next";
    public string PlayerCloseBool = "is_close";


    [Header("Torch Icon")]

    //torch icon stuff
    public Transform TorchIconSpot;
    public GameObject TorchIconPrefab;

    [Header("Audio")]

    public FMOD_StudioEventEmitter CandleRevealSound;

    private bool _isNextBeacon = false;
    private bool _isLit = false;
    private int _beaconIdx = -1;
    private Animator _animator = null;
    private bool _playerIsNear = false;
    private Nights2Spot _closestSpot = null;

    private FMOD_StudioEventEmitter _narrationSound = null;
    private float _narrationStartTime = -1.0f;
    private float _narrationDuration = 20.0f;

    private Nights2Icon _torchIcon;

    public bool IsNarrationPlaying() { return (_narrationStartTime >= 0.0f); }

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

    public Nights2Spot ClosestSpot()
    {
        return _closestSpot;
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
        //print out color of each candle along with where in the progression it lives
        /*int progreshIdx = -1;
        for(int i = 0 ; i < Nights2Mgr.Instance.CandlePathOrderLength() ; i++)
        {
           if (Nights2Mgr.Instance.GetPath(i).LeadsToBeacon == this)
           {
              progreshIdx = i;
              break;
           }
        }
        Debug.Log("color " +  progreshIdx + ": ( " + CandleColor.r * 255.0f + ", " + CandleColor.g * 255.0f + ", " + CandleColor.b * 255.0f + " ) " + gameObject.name); */

        if (Nights2SpotMgr.Instance != null)
            _closestSpot = Nights2SpotMgr.Instance.FindClosestSpotTo(transform.position);

        _animator = gameObject.GetComponent<Animator>();

        EightNightsMgr.GroupID group = Nights2AudioMgr.Instance.GetGroupForBeacon(this);
        _narrationSound = Nights2AudioMgr.Instance.GetNarrationForGroup(group);
        _narrationDuration = Nights2AudioMgr.Instance.GetNarrationTimeForGroup(group);

        SetIsNext(false);
        SetLit(false);

        //set our color
        for (int i = 0; i < DriveColors.Length; i++)
        {
           Renderer r = DriveColors[i].Mesh;
           string propName = DriveColors[i].ColorPropName;
           if ((r != null) && (propName.Length > 0))
              r.material.SetColor(propName, CandleColor);
        }

        //subscribe to state changed events
        if (Nights2Mgr.Instance != null)
           Nights2Mgr.Instance.OnTeleported += OnTeleported;
	}

   void OnTeleported(object sender, Nights2Mgr.TeleportedEventArgs e)
   {
      //cancel out of narration (and stop audio) if the player
      //teleports before its done
      if (IsNarrationPlaying() && (e.NewWorld != Nights2Mgr.WorldID.RoomWorld))
      {
         _narrationSound.Stop();
         _narrationStartTime = -1; //done tracking narration
      }
   }

    void SetAnimatorBool(string boolName, bool val)
    {
        if (_animator != null)
            _animator.SetBool(boolName, val);
    }

    void SpawnIcon()
    {

        //spawn lantern icon
        if ((TorchIconPrefab != null) && (TorchIconSpot != null))
        {
            GameObject spawned = Instantiate(TorchIconPrefab) as GameObject;
            if (spawned != null)
            {
                _torchIcon = spawned.GetComponent<Nights2Icon>();
                spawned.transform.parent = TorchIconSpot;
                spawned.transform.localPosition = Vector3.zero;
                spawned.transform.localRotation = Quaternion.identity;
            }
        }
    }

    void DestroyIcon()
    {
        if (_torchIcon != null)
        {
            _torchIcon.Destroy();
            _torchIcon = null;
        }
    }


    bool IsHiddenInWorld()
    {
        Nights2TorchPlayer.PortalState curPortalState = Nights2TorchPlayer.Instance.GetPortalState();
        Nights2Mgr.Nights2State curState = Nights2Mgr.Instance.GetState();
        bool showIt = (IsLit() && ((curState == Nights2Mgr.Nights2State.BeaconLit) || (curState == Nights2Mgr.Nights2State.SeekingShamash))) || (_isNextBeacon && (curPortalState == Nights2TorchPlayer.PortalState.ThroughExitPortal));
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
        //bool showNearState = _playerIsNear;
        //once we show the beacon in near state, just leave it there till the task is completed
        bool showNearState = IsNext() && (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.NearBeacon);
        SetAnimatorBool(PlayerCloseBool, showNearState);

        if (_playerIsNear && (curState == Nights2Mgr.Nights2State.SeekingBeacon) && !Nights2TorchPlayer.Instance.IsPortalShowing())
            Nights2Mgr.Instance.SetState(Nights2Mgr.Nights2State.NearBeacon);

        //light beacon if torch has been placed in icon
        if ((Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.NearBeacon) && (_torchIcon != null) && _torchIcon.RequiredPropIsNear())
        {
            TriggerTorchLitBeacon();
        }

        //to make sure icon is created/destroyed when cheating
        if (IsNext() && !IsHiddenInWorld() && (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.NearBeacon) && (_torchIcon == null))
        {
           SpawnIcon();
           if (CandleRevealSound != null)
              CandleRevealSound.Play();

           //ping from candle spot when lantern is near
           if (Nights2SpotMgr.Instance != null)
              Nights2SpotMgr.Instance.TriggerSpotFX(Nights2SpotMgr.LightAction.Ping, 1.5f, _closestSpot, .5f);
        }
        else if ((Nights2Mgr.Instance.GetState() != Nights2Mgr.Nights2State.NearBeacon) && (_torchIcon != null))
           DestroyIcon();

        //if showing icon, light up spot
        if ((_torchIcon != null) && (_closestSpot != null))
        {
            Nights2SpotMgr.Instance.MakeSpotActive(_closestSpot);
        }

        if (IsNarrationPlaying())
        {
           //end narration state after its time
           float elapsedNarration = Time.time - _narrationStartTime;
           if (elapsedNarration >= _narrationDuration) //done?
           {
              _narrationStartTime = -1;
           }
        }

	}

    bool IsInSeekingState()
    {
        return (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.SeekingBeacon) ||
                             (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.NearBeacon);
    }

    //See Update(), lighting is handled by torch icon now...
    /*void OnTriggerEnter(Collider other)
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
    }*/

    public void TriggerTorchLitBeacon()
    {
       Debug.Assert(IsNext() && IsInSeekingState());

       Debug.Log("TORCH LIT BEACON!!");

       //turn off spot
       if(Nights2SpotMgr.Instance.ActiveSpot() == _closestSpot)
          Nights2SpotMgr.Instance.MakeSpotActive(null);
       
       if (_narrationSound != null)
       {
          _narrationSound.Play();
          _narrationStartTime = Time.time;
       }

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
