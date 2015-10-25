//
//  Sends messages to the LightJams app using OSC network packets, to update DMX controlled physical lights
//

using UnityEngine;
using System.Collections;
using System;

public class LightJamsMgr : MonoBehaviour 
{
   public float FixedLatency = .1f;

   string kOSCPrefix = "/lj/osc/";

   //events
   public event LJHandler OnLightChanged;     //send out whenever a physical light is expected to change in real life
   public event LJHandler OnLightCommandSent; //when message is sent to hardware, ahead of time
   public class LJEventArgs : EventArgs
   {
      public LJEventArgs(int chan, float i, Color c, bool isPar64) { Channel = chan; Intensity = i; EventColor = c; IsPar64 = isPar64;  }
      public int Channel = 0;
      public float Intensity = 1.0f;
      public Color EventColor = Color.white;
      public bool IsPar64 = false;
   }
   public delegate void LJHandler(object sender, LJEventArgs e);

   	//Singleton
   public static LightJamsMgr Instance { get;  private set; }


   public float GetCurLatency()
   {
      return FixedLatency;
   }

	// Use this for initialization
	void Awake () {
	   Instance = this;

      //make sure the OSCMessenger exists
      if (gameObject.GetComponent<OSCMessenger>() == null)
         gameObject.AddComponent<OSCMessenger>();
	}

   public void SendToLightJams(int channelNum, float val, bool notify = true)
   {
      if (OSCMessenger.Instance != null)
      {
         OSCMessenger.Instance.SendMessage(kOSCPrefix + channelNum, val);

         /*if (OnLightChanged != null)
         {
            OnLightChanged(this, new LJEventArgs(channelNum, val));
         }*/
         //notify anyone who cares
         if (notify && ((OnLightCommandSent != null) || (OnLightChanged != null)))
         {
            SendLightChangedEvent(channelNum, val, Color.white, false);
         }
      }
   }

   public void SendLightChangedEvent(int channelNum, float val, Color color, bool isPar64)
   {
      LJEventArgs eventData = new LJEventArgs(channelNum, val, color, isPar64);

      if (OnLightCommandSent != null)
         OnLightCommandSent(this, eventData);

      //run co-routine to send out event for game after latency elapses (so visuals in-game don't trigger early)
      if (OnLightChanged != null)
         StartCoroutine(SendDelayedUpdateEvent(GetCurLatency(), eventData));
   }

   IEnumerator SendDelayedUpdateEvent(float delayTime, LJEventArgs eventData)
   {
      yield return new WaitForSeconds(delayTime);

      if (OnLightChanged != null)
         OnLightChanged(this, eventData);
   }
	
}
