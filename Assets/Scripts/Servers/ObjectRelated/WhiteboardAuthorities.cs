using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using RoomService;

[RequireComponent(typeof(NetworkIdentity))]
public class WhiteboardAuthorities : NetworkBehaviour
{
	private Whiteboard wb;

	//server variable
	private bool serverSendupdate = false;
	private IEnumerator dummyCoroutine;

	private void Start()
    {
		wb = GetComponent<Whiteboard>();
		//set the animcoroutine
		dummyCoroutine = DummyCoroutine();
		
		//request for Pixels from the server
		StartCoroutine(GetStartupWhiteboard());
	}

	private IEnumerator GetStartupWhiteboard()
    {
		TheRoomServices trs = FindObjectOfType<TheRoomServices>();
		// wait until it find the local player
		yield return new WaitWhile(() => trs.theLocalPlayer); 
		CmdRequestStartupPixels();
    }

	private float nextActionTime = 0.0f;
	private float period = 0.1f;
	private void Update()
    {
		//only run this function on server
        if (isServer)
        {
			if (Time.time > nextActionTime)
			{
				nextActionTime += period;

                if (serverSendupdate) // in order not to clutter the process 
                {
					serverSendupdate = false;
					//synchronize the pixel to the client
					Rpc_GetPixels(netIdentity, wb.getTextureByte());
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

	[Command(requiresAuthority = false)]
	public void CmdRequestStartupPixels()
	{
		serverSendupdate = true;
	}



	/// COMMAND to Send to client and receive Pixels from client ///
	[Command(requiresAuthority = false)]
	public void CmdSendWhiteboardPixels(NetworkIdentity _boardID, byte[] _pixels)
	{
		serverSendupdate = true;
		wb.receiveTextureByte(_pixels);
	}

	[ClientRpc]
	private void Rpc_GetPixels(NetworkIdentity _boardID, byte[] _pixels)
	{
		StartCoroutine(wb.receiveTextureBytess(_pixels));
		//wb.receiveTextureByte(_pixels);
		/*
		StopCoroutine(dummyCoroutine);
		dummyCoroutine = applyPixels(_pixels);
		StartCoroutine(applyPixels(_pixels));
		*/
	}

	private IEnumerator applyPixels(byte[] _pixels)
    {
		yield return null;
		wb.receiveTextureByte(_pixels);
	}


	//this is just a dummy in order to prevent null refenrence at the beginning.
	public IEnumerator DummyCoroutine()
	{
		yield return new WaitForSeconds(0f);
	}
}
