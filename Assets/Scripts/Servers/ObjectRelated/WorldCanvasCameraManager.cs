using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


//this script is used to assign the camera in online world canvas.
//the world canvas in this case, will not work properly without EventCamera, 
//however the EventCamera as well will not be assign automatically since the player will be spawn after the Canvas is initialized

//The logic is when a local player is entering a box collider then assign the "center eye anchor camera" to the "event Canvas"

public class WorldCanvasCameraManager : MonoBehaviour
{
    //get the world canvas
    public Canvas worldCanvasTarget;
    //private string colliderNameToDetect = GlobalSettings.playerPrefabName;

    private void Awake()
    {
//        worldCanvasTarget.GetComponent<OVRRaycaster>().enabled = false;
    }


    private void Update()
    {
        //if the event camera still not assign
        if(!worldCanvasTarget.worldCamera)
        {
            //find the local player
            NetworkIdentity[] players = (NetworkIdentity[])FindObjectsOfType(typeof(NetworkIdentity));
            foreach(NetworkIdentity player in players)
            {
                //if the local player has been found then assign to the canvas
                if (player.isLocalPlayer)
                {
                    AssignCenterEyeAnchor(player.transform.root);
                    break;
                }
            }
        }


        if (worldCanvasTarget.worldCamera)
        {
            //after world canvas is found then disable this script
            this.enabled = false;
        }
            
    }


    private void AssignCenterEyeAnchor(Transform _localPlayer)
    {   
        //get the camera on the OVR
        Camera localEyeCamera = _localPlayer.GetComponentInChildren<XRCameraReference>().GetComponent<Camera>();
        //set the event canvas to the local eye camera.
        worldCanvasTarget.worldCamera = localEyeCamera;

        //enable the ovrraycaster
       // worldCanvasTarget.GetComponent<OVRRaycaster>().enabled = true;
    }


}
