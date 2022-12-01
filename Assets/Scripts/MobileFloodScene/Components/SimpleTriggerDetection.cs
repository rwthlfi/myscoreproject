using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]

public class SimpleTriggerDetection : MonoBehaviour
{
    [Header("Canvas Object")]
    public GameObject _info;

    [Header("The name of the object that need to be detected")]
    public string colliderNameToDetect = "Fill the name";

    [System.NonSerialized] public Transform mainColliderParent;
    [System.NonSerialized] public Collider collidedObject;

    [System.NonSerialized] public bool isCollided = false;
    [System.NonSerialized] public float stayDuration = 0f;


    private void Awake()
    {
    }


    private void OnTriggerStay(Collider _object)
    {
        //check if the collider name and it is not yet collided
        if (ConverterFunction.ContainsAny(_object.name, colliderNameToDetect))
        {
            CountStayingDuration();
            //Debug.Log("object Enter: " + _object);
            collidedObject = _object;
        }
    }

    //triggered when the object enterred the defined-named collider.
    private void OnTriggerEnter(Collider _object)
    {
        //Debug.Log("print " +_object.name);
        //check if the collider name and it is not yet collided
        //if (_object.name == colliderNameToDetect)
        if(ConverterFunction.ContainsAny(_object.name, colliderNameToDetect))
        {
            isCollided = true;
            collidedObject = _object;
            mainColliderParent = _object.transform.parent;
        
        }
    }


    //when the collider it not there anymore.
    //then unsnap the object
    private void OnTriggerExit(Collider _object)
    {
        //Debug.Log("object Exit: " + _object.name);
        //if (_object.name == colliderNameToDetect)
        if (ConverterFunction.ContainsAny(_object.name, colliderNameToDetect))
        {
            isCollided = false;
            collidedObject = null;
            mainColliderParent = null;
            //Debug.Log("durationstay : " + stayDuration);
            resetStayingDuration();
            //Debug.Log("duration Exit : " + stayDuration);
        }
    }

    //check how long the object has stayed.
    private void CountStayingDuration()
    {
        stayDuration += Time.deltaTime;
    }

    private void resetStayingDuration()
    {
        stayDuration = 0f;
    }





    //function to show or Hide corresponding info to player.
    public void ShowInfo()
    {
        if(!_info)
        {
            Debug.Log("no info is attached, do you need it?");
            return;
        }

        _info.gameObject.SetActive(true);
    }

    //disable the info that is not needed
    public void DisableInfo()
    {
        if (!_info)
        {
            Debug.Log("no info is attached, do you need it?");
            return;
        }

        _info.gameObject.SetActive(false);
    }
}
