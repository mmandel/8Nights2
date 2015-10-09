//
//  Controls the lantern
//


using UnityEngine;
using System.Collections;

public class Nights2Lantern : MonoBehaviour 
{
    public string LanternOnBool = "on";


    private Animator _animator = null;

    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        SetAnimatorBool(LanternOnBool, false);
    }

    void SetAnimatorBool(string boolName, bool val)
    {
        if (_animator != null)
            _animator.SetBool(boolName, val);
    }
	
	
	void Update () 
    {
        if ((Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.SeekingBeacon) ||
            (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.FlameExtinguished) ||
            (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.BeaconLit))
        {
            SetAnimatorBool(LanternOnBool, true);
        }
        else
            SetAnimatorBool(LanternOnBool, false);

        //make sure we are parented to the right thing
        if (Nights2CamMgr.Instance != null)
        {
            if (transform.parent != Nights2CamMgr.Instance.GetLanternParent())
            {
                transform.parent = Nights2CamMgr.Instance.GetLanternParent();
                if (!transform.parent.gameObject.activeInHierarchy)
                    transform.parent = null;
                transform.localPosition = Vector3.zero;
            }
        }	
	}
}
