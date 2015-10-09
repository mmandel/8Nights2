using UnityEngine;
using System.Collections;

public class Nights2Shamash : MonoBehaviour 
{
    public string ShamashOnBool = "on";

    public string PlayerCloseBool = "player_close";

    private Animator _animator = null;

	void Start () 
    {
        _animator = gameObject.GetComponent<Animator>();
        SetAnimatorBool(PlayerCloseBool, false);
        SetAnimatorBool(ShamashOnBool, false);
	}
	

	void Update () 
    {
        if ((Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.SeekingShamash) ||
            (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.NearShamash))
        {
            SetAnimatorBool(ShamashOnBool, true);
        }
        else
        {
            SetAnimatorBool(ShamashOnBool, false);
        }
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
            if ((Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.SeekingShamash) ||
                (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.NearShamash))
            {
                Nights2Mgr.Instance.SetState(Nights2Mgr.Nights2State.SeekingBeacon);
            }
        }
    }

    //Nights2NearShamash will call this when the player is close
    public void NotifyPlayerNearby()
    {
        Debug.Log("PLAYER NEAR!");

        SetAnimatorBool(PlayerCloseBool, true);
        if (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.SeekingShamash)
            Nights2Mgr.Instance.SetState(Nights2Mgr.Nights2State.NearShamash);
    }
    public void NotifyPlayerNotNearby()
    {
        SetAnimatorBool(PlayerCloseBool, false);
        Debug.Log("PLAYER EXIT NEAR!");
    }
}
