using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickRotater_Y : MonoBehaviour
{
    [Header("Object To Move")]
    public Transform rotate;
    public float speed = 120;

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

        Vector3 moveAxis = new Vector3(0, thisAxis.x, 0);
        
        moveAxis = moveAxis* speed *Time.unscaledDeltaTime;

        //update position
        rotate.transform.localRotation *= Quaternion.Euler(moveAxis);
    }
}
