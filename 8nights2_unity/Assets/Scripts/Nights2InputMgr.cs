//
//  Provide convenient access to input data on the vive controllers
//

using UnityEngine;
using System.Collections;

public class Nights2InputMgr : MonoBehaviour 
{

    enum WhichController
    {
        Torch = 0,
        Lantern = 1,
    }

    //wraps controller device and exposes functions
    public class InputInfo
    {
        public bool HasDevice() { return (device != null) && device.connected; }
        public bool HasTracking() { return HasDevice() && device.hasTracking; }

        //trigger down this frame?
        public bool GetTriggerDown() { return HasDevice() ? device.GetHairTriggerDown() : false; }
        //state of the trigger (true = pressed)
        public bool GetTriggerState() { return HasDevice() ? device.GetHairTrigger() : false; }

        //touchpad clicked this frame?
        public bool GetTouchpadDown() { return HasDevice() ? device.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad) : false; }
        //state of touchpad click (true = pressed)
        public bool GetTouchpadState() { return HasDevice() ? device.GetPress(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad) : false; }

        //was the red application button pressed this frame?
        public bool GetRedButtonDown() { return HasDevice() ? device.GetPressDown(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu) : false; }
        //state of red application button (true = pressed)
        public bool GetRedButtonState() { return HasDevice() ? device.GetPress(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu) : false; }


        public SteamVR_Controller.Device device = null;
    }

    public static Nights2InputMgr Instance { get; private set; }

    InputInfo[] _devices = new InputInfo[2];

    public InputInfo TorchInfo() { return _devices[(int)WhichController.Torch]; }
    public InputInfo LanternInfo() { return _devices[(int)WhichController.Lantern]; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _devices[(int)WhichController.Torch] = new InputInfo();
        _devices[(int)WhichController.Lantern] = new InputInfo();
    }
	

	void Update () 
    {
        //keep track of which device is which
        SteamVR_Controller.Device torch = Nights2CamMgr.Instance.GetTorchDevice();
        _devices[(int)WhichController.Torch].device = torch;
        SteamVR_Controller.Device lantern = Nights2CamMgr.Instance.GetLanternDevice();
        _devices[(int)WhichController.Lantern].device = lantern;
	}
}
