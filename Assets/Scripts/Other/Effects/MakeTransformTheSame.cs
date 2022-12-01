using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// make an object follow the transform
/// </summary>
public class MakeTransformTheSame : MonoBehaviour
{
    [SerializeField]
    public Transform gameObjectToFollow;
    public bool trackPos = true;
    public Vector3 trackingPositionOffset;
    public bool trackRot = true;
    public Vector3 trackingRotationOffset;


    public bool snapPosition = false;
    [Tooltip("if snapPosition is false., you can also ignore the snapPositionPoint)")]
    public Transform snapPositionPoint;


    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //attach the object to the right hand
        if (snapPosition == false)
        {
            if(trackPos)
                transform.position = gameObjectToFollow.TransformPoint(trackingPositionOffset);
            if(trackRot)
                transform.rotation = gameObjectToFollow.rotation * Quaternion.Euler(trackingRotationOffset);
        }


        //attach the object to the position.
        else if (snapPosition)
        {
            //transform.position = snapPositionPoint.position;

            if (trackPos)
                StartCoroutine(LerpingExtensions.MoveTo(transform, snapPositionPoint.position, 0.5f));
            if(trackRot)
                transform.rotation = snapPositionPoint.rotation;
        }
    }

}
