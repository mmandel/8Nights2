using UnityEngine;
using System.Collections;

public class Nights2Icon : MonoBehaviour 
{
   public PropTypes RequiredProp = PropTypes.Lantern;

   [Space(10)]

   public string PropNearbyBool = "prop_is_near"; //is required prop nearby

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

	void Start () 
   {
      _animator = gameObject.GetComponent<Animator>();
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

      if (RequiredProp == PropTypes.Lantern)
      {
         return (other.GetComponent<Nights2TorchPlayer>() != null) || (other.GetComponent<Nights2Torch>() != null);
      }
      else if (RequiredProp == PropTypes.Torch)
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
