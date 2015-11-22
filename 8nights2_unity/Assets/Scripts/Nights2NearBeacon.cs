//
//  Put on game object that is a child of the game object that has the Nights2Beacon script on it
//  This will detect when the player enters a collider on the same game object, and notify the beacon script
//

using UnityEngine;
using System.Collections;

public class Nights2NearBeacon : MonoBehaviour
{

    private Nights2Beacon _beacon = null;

    void Start()
    {
        _beacon = transform.parent.GetComponent<Nights2Beacon>();
    }


    void OnTriggerEnter(Collider other)
    {
        //see if the player is near
        //if ((other != null) && (other.GetComponent<Nights2TorchPlayer>() != null) || (other.GetComponent<Nights2Torch>() != null))
        if ((other != null) && (other.GetComponent<Nights2Lantern>() != null))
        {
            //Debug.Log("PLAYER NEAR Beacon!!");

            if (_beacon != null)
                _beacon.NotifyPlayerNearby();
        }
    }

    void OnTriggerExit(Collider other)
    {
        //see if the player is near
        //if ((other != null) &&(other.GetComponent<Nights2TorchPlayer>() != null) || (other.GetComponent<Nights2Torch>() != null))
        if ((other != null) && (other.GetComponent<Nights2Lantern>() != null))
        {
            //Debug.Log("PLAYER EXIT BEACON AREA!!");

            if (_beacon != null)
                _beacon.NotifyPlayerNotNearby();
        }
    }
}
