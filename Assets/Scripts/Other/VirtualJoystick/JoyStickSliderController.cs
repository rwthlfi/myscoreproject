using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickSliderController : MonoBehaviour
{
    [Header("Object To Move")]
    public Transform move;
    public float speed = 2;
    public bool invert = false;

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

        Vector3 moveAxis = new Vector3(0, thisAxis.z, 0) * speed * Time.unscaledDeltaTime;

        //update position
        if(!invert)
            move.transform.localPosition += moveAxis;
        else
            move.transform.localPosition -= moveAxis;
    }
}
