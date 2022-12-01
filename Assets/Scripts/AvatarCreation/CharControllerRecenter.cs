using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AvatarCreation
{

    //This script is to automatically recenter the position of the Avatar
    public class CharControllerRecenter : MonoBehaviour
    {
        public CharacterController controller;
        public Transform target;

        private void Update()
        {
            /*
            if (GlobalSettings.DeviceType() == GlobalSettings.Device.WindowsNonVR)
                controller.center = new Vector3(cameraTransformSteam.localPosition.x, 1, cameraTransformSteam.localPosition.z);
            else
                controller.center = new Vector3(cameraTransformOculus.localPosition.x, 1, cameraTransformOculus.localPosition.z);
            */
            controller.center = new Vector3(target.localPosition.x, 1, target.localPosition.z);
            //print("Updating center" + target.position);
        }
    }
}
