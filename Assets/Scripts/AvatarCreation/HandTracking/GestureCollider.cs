using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//this script is use to grab the object since there is no way that OVR grabber can be used along with the XR.
//i might be wrong though.
public class GestureCollider : MonoBehaviour
{
    public Transform currentObject;

    private void OnTriggerEnter(Collider other)
    {
        //if there is no object 
        //and the component has an interactable element in it
        //then assign it
        if (!currentObject && other.gameObject.GetComponent<XRGrabInteractable>())
        {
            currentObject = other.transform;
            print("Obj " + currentObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //if the object that exit the trigger is the same object then remove it
        if(currentObject == other.transform)
            currentObject = null;
    }

}
