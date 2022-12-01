using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//inherit from ObjectAuthorities
public class WhiteboardPenAuthorities : WhiteboardPen
{
    
    //public Vector3 pos;
    public Vector3 rot; // the rotation is somehow being rotated due to the synchro..

    [Header("Script References")]
    private NetworkIdentity whiteboard_netID;
    private WhiteboardAuthorities whiteboardAuthorities;
    public ObjectAuthorities objAuthorities;

    public override void Start()
    {
        base.Start();
        //setup the variables
        whiteboard_netID = whiteboard.GetComponent<NetworkIdentity>();
        whiteboardAuthorities = whiteboard.GetComponent<WhiteboardAuthorities>();
    }


    private float nextActionTime = 0.0f;
    private float period = 0.1f;
    public override void Update()
    {
        base.Update();
        //print("Updating " + touching());
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;

            //if the pen "Touch" the whiteBoard and "being Grabbed" the whiteboard
            //send byte[] to server every x seconds
            if (touching() && objAuthorities.isGrabbed )
            {
                //send them pixel's byte[] array
                SendPixels(whiteboard.getTextureByte());
            }
        }
    }


    private bool touching()
    {
        return lastTouch;
    }

    

    //Command to send pixel to the server
    private void SendPixels(byte[] _pixels)
    {
        if (_pixels == null)
            return;

        //send the pixel array to the server.
        whiteboardAuthorities.CmdSendWhiteboardPixels(whiteboard_netID, _pixels);
    }


}
