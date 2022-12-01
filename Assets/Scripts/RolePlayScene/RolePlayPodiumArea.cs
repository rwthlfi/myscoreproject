using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoomService;
using AvatarCreation;

public class RolePlayPodiumArea : MonoBehaviour
{
    public TheRoomServices theRoomService;


    private void OnTriggerEnter(Collider _collider)
    {
        ShowStatusCanvas(_collider, false);
    }

    private void OnTriggerExit(Collider _collider)
    {
        ShowStatusCanvas(_collider, true);
    }


    private void ShowStatusCanvas(Collider _collider, bool _show)
    {
        //get the collider
        NetworkPlayerSyncVar psv = _collider.GetComponent<NetworkPlayerSyncVar>();
        if (psv) //if it exist
        {
            //print("Deactivated the NameTag");
            //deactivated the nameTag
            psv.statusCanvas.SetActive(_show);
        }
    }


}
