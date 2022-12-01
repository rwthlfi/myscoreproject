using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(Rigidbody))]
//Attach this script to the AP-1002k Object
//This is to manage the foundation component on what has been attached, and what is not.
public class Component_Foundation : MonoBehaviour
{
    //for resetting all the object
    [NonSerialized] public Vector3 originalPos;
    [NonSerialized] public Quaternion originalRot;
    

    [Header("Canvas Object")]
    public Transform info_removePlugs;
    public Transform info_attachSupportColumn;
    public Transform info_attachScrews;

    [System.NonSerialized]
    public List<Collider> ColliderList = new List<Collider>();

    private void Awake()
    {
        //store the original position and rotation for later usage.
        StoreOriginalPosAndRot();
    }

    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        ColliderList.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        ColliderList.Remove(other);
    }


    //To check if the requested collider is exist or not.
    public bool collider_isExist(string _gameObjectName)
    {

        foreach(Collider c in ColliderList)    
        {
            if (c == null)
                continue;
            if (ConverterFunction.ContainsAny(c.gameObject.name, _gameObjectName))
                return true;
        }
        return false;
    }

    //to return the requested gameobject via collider check
    public GameObject gameObjectRequested(string _gameObjectName)
    {
        foreach (Collider c in ColliderList)
        {
            if (c.gameObject.name == _gameObjectName)
                return c.transform.root.gameObject;
        }
        return null;
    }

    public int collider_count(string _gameObjectName)
    {
        int total = 0;
        foreach (Collider c in ColliderList)
        {
            if (c.gameObject.name == _gameObjectName)
                total++;
        }
        return total;
    }

    //Show the corresponding info to the player.
    public void ShowInfo(Transform _info)
    {
        DisableAllInfo();
        _info.gameObject.SetActive(true);
    }

    //disable the info that is not needed
    public void DisableInfo(Transform _info)
    {
        _info.gameObject.SetActive(false);
    }

    public void DisableAllInfo()
    {
        info_removePlugs.gameObject.SetActive(false);
        info_attachSupportColumn.gameObject.SetActive(false);
        info_attachScrews.gameObject.SetActive(false);
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
