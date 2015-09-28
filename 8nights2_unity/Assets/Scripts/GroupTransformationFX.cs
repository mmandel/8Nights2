﻿//
// This is for animations that are scrubbed as tracks are brought int
//

using UnityEngine;
using System.Collections;

public class GroupTransformationFX : MonoBehaviour 
{

   public EightNightsMgr.GroupID Group = EightNightsMgr.GroupID.RiftGroup1;
   [HideInInspector]
   public GameObject ObjWithAnimator = null;
   [Space(10)]
   [AnimatorLayer]
   public int AnimatorLayer;
   [AnimatorState("AnimatorLayer")]
   public string StateToScrub;

   private Animator _animator = null;
   private float _lastU = 0.0f;

	void Start () 
   {
      _animator = (ObjWithAnimator != null) ? ObjWithAnimator.GetComponent<Animator>() : this.gameObject.GetComponent<Animator>();
	}
	
	void Update () 
   {
      if ((StateToScrub.Length == 0) || (_animator == null))
         return;

      _animator.speed = 0.0f;

      bool isCrescendoing = ButtonSoundMgr.Instance.IsGroupCrescendoing(Group);
      bool isReversing = ButtonSoundMgr.Instance.IsGroupCrescendoingReversed(Group);
      float crescendoProgress = ButtonSoundMgr.Instance.GetCrescendoProgressForGroup(Group);
      float trackVolume = EightNightsAudioMgr.Instance.MusicPlayer.GetVolumeForGroup(Group);
      EightNightsAudioMgr.GroupStateData groupAudioState = EightNightsAudioMgr.Instance.GetStateForGroup(Group);

      float u = 0.0f;
      if (isCrescendoing)
      {
         /*if (_lastU > crescendoProgress)
         {
            u = _lastU - (2.0f * Time.deltaTime);
            u = Mathf.Clamp01(u);
         }
         else*/
            u = crescendoProgress;

            if (isReversing)
               u = 1.0f - u;
      }
      else
      {
         /*if (trackVolume > 0.0f)
            u = 1.0f;
         else
            u = 0.0f;*/
         if (isReversing)
            u = 0.0f;
         else if (!isReversing && (groupAudioState.LoopState == EightNightsAudioMgr.StemLoopState.Releasing))
            u = trackVolume;
         else if (groupAudioState.LoopState == EightNightsAudioMgr.StemLoopState.Off)
            u = 0.0f;
         else
            u = 1.0f;
      }

      if (Mathf.Abs(_lastU - u) > .05f)
      {
         if (u > _lastU)
         {
            u = _lastU + (2.0f * Time.deltaTime);
         }
         else if (u < _lastU)
         {
            u = _lastU - (2.0f * Time.deltaTime);
         }

         u = Mathf.Clamp01(u);
      }

      _animator.Play(StateToScrub, AnimatorLayer, u);

      _lastU = u;
	}
}
