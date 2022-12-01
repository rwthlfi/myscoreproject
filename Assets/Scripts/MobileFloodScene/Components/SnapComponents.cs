using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapComponents : MonoBehaviour
{
    public bool isScrew = false;

    [Header("The name of the object to be detected")]
    public string colliderNameToDetect = "Fill the name";
    public RigidbodyConstraints unsnapConstraint = RigidbodyConstraints.None;

    
    //[System.NonSerialized]
    public bool isCollided = false;
    
    public bool allowOnTriggerExit = false;

    private void Update()
    {
        
    }

    //triggered when the object enterred the defined-named collider.
    private void OnTriggerEnter(Collider _object)
    {
        //check if the collider name and it is not yet collided
        if(_object.name == colliderNameToDetect && !isCollided)
        {
            isCollided = true;
            allowOnTriggerExit = false;
            SnapTheObject(_object);
            //wait for x seconds, then enable the on trigger exit
        }
    }


    
    //when the collider it not there anymore.
    //then unsnap the object
    private void OnTriggerExit(Collider _other)
    {
        if (!allowOnTriggerExit)
            return;

        if (_other.name == colliderNameToDetect)
        {
            StartCoroutine(UnsnapTheObject(_other));
        }
    }
    

    //snap the object to the Defined-named collider
    private void SnapTheObject(Collider _other)
    {
        Debug.Log("trying to snap");

        //set the position to the the center of the collider
        GetComponent<ObjectAuthorities>().enabled = false;
        GetComponent<XRGrabInteractable>().enabled = false;

        //freeze the position
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<Rigidbody>().isKinematic = true;


        StartCoroutine(LerpingExtensions.MoveTo(transform, _other.transform.position, 1f));
        //transform.rotation = _other.transform.rotation;
        StartCoroutine(LerpingExtensions.RotateTo(transform, _other.transform.rotation, 1f));

        //wait for x seconds then enable the OnTriggerExit
        StartCoroutine(EnableOnTriggerExit(true, 1.1f));
    }

    /// <summary>
    /// unsnap the object
    /// </summary>
    /// <param name="_other"></param>
    private IEnumerator UnsnapTheObject(Collider _other)
    {
        allowOnTriggerExit = false;
        Debug.Log("trying to unsnap");
        GetComponent<Rigidbody>().constraints = unsnapConstraint;
        GetComponent<Rigidbody>().isKinematic = false;

        GetComponent<ObjectAuthorities>().enabled = true;
        GetComponent<XRGrabInteractable>().enabled = true;
        /*
        if (isScrew)
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);

        }
        */
        yield return new WaitForSeconds(1.1f);

        isCollided = false;
    }



    private IEnumerator EnableOnTriggerExit(bool _value, float _sec)
    {
        allowOnTriggerExit = !_value;
        yield return new WaitForSeconds(_sec);
        allowOnTriggerExit = _value;

    }

    public void EnableAllConstraint()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }
}
