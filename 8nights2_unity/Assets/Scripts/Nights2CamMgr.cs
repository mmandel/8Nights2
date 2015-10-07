//
//  Singleton to access camera + tracked controller info
//

using UnityEngine;
using System.Collections;

public class Nights2CamMgr : MonoBehaviour 
{
    public SteamVR_ControllerManager SteamCtrlMgr;
    public bool StartTorchOnRightCtrl = true; //is torch attached to "right" controller at start?

    [ScriptButton("Swap Torch/Lantern!", "OnSwapPressed")]
    public bool DummySwapCtrls = false;

    private bool _torchOnRightCtrl = true;
    private Transform _rightCtrlTrans = null;
    private Transform _leftCtrlTrans = null;

    public static Nights2CamMgr Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

	void Start () 
    {
        _torchOnRightCtrl = StartTorchOnRightCtrl;

        if (SteamCtrlMgr != null)
        {
            if (SteamCtrlMgr.right != null)
                _rightCtrlTrans = SteamCtrlMgr.right.transform;
            if (SteamCtrlMgr.left != null)
                _leftCtrlTrans = SteamCtrlMgr.left.transform;
        }
	}

    public Transform GetTorchParent()
    {
        return _torchOnRightCtrl ? _rightCtrlTrans : _leftCtrlTrans;
    }

    public Transform GetLanternParent()
    {
        return _torchOnRightCtrl ? _leftCtrlTrans : _rightCtrlTrans;
    }

    void SwapControllers()
    {
        _torchOnRightCtrl = !_torchOnRightCtrl;
    }

    public void OnSwapPressed(string propPath)
    {
        SwapControllers();
    }
	
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.C))
            SwapControllers();
	}
}
