using UnityEngine;
using System.Collections;

public class Nights2Shamash : MonoBehaviour 
{
    public string ShamashOnBool = "on";

    public string PlayerCloseBool = "player_close";

    public string FlameExtinguishedBool = "flame_extinguished";

    private Animator _animator = null;
    private bool _playerIsClose = false;

    public static Nights2Shamash Instance { get; private set; }

   void Awake()
   {
      Instance = this;
   }

	void Start () 
    {
        _animator = gameObject.GetComponent<Animator>();
        _playerIsClose = false;
        SetAnimatorBool(PlayerCloseBool, false);
        SetAnimatorBool(ShamashOnBool, false);
        SetAnimatorBool(FlameExtinguishedBool, false);
	}

    bool ShamashIsOn()
    {
        return (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.SeekingShamash) ||
            (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.NearShamash) ||
            (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.FlameExtinguished);
    }
	

	void Update () 
    {

        SetAnimatorBool(ShamashOnBool, ShamashIsOn());
        SetAnimatorBool(PlayerCloseBool, _playerIsClose && (Nights2Mgr.Instance.GetState() != Nights2Mgr.Nights2State.FlameExtinguished));
        SetAnimatorBool(FlameExtinguishedBool, (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.FlameExtinguished));
    }

    void SetAnimatorBool(string boolName, bool val)
    {
        if (_animator != null)
            _animator.SetBool(boolName, val);
    }

    void OnTriggerEnter(Collider other)
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
    }

    //Nights2NearShamash will call this when the player is close
    public void NotifyPlayerNearby()
    {
        Debug.Log("PLAYER NEAR!");

        _playerIsClose = true;
        if (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.SeekingShamash)
            Nights2Mgr.Instance.SetState(Nights2Mgr.Nights2State.NearShamash);
    }
    public void NotifyPlayerNotNearby()
    {
        _playerIsClose = false;
        Debug.Log("PLAYER EXIT NEAR!");
    }
}
