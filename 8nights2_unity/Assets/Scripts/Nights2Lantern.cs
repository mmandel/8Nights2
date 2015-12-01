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

    void OnEnable()
    {
       //Debug.Log("Lantern enabled");
    }

    void OnDisable()
    {
       //Debug.Log("Lantern disabled");
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
           Transform desiredParent = ((Nights2CamMgr.Instance.GetLanternParent() != null) && Nights2CamMgr.Instance.GetLanternParent().gameObject.activeInHierarchy) ? Nights2CamMgr.Instance.GetLanternParent() : null;
           if (transform.parent != desiredParent)
            {
                transform.parent = desiredParent;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
        }	
	}
}
