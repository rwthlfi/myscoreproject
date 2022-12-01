using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PoleGrip : MonoBehaviour
{
    public GrabPoint LeftHandGrip;
    public GrabPoint RightHandGrip;

    private Transform LeftHand;
    private Transform RightHand;

    /*
    public XRController tempRefHandLeft;
    public XRController tempRefHandRight;
    */
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (LeftHand)
        {
            Vector3 newpose = Vector3.Project((LeftHand.position - transform.position) * 100, transform.up) / 100;
            if (newpose.magnitude < GetComponent<CapsuleCollider>().height / 2 && !LeftHandGrip.Gripped)
            {
                LeftHandGrip.transform.parent.position = newpose + transform.position;
                LeftHandGrip.transform.parent.rotation = Quaternion.LookRotation(-((LeftHand.position) - LeftHandGrip.transform.parent.position), transform.up);

                LeftHandGrip.UpdateOffset();
                Debug.Log("set new pose");

            }
            else if (!LeftHandGrip.Gripped && newpose.magnitude > GetComponent<CapsuleCollider>().height / 2)
            {
                LeftHandGrip.transform.localPosition = new Vector3();
            }
        }
        if (RightHand)
        {
            Vector3 newpose = Vector3.Project((RightHand.position - transform.position) * 100, transform.up) / 100;
            if (newpose.magnitude < GetComponent<CapsuleCollider>().height / 2 && !RightHandGrip.Gripped)
            {
                RightHandGrip.transform.parent.position = newpose + transform.position;
                RightHandGrip.transform.parent.rotation = Quaternion.LookRotation(-((RightHand.position) - RightHandGrip.transform.parent.position), transform.up);

                RightHandGrip.UpdateOffset();
                //Debug.Log("set new pose");

            }
            else if (!RightHandGrip.Gripped && newpose.magnitude > GetComponent<CapsuleCollider>().height / 2)
            {
                RightHandGrip.transform.localPosition = new Vector3();
            }
        }

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Grabber>())
        {
            //get the hand
            XRController hand = other.transform.parent.GetComponent<GripController>().Hand;
            if (hand.controllerNode == XRNode.LeftHand)
            {
                LeftHand = other.transform;
                //disabled physics
                //hand.GetComponent<PhysicsPoser>().controllerObject.GetComponent<Collider>().isTrigger = true;
            }
            else if (hand.controllerNode == XRNode.RightHand)
            {
                RightHand = other.transform;
                //disabled physics
                //hand.GetComponent<PhysicsPoser>().controllerObject.GetComponent<Collider>().isTrigger = true;
            }
            //Debug.Log("found a grabber!");


        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Grabber>())
        {
            //Get the hand
            XRController hand = other.transform.parent.GetComponent<GripController>().Hand;
            if (other.transform.parent.GetComponent<GripController>().Hand.controllerNode == XRNode.LeftHand)
            {
                LeftHand = null;
                //enabled physics
                //hand.GetComponent<PhysicsPoser>().controllerObject.GetComponent<Collider>().isTrigger = false;
            }
            else if (other.transform.parent.GetComponent<GripController>().Hand.controllerNode == XRNode.RightHand)
            {
                RightHand = null;
                //enabled physics
                //hand.GetComponent<PhysicsPoser>().controllerObject.GetComponent<Collider>().isTrigger = false;
            }
            
        }
    }
}