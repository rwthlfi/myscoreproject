using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is to manage the sliding of the door in the scenario
//Attach this script to the DoorColliders

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class SlidingDoor : MonoBehaviour
{
    //trigger variable
    //private string playerPrefabNameToDetect = GlobalSettings.playerPrefabName;

    [Header("The Door Transform Reference")]
    public Transform doorA;
    public Transform doorB;

    [Header("The Open/Close Position Variable")]
    public float speed = 1f;
    Vector3 closePosition_A;
    public Vector3 openPosition_A;

    Vector3 closePosition_B;
    public Vector3 openPosition_B;

    //if the user enter this area then open the door
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.isTrigger == false) // this is used to enable the character controller collider to trigger the action instead of any other trigger collider
        {
            if (_other.gameObject.layer == 6) /*(_other.name == GlobalSettings.playerPrefabName || _other.name == GlobalSettings.playerPrefabOfflineName)*/
            {
                //Debug.Log("Collider " + _other);
                //Debug.Log("openpos " + openPosition_A + " " + openPosition_B);

                StartCoroutine(LerpingExtensions.MoveTo(doorA, openPosition_A, speed));

                if (doorB != null)
                    StartCoroutine(LerpingExtensions.MoveTo(doorB, openPosition_B, speed));
            }
        }
    }

    //if the user leave the area then close the door.
    private void OnTriggerExit(Collider _other)
    {
        if (_other.isTrigger == false) // this is used to enable the character controller collider to trigger the action instead of any other trigger collider
        {
            if (_other.gameObject.layer == 6) /*(_other.name == GlobalSettings.playerPrefabName || _other.name == GlobalSettings.playerPrefabOfflineName)*/
            {
                //Debug.Log("Collider ext" + _other);

                StartCoroutine(LerpingExtensions.MoveTo(doorA, closePosition_A, speed));

                if (doorB != null)
                    StartCoroutine(LerpingExtensions.MoveTo(doorB, closePosition_B, speed));
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        //store the close position for later usage.
        closePosition_A = doorA.transform.position;

        if (doorB != null)
            closePosition_B = doorB.transform.position;

        //set the door to be on the close position
        doorA.transform.position = closePosition_A;

        if (doorB != null)
            doorB.transform.position = closePosition_B;
    }
}

