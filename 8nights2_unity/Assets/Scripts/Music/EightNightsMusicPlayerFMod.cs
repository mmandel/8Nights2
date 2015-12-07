//----------------------------------------------
//
//  This script plays a bunch of stems in sync with each other
//  It is essentially a dupe of last year's version that played using Koreographer,
//  but instead uses fmod to play the audio and fmod params to control volume
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;


//8nights specific stuff for each audio layer
[System.Serializable]
public class EightNightsLayerDetailsFMod
{
    public EightNightsMgr.GroupID Group;
    public string FModParamName; //param that controls volume of this group
    public FMOD_StudioEventEmitter NarrationEvent; //spoken text for this group
    public float NarrationTime; //how long the narration lasts
    [Range(0, 1)]
    public float MixVolume = 1.0f;
    public Nights2StemSubstitute[] Substitutions = new Nights2StemSubstitute[0];
    
    //set volume of param, accounting for substitutions that may need to happen
    public void SetVolume(float vol, FMOD_StudioEventEmitter ev)
    {
       //if we have substitutions, just silence all at first so we don't leave any tracks on accidently
       if (Substitutions.Length > 0)
          SilenceAll(ev);

       FMOD.Studio.ParameterInstance param = ev.getParameter(GetCurParam());
       if (param != null)
       {
          param.setValue(Mathf.Lerp(0.0f, MixVolume, vol));
       }
    }
    
    //get volume of param, accounting for substitutions
    public float GetVolume(FMOD_StudioEventEmitter ev)
    {
       float paramVal = 0.0f;
       FMOD.Studio.ParameterInstance param = ev.getParameter(GetCurParam());
       if (param != null)
          param.getValue(out paramVal);
       return paramVal;
    }

    string GetCurParam()
    {
       if(Substitutions.Length == 0)
          return FModParamName;

       //see if we have any substitutions based on # of beacons lit
       int sIdx = CurSubstitutionIdx();
       if (sIdx >= 0)
          return Substitutions[sIdx].SubstituteParamName;

       //use default param if none of the substitutions matched above
       return FModParamName;
    }

    //returns -1 if no substitution, otherwise the index of the entry in the Substitutions list we should use right now
    public int CurSubstitutionIdx()
    {
       if (Substitutions.Length == 0)
          return -1;

       //see if we have any substitutions based on # of beacons lit
       int nb = (Nights2Mgr.Instance != null) ? Nights2Mgr.Instance.NumCandlesLit() : 0;
       for (int i = 0; i < Substitutions.Length; i++)
       {
          if ((nb >= Substitutions[i].NumBeaconsMin) && (nb <= Substitutions[i].NumBeaconsMax))
             return i;
       }

       return -1;
    }

    void SilenceAll(FMOD_StudioEventEmitter ev)
    {
       FMOD.Studio.ParameterInstance param = ev.getParameter(FModParamName);
       if (param != null)
          param.setValue(0.0f);

       for (int i = 0; i < Substitutions.Length; i++)
       {
          FMOD.Studio.ParameterInstance sParm = ev.getParameter(Substitutions[i].SubstituteParamName);
          if (sParm != null)
             sParm.setValue(0.0f);          
       }
    }
}

//param substitution that begins for a given layer after a give # of beacons are lit
[System.Serializable]
public class Nights2StemSubstitute
{
   public string SubstituteParamName; //use this param to control the group instead
   public int NumBeaconsMin = 4; //valid when this many beacons are active (min)
   public int NumBeaconsMax = 7; //valid when this many beacons are active (max)
};

public class EightNightsMusicPlayerFMod : MonoBehaviour
{

    [Tooltip("The fmod event that plays all the stems (a gameobject with the FMOD_StudioEventEmitter component on it)")]
    public FMOD_StudioEventEmitter FModEvent;

    public EightNightsLayerDetailsFMod[] EightNightsDetails;

    public string BackingLoopFModParam; //param that controls volume of the backing loop
    [Range(0.0f, 1.0f)]
    public float BackingLoopMixVolume = 1.0f;

    public string PeakLoopFModParam;  //param that controls volume of the peak loop
    [Range(0.0f, 1.0f)]
    public float PeakLoopMixVolume = 1.0f;

    bool _firstUpdate = true;


    public float GetElapsedSecs()
    {
        return (FModEvent != null) ? FModEvent.getPlaybackPos() * .001f : 0.0f;
    }
    
    //-1 if not substituting, otherwise the index of the substitution
    public int GetSubstitutionIdxForGroup(EightNightsMgr.GroupID group)
    {
        EightNightsLayerDetailsFMod layerDetails = GetLayerDetailsForGroup(group);
        if (layerDetails != null)
        {
           return layerDetails.CurSubstitutionIdx();
        }
        return -1;
    }

    public FMOD_StudioEventEmitter GetNarrationForGroup(EightNightsMgr.GroupID group)
    {
       EightNightsLayerDetailsFMod layerDetails = GetLayerDetailsForGroup(group);
       if (layerDetails != null)
       {
          return layerDetails.NarrationEvent;
       }
       return null;
    }

    public float GetNarrationTimeForGroup(EightNightsMgr.GroupID group)
    {
       EightNightsLayerDetailsFMod layerDetails = GetLayerDetailsForGroup(group);
       if (layerDetails != null)
       {
          return layerDetails.NarrationTime;
       }
       return 20.0f;
    }

    public float GetVolumeForGroup(EightNightsMgr.GroupID group)
    {
        EightNightsLayerDetailsFMod layerDetails = GetLayerDetailsForGroup(group);
        if (layerDetails != null)
        {
           float paramVal = layerDetails.GetVolume(FModEvent);
           return Mathf.InverseLerp(0.0f, layerDetails.MixVolume, paramVal);
        }
        return 0.0f;
    }

    public void SetVolumeForGroup(EightNightsMgr.GroupID group, float volume)
    {
        //look up which layer
        EightNightsLayerDetailsFMod layerDetails = GetLayerDetailsForGroup(group);

        if (layerDetails != null)
        {
           layerDetails.SetVolume(Mathf.Lerp(0.0f, layerDetails.MixVolume, volume), FModEvent);
        }
    }

    public EightNightsLayerDetailsFMod GetLayerDetailsForGroup(EightNightsMgr.GroupID group)
    {
        //look up which layer
        foreach (EightNightsLayerDetailsFMod d in EightNightsDetails)
        {
            if (d.Group == group)
                return d;
        }

        return null;
    }

    public void SetBackingLoopVolume(float volume)
    {
        if (BackingLoopFModParam.Length == 0)
            return;

        FMOD.Studio.ParameterInstance param = FModEvent.getParameter(BackingLoopFModParam);
        if (param != null)
        {
            param.setValue( Mathf.Lerp(0.0f, BackingLoopMixVolume, volume));
        }
    }

    public float GetBackingLoopVolume()
    {
        float paramVal = 0.0f;
        if (BackingLoopFModParam.Length == 0)
            return paramVal;
        FMOD.Studio.ParameterInstance param = FModEvent.getParameter(BackingLoopFModParam);
        if (param != null)
            param.getValue(out paramVal);

        return Mathf.InverseLerp(0.0f, BackingLoopMixVolume, paramVal);
    }

    public void SetPeakLoopVolume(float volume)
    {
        if (PeakLoopFModParam.Length == 0)
            return;
        FMOD.Studio.ParameterInstance param = FModEvent.getParameter(PeakLoopFModParam);
        if (param != null)
        {
            param.setValue(Mathf.Lerp(0.0f, PeakLoopMixVolume, volume));
        }
    }

    public float GetPeakLoopVolume()
    {
        float paramVal = 0.0f;
        if (PeakLoopFModParam.Length == 0)
            return 0.0f;     
        FMOD.Studio.ParameterInstance param = FModEvent.getParameter(PeakLoopFModParam);
        if (param != null)
            param.getValue(out paramVal);

        /*FMOD.DSP spectrum;
        IntPtr unmanagedData;
        uint unmanagedDataLen = 0;
        var result = spectrum.getParameterData((int)FMOD.DSP_FFT.SPECTRUMDATA, out unmanagedData, out unmanagedDataLen);
        FMOD.DSP_PARAMETER_FFT waveFormBuffer = (FMOD.DSP_PARAMETER_FFT)Marshal.PtrToStructure(unmanagedData, typeof(FMOD.DSP_PARAMETER_FFT));*/

        return Mathf.InverseLerp(0.0f, PeakLoopMixVolume, paramVal);
    }

    void Awake()
    {

    }

    void Start()
    {
        _firstUpdate = true;
    }

    void OnDisable()
    {

    }
   

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(3.0f);

        FModEvent.Play();
    }

    void Update()
    {
        if (_firstUpdate)
        {
            if ((FModEvent != null) && !FModEvent.startEventOnAwake)
                StartCoroutine(DelayedStart());
            _firstUpdate = false;
        }
    }

    #region Playback Control

    public void Play()
    {
        if (FModEvent == null)
            return;
        FModEvent.Play();
    }

    public void Restart()
    {
        if (FModEvent == null)
            return;
        FModEvent.Stop();
        FModEvent.Play();
    }


    public void Pause()
    {
        if (FModEvent == null)
            return;
        //TODO: how do we do this?
    }

    public void Continue()
    {
        if (FModEvent == null)
            return;
        //TODO: how do we do this?
    }

    public void Stop()
    {
        if (FModEvent == null)
            return;
        FModEvent.Stop();
    }

    #endregion
}
