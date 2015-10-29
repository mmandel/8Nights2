//
// This manages the animation states for the portal
//

using UnityEngine;
using System.Collections;

public class Nights2Portal : MonoBehaviour 
{
    public Transform PortalDestTrans;
    public string ShowPortalTrigger = "show"; //show the portal
    public string CancelPortalTrigger = "cancel"; //hide the portal due to cancelling (player stepped off path)
    public string ActivatedTrigger = "activated"; //play effect with portal is activated, and then hide

    [Space(10)]
    public FMOD_StudioEventEmitter PortalOpenSound;
    public FMOD_StudioEventEmitter PortalActivatedSound;

    private Animator _animator = null;

	void Start () 
    {
        _animator = this.gameObject.GetComponent<Animator>();
	}

    public void TriggerShowPortal()
    {
        if ((_animator != null) && (ShowPortalTrigger.Length > 0))
            _animator.SetTrigger(ShowPortalTrigger);

        if (PortalOpenSound != null)
            PortalOpenSound.Play();
    }

    public void TriggerCancelPortal()
    {
        if ((_animator != null) && (CancelPortalTrigger.Length > 0))
            _animator.SetTrigger(CancelPortalTrigger);

        if (PortalOpenSound != null)
            PortalOpenSound.Stop();
    }

    public void TriggerActivatedPortal()
    {
        if ((_animator != null) && (ActivatedTrigger.Length > 0))
            _animator.SetTrigger(ActivatedTrigger);

        if (PortalActivatedSound != null)
            PortalActivatedSound.Play();

        if (PortalOpenSound != null)
            PortalOpenSound.Stop();
    }
	
}
