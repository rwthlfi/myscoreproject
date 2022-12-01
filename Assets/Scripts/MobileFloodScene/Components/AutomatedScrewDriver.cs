using UnityEngine;
using System;


public class AutomatedScrewDriver : MonoBehaviour
{
    [Header("Object Components")]
    private Transform screwingPoint;
    private SimpleTriggerDetection simpleTriggerDetection;


    //for resetting all the object
    [NonSerialized] public Vector3 originalPos;
    [NonSerialized] public Quaternion originalRot;

    private void Awake()
    {
        screwingPoint = GetComponentInChildren<SimpleTriggerDetection>().gameObject.transform;
        simpleTriggerDetection = GetComponentInChildren<SimpleTriggerDetection>();

        //store the original position and rotation for later usage.
        StoreOriginalPosAndRot();
    }


    // Update is called once per frame
    void Update()
    {
        if (!simpleTriggerDetection.mainColliderParent)
            return;

        if (ConverterFunction.ContainsAny(simpleTriggerDetection.mainColliderParent.name, MobileFloodConfig.screw)
            && !isScrewIn())
        {
            Screwing_In();
            //print("aa");
        }



        else if (ConverterFunction.ContainsAny(simpleTriggerDetection.mainColliderParent.name, MobileFloodConfig.Plug) 
                 && !isScrewOut())
        {
            Screwing_Out();
        }



    }

    //screwing the screw in.
    private void Screwing_In()
    {
        //rotate the screwing point and then of course, the Screw
        screwingPoint.localRotation *= Quaternion.AngleAxis(MobileFloodConfig.screwingSpeed * Time.deltaTime, Vector3.back);

        //The Actual screw
        var screwRoot = simpleTriggerDetection.mainColliderParent;
        screwRoot.localRotation *= Quaternion.AngleAxis(MobileFloodConfig.screwingSpeed * Time.deltaTime, Vector3.up);

        //Debug.Log("screw: " + screwRoot);
        //and push the screw further down on the y axis
        Vector3 maxDepth = new Vector3(
                                        screwRoot.position.x
                                        , MobileFloodConfig.maxScrewDepth
                                        , screwRoot.position.z
                                      );

        screwRoot.position = Vector3.Lerp(screwRoot.position, maxDepth, Time.deltaTime * MobileFloodConfig.screwingSpeed/500);


    }

    //is the screw is in place ? 
    public bool isScrewIn()
    {
        var screwRoot = simpleTriggerDetection.mainColliderParent;
        if (screwRoot.position.y <= MobileFloodConfig.maxScrewDepth + MobileFloodConfig.maxScrewTolerance) //plus a bit of constant to play on
            return true;
        else
            return false;
    }
    

    // - - - - - - -  SCREW OUUUUTTT - - - - - - - //
    //screwing out the screw!
    private void Screwing_Out()
    {
        //rotate the Drill's screwing point and the of course, 
        screwingPoint.localRotation *= Quaternion.AngleAxis(MobileFloodConfig.screwingSpeed * Time.deltaTime, Vector3.forward);

        //The Actual screw
        var screwRoot = simpleTriggerDetection.mainColliderParent;
        screwRoot.localRotation *= Quaternion.AngleAxis(MobileFloodConfig.screwingSpeed * Time.deltaTime, Vector3.down);

        //and push the screw further up on the y axis
        Vector3 maxDepth = new Vector3(
                                        screwRoot.position.x
                                        , MobileFloodConfig.maxPlugOut
                                        , screwRoot.position.z
                                      );

        screwRoot.position = Vector3.Lerp(screwRoot.position, maxDepth, Time.deltaTime * MobileFloodConfig.screwingSpeed / 500);


    }
    //the counterpart of screwing in
    public bool isScrewOut()
    {
        var screwRoot = simpleTriggerDetection.mainColliderParent;
        if (screwRoot.position.y >= MobileFloodConfig.maxPlugOut - MobileFloodConfig.maxScrewTolerance) //plus a bit of constant to play on
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
