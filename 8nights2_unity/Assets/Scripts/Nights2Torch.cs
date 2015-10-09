//
//  Controls the torch
//

using UnityEngine;
using System.Collections;

public class Nights2Torch : MonoBehaviour 
{
    public string TorchOnBool = "on";


    private Animator _animator = null;

	void Start () 
    {
        _animator = gameObject.GetComponent<Animator>();
        SetAnimatorBool(TorchOnBool, false);
	}

    void SetAnimatorBool(string boolName, bool val)
    {
        if (_animator != null)
            _animator.SetBool(boolName, val);
    }
	
	void Update () 
    {

        if (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.SeekingBeacon)
            SetAnimatorBool(TorchOnBool, true);
        else
            SetAnimatorBool(TorchOnBool, false);

	    //make sure we are parented to the right thing
        if (Nights2CamMgr.Instance != null)
        {
            if (transform.parent != Nights2CamMgr.Instance.GetTorchParent())
            {
                transform.parent = Nights2CamMgr.Instance.GetTorchParent();
                if (!transform.parent.gameObject.activeInHierarchy)
                    transform.parent = null;
                transform.localPosition = Vector3.zero;
            }
        }
	}
}
