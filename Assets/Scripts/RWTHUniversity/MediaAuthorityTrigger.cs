using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MediaAuthorityTrigger : MonoBehaviour
{
    public NetworkIdentity currentPlayer;
    public bool isAllow = false;
    public MediaLinksLoader mediaLinkLoader;

    [System.NonSerialized]public int noAuthority = -1;

    private void OnTriggerEnter(Collider other)
    {
        //if it Not already assign, forget it
        if (!isAllow)
            return;
        //if current player is not yet assign, then do it.
        else if(currentPlayer == null && other.tag == "PlayerCloth")
        {
            currentPlayer = other.transform.root.GetComponent<NetworkIdentity>();
            GiveAuthority((int)currentPlayer.netId);

            print("Enter " + currentPlayer.name + " id: " + currentPlayer.netId);
        }

        print("someone Enter " + other.name + " allow " + isAllow);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isAllow && other == null)
            return;

        //if player is empty then ->
        else if (currentPlayer == null && other.tag == "PlayerCloth")
        {
            currentPlayer = other.transform.root.GetComponent<NetworkIdentity>();
            GiveAuthority((int)currentPlayer.netId);
            //print("Triggered ");
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isAllow) // dont do anything if it is not allowed
            return;

        if (currentPlayer == other.transform.root.GetComponent<NetworkIdentity>())
        {
            //print("Exit " + currentPlayer.name + " id: " + currentPlayer.netId);
            currentPlayer = null;
            GiveAuthority(noAuthority);

        }

    }

    //trigger the authority
    private void GiveAuthority(int _netId)
    {
        //print("Give Authority " + _netId);
        mediaLinkLoader.ChangeAuthority(_netId);
    }
}
