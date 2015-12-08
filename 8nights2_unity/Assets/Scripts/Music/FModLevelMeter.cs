//
//  Looks for an FMOD_StudioEventEmitter on the same game object and outputs the audio level of the playing event
//  Derived from:
//  http://www.fmod.org/questions/question/how-can-i-get-the-output-level-of-an-event-in-unity/
//

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FMOD_StudioEventEmitter))]
public class FModLevelMeter : MonoBehaviour 
{
   public float CurLevel = 0.0f;

   private FMOD.DSP[] _dsps = new FMOD.DSP[0];
   private FMOD.DSP_METERING_INFO  _meterInfo = new FMOD.DSP_METERING_INFO();
   FMOD.ChannelGroup _group;

	void Start () 
   {
      FMOD_StudioEventEmitter emitter = gameObject.GetComponent<FMOD_StudioEventEmitter>();
      FMOD.Studio.System system = FMOD_StudioSystem.instance.System;
      system.update();
      system.flushCommands();

      /*FMOD.Studio.EventInstance evt1;
      FMOD.Studio.EventDescription evtDesrcription;

      system.getEvent(emitter.asset.path, out evtDesrcription);
      evtDesrcription.createInstance(out evt1);
      evt1.start();*/
      FMOD.Studio.EventInstance evt1 = emitter.getInstance();

      system.flushCommands();
      system.update();

      evt1.getChannelGroup(out _group);


      system.flushCommands();
      system.update();

      //get DSP
      int numDSPs;
      _group.getNumDSPs(out numDSPs);

      //if (numDSPs > 1) //just first for now, makes level go above 1 otherwise.  Might revisit...
      //   numDSPs = 1;

      _dsps = new FMOD.DSP[numDSPs];
      for (int i = 0; i < numDSPs; i++)
      {
         FMOD.DSP dsp;
         _group.getDSP(i, out dsp);
         dsp.setMeteringEnabled(true, true);
         _dsps[i] = dsp;
      }
   
      system.flushCommands();
      system.update();
	}
	
	void Update () 
   {
      if (_dsps.Length == 0)
         return;

      float curOutput = 0.0f;

      for (int i = 0; i < _dsps.Length; i++)
      {
         _dsps[i].getMeteringInfo(null, _meterInfo);
         float outpeaks = _meterInfo.peaklevel[0] + _meterInfo.peaklevel[1];
         float rmss = _meterInfo.rmslevel[0] + _meterInfo.rmslevel[1];

         curOutput += outpeaks + rmss;
      }

      CurLevel = curOutput;

      /*if (gameObject.name.Contains("SpokenCandle1"))
      {
         int numDSPs;
         _group.getNumDSPs(out numDSPs);
         int numGroups;
         _group.getNumGroups(out numGroups);
         FMOD_StudioEventEmitter emitter = gameObject.GetComponent<FMOD_StudioEventEmitter>();
         FMOD.Studio.EventInstance evt1 = emitter.getInstance();
         FMOD.ChannelGroup g;
         evt1.getChannelGroup(out g);
         g.getNumDSPs(out numDSPs);
         g.getNumGroups(out numGroups);
         bool isPlaying = emitter.getPlaybackState() == FMOD.Studio.PLAYBACK_STATE.PLAYING;
         Debug.Log("Level: " + CurLevel);
      }*/

      /*if (CurLevel > 0)
      {
         Debug.Log("Level: " + CurLevel + " " + gameObject.name);
      }*/
	}
}
