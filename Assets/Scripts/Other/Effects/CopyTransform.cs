using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    [Header("Position Variable")]
    public Transform targetTransformPos;
    public Vector3 trackingPositionOffset;

    [Header("Rotation Variable")]
    public Transform targetTransformRot;
    [Range(0, 1)]
    public float rotWeight = 0.25f;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = targetTransformPos.position - trackingPositionOffset;
        //this.transform.rotation = targetTransformRot.rotation;

        //Quaternion.Lerp(this.transform.rotation, targetTransformRot.rotation, rotWeight);
    }
}
