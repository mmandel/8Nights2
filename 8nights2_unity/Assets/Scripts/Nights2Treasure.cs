//
//  Treasure box sprouts up and waits for both the lantern and torch to be placed on their icons before unlocking the treasure 
//  and imbueing the torch with its magic
//

using UnityEngine;
using System.Collections;

public class Nights2Treasure : MonoBehaviour 
{
   public Nights2Icon TorchIcon;
   public Nights2Icon LanternIcon;

   [Space(10)]

   //"magic" that is inside the treasure box and flies to the torch when the box is opened
   public Transform MagicTrans;

   [Header("Animator Triggers")]

   public string TorchLockedBool = "lock1_locked";
   public string LanternLockedBool = "lock2_locked";
   public string UnlockedMagicTrigger = "unlocked";

   private Animator _animator = null;
   private bool _isUnlocked = false;

	void Start () 
   {
      _animator = gameObject.GetComponent<Animator>();
      _isUnlocked = false;
	}
	
	void Update () 
   {
      bool torchUnlocked = (TorchIcon != null) ? TorchIcon.RequiredPropIsNear() : false;
      bool lanternUnlocked = (LanternIcon != null) ? LanternIcon.RequiredPropIsNear() : false;
         
      if((_animator != null) && (TorchLockedBool.Length > 0))
      {
         _animator.SetBool(TorchLockedBool, !torchUnlocked);
      }

      if ((_animator != null) && (LanternLockedBool.Length > 0))
      {
         _animator.SetBool(LanternLockedBool, !lanternUnlocked);
      }

      bool wasUnlocked = _isUnlocked;
      _isUnlocked = torchUnlocked && lanternUnlocked;

      if (!wasUnlocked && _isUnlocked && (_animator != null))
      {
         _animator.SetTrigger(UnlockedMagicTrigger);

         //TODO: make magic fly up and then at torch, using PD tracker
      }
	}
}
