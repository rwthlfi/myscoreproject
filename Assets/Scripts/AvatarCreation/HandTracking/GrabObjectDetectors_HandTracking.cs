using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AvatarCreationHandTracking
{
    //to assign an object for later use which is transfering the pos/rot to the server
    //this is different than "GrabObjectsDetectors_Controller", 
    //because this script will only be activated during the hand tracking, and not with controller
    [RequireComponent(typeof(GestureEvent))]
    public class GrabObjectDetectors_HandTracking : GrabAnalyser
    {
        [Header("Hand tracking variable")]
        public GestureEvent gestureEvent;

        //script reference
        //private GrabAnalyser grabAnalyser;

        // Start is called before the first frame update
        void Start()
        {
            gestureEvent = GetComponent<GestureEvent>();
        }


        private float nextActionTime = 0.0f;
        private float period = 0.1f;
        // Update is called once per frame
        void Update()
        {
            // Start record the player hands every x seconds
            if (Time.time > nextActionTime)
            {
                // Start record the player hands every x seconds
                nextActionTime += period;
                //check if the player has item or not.
                if (hasItem())
                {
                    EnablePhysic(false);
                    //LogPosition
                    LogPosition();
                    //send move command 
                    //(it should not be this.transform, but the pos & rot from "attachPoint"
                    //because it has the offset.
                    SendMoveObjCommand(gestureEvent.attachPoint);
                }

                //if the player doesnt have item and there is pos record
                else if (!hasItem() && posRecord.Count > 0)
                {
                    EnablePhysic(true);

                    //send release obj
                    SendReleaseObjCommand();

                    //send applicable force
                    if (!isForced)
                    {
                        isForced = true;
                        StartCoroutine(ForceCooldownRoutine()); // start the cooldown.
                        SendAppliedForce(GetForce());
                    }

                    DeserializeVariable();
                    //clear the pos record
                    posRecord.Clear();
                    //print("nothing");
                }
            }
        }

        //to check if the player is currently grabbing an object or not
        private bool hasItem()
        {
            //if there is an object being grabbed.
            if (gestureEvent.currentNetworkedObj)
            {
                targetObj = gestureEvent.currentNetworkedObj;
                SetVariable(targetObj);
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}