//
//  Controls the torch
//

using UnityEngine;
using System.Collections;

public class Nights2Torch : MonoBehaviour 
{
	void Start () 
    {
	
	}
	
	void Update () 
    {
	    //make sure we are parented to the right thing
        if (Nights2CamMgr.Instance != null)
        {
            if (transform.parent != Nights2CamMgr.Instance.GetTorchParent())
            {
                transform.parent = Nights2CamMgr.Instance.GetTorchParent();
                if (!transform.parent.gameObject.activeInHierarchy)
                    transform.parent = null;
                transform.localPosition = Vector3.zero;
            }
        }
	}
}
