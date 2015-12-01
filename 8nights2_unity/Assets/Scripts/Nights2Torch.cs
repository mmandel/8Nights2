//
//  Controls the torch
//

using UnityEngine;
using System.Collections;

public class Nights2Torch : MonoBehaviour 
{
    public string TorchOnBool = "on";
    public string TorchHasMagicBool = "has_magic";
    [Space(10)]
    public Renderer TorchColorRend;
    public string TorchColorProp = "_Color";


    private Animator _animator = null;
    private Color _offFlameColor = Color.red;

	void Start () 
    {
        _animator = gameObject.GetComponent<Animator>();
        SetAnimatorBool(TorchOnBool, false);

        //save off default color to shove on torch when its off
        if (TorchColorRend != null)
            _offFlameColor = TorchColorRend.material.GetColor(TorchColorProp);
	}

    void SetAnimatorBool(string boolName, bool val)
    {
        if (_animator != null)
            _animator.SetBool(boolName, val);
    }

    bool IsFlameOn()
    {
        return (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.SeekingBeacon) || (Nights2Mgr.Instance.GetState() == Nights2Mgr.Nights2State.NearBeacon);
    }
	
	void Update () 
    {
        //update animator with status of torch
        SetAnimatorBool(TorchOnBool, IsFlameOn());

        bool shouldHaveMagic = Nights2Mgr.Instance.TorchHasMagic() && IsFlameOn();
        SetAnimatorBool(TorchHasMagicBool, shouldHaveMagic);

        if (TorchColorRend != null)
        {
            Color nextCandleColor = (Nights2Mgr.Instance.NextBeacon() != null) ? Nights2Mgr.Instance.NextBeacon().CandleColor : _offFlameColor;
            TorchColorRend.material.SetColor(TorchColorProp, IsFlameOn() ? nextCandleColor : _offFlameColor);
        }

	    //make sure we are parented to the right thing
        if (Nights2CamMgr.Instance != null)
        {
           Transform desiredParent = ((Nights2CamMgr.Instance.GetTorchParent() != null) && Nights2CamMgr.Instance.GetTorchParent().gameObject.activeInHierarchy) ? Nights2CamMgr.Instance.GetTorchParent() : null;
           if (transform.parent != desiredParent)
            {
                transform.parent = desiredParent;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
        }
	}
}
