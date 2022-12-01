using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace AvatarCreationHandTracking
{
    public class GrabAnalyser : MonoBehaviour
    {
        [Header("GameObject Variable")]
        public GameObject targetObj;
        public Rigidbody targetRb;
        public NetworkIdentity targetNi;
        [System.NonSerialized] public bool isForced = false;
        float forceCooldown = 0.15f;

        //Logging Variable
        [System.NonSerialized]
        public List<Vector3> posRecord = new List<Vector3>();


        [Header("Server variable")]
        public ObjectAuthorities objectAuthority;


        //initialize the variable needed for the network synchronization
        public void SetVariable(GameObject _other)
        {
            if (!targetRb) // null reference check
                targetRb = _other.GetComponent<Rigidbody>();

            if (!targetNi) // null reference check
                targetNi = _other.GetComponent<NetworkIdentity>();

            if (!objectAuthority)
                objectAuthority = _other.GetComponent<ObjectAuthorities>();
        }

        //desserialize the variable to free the memory.
        public void DeserializeVariable()
        {
            targetObj = null;
            targetRb = null;
            targetNi = null;
            objectAuthority = null;
        }


        //component to disable during grabbing
        public void EnablePhysic(bool _bool)
        {
            //Null reference Checker.
            if (!targetRb)
            {
                if (targetObj)
                    targetRb = targetObj.GetComponent<Rigidbody>();

                return;
            }


            //Enable and Disable Gravity
            //Server 
            if (objectAuthority)
                objectAuthority.CmdAllowPhysics(targetNi, _bool);
            //Locally
            targetRb.useGravity = _bool;

            if (!_bool)
            {
                targetRb.velocity = Vector3.zero;
                targetRb.angularVelocity = Vector3.zero;
            }

        }


        //Get and send command to the Network Identity
        public void SendMoveObjCommand(Transform _trans)
        {
            if (!targetNi)
                return;

            //tell the object that it has been grabbed.
            objectAuthority.isGrabbed = true;

            //print("SendCommand");

            Vector3 pos = _trans.TransformPoint(objectAuthority.posOff);
            Quaternion rot = _trans.rotation * Quaternion.Euler(objectAuthority.rotOff);


            objectAuthority.CmdMoveServerObj(targetNi,
                                            pos,
                                            rot);

            //send command to server
            //objectAuthority.CmdMoveServerObj(targetNi, _pos, _rot);
        }

        public void SendReleaseObjCommand()
        {
            if (!targetNi)
                return;

            //tell the object that it is not grabbed anymore
            objectAuthority.isGrabbed = false;

            //send command to server
            objectAuthority.CmdReleaseServerObj(targetNi);
        }

        public void SendAppliedForce(Vector3 _force)
        {
            if (!targetNi)
                return;

            //send command to server
            objectAuthority.CmdApplyForce(targetNi, _force);
        }


        //to log the position of the hand.
        public void LogPosition()
        {
            posRecord.Add(targetObj.transform.position);
            //count maximum x position, in which after, clear the list and add the last one.
            if (posRecord.Count > 30)
            {
                Vector3 cachePos = posRecord[30];
                posRecord.Clear();
                posRecord.Add(cachePos);
                //and thus the circle of loggin begin again .
            }
        }

        public Vector3 GetForce()
        {
            //Rigidbody rb = selectTarget.GetComponent<Rigidbody>();
            //check if there is a position being recorded.
            if (posRecord.Count > 1)
            {
                if (!targetObj)
                    return Vector3.zero;
                //get the difference between position of x seconds ago and the position now
                Vector3 diff = (targetObj.transform.position - posRecord[0]);

                //clear the record
                posRecord.Clear();

                //Debug.Log("Force " + diff);
                return diff;
                //apply the force according to the diff
                //rb.AddForce(diff * 500f);
            }

            //Debug.Log("No Force?");
            return Vector3.zero;
        }


        //for creating the cooldown in order not to apply many forces.
        public IEnumerator ForceCooldownRoutine()
        {
            isForced = true;
            yield return new WaitForSeconds(forceCooldown);
            isForced = false;

        }
    }
}