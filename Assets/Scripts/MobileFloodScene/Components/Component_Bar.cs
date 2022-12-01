using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_Bar : MonoBehaviour
{
    //position and rotation of the original transform
    //remember that you the object is controlled by the "Grab points"
    [System.NonSerialized] public Vector3 originalPos_mainBody;
    [System.NonSerialized] public Quaternion originalRot_mainBody;


    private void Awake()
    {
        StoreOriginalPosAndRot();
    }

    //to store the orignal position and rotation for later usage.
    private void StoreOriginalPosAndRot()
    {
        originalPos_mainBody = this.transform.position;
        originalRot_mainBody = this.transform.rotation;
    }

    //this function is to return the state of the object Transform pos and rot.
    //it is being called from the MobileFloodManager
    public IEnumerator ResetTransform()
    {
        yield return new WaitForSeconds(0.5f);
        this.transform.position = originalPos_mainBody;
        this.transform.rotation = originalRot_mainBody;
    }
}
