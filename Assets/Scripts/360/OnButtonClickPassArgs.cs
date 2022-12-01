using UnityEngine;
using System.Collections;

public class OnButtonClickPassArgs : MonoBehaviour
{
    private GameObject _mainPlayer;
    public GameObject trigger;
    public float xpos, ypos, zpos, xrot, yrot, zrot;
    public bool inside3DRoom;

    public void Pass360TeleportParameter()
    {
        //_mainPlayer = trigger.GetComponent<TriggerColliderPassPlayerInfo>().mainPlayer;

        trigger.GetComponent<Teleport360>().TeleportPosition(xpos, ypos, zpos, xrot, yrot, zrot, inside3DRoom);
    }
}
