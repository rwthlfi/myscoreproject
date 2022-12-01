using RoomService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using AvatarCreation;

public class Camera360Capture : MonoBehaviour
{
    [Header("Camera Variable")]
    public Camera targetCamera;
    public RenderTexture renderTexture_Cube;
    public RenderTexture renderTexture_equirect;
    public Material materialTarget;
    public bool allowStream = false;


    [Header("Triggering Variable")]
    public bool useTriggerEnter = false;
    public bool useButtonUI = false;

    [Header("Player Reference Variable")]
    public TheRoomServices theRoomService;
    XRCameraReference xRCameraReference;
    NetworkPlayerSetup ps = null;

    [Header("UI variable")]
    public Vector3 bloatedSize = new Vector3(50, 50, 50);

    [Header("allow Capture Panorama")]
    public bool allowCapturePanorama = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        //if not using trigger enter, forget it
        if (!useTriggerEnter)
            return;


        ps = other.transform.root.GetComponent<NetworkPlayerSetup>();
        //if player setup exist and it is a local player, then stream it.
        if (ps && ps.isLocalPlayer)
            allowStream = true;

    }


    private void OnTriggerExit(Collider other)
    {
        //if not using trigger enter, forget it
        if (!useTriggerEnter)
            return;

        if (other.transform.root.GetComponent<NetworkPlayerSetup>() == ps)
            allowStream = false;
    }


    // Update is called once per frame
    void Update()
    {
        if(allowCapturePanorama)
            Capture360();


        if (!allowStream) // if it doesnt allow the stream, dont do anything
            return;

        if(useButtonUI && allowStream) //put the buble in the users head always
            this.transform.position = xRCameraReference.transform.position;

        RenderImage();

    }

    /// <summary>
    /// to render the image to the 3D sphere
    /// </summary>
    private void RenderImage()
    {
        targetCamera.RenderToCubemap(renderTexture_Cube);

        //convert to equirect
        renderTexture_Cube.ConvertToEquirect(renderTexture_equirect);

        //render to Materials
        materialTarget.SetTexture("_MainTex", renderTexture_equirect);
    }

    public void UI_TriggerVR()
    {
        //get the room service
        xRCameraReference = theRoomService.theLocalPlayer.GetComponentInChildren<XRCameraReference>();
        //this.transform.SetParent(xRCameraReference.transform, false);

        //bloated up the 
        IncreaseVRBubble(bloatedSize);

        allowStream = true;
    }

    public void UI_ExitVR()
    {
        this.transform.localScale = Vector3.zero;
        allowStream = false;
    }

    public void IncreaseVRBubble(Vector3 _value)
    {
        StartCoroutine(LerpingExtensions.ScaleTo(this.transform, _value, 1f));
    }

    private void Capture360()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Render to Cubemap
            targetCamera.RenderToCubemap(renderTexture_Cube);

            //convert to equirect
            renderTexture_Cube.ConvertToEquirect(renderTexture_equirect);

            //Save to files
            Save360(renderTexture_equirect);
        }
    }

    int fileCount = 0;
    //to save the 360 pic in the Render Texture
    private void Save360(RenderTexture _rt)
    {
        Texture2D tex = new Texture2D(_rt.width, _rt.height);
        RenderTexture.active = _rt;

        tex.ReadPixels(new Rect(0, 0, _rt.width, _rt.height), 0, 0);
        RenderTexture.active = null;

        byte[] bytes = tex.EncodeToJPG();

        string path = Application.dataPath + "/Panaroma" + ".jpg";
        string filename_current = path;

        while (File.Exists(filename_current))
        {
            fileCount++;
            filename_current = Path.GetDirectoryName(path)
                             + Path.DirectorySeparatorChar
                             + Path.GetFileNameWithoutExtension(path)
                             + fileCount.ToString()
                             + Path.GetExtension(path);
        }

        File.WriteAllBytes(filename_current, bytes);



        Debug.Log("saved at: " + path);
    }


}
