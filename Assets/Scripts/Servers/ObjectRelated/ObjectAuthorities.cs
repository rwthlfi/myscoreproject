using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.XR.Interaction.Toolkit;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRGrabInteractable))]
public class ObjectAuthorities : NetworkBehaviour
{
	//cacheVariable
	[System.NonSerialized] public Rigidbody rb;
	private XRGrabInteractable xrGrab;

	//[System.NonSerialized]
	public bool isGrabbed = false;

	// you know.. the force multiplier is to give more "realism" to an object, 
	//e.g. if the object is heavy then you cant really throw it. 
	// on the other hand if the object is light, you can throw them further.
	//the bad things is.., you need to change this value on the server eachtime.
	public float forceMultiplier = 1f; //change this value in the editor


	[Header("Offset Position & Rotation")]
	public Vector3 posOff = Vector3.zero;
	public Vector3 rotOff = Vector3.zero;

	[Header("ResetPosition")] // just leave this to zero if you are not using "Ui_ResetPos"
	public Vector3 resetPos = Vector3.zero;
	public Vector3 resetRot = Vector3.zero;


	public virtual void Start()
	{
		rb = GetComponent<Rigidbody>();
		xrGrab = GetComponent<XRGrabInteractable>();
	}


	public void Update()
	{
		isGrabbed = xrGrab.isSelected;
		//Debug.Log("Result " + isGrabbed);
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



	//Command to Move the Networked Object & Stuff.

	[Command(requiresAuthority = false)]
	public void CmdAllowPhysics(NetworkIdentity _grabID, bool _value)
	{
		UsePhysic(rb, _value);
		Rpc_EnableForceOnClient(_grabID, false);

	}

	// MOVE THE OBJECT //
	[Command(requiresAuthority = false)]
	public void CmdMoveServerObj(NetworkIdentity _grabID, Vector3 _pos, Quaternion _rot)
	{
		UsePhysic(rb, false);
		Rpc_EnableForceOnClient(_grabID, false);
		RestartVelocity(rb);

		//move stuffs
		_grabID.transform.position = _pos;
		_grabID.transform.rotation = _rot;

		//print("move: " + rb.name + " physics " + rb.useGravity);
	}


	[Command(requiresAuthority = false)]
	public void CmdReleaseServerObj(NetworkIdentity _grabID)
	{
		//Enable the physics for the moment.
		UsePhysic(rb, true);
		Rpc_EnableForceOnClient(_grabID, true);
		//print("release: " + rb.name);
	}

	[Command(requiresAuthority = false)]
	public void CmdApplyForce(NetworkIdentity _grabID, Vector3 _force)
	{
		Vector3 convert = new Vector3(_force.x * forceMultiplier,
									  _force.y * forceMultiplier * 2.5f,
									  _force.z * forceMultiplier);

		UsePhysic(rb, true);
		Rpc_EnableForceOnClient(_grabID, true);
		rb.AddForce(convert);
		//print("force: " + rb.name + " " + convert);
	}

	[ClientRpc]
	private void Rpc_EnableForceOnClient(NetworkIdentity _grabId, bool _use)
	{
		Rigidbody rb = _grabId.GetComponent<Rigidbody>();
		UsePhysic(rb, _use);
	}


	private void UsePhysic(Rigidbody _rb, bool _use)
	{
		//_rb.velocity = Vector3.zero;
		//_rb.angularVelocity = Vector3.zero;
		_rb.useGravity = _use;
	}

	private void RestartVelocity(Rigidbody _rb)
	{
		_rb.velocity = Vector3.zero;
		_rb.angularVelocity = Vector3.zero;
		_rb.AddForce(Vector3.zero, ForceMode.VelocityChange);
	}


	//to return the object to the original position
	public void Ui_ResetPos()
    {
		Cmd_ResetPos(resetPos);
    }

	[Command(requiresAuthority = false)]
	public void Cmd_ResetPos(Vector3 _oriPos)
    {
		this.gameObject.transform.position = _oriPos;
    }

}