using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attach this script to the component screw
public class Component_Plug : MonoBehaviour
{
    
    [System.NonSerialized] public Vector3 originalPos;
    [System.NonSerialized] public Quaternion originalRot;


    [Header("Canvas Object ")]
    public GameObject _infoScrewThis;

    private void Start()
    {
        StoreOriginalPosAndRot();
    }

    private void Update()
    {
        if (this.transform.position.y <= MobileFloodConfig.maxPlugOut)
            _infoScrewThis.gameObject.SetActive(true);

        else
            _infoScrewThis.gameObject.SetActive(false);
    }


    //check if the Screw is already screwed.
    public bool isAlreadyOut()
    {
        if (this.transform.position.y >= MobileFloodConfig.maxPlugOut - MobileFloodConfig.maxScrewTolerance)
            return true;
        else
            return false;
    }

    //Show the corresponding info to the player.
    public void ShowPlugThisInfo()
    {
        _infoScrewThis.gameObject.SetActive(true);
    }

    //disable the info that is not needed
    public void DisablePlugThisInfo()
    {
        _infoScrewThis.gameObject.SetActive(false);
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
        ShowPlugThisInfo();
    }
}
