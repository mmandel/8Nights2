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
    private Transform _headTrans = null;

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

            _headTrans = SteamCtrlMgr.transform.FindChild("Camera (head)");
        }
	}

    public Transform GetTorchParent()
    {
        return _torchOnRightCtrl ? _rightCtrlTrans : _leftCtrlTrans;
    }

    public SteamVR_Controller.Device GetTorchDevice()
    {
        Transform torchTrans = GetTorchParent();
        if (torchTrans != null)
        {
            SteamVR_Controller.Device device = SteamVR_Controller.Input((int)torchTrans.gameObject.GetComponent<SteamVR_TrackedObject>().index);
            return device;
        }
        return null;
    }

    public Transform GetLanternParent()
    {
        return _torchOnRightCtrl ? _leftCtrlTrans : _rightCtrlTrans;
    }


    public SteamVR_Controller.Device GetLanternDevice()
    {
        Transform lanternTrans = GetLanternParent();
        if (lanternTrans != null)
        {
            SteamVR_Controller.Device device = SteamVR_Controller.Input((int)lanternTrans.gameObject.GetComponent<SteamVR_TrackedObject>().index);
            return device;
        }
        return null;
    }

    public Transform GetHeadTrans()
    {
        return _headTrans;
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
