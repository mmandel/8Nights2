//----------------------------------------------
//
//  This script plays a bunch of stems in sync with each other
//  It is essentially a dupe of last year's version that played using Koreographer,
//  but instead uses fmod to play the audio and fmod params to control volume
//
//----------------------------------------------

using UnityEngine;
using System.Collections;


//8nights specific stuff for each audio layer
[System.Serializable]
public class EightNightsLayerDetailsFMod
{
    public EightNightsMgr.GroupID Group;
    public string FModParamName; //param that controls volume of this group
    [Range(0, 1)]
    public float MixVolume = 1.0f;
}

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

    public float GetVolumeForGroup(EightNightsMgr.GroupID group)
    {
        EightNightsLayerDetailsFMod layerDetails = GetLayerDetailsForGroup(group);
        if (layerDetails != null)
        {
            float paramVal = 0.0f;
            FMOD.Studio.ParameterInstance param = FModEvent.getParameter(layerDetails.FModParamName);
            if (param != null)
                param.getValue(out paramVal);
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
            FMOD.Studio.ParameterInstance param = FModEvent.getParameter(layerDetails.FModParamName);
            if (param != null)
            {
                param.setValue(Mathf.Lerp(0.0f, layerDetails.MixVolume, volume));
            }
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
        FMOD.Studio.ParameterInstance param = FModEvent.getParameter(BackingLoopFModParam);
        if (param != null)
        {
            param.setValue( Mathf.Lerp(0.0f, BackingLoopMixVolume, volume));
        }
    }

    public float GetBackingLoopVolume()
    {
        float paramVal = 0.0f;
        FMOD.Studio.ParameterInstance param = FModEvent.getParameter(BackingLoopFModParam);
        if (param != null)
            param.getValue(out paramVal);

        return Mathf.InverseLerp(0.0f, BackingLoopMixVolume, paramVal);
    }

    public void SetPeakLoopVolume(float volume)
    {
        FMOD.Studio.ParameterInstance param = FModEvent.getParameter(PeakLoopFModParam);
        if (param != null)
        {
            param.setValue(Mathf.Lerp(0.0f, PeakLoopMixVolume, volume));
        }
    }

    public float GetPeakLoopVolume()
    {
        float paramVal = 0.0f;
        FMOD.Studio.ParameterInstance param = FModEvent.getParameter(PeakLoopFModParam);
        if (param != null)
            param.getValue(out paramVal);

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
