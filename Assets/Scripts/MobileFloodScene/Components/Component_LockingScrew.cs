
using UnityEngine;
using System;

public class Component_LockingScrew : MonoBehaviour
{
    [Header("Locking Screw var")]
    public bool mainLockingScrew = false;

    private Transform thisScrew;
    private Vector3 targetPos;
    public float offsetTolerance = 0.001f;

    [Header("UI variable")]
    public GameObject info_isNotScrewed;


    //for resetting all the object
    [NonSerialized] public Vector3 originalPos;
    [NonSerialized] public Quaternion originalRot;

    private void Awake()
    {
        //store the original position and rotation for later usage.
        StoreOriginalPosAndRot();
    }

    private void Start()
    {
        //get this screw gameObject for convinience use
        thisScrew = this.transform;


        //set the target position of the screw when it is screwed
        if (mainLockingScrew) // target position for the Main Locking Screw.
        {
            targetPos = new Vector3(thisScrew.localPosition.x,
                                    MobileFloodConfig.lockMainScrewDepth,
                                    thisScrew.localPosition.z);
        }

        else // change the target position to the locking screw
        {
            targetPos = new Vector3(MobileFloodConfig.lockScrewDepth,
                                    thisScrew.localPosition.y,
                                    thisScrew.localPosition.z);
        }
    }


    private float nextActionTime = 0.0f;
    private float period = 1f; // -> update every x seconds to reduce cpu consumption
    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
            //do things below 

            //activate the info according, if the screw is screwed or not
            info_isNotScrewed.SetActive(!isLockScrewIn());
        }


    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == MobileFloodConfig.drillScrewPoint)
        {
            if (!isLockScrewIn())
            {
                //do something here
                //move deeper
                thisScrew.localPosition = Vector3.Lerp(thisScrew.localPosition, 
                                                              targetPos, 
                                                              Time.deltaTime * 0.75f);

                //Rotate the screw
                if (mainLockingScrew) // its the main screw.
                {
                    thisScrew.localRotation *= Quaternion.AngleAxis(MobileFloodConfig.screwingSpeed / 2f * Time.deltaTime, Vector3.down);
                }

                else // its the normal screw
                {
                    thisScrew.localRotation *= Quaternion.AngleAxis(MobileFloodConfig.screwingSpeed / 2f * Time.deltaTime, Vector3.left);
                }

            }
        }
    }


    /// <summary>
    /// check if the screw is in or not
    /// </summary>
    /// <param name="_lockingScrew">which screw should we check</param>
    /// <returns></returns>
    public bool isLockScrewIn()
    {
        //the main screw
        if (mainLockingScrew 
            && thisScrew.localPosition.y <= MobileFloodConfig.lockMainScrewDepth - offsetTolerance)
        {
            return true;
        }


        //the normal Locking screw
        else if(!mainLockingScrew 
                && thisScrew.localPosition.x >= MobileFloodConfig.lockScrewDepth - offsetTolerance)
        {
                return true;
        }

        return false;
    }


    private void StoreOriginalPosAndRot()
    {
        originalPos = transform.localPosition;
        originalRot = transform.localRotation;
    }

    //this function is to return the state of the object Transform pos and rot.
    //it is being called from the MobileFloodManager
    public void ResetTransform()
    {
        transform.localPosition = originalPos;
        transform.localRotation = originalRot;

    }

}
