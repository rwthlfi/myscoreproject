using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class MediaImageSync : NetworkBehaviour
{
    [Header("Target GameObject")]
    public Renderer mediaPlane;

    [Header("SyncVar Variable")]
    [SyncVar(hook = nameof(OnImageLinkChanged))]
    public string imageLink;

    [SyncVar(hook = nameof(OnMediaStatus))]
    public int mediaStatus = 0;
    public enum mediaStatusEnum { doingNothing, currentlyPreparing, errorNoLink };


    void OnImageLinkChanged(string _Old, string _New)
    {
        ChangeNewImage(_New);
    }

    //to show the status of whats happening for the user.
    void OnMediaStatus(int _Old, int _New)
    {
        if (_New == (int)mediaStatusEnum.doingNothing)
        {
        }
        else if (_New == (int)mediaStatusEnum.currentlyPreparing)
        {
        }
        else if (_New == (int)mediaStatusEnum.errorNoLink)
        {
        }
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// to Change a new Image which will sync everything to other client.
    /// </summary>
    /// <param name="_str">Give the link to the Images</param>
    public void Ui_ChangeNewImage(string _str)
    {
        //ChangeNewImage(_str);
        //show the preparing message
        CmdUpdatediaStatus(mediaStatusEnum.currentlyPreparing);

        StartCoroutine(SQLloader.LoadLinkFromWeb(_str, returnValue =>
        {
            if (!returnValue)
            {
                CmdUpdatediaStatus(mediaStatusEnum.errorNoLink);
                print("Image Link Does not exist -> proceed to do Nothing");
            }
            else
            {
                CmdUpdatediaStatus(mediaStatusEnum.doingNothing);
                print("Image Link exist -> Do something");
                //assign the url

                //Change the link in the server
                CmdSyncImageLink(_str);

            }
        }));

    }

    public void ChangeNewImage(string _str)
    {
        StartCoroutine(SQLloader.LoadImageFromWeb(_str, mediaPlane.GetComponent<Renderer>()));
    }


    //change the url from client's input to the server
    [Command(requiresAuthority = false)]
    public void CmdSyncImageLink(string _link)
    {
        imageLink = _link;
        print("currentLink " + _link);

    }

    //Player change the timeStamp on the server, due to new video viewing, or maybe slider being slided.
    [Command(requiresAuthority = false)]
    public void CmdUpdatediaStatus(mediaStatusEnum _value)
    {
        mediaStatus = (int)_value;
    }
}
