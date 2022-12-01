using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the Glass Cube in the mobile flood
public class GlassCube : MonoBehaviour
{
    [System.NonSerialized] public Vector3 originalPos;
    [System.NonSerialized] public Quaternion originalRot;

    // Start is called before the first frame update
    void Start()
    {
        StoreOriginalPosAndRot();
    }


    //open the glass cube for the people to access the mobile flood component
    public void OpenGlassCube()
    {
        //origin is y = 1.5f
        if (this.transform.position.y <= MobileFloodConfig.glassCubeOpenY_pos + MobileFloodConfig.glassCubeTolerance)
            return; // means its already open

        //print("pos " + this.transform.position.y);

        Vector3 openPos = new Vector3(originalPos.x,
                                      MobileFloodConfig.glassCubeOpenY_pos,
                                      originalPos.z);

        StartCoroutine(LerpingExtensions.MoveTo(this.transform, openPos, 2f));
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
        StartCoroutine(LerpingExtensions.MoveTo(this.transform, originalPos, 2f));
    }
}
