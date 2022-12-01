using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;



public class Interactable : MonoBehaviour
{
    public int touchCount;
    public XRController Hand;
    public bool gripped;
    public bool SecondGripped;
    public GameObject GrippedBy;
    private PoleGrip poleGrip = null;


    void Start()
    {
        poleGrip = GetComponentInChildren<PoleGrip>();
        //Physics.IgnoreLayerCollision(22, 24);
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        //Physics.IgnoreLayerCollision(22, 24);
        /*
        if (collision.gameObject.GetComponent<XRController>())
        {
            touchCount++;
        }
        */

    }
    private void OnCollisionExit(Collision collision)
    {
        /*
        if (collision.gameObject.GetComponent<XRController>())
        {
            Debug.Log("qwer " + collision.gameObject);
            touchCount--;
        }
        */
    }
    

    private void Update()
    {
        if (poleGrip.LeftHandGrip.Gripped && poleGrip.RightHandGrip.Gripped)
            gripped = true;
        else
            gripped = false;
    }

}