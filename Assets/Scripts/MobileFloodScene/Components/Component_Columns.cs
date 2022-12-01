using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class Component_Columns : NetworkBehaviour
{
    [Header("SyncVar Variable")]
    [SyncVar(hook = nameof(OnOilChanged))]
    public bool oiledValue = false;

    //position and rotation of the original transform
    [System.NonSerialized] public Vector3 originalPos;
    [System.NonSerialized] public Quaternion originalRot;

    [Header("Canvas Object")]
    public Transform info_LubricateThecolumn;
    public Transform info_attachLockingDevice;

    [Header("Renderer Object")]
    public Renderer rubber;

    [Header("Script referens")]
    public MobileFloodSceneUI mobilesSceneUI;



    //synchro the oil value
    void OnOilChanged(bool _old, bool _new)
    {
        //do something here.
        if (_new)
            rubber.material.color = MobileFloodConfig.colorOiled;
        else
            rubber.material.color = MobileFloodConfig.colorNotOiled;
    }



    // Start is called before the first frame update
    void Start()
    {
        rubber.material.color = Color.white;
        //set the initial Color of the rubber
        StoreOriginalPosAndRot();
    }


    private float nextActionTime = 0.0f;
    public float period = 0.25f;
    private void Update()
    {
        
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
            //execute every x seconds
            //print("mobile flood ID " + mobilesSceneUI.stepID + ": " + oiledValue);

            //if the column is not oiled yet and However, the server tells that the mobile flood is already pass the lubricate column
            //then change the color.
            if (isClient)
            {
                // if the client is not yet oiled -> sync the value
                if (oiledValue)
                    ChangeColor();
            }



            //server will check its color.
            if (isServer)
            {
                SetOiledValue();

            }
        }
    }


    //change the color accordingly
    public void ChangeColor()
    {
        rubber.material.color = Color.Lerp(rubber.material.color,
                                            MobileFloodConfig.colorOiled,
                                            MobileFloodConfig.oiledDuration * Time.deltaTime * 0.25f
                                          );
    }

    //check if the part already oiled.
    public bool isOiled()
    {
        //just compare the Red color, since the RGB will have the same value (black color)
        if (rubber.material.color.r <= MobileFloodConfig.colorTolerance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    //Show the corresponding info to the player.
    public void ShowInfo(Transform _info)
    {
        _info.gameObject.SetActive(true);
    }

    //disable the info that is not needed
    public void DisableInfo(Transform _info)
    {
        _info.gameObject.SetActive(false);
    }


    //to store the orignal position and rotation for later usage.
    private void StoreOriginalPosAndRot()
    {
        originalPos = this.transform.position;
        originalRot = this.transform.rotation;


    }

    //this function is to return the state of the object Transform pos and rot.
    //it is being called from the MobileFloodManager
    public void ResetTransform()
    {
        this.gameObject.SetActive(false);
        this.transform.position = originalPos;
        this.transform.rotation = originalRot;

        this.gameObject.SetActive(true);

        
        //ResetTransform the oil
        //Cmd_ResetOiled();
        rubber.material.color = MobileFloodConfig.colorNotOiled;

        //hide the message
        DisableInfo(info_attachLockingDevice);
        DisableInfo(info_LubricateThecolumn);
        
    }

    [Server]
    private void SetOiledValue()
    {
        oiledValue = isOiled();
    }
    
    public void resetOiled()
    {
        print("Reset");
        rubber.material.color = MobileFloodConfig.colorNotOiled;
    }

}
