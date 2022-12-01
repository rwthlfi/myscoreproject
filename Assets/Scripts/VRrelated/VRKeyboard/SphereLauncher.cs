using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using VRKeys;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    using Valve.VR;
#endif


public class SphereLauncher : MonoBehaviour
{
    [Header("Script reference")]
    public AvatarCreation.VRPlatformDetector vrPlatform;
    [Header("General Controller")]
    public XRController controller = null;


#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    public SteamVRBasedController steamVRBasedController;
#endif
    private bool isUsingSteamVR = false;

    //general Variable
    private LineRenderer LR;
    public float offset = 0.15f;
    public bool isPressed = false;


    // Start is called before the first frame update
    void Start()
    {  //check if the controller is using a steamVR or not.
        isUsingSteamVR = vrPlatform.useSteamVR;

        //assign the line renderer based on the controller type in the VR Rig.
        LR = controller.GetComponent<LineRenderer>();

        //if it is otherwise using windows then 
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (isUsingSteamVR)
            LR = steamVRBasedController.GetComponent<LineRenderer>(); ;
#endif
    }

    // Update is called once per frame
    void Update()
    {

        
//if it is using steamVR., then only executed the code in this if.
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (isUsingSteamVR)
        {
            if (steamVRBasedController.triggerValue > 0.1f)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition,
                                                                 new Vector3(0,
                                                                             //Mathf.Abs(LR.GetPosition(0).z - offset), 
                                                                             offset,
                                                                             0),
                                                                 Time.fixedDeltaTime);
                isPressed = true;

            }

            else
            {
                transform.localPosition = Vector3.zero;
                isPressed = false;
            }
                

            //stop executing the code.
            return;
        }
#endif

        
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float pointerValue))
        {
            if (pointerValue > 0.1f)
            //launch sphere
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, 
                                                              new Vector3(0, 
                                                                          //Mathf.Abs(LR.GetPosition(0).z - offset), 
                                                                          offset,
                                                                          0), 
                                                              Time.fixedDeltaTime);
                isPressed = true;
            }

            else
            {
                transform.localPosition = Vector3.zero;
                isPressed = false;
            }
        }
    }


}
