using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_Cover : MonoBehaviour
{
    [NonSerialized] public Vector3 originalPos;
    [NonSerialized] public Quaternion originalRot;

    private void Awake()
    {
        StoreOriginalPosAndRot();
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
