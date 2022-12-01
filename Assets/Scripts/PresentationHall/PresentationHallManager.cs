using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;
using RoomService;

[RequireComponent(typeof(NetworkIdentity))]
public class PresentationHallManager : NetworkBehaviour
{
    public Canvas canvasControlPanel;

    [Header("Video List")]
    //to store all the Presentation's URL
    public List<string> videoUrlList = new List<string>();
    public Transform videoURLContent;
    public TextMeshProUGUI currentVideo;
    private string currentCacheVideo = "";

    [Header("Info Video")]
    public GameObject info_noVideoSelected;
    public GameObject info_stopVideos;

    [Header("Video Bubble ")]
    public VideoPlayer video360;
    public bool allowStream;
    
    [Header("Player Reference")]
    public TheRoomServices theRoomService;
    XRCameraReference xRCameraReference;

    [Header("Script Reference")]
    public SimpleObjectPool video_Prefab;
    private enum videoButtonHierarchy { button, cacheURL };


    // Start is called before the first frame update
    void Start()
    {
        video360.gameObject.SetActive(false);
        InitVideoButtonsList();
        canvasControlPanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        //if currently streaming make the 360 buble follow the player
        if (allowStream)
        {
            StreamURL();
        }

    }

    /// <summary>
    /// init the buttons for every url in the list
    /// </summary>
    public void InitVideoButtonsList()
    {
        //clear the content first
        foreach (Transform child in videoURLContent)
            Destroy(child.gameObject);

        //get all the string in the list
        foreach(string str in videoUrlList)
        {
            print("a " + str);
            //create the button and put it in a container
            var vidUrlButton = CreateVideoURLButton(str);
            vidUrlButton.transform.SetParent(videoURLContent, false);
        }
    }


    //function to create the bookmarked button in the website
    private GameObject CreateVideoURLButton(string _str)
    {
        //get the button
        var vidUrlButton = video_Prefab.GetObject();
        var fileName = ConverterFunction.extractFileNamefromURL(_str);

        //assign the name by getting it from the PlayerSyncVARHook
        vidUrlButton.transform.GetChild((int)videoButtonHierarchy.button)
                                  .GetComponentInChildren<TextMeshProUGUI>().text = fileName;


        //assign the name by getting it from the PlayerSyncVARHook
        vidUrlButton.transform.GetChild((int)videoButtonHierarchy.cacheURL).name = _str;


        //assign the button ability
        vidUrlButton.transform.GetChild((int)videoButtonHierarchy.button)
                                   .GetComponent<Button>().onClick.AddListener(delegate
                                   {
                                       //put your function to forced turning off screen here.
                                       currentVideo.text = fileName;
                                       currentCacheVideo = _str;
                                   }
                                   );
        return vidUrlButton;
    }

    //ui -> attached to the play button
    public void Ui_PlayVideoOnAllClients()
    {
        if (currentCacheVideo == "")
        {
            StartCoroutine(CoroutineExtensions.HideAfterSeconds(info_noVideoSelected, 3f));
            return;
        }
            

        //sent the link to server
        Cmd_ChangeUrl(currentCacheVideo);
    }

    //sent to server

    //to tell the server to start the tutorial.
    [Command(requiresAuthority = false)]
    public void Cmd_ChangeUrl(string _str) { Rpc_ChangeUrl(_str); }
    
    //set to client
    [ClientRpc]
    public void Rpc_ChangeUrl(string _str) { ChangeUrlVideo(_str); }

    //Start the video
    private void ChangeUrlVideo(string _str)
    {
        //get the position on the XRCameraReference.
        xRCameraReference = theRoomService.theLocalPlayer.GetComponentInChildren<XRCameraReference>();
        allowStream = true;


        video360.gameObject.SetActive(true);
        video360.url = _str;
        video360.Play();
    }

    double vidTime;
    double currentVidTime;
    private void StreamURL()
    {
        if (!xRCameraReference) // just in case there is not xr camera reference then assign it.
            theRoomService.theLocalPlayer.GetComponentInChildren<XRCameraReference>();

        //make sure to follow the head position
        video360.transform.position = xRCameraReference.transform.position;

        //also check if the Video has finish
        vidTime = video360.url.Length;
        currentVidTime = video360.time;
        if (currentVidTime >= vidTime)
        {
            allowStream = false;
            video360.Stop();
            video360.gameObject.SetActive(false);
        }
    }


    //send stop video message to all clients
    public void Ui_StopVideoOnAllClients()
    {
        Cmd_StopVideoUrl();
    }
    
    
    //to tell the server to start the tutorial.
    [Command(requiresAuthority = false)]
    public void Cmd_StopVideoUrl() { Rpc_StopVideoUrl(); }

    //set to client
    [ClientRpc]
    public void Rpc_StopVideoUrl() { StopVideo(); }

    //change all the url
    public void StopVideo()
    {
        currentCacheVideo = "";
        allowStream = false;
        video360.Stop();
        video360.gameObject.SetActive(false);
        StartCoroutine(CoroutineExtensions.HideAfterSeconds(info_stopVideos, 3f));
    }
}
