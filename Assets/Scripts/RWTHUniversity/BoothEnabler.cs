using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using RoomService;

public class BoothEnabler : MonoBehaviour
{
    [Header("Reference")]
    public NetworkIdentity currentPlayer;
    public NetworkIdentity cachePlayer;

    [Header("Things to disabled")]
    public List<GameObject> activatedList = new List<GameObject>();
    public bool useMediaLinkLoader = true; // if this one is true in the editor, you can just ignore to fill the mediaLinkLoader
    public MediaLinksLoader mediaLinkLoader;
    public bool useOnlyMediaVideoSync = false; // the same like "useMediaLinkLoader, However., you can just use one of these.
    public MediaVideoSync mediaVideoSync;


    private void Start()
    {
        //disable all the canvas object at the beginning
        activateAllUI(false);


        if (useOnlyMediaVideoSync)
        {
            if (mediaVideoSync.isServer)
            {
                mediaVideoSync.mediaPlane.gameObject.SetActive(true);
                mediaVideoSync.mediaVideoPlayer.url = mediaVideoSync.videoLink;
            }
            else
                mediaVideoSync.mediaPlane.gameObject.SetActive(false);

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //if current player is not yet assign, then do it.
        if ( currentPlayer == null && other.tag == "Player" && other.isTrigger == false)
        {
            cachePlayer = other.transform.root.GetComponent<NetworkIdentity>();
            if (cachePlayer.isLocalPlayer)
            {
                //assign the player.
                currentPlayer = cachePlayer;
                //do something here.
                activateAllUI(true);
                activateVideoElement(true);

                print("Enter " + currentPlayer.name + " id: " + currentPlayer.netId);

            }
        }

    }

    
    private void OnTriggerStay(Collider other)
    {
        //if player is empty then ->
        if (currentPlayer == null && other.tag == "Player" && other.isTrigger == false)
        {
            if(!cachePlayer)
                cachePlayer = other.transform.root.GetComponent<NetworkIdentity>();

            if (cachePlayer.isLocalPlayer)
            {
                //assign the player.
                currentPlayer = cachePlayer;
                //do something here.
                activateAllUI(true);
                activateVideoElement(true);
            }

            //print("Triggered ");
        }

    }
    
    

    
    private void OnTriggerExit(Collider other)
    {
        //if current player exist
        //&& the currentplayer is the local Player.
        if (currentPlayer && currentPlayer == other.transform.root.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            print("Exit " + currentPlayer.name + " id: " + currentPlayer.netId);
            currentPlayer = null;
            cachePlayer = null;

            //do something here.
            activateAllUI(false);
            activateVideoElement(false);
        }

    }
    


    private void activateAllUI(bool _value)
    {
        foreach(GameObject go in activatedList)
        {
            go.SetActive(_value);
        }
    }


    private void activateVideoElement(bool _value)
    {
        if (useMediaLinkLoader)
        {
            if (SQLloader.GetFileExtenstion(mediaLinkLoader.currentLink) == mediaLinkLoader.videoFormat)
            {
                //print("yep Video");
                mediaLinkLoader.mediaVideoSync.mediaPlane.gameObject.SetActive(_value);
                mediaLinkLoader.mediaImageSync.mediaPlane.gameObject.SetActive(!_value);
                if (_value)
                {
                    mediaLinkLoader.mediaVideoSync.mediaVideoPlayer.url = mediaLinkLoader.currentLink;
                    //print("yep Video true");

                }
            }
        }

        else if(useOnlyMediaVideoSync)
        {
            print("a " + SQLloader.GetFileExtenstion(mediaVideoSync.videoLink));

            if (SQLloader.GetFileExtenstion(mediaVideoSync.videoLink) == GlobalSettings.formatVideo)
            {
                print("yep Video");
                mediaVideoSync.mediaPlane.gameObject.SetActive(_value);
                if (_value)
                {
                    print("yep Video active");
                    //mediaVideoSync.mediaVideoPlayer.url = mediaLinkLoader.currentLink;
                }
            }
        }
    }
}
