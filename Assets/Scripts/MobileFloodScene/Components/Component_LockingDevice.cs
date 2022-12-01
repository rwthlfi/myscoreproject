using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using System;

public class Component_LockingDevice : NetworkBehaviour
{
    [Header("GameObject Locking components")]
    public Component_LockingScrew lockMainScrew;
    public Component_LockingScrew lockingScrew_0;
    public Component_LockingScrew lockingScrew_1;

    [Header("GameObject For reseting")]
    public Transform lockMainTrans;
    [NonSerialized] public Vector3 oriPos_mainScrew;
    [NonSerialized] public Quaternion oriRot_mainScrew;


    public Transform lockScrewTrans_0;
    [NonSerialized] public Vector3 oriPos_Screw0;
    [NonSerialized] public Quaternion oriRot_Screw0;


    public Transform lockScrewTrans_1;
    [NonSerialized] public Vector3 oriPos_Screw1;
    [NonSerialized] public Quaternion oriRot_Screw1;


    [Header("UI info variale")]
    public GameObject info_mainScrewIsNotScrewed;


    //for resetting all the object
    [NonSerialized] public Vector3 originalPos;
    [NonSerialized] public Quaternion originalRot;


    // Start is called before the first frame update
    void Start()
    {

        //store the original position and rotation for later usage.
        StoreOriginalPosAndRot();
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

        }
    }



    //check if all the component is properly screwed
    public bool isAllLocksProperlyScrewed()
    {
        if (!lockMainScrew.isLockScrewIn()
            || !lockingScrew_0.isLockScrewIn() 
            || !lockingScrew_1.isLockScrewIn()
           )
        {
            return false;
        }

        return true;
    
    }


    private void StoreOriginalPosAndRot()
    {
        //the main component
        originalPos = transform.position;
        originalRot = transform.rotation;
        
        
        //the child screws
        oriPos_mainScrew = lockMainTrans.localPosition;
        oriRot_mainScrew = lockMainTrans.localRotation;
        oriPos_Screw0 = lockScrewTrans_0.localPosition;
        oriRot_Screw0 = lockScrewTrans_0.localRotation;
        oriPos_Screw1 = lockScrewTrans_1.localPosition;
        oriRot_Screw1 = lockScrewTrans_1.localRotation;


    }

    //this function is to return the state of the object Transform pos and rot.
    //it is being called from the MobileFloodManager
    public IEnumerator ResetTransform()
    {
        yield return new WaitForSeconds(0.5f);
        transform.position = originalPos;
        transform.rotation = originalRot;

        //the child screws
        lockMainTrans.localPosition = oriPos_mainScrew;
        lockMainTrans.localRotation = oriRot_mainScrew;

        lockScrewTrans_0.localPosition = oriPos_Screw0;
        lockScrewTrans_0.localRotation = oriRot_Screw0;
        lockScrewTrans_1.localPosition = oriPos_Screw1;
        lockScrewTrans_1.localRotation = oriRot_Screw1;


    }
    public void ResetPosRot()
    {
        ResetTransform();
        /*
        lockMainScrew.ResetTransform();
        lockingScrew_0.ResetTransform();
        lockingScrew_1.ResetTransform();
        */
    }

}
