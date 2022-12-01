using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//to always point at a gameObject
public class AlwaysPointing : MonoBehaviour
{
    Vector3 zeroScale = Vector3.zero;
    Vector3 fullScale = Vector3.one;
    public Transform target; // the target object that the arrow should point.

    private float dist; // the distance between the "target" and this object.
    public float hideDistance = 1.5f;

    // Update is called once per frame
    void Update()
    {
        //if the target exist then execute look at.
        if (target)
        {
            // Rotate the camera every frame so it keeps looking at the target
            transform.LookAt(target);
            HideArrow();
        }
    }

    //when the arrow is approaching a certain distance, then 
    private void HideArrow()
    {
        //calculate the distance between the two objects
        dist = Vector3.Distance(this.transform.position, target.transform.position);
        //if the distance is smaller than the "definedhideDistance" then deactivated them.
        if (dist <= hideDistance)
            this.gameObject.SetActive(false);
    }
}
