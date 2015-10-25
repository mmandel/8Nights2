﻿//
// for hooking up FX to lights based on their fade value (presumably driven by audio FFT)
// these fx function by scrubbing or cycling animations using the value of the light's fade
//

using UnityEngine;
using System.Collections;

public class ScrubFX : MonoBehaviour 
{

   [Header("Light To Scrub With")]
   public EightNightsMgr.GroupID Group;
   public EightNightsMgr.LightID Light;
   [Tooltip("Optional LightEffect to sync directly from. if set, will ignore Group and Light properties.  This is if you want to sync with FFT data that isn't driving the lights")]
   public FFTLightEffect SyncFromLightEffect;

   [Header("Scrub Animation")]
   public bool EnableAnimScrub = false;
   [AnimatorLayer]
   public int ScrubLayer;
   [AnimatorState("ScrubLayer")]
   public string ScrubState;

   [Header("Cycle Animation")]
   public bool EnableAnimCycle = false;
   [AnimatorLayer]
   public int CycleLayer;
   [AnimatorState("CycleLayer")]
   public string CycleState;
   public float CycleSpeedMin = 0.0f;
   public float CycleSpeedMax = 1.0f;
   public AnimationCurve CycleEase;

   private float _curCycleVal = 0.0f;

   private Animator _animator;

   public ScrubFX()
   {
      CycleEase = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
   }

	void Start () 
   {
      if (EightNightsMgr.Instance != null)
         EightNightsMgr.Instance.OnLightChanged += OnLightChanged;
      else if (LightMgr.Instance != null)
         LightMgr.Instance.OnLightChanged += OnLightMgrLightChanged;
      _animator = gameObject.GetComponent<Animator>();
	}

   void OnLightMgrLightChanged(object sender, LightMgr.LightEventArgs e)
   {
      if (_animator == null)
         return;

      if (SyncFromLightEffect != null)
         return;

      if ((e.Group == Group) && (e.Light == Light))
      {
         float val = e.Data.LightIntensity;

         UpdateWithScrubValue(val);
      }
   }

   void OnLightChanged(object sender, EightNightsMgr.LightEventArgs e)
   {
      if(_animator == null)
         return;

      if (SyncFromLightEffect != null)
         return;

      if ((e.Group == Group) && (e.Light == Light))
      {
         float val = e.Data.LightIntensity;

         UpdateWithScrubValue(val);
      }
   }

   void UpdateWithScrubValue(float val)
   {
      if (EnableAnimCycle && (CycleState.Length > 0))
      {
        // _animator.speed = 0.0f;
         const float kValScale = 4.5f; //to keep speed values in normal 0..1 range
         _curCycleVal += Mathf.Lerp(CycleSpeedMin * kValScale * Time.deltaTime, CycleSpeedMax * kValScale * Time.deltaTime, CycleEase.Evaluate(val));
         _curCycleVal = _curCycleVal % 1.0f; //wrap around
         _animator.Play(CycleState, CycleLayer, _curCycleVal);
      }

      if (EnableAnimScrub && (ScrubState.Length > 0))
      {
         //_animator.speed = 0.0f;
         _animator.Play(ScrubState, ScrubLayer, val);
      }
   }

   void Update()
   {
      if (SyncFromLightEffect != null)
      {
         UpdateWithScrubValue(SyncFromLightEffect.GetSignalValue());
      }
   }

}
