using UnityEngine;
using System.Collections;

public class Nights2Icon : MonoBehaviour 
{
   public PropTypes RequiredProp = PropTypes.Lantern;

   [Space(10)]

   public string PropNearbyBool = ""; //is required prop nearby
   public string ExitBool = ""; //trigger exit anim
   public float DestroyDelay = 1.0f; //how long to wait for exit anim to play before destroying

   public enum PropTypes
   {
      Lantern,
      Torch
   };

   private Animator _animator;
   private bool _requiredPropNear = false;

   public bool RequiredPropIsNear()
   {
      return _requiredPropNear;
   }

   public void Destroy()
   {
       StartCoroutine(DelayedExit());
   }

   IEnumerator DelayedExit()
   {
       SetExitBool(true);
       yield return new WaitForSeconds(DestroyDelay);
       DestroyObject(this.gameObject);
   }

	void Start () 
   {
      _animator = gameObject.GetComponent<Animator>();
      SetExitBool(false);
	}

    void SetExitBool(bool b)
    {
        if ((_animator != null) && (ExitBool.Length > 0))
            _animator.SetBool(ExitBool, b);
    }

   void Update()
   {
      if ((_animator != null) && (PropNearbyBool.Length > 0))
         _animator.SetBool(PropNearbyBool, _requiredPropNear);
   }

   bool IsRequiredProp(Collider other)
   {
      if(other == null)
         return false;

      if (RequiredProp == PropTypes.Torch)
      {
         return (other.GetComponent<Nights2TorchPlayer>() != null) || (other.GetComponent<Nights2Torch>() != null);
      }
      else if (RequiredProp == PropTypes.Lantern)
      {
         return (other.GetComponent<Nights2Lantern>() != null);
      }

      return false;
   }

   void OnTriggerEnter(Collider other)
   {
      if (IsRequiredProp(other))
      {
         _requiredPropNear = true;
      }
   }

   void OnTriggerExit(Collider other)
   {
      if (IsRequiredProp(other))
      {
         _requiredPropNear = false;
      }
   }

}
