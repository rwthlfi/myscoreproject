using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarCreation
{
    public class TeleportXrBackup : MonoBehaviour
    {
        public CharacterController XRPlayer;
        public Transform teleportReticle;
        private Vector3 position;

        private void Update()
        {
            position = teleportReticle.position;
        }
        private float offset = 0.2f;
        public void OnTeleportingSelectExit()
        {
            //check if it is a valid target
            if (teleportReticle.gameObject.activeSelf)
            {
                //take the XR Network Player
                //Vector3 position = teleportReticle.position;

                //move the location according to the reticle position
                XRPlayer.transform.position = new Vector3(position.x,
                                                          position.y + offset,
                                                          position.z
                                                          );
            }
            //Debug.Log("Exitpressed " + teleportReticle.position);
        }
    }
}
