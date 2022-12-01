using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickRotater : MonoBehaviour
{
    [Header("Object To Move")]
    public Transform rotate;
    public float speed = 120;
    public bool xAllow, yAllow, zAllow = true;

    //the joystick
    Vector3 thisAxis;


    private void Awake()
    {
        thisAxis = transform.localPosition;
    }

    void Update()
    {
        ChangeRotation();
    }

    private void ChangeRotation()
    {
        //get the joystick position
        thisAxis = transform.localPosition;
        //if the position is zero, dont do anything
        if (thisAxis == Vector3.zero)
            return;

        Vector3 moveAxis = Vector3.zero;
        if (xAllow)
            moveAxis = new Vector3(thisAxis.x, moveAxis.y, moveAxis.z);
        if (yAllow)
            moveAxis = new Vector3(moveAxis.x, thisAxis.y, moveAxis.z);
        if (zAllow)
            moveAxis = new Vector3(moveAxis.x, moveAxis.y, -thisAxis.z);

        moveAxis = moveAxis* speed *Time.unscaledDeltaTime;

        //update position
        rotate.transform.localRotation *= Quaternion.Euler(moveAxis);
    }
}
