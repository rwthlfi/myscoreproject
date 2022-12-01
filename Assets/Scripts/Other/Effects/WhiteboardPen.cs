using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhiteboardPen : MonoBehaviour
{
    public Whiteboard whiteboard;
    private RaycastHit touch;
    public Transform tip;
    public Color color;
    private float tipHeight;
    private Vector3 pos;

    //Misc var
    [System.NonSerialized] public bool lastTouch;
    private Quaternion lastAngle;

    [Header("ColorPicker")]
    [Tooltip("if You are using color picker, then assign the color picker")]
    public ColorPicker colorPicker;


    // Start is called before the first frame update
    public virtual void Start()
    {
    }

    // Update is called once per frame
    public virtual void Update()
    {
        //if ColorPicker  exist, then assigned the color
        if (colorPicker)
            color = colorPicker.color;


        //The drawing function
        tipHeight = tip.localScale.y;
        pos = tip.position ;

        if (lastTouch)
            tipHeight *= 1.1f;

        if(Physics.Raycast(pos, -transform.up, out touch, tipHeight))

        //if (Physics.SphereCast(tip.position, tip.lossyScale.x / 2, -transform.up, out touch, tipHeight))
        {
            if (!(touch.collider.tag == "Whiteboard") || touch.collider.gameObject != whiteboard.gameObject)
                return;
            
            //whiteboard = touch.collider.GetComponent<Whiteboard>();
            //Debug.Log("Touching");

            whiteboard.SetColor(color);
            whiteboard.SetTouchPosition(touch.textureCoord.x, touch.textureCoord.y);
            whiteboard.ToggleTouch(true);

            if (!lastTouch)
            {
                lastTouch = true;
                lastAngle = transform.rotation;

            }
        }

        else
        {
            whiteboard.ToggleTouch(false);
            lastTouch = false;
        }

        /*
        if (lastTouch)
            transform.rotation = lastAngle;
        */
    }



    /*
    //Only For Debugging Purpose
    private void OnDrawGizmos()
    {
        float tipHeight = tip.localScale.y;
        tipHeight *= 1.1f;

        RaycastHit hit;
        bool isHit = Physics.SphereCast(tip.position, tip.lossyScale.x / 2, -transform.up, out hit, tipHeight);

        if (isHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(tip.position, -transform.up * hit.distance);
            Gizmos.DrawSphere(tip.position - transform.up * hit.distance,
                              tip.lossyScale.x * 1);
        }

        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(tip.position, -transform.up * tipHeight);

        }
            
    }
    */
}
