//
//  This game object will move with the torch carrier's head
//

using UnityEngine;
using System.Collections;

public class Nights2TorchPlayer : MonoBehaviour 
{

	void Update () 
    {
        //keep it attached to the torch carrier's head
        transform.parent = Nights2CamMgr.Instance.GetHeadTrans();
        transform.localPosition = Vector3.zero;	
	}
}
