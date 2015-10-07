//
//  Controls the lantern
//


using UnityEngine;
using System.Collections;

public class Nights2Lantern : MonoBehaviour 
{

	void Start () 
    {
	
	}
	
	void Update () 
    {
        //make sure we are parented to the right thing
        if (Nights2CamMgr.Instance != null)
        {
            if (transform.parent != Nights2CamMgr.Instance.GetLanternParent())
            {
                transform.parent = Nights2CamMgr.Instance.GetLanternParent();
                if (!transform.parent.gameObject.activeInHierarchy)
                    transform.parent = null;
                transform.localPosition = Vector3.zero;
            }
        }	
	}
}
