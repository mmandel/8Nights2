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

   private FMOD.DSP _dsp;
   private FMOD.DSP_METERING_INFO  _meterInfo = new FMOD.DSP_METERING_INFO();

	void Start () 
   {
      FMOD_StudioEventEmitter emitter = gameObject.GetComponent<FMOD_StudioEventEmitter>();
      FMOD.Studio.System system = FMOD_StudioSystem.instance.System;
      system.update();
      system.flushCommands();

      FMOD.ChannelGroup group;

      /*FMOD.Studio.EventInstance evt1;
      FMOD.Studio.EventDescription evtDesrcription;

      system.getEvent(emitter.asset.path, out evtDesrcription);
      evtDesrcription.createInstance(out evt1);
      evt1.start();*/
      FMOD.Studio.EventInstance evt1 = emitter.getInstance();

      system.flushCommands();
      system.update();

      evt1.getChannelGroup(out group);

      system.flushCommands();
      system.update();

      //get DSP
      group.getDSP(0, out _dsp);
      _dsp.setMeteringEnabled(true, true);
            
      system.flushCommands();
      system.update();
	}
	
	void Update () 
   {
      if (_dsp == null)
         return;

      float curOutput = 0.0f;

      _dsp.getMeteringInfo(null, _meterInfo);
      float outpeaks = _meterInfo.peaklevel[0] + _meterInfo.peaklevel[1];
      float rmss = _meterInfo.rmslevel[0] + _meterInfo.rmslevel[1];

      curOutput += outpeaks + rmss;

      CurLevel = curOutput;

      //Debug.Log("Level: " + CurLevel);
	}
}
