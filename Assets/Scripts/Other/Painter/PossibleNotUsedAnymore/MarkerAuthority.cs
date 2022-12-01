using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(ObjectAuthorities))]
public class MarkerAuthority : Marker
{
    [Header("Script References")]
    private NetworkIdentity paintboard_netID;
    private PaintReceiverAuthority paintReceiverAuthorities;
    public ObjectAuthorities objAuthorities;

    private PaintReceiver tempPaintReceiver;

    /*
    [Header("Offline Debugger Variable")]
    public PaintReceiver rndroffline;
    */

    // Start is called before the first frame update
    void Start()
    {
        InitBoardNetwork();
        tempPaintReceiver = paintReceiver; //cache the paint receiver.
        painter.enabled = false;
    }

    private void Update()
    {
        if (objAuthorities.rb) // if not grabbed && the rigidbody exist
        {
            
            objAuthorities.rb.useGravity = false;
            objAuthorities.rb.isKinematic = false;
            objAuthorities.rb.velocity = Vector3.zero;
            objAuthorities.rb.angularVelocity = Vector3.zero;
            
            objAuthorities.rb.Sleep();
        }

    }

    private float nextActionTime = 0.0f;
    private float period = 0.05f;
    public override void LateUpdate()
    {
        base.LateUpdate();


        //check if it drawn on the same board., otherwise change it to the actual.
        if (painter.isDrawn && tempPaintReceiver != paintReceiver)
        {
            tempPaintReceiver = paintReceiver;
            InitBoardNetwork();
        }

        
        //synchron the drawing to the server
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;

            if (!paintReceiverAuthorities)
                return;
            //if it is NOT a server then execute this{
            if(!paintReceiverAuthorities.isServer)
            {
                //if the pen "Touch" the whiteBoard and "being Grabbed" the whiteboard
                //send byte[] to server every x seconds
                if (objAuthorities.isGrabbed && painter.isDrawn)
                {
                    //send them pixel's byte[] array
                    //SendPixels(paintReceiver.getCurrentColors());
                    SendPixels(paintReceiver.getTextureByte());
                    //Send_OfflinePixels(paintReceiver.getTextureByte());

                    //this isOwner is to disable the receiving Texturebytes.
                    //otherwise the localTexture will not be updated
                    //this variable is used in the PaintReceiverAuthority-> Rpc_GetPixels
                    paintReceiverAuthorities.isOwner = true;
                }
                else
                    paintReceiverAuthorities.isOwner = false;// this is local variable nothing to do with server
            }
            
        }
    }

    private void InitBoardNetwork()
    {
        if (!paintReceiver)
            return;

        print("exist " + paintReceiver.name);
        paintReceiverAuthorities = paintReceiver.GetComponent<PaintReceiverAuthority>();
        paintboard_netID = paintReceiver.GetComponent<NetworkIdentity>();
        objAuthorities = GetComponent<ObjectAuthorities>();
    }



    //Command to send pixel to the server
    private void SendPixels(byte[] _pixels)
    {
        if (_pixels == null)
            return;
        //Debug.Log("send pixels to server ");

        //send the pixel array to the server.
        if(paintReceiverAuthorities)
          paintReceiverAuthorities.CmdSendPaintBoardPixels(paintboard_netID, _pixels);
    }

    /*
    //For offline testing.
    private void Send_OfflinePixels(byte[] _pixels)
    {
        if (_pixels == null)
            return;
        StartCoroutine(rndroffline.receiveOfflineTextureBytes(_pixels));
    }*/


    /// <summary>
    /// attach this to the Grabbing event in the XR
    /// </summary>
    public void UI_XRSelectEnterEvent()
    {
        painter.enabled = true;
    }

    /// <summary>
    /// attach this to the Release event in the XR
    /// </summary>
    public void UI_XRSelectExitEvent()
    {
        painter.enabled = false;
    }
}
