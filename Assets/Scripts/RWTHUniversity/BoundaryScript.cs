using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AvatarCreation;

public class BoundaryScript : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && other.isTrigger == false
            && other.GetComponent<NetworkPlayerSyncVar>().isLocalPlayer)
        {
            meshRenderer.enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && other.isTrigger == false
            && other.GetComponent<NetworkPlayerSyncVar>().isLocalPlayer)
        {
            meshRenderer.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player" && other.isTrigger == false
            && other.GetComponent<NetworkPlayerSyncVar>().isLocalPlayer)
        {
            meshRenderer.enabled = false;
        }
    }
}
