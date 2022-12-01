using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Mirror;


//DONT FORGET TO MAKE THE COLLIDER SMALLER
public class GrabObjectsDetectors : MonoBehaviour
{
    [Header("XR ToolkitVariable")]
    private bool useSteamVR = false;
    public XRController controller = null;
    public XRDirectInteractor xrDirectInt = null;



    [Header("GameObject Variable")]
    public GameObject targetObj;
    public Rigidbody targetRb;
    public NetworkIdentity targetNi;
    bool isForced = false;
    float forceCooldown = 0.15f;

    //Logging Variable
    private List<Vector3> posRecord = new List<Vector3>();

    [Header("Server variable")]
    public ObjectAuthorities objectAuthority;


#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    private SteamVRBasedController steamVRBasedController = null;
#endif


    private void Start()
    {
        //setup the variable
        //objectAuthority = GetComponentInParent<ObjectAuthorities>();
        controller = this.GetComponent<XRController>();
        xrDirectInt = this.GetComponent<XRDirectInteractor>();

        //setup the controller for steamVr system.
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        useSteamVR = controller.GetComponentInParent<AvatarCreation.VRPlatformDetector>().useSteamVR;
        if (useSteamVR)
            steamVRBasedController = controller.GetComponent<SteamVRBasedController>();
#endif
    }

    private float nextActionTime = 0.0f;
    private float period = 0.01f;

    private void Update()
    {
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
                SendMoveObjCommand(this.transform);
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


    /*

    //get the Trigger button if it push or not
    private bool isGripping()
    {
        //check Grip from SteamVR.
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (useSteamVR){
            if (steamVRBasedController.gripValue > 0.5f) return true;
            else return false;
        }
#endif

        //Check Grip from Oculus
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue)){
            if (gripValue > 0.5f) return true;
            else return false;
        }

        print("No VR Input Detected");
        return false;
    }
    */

    //to check if the player is currently grabbing an object or not
    private bool hasItem()
    {
        //if there is an object being grabbed.
        if (xrDirectInt.selectTarget)
        {
            targetObj = xrDirectInt.selectTarget.gameObject;
            SetVariable(targetObj);
            //print("obj " + xrDirectInt.selectTarget.name);
            return true;
        }
        else
        {
            return false;
        }
    }

    //initialize the variable needed for the network synchronization
    private void SetVariable(GameObject _other)
    {
        if (!targetRb) // null reference check
            targetRb = _other.GetComponent<Rigidbody>();

        if (!targetNi) // null reference check
            targetNi = _other.GetComponent<NetworkIdentity>();

        if (!objectAuthority)
            objectAuthority = _other.GetComponent<ObjectAuthorities>();
    }

    //desserialize the variable to free the memory.
    private void DeserializeVariable()
    {
        targetObj = null;
        targetRb = null;
        targetNi = null;
        objectAuthority = null;
    }






    //component to disable during grabbing
    private void EnablePhysic(bool _bool)
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
    private void SendMoveObjCommand(Transform _trans)
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

    private void SendReleaseObjCommand()
    {
        if (!targetNi)
            return;

        //tell the object that it is not grabbed anymore
        objectAuthority.isGrabbed = false;

        //send command to server
        objectAuthority.CmdReleaseServerObj(targetNi);
    }

    private void SendAppliedForce(Vector3 _force)
    {
        if (!targetNi)
            return;

        //send command to server
        objectAuthority.CmdApplyForce(targetNi, _force);
    }


    //to log the position of the hand.
    void LogPosition()
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

    private Vector3 GetForce()
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
    private IEnumerator ForceCooldownRoutine()
    {
        isForced = true;
        yield return new WaitForSeconds(forceCooldown);
        isForced = false;

    }
}
