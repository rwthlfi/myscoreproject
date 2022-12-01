using System.Collections;
using UnityEngine;
using Threading;
using System.IO.Compression;
using System.IO;
using Mirror;

[RequireComponent(typeof(PaintReceiver))]
public class PaintReceiverAuthority : NetworkBehaviour
{
    private PaintReceiver pr;
    public bool isOwner = false;

    //server variable
    private bool serverSendupdate = false;

    void Start()
    {
		pr = GetComponent<PaintReceiver>();

        //request for Pixels from the server
        StartCoroutine(GetStartupPainterBoard());
    }

    private IEnumerator GetStartupPainterBoard()
    {
        // wait until it find the local player
        //yield return new WaitWhile(() => trs.theLocalPlayer.isActiveAndEnabled );
        yield return new WaitForSeconds(3.5f); // only a bit of tolerance to avoid being rushed.
        CmdRequestStartupPixels();
    }


    private float nextActionTime = 0.0f;
    private float period = 0.05f;

    private void Update()
    {
        //only run this function on server
        if (isServer)
        {
            //print("server ");
            if (Time.time > nextActionTime)
            {
                nextActionTime += period;
                pr.isModifiable = false;

                if (serverSendupdate) // in order not to clutter the process 
                {
                    serverSendupdate = false;
                    //synchronize the pixel to the client
                    Rpc_GetPixels(netIdentity, pr.getTextureByte());
                }
            }
        }
    }


	/// ASSIGN AND REMOVE CLIENT AUTHORITY///
	[Command(requiresAuthority = false)]
	public void CmdSetAuthority(NetworkIdentity grabID, NetworkIdentity playerID)
	{
		grabID.AssignClientAuthority(playerID.connectionToClient);

	}

	[Command(requiresAuthority = false)]
	public void CmdRemoveAuthority(NetworkIdentity grabID, NetworkIdentity playerID)
	{
		grabID.RemoveClientAuthority();
	}


    //To Request the Start Up Pixels, in case people joining in.
	[Command(requiresAuthority = false)]
	public void CmdRequestStartupPixels()
	{
        Rpc_SetTheStartupTexture(pr.getTextureByte());
        //Debug.Log("Startup tex length is: " + pr.getTextureByte().Length);
        //serverSendupdate = true;
    }

    /// <summary>
    /// To Synchron the pixels from Server to All Clients.
    /// </summary>
    [ClientRpc]
    public void Rpc_SetTheStartupTexture(byte[] _pixels)
    {
        pr.newTexture.LoadImage(_pixels);
        pr.newTexture.Apply();
        pr.currentTexture = pr.newTexture.GetPixels32();
    }



    /// COMMAND to send from client to Server///
    [Command(requiresAuthority = false)]
	public void CmdSendPaintBoardPixels(NetworkIdentity _boardID, byte[] _pixels)
	{
		serverSendupdate = true;
        pr = _boardID.GetComponent<PaintReceiver>();
        StartCoroutine(pr.receiveTextureBytes(_pixels));
	}


    /// <summary>
    /// To Synchron the pixels from Server to All Clients.
    /// </summary>
	[ClientRpc]
	private void Rpc_GetPixels(NetworkIdentity _boardID, byte[] _pixels)
	{
        if(!isOwner) 
		    StartCoroutine(pr.receiveTextureBytes(_pixels));
        //Debug.Log("prints " + _pixels.Length);
        
	}
}
