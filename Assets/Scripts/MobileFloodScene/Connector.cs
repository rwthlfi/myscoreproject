using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Connector : MonoBehaviour
{
    [Header("Main Game Object")]
    public Transform grabA;
    public Transform grabB;
    public Transform BoxOne;

    [Header("Grabbing percentage")]
    public TextMeshProUGUI grabA_text;
    public TextMeshProUGUI grabB_text;
    private float grabA_percent;
    private float grabB_percent;

    [Header("Helper Game Object")]
    public Transform originA;
    public Transform originB;

    

    //Color to use when the hand is to far or not connected.
    Color colorConnect = Color.green;
    Color colorBreak = Color.red;

    //the local distance grabber
    /*
    DistanceGrabber rightGrabber;
    DistanceGrabber leftGrabber;
    */
    bool grabberIsFound = false;

    [Header("Hand distance until the connection breaks")]
    public float distanceOffset;
    private bool RotateObject = true;


    private void Awake()
    {
        RotateBetweenTwoObject();
    }

    private void FixedUpdate()
    {
        //this assignment will be done only once.
        AssignTheDistanceGrabber();

        //measure the grabbing percentage of both hand
        MeasureHandGrip();

        //rotate the object based on the position of the hand
        if (RotateObject)
            RotateBetweenTwoObject();

        //check Distance between the actual grabbing position and the initial grabbing position.
        //only check if the cubbed is grabbed
        CheckDistance(grabA, originA);
        CheckDistance(grabB, originB);

        //ResetToDefault();
    }


    //Find the local distance grabber
    private void AssignTheDistanceGrabber()
    {
        if (grabberIsFound)
            return;

        /*
        //if the object still empty, try to find the local grabber
        if (!rightGrabber || !leftGrabber)
        {
            //the use of "Find" should be ok
            //because all the other player has their "Distancegrabbable turn off"
            //which means that the "Find" will not find them.
            GameObject right = GameObject.Find("DistanceGrabHandRight");
            GameObject left = GameObject.Find("DistanceGrabHandLeft");
            if (!right || !left) // if object is not found then return
                return;

            //otherwise find the component of the distance grabber and assign them.
            rightGrabber = right.GetComponent<DistanceGrabber>();
            leftGrabber = left.GetComponent<DistanceGrabber>();
        }

        if (rightGrabber && leftGrabber)
            grabberIsFound = true;
        */
            
    }


    private void MeasureHandGrip()
    {
        /*
        grabA_percent = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch) * 100f;
        grabB_percent = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch) * 100f;

        grabA_text.text = grabA_percent.ToString("F2");
        grabB_text.text = grabB_percent.ToString("F2");
        //Debug.Log("value A: " + grabA_percent);
        //Debug.Log("value B: " + grabB_percent);

        
        //if (grabA_percent <= 70f || grabB_percent <= 70f)
            //ReleaseObject();
        */
        
    }



    //Rotate between two object
    private void RotateBetweenTwoObject()
    {
        Vector3 midpoint = (grabA.transform.position + grabB.transform.position) / 2;
        BoxOne.transform.position = midpoint;
        BoxOne.transform.LookAt(grabA.transform);
    }


    //check the distance from the origin point 
    private void CheckDistance(Transform _grab, Transform _originGrab)
    {
        /*
        _grab.GetComponent<DistanceGrabbable>().transform.rotation = new Quaternion(0, 0, 0, 0);


        //find the distance between the Grabbing hand and its original position
        float distance = Vector3.Distance(_grab.position, _originGrab.position);
        //Debug.Log("percent: " + distance);

        //calculate the percentage based on how far the hand has travelled from the original position
        float percent = (distanceOffset - distance) / distanceOffset;
        _originGrab.GetComponent<Renderer>().material.color = Color.Lerp(colorBreak, colorConnect, percent);

        //if the distance is too far, then break the connection
        if (distance >= distanceOffset)
        {
            //Debug.Log("Too FAR! Break connection");
            //if the component is not grabbed, don't need to do anything
            if (_grab.GetComponent<DistanceGrabbable>().isGrabbed)
            {
                ReleaseObject();
            }
            else
            {
                ResetGrabbingPosRot(_grab, _originGrab);
            }
        }
        */
    }

    //just to reset the grabbing position & rotation
    private void ResetGrabbingPosRot(Transform _grab, Transform _originGrab)
    {
        _grab.position = _originGrab.position;
        _grab.rotation = _originGrab.rotation;
    }


    //Find the game Object attached to the Hand and afterwards release them
    private void ReleaseObject()
    {
        /*
        //For right hand
        OVRGrabbable rightObject = rightGrabber.grabbedObject;
        if(rightObject)
            rightGrabber.ForceRelease(rightObject.GetComponent<DistanceGrabbable>());

        //For left hand
        OVRGrabbable leftObject = leftGrabber.grabbedObject;
        if (leftObject)
            leftGrabber.ForceRelease(leftObject.GetComponent<DistanceGrabbable>());
        */
        
    }
    private void ResetToDefault()
    {

        Rigidbody a = grabA.GetComponent<Rigidbody>();
        Rigidbody b = grabB.GetComponent<Rigidbody>();
        a.angularVelocity = Vector3.zero;
        b.angularVelocity = Vector3.zero;

        a.freezeRotation = true;
        b.freezeRotation = true;

        /*
        Rigidbody a = grabA.GetComponent<Rigidbody>();
        Rigidbody b = grabB.GetComponent<Rigidbody>();
        float speedA = a.velocity.magnitude;
        float speedB = b.velocity.magnitude;


        //if it falls weirdly then just reset the position
        if (speedA > MobileFloodConfig.maxFallingSpeed
            ||
            speedB > MobileFloodConfig.maxFallingSpeed)
        {

            //reset speed
            a.velocity = Vector3.zero;
            b.velocity = Vector3.zero;

            a.angularVelocity = Vector3.zero;
            b.angularVelocity = Vector3.zero;

            //reset position
            a.transform.position = new Vector3(
                                               a.transform.position.x,
                                               MobileFloodConfig.resetYpos,
                                               a.transform.position.z
                                               );


            b.transform.position = new Vector3(
                                               a.transform.position.x,
                                               MobileFloodConfig.resetYpos,
                                               a.transform.position.z
                                               );
        }

        */
    }

}
