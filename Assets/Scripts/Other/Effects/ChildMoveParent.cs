using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChildMoveParent : MonoBehaviour
{
    [Header("Object Reference")]
    public List<Transform> parents = new List<Transform>();
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;


    // Update is called once per frame
    void Update()
    {
        if (parents.Count == 0)
        {
            Debug.Log("Parents is not assigned, are you sure?");
            return;
        }

        foreach (Transform parent in parents)
        {
            parent.transform.position = this.transform.TransformPoint(trackingPositionOffset);
            parent.transform.rotation = this.transform.rotation * Quaternion.Euler(trackingRotationOffset);
        }

    }

}
