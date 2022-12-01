using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyTexture : MonoBehaviour
{
    [Header("AllowablePlatform")]
    GlobalSettings.Device currentDevice;

    [Header("Origin Texture")]
    public RawImage originMat;
    private Texture originTexture;
    
    [Header("Destination Texture")]
    public MeshRenderer destinationMat;

    private void Start()
    {
        //get the platform., 
        currentDevice = GlobalSettings.DeviceType();

        //if it is on android, then turn it on.
        if (currentDevice == GlobalSettings.Device.Android)
            this.gameObject.SetActive(true);

        //otherwise disable this gameObj
        else
            this.gameObject.SetActive(false);


    }

    private void Update()
    {
        Copying();
    }


    private void Copying()
    {
        //Get the origin texture
        originTexture = originMat.mainTexture;

        //Copy the texture
        destinationMat.material.mainTexture = originTexture;
    }

}
