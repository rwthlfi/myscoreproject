using System;
using UnityEngine;

public class OilSprayCan : MonoBehaviour
{
    [Header("Object Components")]
    public Transform sprayingPoint;
    private SimpleTriggerDetection simpleTriggerDetection;

    //for resetting all the object
    [NonSerialized] public Vector3 originalPos;
    [NonSerialized] public Quaternion originalRot;

    private void Awake()
    {
        simpleTriggerDetection = GetComponentInChildren<SimpleTriggerDetection>();

        //store the original position and rotation for later usage.
        StoreOriginalPosAndRot();
    }

    // Update is called once per frame
    void Update()
    {
        //is the spraying point already collided with the rubber stuff. and if it is already been sprayed
        if (simpleTriggerDetection.isCollided && !isSprayed())
        {
            //if not then spray it!
            //which will change the material color.
            Spraying(simpleTriggerDetection.mainColliderParent.gameObject);
        }
    }


    //get the rubber component and spray them on
    private void Spraying(GameObject _mainObj)
    {
        _mainObj.GetComponent<Component_Columns>().ChangeColor();
    }


    //is the screw is in place ? 
    public bool isSprayed()
    {
        //get the main parent of the component
        var mainObj = simpleTriggerDetection.mainColliderParent.gameObject;
        if (mainObj.GetComponent<Component_Columns>().isOiled())
            return true;

        else
            return false;
    }
    private void StoreOriginalPosAndRot()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;
    }

    //this function is to return the state of the object Transform pos and rot.
    //it is being called from the MobileFloodManager
    public void ResetTransform()
    {
        transform.position = originalPos;
        transform.rotation = originalRot;

    }
}
