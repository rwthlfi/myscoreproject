using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLever : MonoBehaviour
{
    [Header("The Actual grabbing ~ the networked Object")]
    // this is the Actual Grabbing, we need this cause we require the XRGrab to be in the root.
    public Transform toFollow; 
    
    // the origin position is the world position in the editor., it is required in order to miminize the traffic in the network

    public Vector3 originFollowPos;  // basically just copy the position value of this gameObject and paste it in the editor
    //Now you ask... if i just need to copy paste, why dont i just use "originFollowPos = this.transform.position" ?
    //Its because you cant... cause at start the position has changed depending on the network.


    private float percentage;

    [Header("Lever 3D model")]
    public Transform leverModel;
    public float leverSpeed = 0.5f;


    [Header("List of the Wall")]
    public List<Transform> walls = new List<Transform>();
    public List<Vector3> wallsOriPos = new List<Vector3>();
    public float wallsClampValue = 1f;

    bool finishSetup = false;

    [Header("Limit")]
    public float minY = -100f;
    public float maxY = 100f;


    private void Start()
    {
        finishSetup = true;
    }

    // Update is called once per frame
    void Update()
    {
        //wait a bit till the setup finish., otherwise it will store a different value at the beginning.
        if (!finishSetup)
            return;


        ClampActualGrabPosition();
        LeverModelPosAnim();
        WallModelPosUpdate();
    }


    /// <summary>
    /// to make the Lever only moveable on the Y Axis
    /// </summary>
    private void ClampActualGrabPosition()
    {
        toFollow.position = new Vector3(originFollowPos.x,        
                                        Mathf.Clamp(toFollow.position.y,
                                                    originFollowPos.y - Mathf.Abs(minY),
                                                    originFollowPos.y + Mathf.Abs(maxY)),
                                        originFollowPos.z);

        //store the percentage for the WallModelPosUpdate
        percentage = (toFollow.position.y - originFollowPos.y ) / Mathf.Abs(maxY-minY);
    }


    //update the lever position so it follows the "ActualGrabbing" ~ only for Visual effect
    private void LeverModelPosAnim()
    {
        leverModel.position = Vector3.Lerp(leverModel.position, toFollow.position, Time.deltaTime * leverSpeed);
    }


    //update the wall position according to the "ActualGrabbing" ~ only for visual Effect.
    private void WallModelPosUpdate()
    {
        for(int i = 0; i < walls.Count; i++)
        {
            walls[i].localPosition = Vector3.Lerp(walls[i].localPosition,

                                             //Clamping the position.
                                             new Vector3(wallsOriPos[i].x,
                                                         wallsOriPos[i].y + (percentage  * wallsClampValue),
                                                         wallsOriPos[i].z),


                                             Time.deltaTime * leverSpeed);

        }

    }

}
