using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoomService;
using UnityStandardAssets.Characters.FirstPerson;

public class TeleportingPlayer : MonoBehaviour
{
    [Header("Script Reference")]
    public TheRoomServices theRoomService;

    [Header("targetLoc")]
    public List<Transform> targetLocList = new List<Transform>();

    private Transform cacheLocalPlayer;
    private Vector3 cachePos;
    private Quaternion cacheRot;

    public FirstPersonController _firstPersonController;

    private void AssignLocalPlayer()
    {
        cacheLocalPlayer = theRoomService.theLocalPlayer.gameObject.transform;
    }

    private void GetFirstPersonController()
    {
        _firstPersonController = theRoomService.theLocalPlayer.GetComponent<FirstPersonController>();
    }

    public void Ui_TeleportToLocation(int _id)
    {
        StartCoroutine(Ui_teleportClick(_id));
    }

    //teleport the User according to the ID of the video.
    public IEnumerator Ui_teleportClick(int _id)
    {
        if (!cacheLocalPlayer)
        {
            AssignLocalPlayer();
        }

        //For FPC the FirstPersonController needs to deactivated before the teleport, otherwise the FPC overrides the updated position with the last one
        if (GlobalSettings.DeviceType() == GlobalSettings.Device.WindowsNonVR)
        {
            if (!_firstPersonController)
            {
                GetFirstPersonController();
            }

            StartCoroutine(FirstPersonTeleport());
        }

        //cache the player position and rotation
        cachePos = cacheLocalPlayer.position;
        cacheRot = cacheLocalPlayer.rotation;

        yield return new WaitForSeconds(0.1f);

        //get the local player and "teleport" him to the target location position & Rotation
        cacheLocalPlayer.position = targetLocList[_id].position;
        cacheLocalPlayer.rotation = targetLocList[_id].rotation;
    }


    public void Ui_ButtonGoBack()
    {
        StartCoroutine(Ui_GoBackToCacheLocation());
    }

    //To return to the previous position
    public IEnumerator Ui_GoBackToCacheLocation()
    {
        //For FPC the FirstPersonController needs to deactivated before the teleport, otherwise the FPC overrides the updated position with the last one
        if (GlobalSettings.DeviceType() == GlobalSettings.Device.WindowsNonVR)
        {
            if (!_firstPersonController)
            {
                GetFirstPersonController();
            }

            StartCoroutine(FirstPersonTeleport());
        }

        yield return new WaitForSeconds(0.1f);

        cacheLocalPlayer.position = cachePos;
        cacheLocalPlayer.rotation = cacheRot;
    }

    IEnumerator FirstPersonTeleport()
    {
        _firstPersonController.enabled = false;

        yield return new WaitForSeconds(0.2f);

        _firstPersonController.enabled = true;
    }
}