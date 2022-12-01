using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
// Attach this script to every "World Canvas" Available
// this script will automatically search for the XR Camera and assign that Camera to the world Canvas.
public class CanvasCameraAssigner : MonoBehaviour
{
    //for cache-ing the variable needed.
    private Canvas thisCanvas;
    private XRCameraReference xrCameraRef;
    private CanvasCameraPointerReference canvasCameraPointer;

    //cache the camera, so the system doesnt need to go back and forth reassigning them
    


    // Start is called before the first frame update
    private IEnumerator Start()
    {
        thisCanvas = this.GetComponent<Canvas>();
        yield return new WaitForSeconds(1f);
        RefererenceXRCam();
    }

    //find the xr Camera Reference,
    //cause this camera is dependable on the platform we are using
    public void RefererenceXRCam()
    {
        if (FindObjectOfType<XRCameraReference>() != null)
        {
            print("Ref XR");
            xrCameraRef = FindObjectOfType<XRCameraReference>(false);
            thisCanvas.worldCamera = xrCameraRef.GetComponent<Camera>();
        }
    }


    //find the canvasCameraPointer
    //there will always be one CanvasCameraPointer
    public void RefererenceCanvasCameraPointer()
    {
        if (FindObjectOfType<CanvasCameraPointerReference>() != null)
        {
            print("Ref Canvaspointer");
            canvasCameraPointer = FindObjectOfType<CanvasCameraPointerReference>(false);
            thisCanvas.worldCamera = canvasCameraPointer.GetComponent<Camera>();
        }
    }



    //re-checking every X period 
    //in order to make sure that the camera is being assigned to the canvas
    private float nextActionTime = 0f;
    private float period = 3f;
    private void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
            // Really2 checking if the xr Camera is assigned or not.

            //check if using hand tracking
            if (GlobalSettings.handTrackingActive())
            {
                if (!canvasCameraPointer)
                    RefererenceCanvasCameraPointer();
                else // assign the camera
                    thisCanvas.worldCamera = canvasCameraPointer.GetComponent<Camera>();
            }

            //else use the head Camera 
            else
            {
                if (!xrCameraRef)
                    RefererenceXRCam();
                else
                    thisCanvas.worldCamera = xrCameraRef.GetComponent<Camera>();
            }

        }
    }


}