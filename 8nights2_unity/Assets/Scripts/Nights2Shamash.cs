using UnityEngine;
using System.Collections;

public class Nights2Shamash : MonoBehaviour 
{
    public Transform TunnelPivot; //turn this towards the player to make the tunnel aim at him/her
    public float MinTimeBeforeClose = 2.0f; //wait this many seconds before you allow the player to be close, this is to keep the tunnel from going away too fast when coming from a close vantage point
    public float TunnelPivotSpeed = 30.0f;

    [Space(10)]

    public Renderer ShamashColorRend;
    public string   ShamashColorProp = "_Color";
    public Par64Value HardwareValue;

    [Space(10)]

    public FMOD_StudioEventEmitter ShamashRevealSound;
    public FMOD_StudioEventEmitter TorchLitSound;

    [Space(10)]

    //torch icon stuff
    public Transform TorchIconSpot;
    public GameObject TorchIconPrefab;

    [Space(10)]

    public string ShamashOnBool = "on";

    public string ShamashHiddenBool = "hidden";

    public string PlayerCloseBool = "player_close";

    public string FlameExtinguishedBool = "flame_extinguished";

    private Animator _animator = null;
    private bool _playerIsClose = false;

    private float _closeTimerLeft = -1.0f;

    public static Nights2Shamash Instance { get; private set; }

    private Quaternion _defaultTunnelRot = Quaternion.identity;
    private Nights2Spot _closestSpot = null;

    private Nights2Icon _torchIcon;

    void Awake()
    {
        Instance = this;
    }

	void Start () 
    {
        if (Nights2SpotMgr.Instance != null)
            _closestSpot = Nights2SpotMgr.Instance.FindClosestSpotTo(transform.position);

        _animator = gameObject.GetComponent<Animator>();
        _playerIsClose = false;
        SetAnimatorBool(PlayerCloseBool, false);
        SetAnimatorBool(ShamashOnBool, false);
        SetAnimatorBool(FlameExtinguishedBool, false);

        if (TunnelPivot != null)
            _defaultTunnelRot = TunnelPivot.rotation;

        //subscribe to state changed events
        if (Nights2Mgr.Instance != null)
            Nights2Mgr.Instance.OnStateChanged += OnNights2StateChanged;
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

    public Nights2Spot ClosestSpot()
    {
        return _closestSpot;
    }

    Vector3 GetTunnelTarget()
    {
        Vector3 playerPos = Nights2CamMgr.Instance.GetHeadTrans().position;
        playerPos.y = TunnelPivot.position.y;
        return playerPos;
    }

    void OnNights2StateChanged(object sender, Nights2Mgr.StateChangedEventArgs e)
    {
        if ((e.NewState == Nights2Mgr.Nights2State.SeekingShamash) || (e.NewState == Nights2Mgr.Nights2State.FlameExtinguished))
        {
            //light up shamash spot
            if(Nights2SpotMgr.Instance != null)
                Nights2SpotMgr.Instance.MakeSpotActive(_closestSpot);

            _closeTimerLeft = MinTimeBeforeClose;

            //aim tunnel at player
            if (TunnelPivot != null)
            {
                Vector3 playerPos = GetTunnelTarget();
                TunnelPivot.LookAt(playerPos, Vector3.up);
                TunnelPivot.Rotate(Vector3.up, 180.0f); //oops, z axis in data is opposite of what it should be, so correct by 180 degrees
            }
        }
    }

    bool ShamashIsOn()
    {
       return _playerIsClose &&  //we only show shamash ones lantern reveals it
            ((Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.SeekingShamash) ||
            (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.NearShamash) ||
            (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.FlameExtinguished));
    }


    void Update()
    {

       Nights2Mgr.Nights2State curNightsState = Nights2Mgr.Instance.GetState();

       if (_closeTimerLeft >= 0)
          _closeTimerLeft -= Time.deltaTime;
       bool cantBeClose = (_closeTimerLeft >= 0.0f);

       //set our color to match that of the active candle
       if ((ShamashColorRend != null) && (Nights2Mgr.Instance.NextBeacon() != null))
       {
           ShamashColorRend.material.SetColor(ShamashColorProp, Nights2Mgr.Instance.NextBeacon().CandleColor);
           //also set this on the physical light
           if (HardwareValue != null)
               HardwareValue.LightColor = Nights2Mgr.Instance.NextBeacon().CandleColor;
       }

       SetAnimatorBool(ShamashHiddenBool, (curNightsState != Nights2Mgr.Nights2State.SeekingShamash) && (curNightsState != Nights2Mgr.Nights2State.NearShamash));
       SetAnimatorBool(ShamashOnBool, ShamashIsOn());
       SetAnimatorBool(PlayerCloseBool, _playerIsClose && !cantBeClose);
       SetAnimatorBool(FlameExtinguishedBool, (curNightsState == Nights2Mgr.Nights2State.FlameExtinguished));

       //turn tunnel slowly towards player so they never clip into it
       if ((TunnelPivot != null) && ((curNightsState == Nights2Mgr.Nights2State.SeekingShamash) || (curNightsState == Nights2Mgr.Nights2State.FlameExtinguished)))
       {
          Quaternion targetRotation = Quaternion.LookRotation(GetTunnelTarget() - TunnelPivot.position);
          targetRotation *= Quaternion.Euler(Vector3.up * 180.0f); //to fix error in data setup, where lookat will be 180 degrees off

          TunnelPivot.rotation = Quaternion.Slerp(TunnelPivot.rotation, targetRotation, TunnelPivotSpeed * Time.deltaTime);
       }

       if ((Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.NearShamash) && (_torchIcon != null) && _torchIcon.RequiredPropIsNear())
       {
          Nights2Mgr.Instance.SetState(Nights2Mgr.Nights2State.SeekingBeacon);
          //DestroyIcon(); //(happens in below...)
       }

       //to make sure icon is created/destroyed when cheating
       if ((Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.NearShamash) && (_torchIcon == null))
       {
          SpawnIcon();
          if (ShamashRevealSound != null)
             ShamashRevealSound.Play();
       }
       else if ((Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.SeekingBeacon) && (_torchIcon != null))
       {
          DestroyIcon();
          if (TorchLitSound != null)
             TorchLitSound.Play();
       }
    }

    void SetAnimatorBool(string boolName, bool val)
    {
        if (_animator != null)
            _animator.SetBool(boolName, val);
    }

   //MMANDEL: we no have the icon tell us when the torch is near (see Update())
    /*void OnTriggerEnter(Collider other)
    {
        //see if the torch is colliding with us
        if ((other != null) && other.GetComponent<Nights2Torch>() != null)
        {
            Debug.Log("TORCH ENTER!!");

            //transision to seeking beacon state when torch is lit by shamash
            if (ShamashIsOn())
            {
                Nights2Mgr.Instance.SetState(Nights2Mgr.Nights2State.SeekingBeacon);
            }
        }
    }*/

    //Nights2NearShamash will call this when the player is close
    public void NotifyPlayerNearby()
    {
        Debug.Log("PLAYER NEAR!");

        _playerIsClose = true;
        if (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.SeekingShamash)
        {
           //SpawnIcon(); (happens in Update())
           Nights2Mgr.Instance.SetState(Nights2Mgr.Nights2State.NearShamash);
        }
    }
    public void NotifyPlayerNotNearby()
    {
        _playerIsClose = false;
        Debug.Log("PLAYER EXIT NEAR!");
    }
}
