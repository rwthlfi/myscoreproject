using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace AvatarCreationHandTracking
{
    [RequireComponent(typeof(GestureCollider))]
    public class GestureEvent : MonoBehaviour
    {
        [SerializeField]
        private bool leftHand = false;

        [Header("Press & Release Variable")]
        public GestureUIPointer gestureUIPointer;
        private bool currentlyPress = false;

        [Header("Interactor Variable")]
        public GestureCollider gestureCollider;
        public Transform attachPoint;
        public GameObject currentNetworkedObj;

        [Header("Teleport Ability")]
        public GestureTeleport gestureTeleport;

        [Header("Pointing Ability")]
        public Stabilizer handStabilizer; // Yes there are two stablizer., one of them is attached to CustomHand
        public Stabilizer indexfingerStabilizer; // Yes there are two stablizer., one of them is attached to the finger tips


        private void Start()
        {
            gestureCollider = GetComponent<GestureCollider>();
        }


        //releasing gesture..., usually use to simulate mouse up
        public void Release()
        {
            ChangeStabilizer_ToHand(true);
            gestureUIPointer.Release();
            gestureUIPointer.ShowRay(true);
            currentlyPress = false;
            //print(handSide() + "Release");
        }

        //releasing gesture..., usually use to simulate mouse down
        public void Press()
        {
            //so it will only be invoke once until the "Release" event is triggered;
            if (currentlyPress)
                return;
            ChangeStabilizer_ToHand(true);
            currentlyPress = true;
            gestureUIPointer.Press();
            //print(handSide() + "Press");

        }


        //Raymond: Do we need this..? 
        public void Pointing()
        {
            ChangeStabilizer_ToHand(false);

            PointingTouchEvent();
            //print(handSide() + "pointing");

        }

        public void MetalTeleport()
        {
            ChangeStabilizer_ToHand(true);
            //print(handSide() + "metalTeleport");
            gestureTeleport.ActivatedTeleport(true);

        }

        public void GrabObject()
        {
            ChangeStabilizer_ToHand(true);
            //print(handSide() + "Grab an object");
            if (gestureCollider.currentObject)
            {
                //if it has object authorities, assign them, and let the movement be decided by the server
                if (gestureCollider.currentObject.GetComponent<ObjectAuthorities>())
                {
                    //assign the variable 
                    currentNetworkedObj = gestureCollider.currentObject.gameObject;
                }

                //otherwise follow the attachpoint. Because there is no networking
                else
                {
                    gestureCollider.currentObject.position = attachPoint.transform.position;
                    gestureCollider.currentObject.rotation = attachPoint.transform.rotation;
                }

            }
        }


        public void NoGesture()
        {
            //reset or deatching everyhing
            currentNetworkedObj = null;
            gestureTeleport.ActivatedTeleport(false);
            gestureUIPointer.Release();
            //gestureUIPointer.ShowRay(false);
            currentlyPress = false;
            //print(handSide() + "No Gesture");
        }


        //to get the object that is currently being raycasted
        //use for clicking or even teleporting the object

        RaycastHit hit;
        public LayerMask ValidLayers;
        public void RayCastingTheObject()
        {
            //if it doesnt hit anything, disable the clicking.
            if (Physics.Raycast(transform.position, transform.up, out hit, 1000, ValidLayers, QueryTriggerInteraction.Ignore))
            {
                print(hit.transform.gameObject.name);
            }

        }

        private void ChangeStabilizer_ToHand(bool _value)
        {
            handStabilizer.enabled = _value;
            indexfingerStabilizer.enabled = !_value;
        }

        float minDiff = 0.05f;
        float diff;
        public bool press = false;
        private void PointingTouchEvent()
        {
            //if it is not activated, dont do anything
            /*
            if (gestureUIPointer.lineRenderer.enabled == false)
            {
                diff = (gestureUIPointer.lineRenderer.GetPosition(0) - gestureUIPointer.lineRenderer.GetPosition(1)).magnitude;
                print("currentRay " + (gestureUIPointer.lineRenderer.GetPosition(0) - gestureUIPointer.lineRenderer.GetPosition(1)).magnitude);
                return;
            }
            */

            diff = (gestureUIPointer.lineRenderer.GetPosition(0) - gestureUIPointer.lineRenderer.GetPosition(1)).magnitude;
            //print("currentRay " + (gestureUIPointer.lineRenderer.GetPosition(0) - gestureUIPointer.lineRenderer.GetPosition(1)).magnitude);

            if (diff <= minDiff)
            {
                //then trigger press event
                press = true;
                Press();
            }

            if (press && diff >= minDiff)
            {
                press = false;
                //trigger release event
                Release();
            }

        }



        //just for debugging to know which hand is making the gesture
        private string handSide()
        {
            return leftHand ? "left: " : "right: ";
        }

    }
}