using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTrackStartControl : MonoBehaviour
{
    [Header("Misc")]
    public GameObject leftViveController;
    public GameObject rightViveController;
    //public LocomotionListener locomotion;
    public GameObject pillerFBX;

    [Header("Flood Protection")]
    public GameObject flood;
    public GameObject tutorialManager;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        leftViveController.SetActive(false);
        rightViveController.SetActive(false);

        yield return new WaitForSeconds(2f);

        //locomotion.moveSpeed = 2;
        //locomotion.turnSpeed = 100f;

        yield return new WaitForSeconds(2f);

        tutorialManager.SetActive(true);
    }

    public void Turn180()
    {
        transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
    }

    public void StartFlood()
    {
        if (flood != null)
        {
            ResetFlood();
            flood.SetActive(true);
        }
    }

    public void ResetFlood()
    {
        flood.transform.position = new Vector3(0.22f, 0.5f, -10f);
        flood.transform.rotation = new Quaternion(0.0f, 0.0f, 0f, 0f);
        flood.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        flood.GetComponent<Rigidbody>().rotation = Quaternion.Euler(0, 0, 0);
        flood.GetComponent<ConstantForce>().force = new Vector3(0f,0f,50f);
    }
}