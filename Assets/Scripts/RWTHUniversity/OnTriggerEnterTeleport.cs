using UnityEngine;
using AvatarCreation;

public class OnTriggerEnterTeleport : MonoBehaviour
{
    public DoorTimeEnabler doorTimeEnabler;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Player" 
            && other.isTrigger == false 
            && other.GetComponent<NetworkPlayerSyncVar>().isLocalPlayer)) // online
            doorTimeEnabler.Ui_GoBack();    
    }
}
