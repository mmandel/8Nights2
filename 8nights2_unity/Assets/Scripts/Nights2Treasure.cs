//
//  Treasure box sprouts up and waits for both the lantern and torch to be placed on their icons before unlocking the treasure 
//  and imbueing the torch with its magic
//

using UnityEngine;
using System.Collections;

public class Nights2Treasure : MonoBehaviour 
{
   [Header("Torch")]
   public Transform TorchIconSpot;
   public GameObject TorchIconPrefab;

   [Header("Lantern")]
   public Transform LanternIconSpot;
   public GameObject LanternIconPrefab;

   [Header("Light FX")]
   public float LightSeqInterval = .2f; //time between each light coming on
   public float LightSeqHoldTime = 2.0f; //time to hold all lights on for, once they sequence in

   [Header("Destroy")]
   public float DelayToDestroy = 4.0f;

   [Header("Magic")]
   public float MagicInitialWaitTime = .75f;
   public float MagicRiseAmount = 1.0f;
   public float MagicRiseTime = 1.0f;
   public float MagicToTorchTime = 1.5f;
   public float HasMagicDistThresh = .05f; //2 inches
   //"magic" that is inside the treasure box and flies to the torch when the box is opened
   public Transform MagicTrans;

   [Header("Animator Triggers")]
   public string TorchLockedBool = "lock1_locked";
   public string LanternLockedBool = "lock2_locked";
   public string UnlockedMagicTrigger = "unlocked";

   [Header("Sounds")]
   public FMOD_StudioEventEmitter CompletedSound;
   public FMOD_StudioEventEmitter RevealSound;
   public FMOD_StudioEventEmitter UnlockSound;

   [Header("Debug")]
   [ScriptButton("Unlock 1!", "OnUnlock1")]
   public bool DummyTriggerLock1;
   [ScriptButton("Unlock 2!", "OnUnlock2")]
   public bool DummyTriggerLock2;
   [ScriptButton("Open!", "OnOpen")]
   public bool DummyOpenBox;

   private Animator _animator = null;
   private bool _isUnlocked = false;
   private Nights2Icon _torchIcon;
   private Nights2Icon _lanternIcon;
   private float _destroyTimerStart = -1.0f;

   private float _magicTimer = -1.0f;
   private PhysicsTracker _magicTracker = null;
   private MagicPhase _magicState = MagicPhase.Initial;

   private bool _forceUnlock1 = false;
   private bool _forceUnlock2 = false;
   private bool _forceOpen = false;
   private int _lastNumUnlocked = 0;
   private bool _firstUpdate = false;

   enum MagicPhase
   {
      Initial,
      Waiting,
      Rising,
      ToTorch,
      Hidden
   }

   public bool IsUnlocked() { return _isUnlocked; }

	void Start () 
   {
      _firstUpdate = true;
      _lastNumUnlocked = 0;

      _animator = gameObject.GetComponent<Animator>();
      _isUnlocked = false;

      //setup magic obj with physics tracker
      if(MagicTrans != null)
      {
         _magicTracker = MagicTrans.GetComponent<PhysicsTracker>();
         if (_magicTracker == null)
            _magicTracker = MagicTrans.gameObject.AddComponent<PhysicsTracker>();
         MagicTrans.gameObject.SetActive(false);

         _magicState = MagicPhase.Initial;
      }

      //spawn torch icon
      if ((TorchIconPrefab != null) && (TorchIconSpot != null))
      {
         GameObject spawned = Instantiate(TorchIconPrefab) as GameObject;
         if (spawned != null)
         {
            _torchIcon = spawned.GetComponent<Nights2Icon>();
            spawned.transform.parent = TorchIconSpot;
            spawned.transform.localPosition = Vector3.zero;
            spawned.transform.localRotation = Quaternion.identity;
         }
      }

      //spawn lantern icon
      if ((LanternIconPrefab != null) && (LanternIconSpot != null))
      {
         GameObject spawned = Instantiate(LanternIconPrefab) as GameObject;
         if (spawned != null)
         {
            _lanternIcon = spawned.GetComponent<Nights2Icon>();
            spawned.transform.parent = LanternIconSpot;
            spawned.transform.localPosition = Vector3.zero;
            spawned.transform.localRotation = Quaternion.identity;
         }
      }
	}
	
	void Update () 
   {
      if (_firstUpdate)
      {
         _firstUpdate = false;
         if (RevealSound != null)
            RevealSound.Play();
      }

      bool torchUnlocked = (_torchIcon != null) ? (_forceUnlock1 || _torchIcon.RequiredPropIsNear()) : false;
      bool lanternUnlocked = (_lanternIcon != null) ? (_forceUnlock2 || _lanternIcon.RequiredPropIsNear()) : false;

      int curNumUnlocked = 0;
      if (torchUnlocked)
         curNumUnlocked++;
      if (lanternUnlocked)
         curNumUnlocked++;

      //first icon unlocked
      if ((_lastNumUnlocked == 0) && (curNumUnlocked == 1))
      {
         if (UnlockSound != null)
            UnlockSound.Play();
      }

      _lastNumUnlocked = curNumUnlocked;
         
      if((_animator != null) && (TorchLockedBool.Length > 0))
      {
         _animator.SetBool(TorchLockedBool, !torchUnlocked);
      }

      if ((_animator != null) && (LanternLockedBool.Length > 0))
      {
         _animator.SetBool(LanternLockedBool, !lanternUnlocked);
      }

      bool wasUnlocked = _isUnlocked;
      _isUnlocked = _forceOpen || (torchUnlocked && lanternUnlocked);

      if (!wasUnlocked && _isUnlocked && (_animator != null))
      {
         if (CompletedSound != null)
            CompletedSound.Play();

         _animator.SetTrigger(UnlockedMagicTrigger);

         //trigger fx on the physical lights
         Nights2Mgr.Instance.FXTurnOnInSequence(new Nights2Mgr.TurnOnInSequenceParams(LightSeqHoldTime, LightSeqInterval, Nights2Mgr.Instance.ClockwiseCandleOrder));

         //transition lantern + torch icons out
         if (_lanternIcon != null)
             _lanternIcon.Destroy();
         if (_torchIcon != null)
             _torchIcon.Destroy();

         //make magic fly up and then at torch, using PD tracker
         if (_magicTracker != null)
         {
            MagicTrans.gameObject.SetActive(true);
            _magicTracker.enabled = false;
            _magicState = MagicPhase.Waiting;
            _magicTimer = Time.time;
         }

         //start destroy timer
         _destroyTimerStart = Time.time;
      }

      if (_destroyTimerStart > 0.0f)
      {
         float elapsed = Time.time - _destroyTimerStart;
         if (elapsed >= DelayToDestroy)
         {
            if (_torchIcon != null)
               DestroyObject(_torchIcon.gameObject);
            if (_lanternIcon != null)
               DestroyObject(_lanternIcon.gameObject);
            DestroyObject(this.gameObject);
         }
      }

      if (_magicTimer > 0.0f)
      {
         float elapsed = Time.time - _magicTimer;
         switch (_magicState)
         {
            case MagicPhase.Waiting:
               if (elapsed >= MagicInitialWaitTime)
               {
                  _magicTracker.enabled = true;
                  _magicTracker.UseOverride(true);

                  if (MagicRiseAmount > float.Epsilon)
                  {
                     _magicState = MagicPhase.Rising;
                     _magicTracker.SetOverrideTargetPos(MagicTrans.transform.position + new Vector3(0.0f, MagicRiseAmount, 0.0f));
                  }
                  else //skip straight to ToTorch state
                  {
                     _magicState = MagicPhase.ToTorch;
                     _magicTracker.SetOverrideTargetPos(Nights2CamMgr.Instance.GetTorchParent().position);
                  }
                  _magicTimer = Time.time;
               }
               break;
            case MagicPhase.Rising:
               if (elapsed >= MagicRiseTime)
               {
                  _magicState = MagicPhase.ToTorch;
                  _magicTimer = Time.time;
                  _magicTracker.SetOverrideTargetPos(Nights2CamMgr.Instance.GetTorchParent().position);
               }
               break;
            case MagicPhase.ToTorch:
               _magicTracker.SetOverrideTargetPos(Nights2CamMgr.Instance.GetTorchParent().position);
               float distToTorch = (Nights2CamMgr.Instance.GetTorchParent().position - MagicTrans.transform.position).magnitude;
               //imbue torch when magic gets close
               if (distToTorch <= HasMagicDistThresh)
                  Nights2Mgr.Instance.SetTorchHasMagic(true);
               if (elapsed >= MagicToTorchTime)
               {
                  Nights2Mgr.Instance.SetTorchHasMagic(true); //just in case...
                  _magicState = MagicPhase.Hidden;
                  _magicTracker.gameObject.SetActive(false);
               }
               break;
            default: break;
         }
      }
	}

   public void OnUnlock1(string propPath)
   {
      _forceUnlock1 = true;
   }

   public void OnUnlock2(string propPath)
   {
      _forceUnlock2 = true;
   }

   public void ForceOpen()
   {
       _forceOpen = true;
   }

   public void OnOpen(string propPath)
   {
       ForceOpen();
   }  
}
