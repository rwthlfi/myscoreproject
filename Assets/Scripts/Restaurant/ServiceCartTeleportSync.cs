using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Collections;

public class ServiceCartTeleportSync : NetworkBehaviour
{
    public List<GameObject> teleportGameObjects;
    public Vector3 vectorOri, vectorTarget;
    public GameObject teleportButtonToKitchen, teleportButtonToBuffet;
    public AudioSource teleportSound;
    private Collider[] _colliders, _collidersChildren;

    [SyncVar(hook = nameof(ServiceCartInKitchen))]
    public bool CartInKitchen;
    void ServiceCartInKitchen(bool _old, bool _new) { }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        _colliders = GetComponents<Collider>();
        _collidersChildren = GetComponentsInChildren<Collider>();

        if (CartInKitchen)
        {
            CartPosKitchen();
            teleportButtonToBuffet.SetActive(true);
            teleportButtonToKitchen.SetActive(false);
        }
        else
        {
            CartPosBuffet();
            teleportButtonToKitchen.SetActive(true);
            teleportButtonToBuffet.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!teleportGameObjects.Contains(other.gameObject) && other.gameObject.GetComponent<GameObjectSnapReference>())
            teleportGameObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (teleportGameObjects.Contains(other.gameObject))
            teleportGameObjects.Remove(other.gameObject);
    }

    public void TeleportObjects()
    {
        Cmd_TeleportObjects(teleportGameObjects);
    }

    [Command(requiresAuthority = false)]
    private void Cmd_TeleportObjects(List<GameObject> teleportGameObjects)
    {
        Rpc_TeleportObjects(teleportGameObjects);
        Server_TeleportObjects(teleportGameObjects);
    }

    [Server]
    private void Server_TeleportObjects(List<GameObject> teleportGameObjects)
    {
        TeleportObjectsFinal(teleportGameObjects);
    }

    [ClientRpc]
    private void Rpc_TeleportObjects(List<GameObject> teleportGameObjects)
    {
        TeleportObjectsFinal(teleportGameObjects);
    }

    private void TeleportObjectsFinal(List<GameObject> teleportGameObjects)
    {
        teleportSound.Play();

        if (CartInKitchen)
        {
            teleportButtonToBuffet.SetActive(false);

            CartPosBuffet();

            CartInKitchen = false;

            teleportButtonToKitchen.SetActive(true);
        }
        else
        {
            teleportButtonToKitchen.SetActive(false);

            CartPosKitchen();

            CartInKitchen = true;

            teleportButtonToBuffet.SetActive(true);
        }
    }

    private void CartPosKitchen()
    {
        foreach (Collider col in _colliders)
        { 
            col.enabled = false;
        }
        foreach (Collider colChil in _collidersChildren)
        {
            colChil.enabled = false;
        }

        transform.position = new Vector3(vectorOri.x, vectorOri.y, vectorOri.z);

        foreach (GameObject tgo in teleportGameObjects)
        {
            tgo.transform.position = new Vector3(tgo.transform.position.x + (vectorOri.x - vectorTarget.x), tgo.transform.position.y + 0.04f, tgo.transform.position.z + (vectorOri.z - vectorTarget.z));
        }

        foreach (Collider col in _colliders)
        {
            col.enabled = true;
        }
        foreach (Collider colChil in _collidersChildren)
        {
            colChil.enabled = true;
        }
    }

    private void CartPosBuffet()
    {
        foreach (Collider col in _colliders)
        {
            col.enabled = false;
        }
        foreach (Collider colChil in _collidersChildren)
        {
            colChil.enabled = false;
        }

        transform.position = new Vector3(vectorTarget.x, vectorTarget.y, vectorTarget.z);

        foreach (GameObject tgo in teleportGameObjects)
        {
            tgo.transform.position = new Vector3(tgo.transform.position.x - (vectorOri.x - vectorTarget.x), tgo.transform.position.y + 0.04f, tgo.transform.position.z - (vectorOri.z - vectorTarget.z));
        }

        foreach (Collider col in _colliders)
        {
            col.enabled = true;
        }
        foreach (Collider colChil in _collidersChildren)
        {
            colChil.enabled = true;
        }
    }
}