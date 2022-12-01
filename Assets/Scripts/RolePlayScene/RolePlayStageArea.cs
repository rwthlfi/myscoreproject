using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using RoomService;
using AvatarCreation;
using AvatarTablet;

public class RolePlayStageArea : MonoBehaviour
{
    public TheRoomServices theRoomService;


    private void OnTriggerEnter(Collider _collider)
    {
        ChangeSpeakRadiusToEveryone(_collider, true);
    }

    private void OnTriggerExit(Collider _collider)
    {
        ChangeSpeakRadiusToEveryone(_collider, false);
    }


    private void ChangeSpeakRadiusToEveryone(Collider _collider, bool _maxRadius)
    {
        //get the microphone window

        NetworkPlayerSyncVar np = _collider.GetComponent<NetworkPlayerSyncVar>();

        if(np && np.isLocalPlayer)
        {
            if(_maxRadius)
                np.Ui_setSpeakingEveryone(20);
            else
                np.Ui_setSpeakingEveryone(15);
        }

        
    }
}
