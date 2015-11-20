//
//  Put on game object that is a child of the game object that has the Nights2Shamash script on it
//  This will detect when the player enters a collider on the same game object, and notify the shamash script
//

using UnityEngine;
using System.Collections;

public class Nights2NearShamash : MonoBehaviour 
{

    private Nights2Shamash _shamash = null;

	void Start () 
    {
        _shamash = transform.parent.GetComponent<Nights2Shamash>();
	}
	

    void OnTriggerEnter(Collider other)
    {
        //see if the player is near
        if ((other != null) && (other.GetComponent<Nights2Lantern>() != null))
        {
            //Debug.Log("PLAYER NEAR SHAMASH!!");

            if (_shamash != null)
                _shamash.NotifyPlayerNearby();
        }
    }

    void OnTriggerExit(Collider other)
    {
        //see if the player is near
        if ((other != null) && (other.GetComponent<Nights2TorchPlayer>() != null) || (other.GetComponent<Nights2Torch>() != null))
        {
            //Debug.Log("PLAYER EXIT SHAMASH AREA!!");

            if (_shamash != null)
                _shamash.NotifyPlayerNotNearby();
        }
    }
}
