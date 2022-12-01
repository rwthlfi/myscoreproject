using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickController : MonoBehaviour
{
    [Header("Object To Move")]
    public Transform move;
    public float speed = 2;
    public bool xAllow, yAllow, zAllow = true;
    public bool invert;
    //the joystick
    Vector3 thisAxis;


    private void Awake()
    {
        thisAxis = transform.localPosition;
    }

    void Update()
    {
        ChangePosition();
    }

    private void ChangePosition()
    {
        //get the joystick position
        thisAxis = transform.localPosition;
        //if the position is zero, dont do anything
        if (thisAxis == Vector3.zero)
            return;


        var moveAxis = Vector3.zero;
        if (xAllow)
            moveAxis = new Vector3(thisAxis.x, moveAxis.y, moveAxis.z);
        if (yAllow)
            moveAxis = new Vector3(moveAxis.x, thisAxis.y, moveAxis.z);
        if (zAllow)
            moveAxis = new Vector3(moveAxis.x, moveAxis.y, thisAxis.z);

        moveAxis = moveAxis * speed * Time.unscaledDeltaTime;


        //update position
        if (!invert)
            move.transform.localPosition += moveAxis;
        else
            move.transform.localPosition -= moveAxis;
    }
}
