using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace AvatarCreation
{
    public class RayController : MonoBehaviour
    {
        [Header("Oculus")]
        public XRController rayInteractor;
        public InputHelpers.Button teleportButton;

        [Header("SteamVR")]
        public SteamVRBasedController steamVRBasedController;
        private XRRayInteractor xrRayInteractor;
        private XRInteractorLineVisual xrInteractorLineVisual;


        [Header("Extra Variable")]
        public float activationThreshold = 0.1f;
        private bool useSteamVR = false;

        private void Start()
        {
            //if steamVR controller exist then use the steamVR
            //steamVRBasedController = GetComponent<SteamVRBasedController>();

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            if (steamVRBasedController)
            {
                useSteamVR = steamVRBasedController.GetComponentInParent<VRPlatformDetector>().useSteamVR;
                if (useSteamVR)
                {
                    xrRayInteractor = steamVRBasedController.GetComponent<XRRayInteractor>();
                    xrInteractorLineVisual = steamVRBasedController.GetComponent<XRInteractorLineVisual>();
                }
            }
#endif

        }

        // Update is called once per frame
        void Update()
        {
            if (!rayInteractor)
            {
                //Debug.LogWarning("the Ray is not assign");
                return;
            }

            //activated them according to the controller
            if (!useSteamVR)
            {
                rayInteractor.gameObject.SetActive(CheckIfActivated_Oculus(rayInteractor));
            }
            else if (useSteamVR)
                CheckIfActivated_SteamVR();
        }

        public bool CheckIfActivated_Oculus(XRController controller)
        {
            InputHelpers.IsPressed(controller.inputDevice, teleportButton, out bool isActivated, activationThreshold);
            return isActivated;
        }

        public void CheckIfActivated_SteamVR()
        {

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            xrRayInteractor.enabled = steamVRBasedController.teleportButtonValue;
            xrInteractorLineVisual.reticle.SetActive(xrRayInteractor.enabled);
#endif
        }

    }
}
