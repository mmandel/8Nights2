//
//  Communicate with Monoprice's Par64 RBG stage light using LightJams
//
//  DMX spec for the device is here:
//  http://downloads.monoprice.com/files/manuals/612720_Manual_150414.pdf
//

using UnityEngine;
using System.Collections;

public class Par64Value : MonoBehaviour 
{
    [Range(0, 511)]
    public int StartChannel = 0;

    [Space(10)]

    [Range(0.0f, 1.0f)]
    public float MasterDimmer = 1.0f;

    [Space(10)]

    public SpecialMode Mode = SpecialMode.None;

    [Space(10)]

    //RGB val of the light - only applies if SpecialMode = None
    [Tooltip("Target color of the light, only applies if SpecialMode is None")]
    public Color LightColor = Color.blue;

    [Space(10)]

    //strobe speed - only applies if SpecialMode != None
    [Tooltip("Strobe speed, only applies if SpecialMode is NOT None")]
    [Range(0.0f, 1.0f)]
    public float StrobeSpeed = 0.0f;

    //special mode param -  only applies if SpecialMode != None
    [Tooltip("This adjusts something about the special mode that's selected.  Doesnt apply if special mode is none")]
    [Range(0.0f, 1.0f)]
    public float SpecialModeParam = 0.0f;


    public enum SpecialMode
    {
        None,
        DarkToBright,
        BrightToDark,
        DarkToBrightToDark,
        RGBGradient,
        ThreeColorCycling,
        SevenColorCycling,
        SoundActivated
    }


	void Start () 
    {
	    //hmm, nothing to do yet
	}

    void OnDisable()
    {
        //turn off when we exit play mode
        if ((LightJamsMgr.Instance != null))
            LightJamsMgr.Instance.SendToLightJams(StartChannel, 0.0f);
    }

    //utility function for basic Par64 color setting
    public static void SetPar64Color(int startChannel, float masterDim, Color c)
    {
        //master dimmer
        LightJamsMgr.Instance.SendToLightJams(startChannel, masterDim);

        //clear special mode
        LightJamsMgr.Instance.SendToLightJams(startChannel + 6, 0.0f);

        //Color
        LightJamsMgr.Instance.SendToLightJams(startChannel + 1, c.r);
        LightJamsMgr.Instance.SendToLightJams(startChannel + 2, c.g);
        LightJamsMgr.Instance.SendToLightJams(startChannel + 3, c.b);

        //"Dimming", do we need this?
        LightJamsMgr.Instance.SendToLightJams(startChannel + 5, 0.0f);
    }
	
	void Update () 
    {
        if (LightJamsMgr.Instance == null)
            return;

        if (Mode == SpecialMode.None)
        {
            SetPar64Color(StartChannel, MasterDimmer, LightColor);
        }
        else
        {
            //master dimmer
            LightJamsMgr.Instance.SendToLightJams(StartChannel, MasterDimmer);

            //figure out which value range to use when setting special param channel
            int modeParamMin = 0;
            int modeParamMax = 31;
            switch (Mode)
            {
                case SpecialMode.DarkToBright:
                    modeParamMin = 32;
                    modeParamMax = 63;
                    break;
                case SpecialMode.BrightToDark:
                    modeParamMin = 64;
                    modeParamMax = 95;
                    break;
                case SpecialMode.DarkToBrightToDark:
                    modeParamMin = 96;
                    modeParamMax = 127;
                    break;
                case SpecialMode.RGBGradient:
                    modeParamMin = 128;
                    modeParamMax = 159;
                    break;
                case SpecialMode.ThreeColorCycling:
                    modeParamMin = 160;
                    modeParamMax = 191;
                    break;
                case SpecialMode.SevenColorCycling:
                    modeParamMin = 192;
                    modeParamMax = 223;
                    break;
                case SpecialMode.SoundActivated:
                    modeParamMin = 224;
                    modeParamMax = 255;
                    break;
            }

            //get actual mode value (0 to 255)
            float modeParamVal = Mathf.Lerp(modeParamMin, modeParamMax, SpecialModeParam);
            //remap to (0, 1)
            modeParamVal = Mathf.InverseLerp(0.0f, 255.0f, modeParamVal);

            //set mode param
            LightJamsMgr.Instance.SendToLightJams(StartChannel + 6, modeParamVal);

            //strobe value (15 to 255)
            float strobeVal = Mathf.Lerp(15.0f, 255.0f, StrobeSpeed);
            //remap to (0, 1)
            strobeVal = Mathf.InverseLerp(0.0f, 255.0f, strobeVal);

            //set strobe speed
            LightJamsMgr.Instance.SendToLightJams(StartChannel + 5, strobeVal);
        }
	}
}
