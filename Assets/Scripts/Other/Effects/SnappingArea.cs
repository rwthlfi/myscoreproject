using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attach this script to the object that you want them to snapped
//for example, if a player enter a certain collider, then the panel on their hand will be "snapped" to the object transform
public class SnappingArea : MonoBehaviour
{
    [Tooltip("The name of the object to be detected")]
    public string colliderNameToDetect = "HipSensor";


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == colliderNameToDetect)
        {
            //Debug.Log("player has enter: " + other.name);
            SnapTheObject(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == colliderNameToDetect)
        {
            //Debug.Log("player is staying: " + other.name);
            SnapTheObject(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == colliderNameToDetect)
        {
            //Debug.Log("player has exit: " + other.name);
            unsnapTheObject(other);
        }
    }

    private void SnapTheObject(Collider _other)
    {
        //Get the main parent
        Transform mainParent = _other.transform.root;

        //Get the simple Panel reference
        SimplePanel sp = mainParent.GetComponentInChildren<SimplePanel>();

        //set the snapposition point to the this object.
        sp.GetComponent<MakeTransformTheSame>().snapPositionPoint = this.transform;

        //set the snap position to true
        sp.GetComponent<MakeTransformTheSame>().snapPosition = true;

    }

    private void unsnapTheObject(Collider _other)
    {
        //Get the main Parent
        Transform mainParent = _other.transform.root;

        //Get the simplepanel reference
        SimplePanel sp = mainParent.GetComponentInChildren<SimplePanel>();

        //Re-attach the makeTransformTothe hand.
        sp.GetComponent<MakeTransformTheSame>().snapPosition = false;
    }


}
