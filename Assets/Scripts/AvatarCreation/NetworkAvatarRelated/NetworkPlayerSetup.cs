using UnityEngine;
using Mirror;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System.Collections.Generic;
using AvatarTablet;

namespace AvatarCreation
{

    //the Logic for the player setup is to disabled everyhing in the player prefabs
    //therefore when its load the player, it doesnt consume many resources and the process will be faster
    //afterwards, when the local player is ready, enable the component needed by that Local avatar.

    public class NetworkPlayerSetup : NetworkBehaviour
    {
        [Header("VR Related")]
        public XRRig xrRig;
        public VRPlatformDetector vrPlatformDetector;
        public LocomotionSystem locomotionSystem;
        public TeleportationProvider teleportationProvider;


        [Header("Player Object")]
        public AvatarController avatarController;
        public AnimationController animationController;
        public GameObject showKeyCanvas;
        public GameObject keyboard;

        [Header("Script Reference")]
        public AvatarSettings avatarSettings;
        public GameObject library;
        public NetObjSpawner netObjSpawner;
        public ShareScreenWindow shareScreenWindow;



        public override void OnStartLocalPlayer()
        {
            EnableTheXrRig();

            avatarController.enabled = true;
            avatarController.isLocalPlayer = true;

            avatarSettings.SetPlayerValue();
            avatarSettings.SetUiSettingValue();



            showKeyCanvas.SetActive(true);
            keyboard.SetActive(true);
        }


        /// <summary>
        /// to enable the xr rig accordingly on local player
        /// </summary>
        private void EnableTheXrRig()
        {
            //enable the rig's script
            xrRig.enabled = true;
            vrPlatformDetector.enabled = true;
            locomotionSystem.enabled = true;
            teleportationProvider.enabled = true;

            //enable the needed library script
            animationController.enabled = true;
            library.SetActive(true);
            netObjSpawner.enabled = true;
            shareScreenWindow.enabled = true;


        }
    }

}

