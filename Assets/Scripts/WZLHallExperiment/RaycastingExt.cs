using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastingExt : MonoBehaviour
{
    [Header("Origin & Ray Variable")]
    public GameObject origin;
    RaycastHit hit;
    public LayerMask ValidLayers;
    public string tagName = "ReplaceThisWithYourTagObject";
    public bool autoRayDirection = false;
    public Vector3 rayDirection = new Vector3(0, 0, 1);
    private Vector3 cacheRay = Vector3.zero;
    public float rayCastLength = 1000f;


    //The hit target variable 
    [NonSerialized] public bool isHit = false;
    public GameObject hitTarget;
    public float hitDistance;
    public Vector3 endDistance;


    [Header("Draw render Line")] // you can leave the linerenderer and end point empty if the "drawLine" set to false
    public bool drawLine;
    public LineRenderer lineRenderer;
    public float lineWidth = 0.005f;
    public MeshRenderer endPointObj;
    public Vector3 endPointScale = new Vector3(0.1f, 0.1f, 0.1f);


    // Update is called once per frame
    void Update()
    {
        //the ray will always point according to the offset no matter what the Origin's rotation is
        if (autoRayDirection)
            cacheRay = rayDirection;
        else // the ray will take origin's rotation into consideration.
            cacheRay = origin.transform.TransformDirection(rayDirection);

        if (Physics.Raycast(origin.transform.position, cacheRay, out hit, rayCastLength, ValidLayers, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.gameObject.tag == tagName || tagName == "")
            {
                //Just drawing for better visualization
                Debug.DrawRay(origin.transform.position, cacheRay * hit.distance, Color.yellow);

                //set the object trigger
                isHit = true;
                hitTarget = hit.transform.gameObject;

                hitDistance = hit.distance;
                //print("hit: " + hitTarget.name);

                //just drawing it for better visualization
                DrawLineRenderer(origin.transform.position, hit.point,
                                 lineWidth, Color.yellow, endPointScale);

                //update the endDistance
                endDistance = hit.point;
            }

            else // when it hits the layer but not the same tag
            {
                //Just drawing for better visualization
                Debug.DrawRay(origin.transform.position, cacheRay * rayCastLength, Color.white);

                //just drawing it for better visualization
                DrawLineRenderer(origin.transform.position, origin.transform.position + cacheRay * rayCastLength,
                                 lineWidth, Color.white, endPointScale);

                //update the endDistance
                endDistance = origin.transform.position + cacheRay * rayCastLength;
                //Debug.Log("hit " + hit.transform.name);

                hitTarget = null;
            }
        }
        else
        {
            //Just drawing for better visualization
            Debug.DrawRay(origin.transform.position, cacheRay * rayCastLength, Color.white);

            isHit = false;
            hitTarget = null;

            hitDistance = 0f;
            //print("Did not Hit");

            //just drawing it for better visualization
            DrawLineRenderer(origin.transform.position, origin.transform.position + cacheRay * rayCastLength,
                             lineWidth, Color.white, endPointScale);

            //update the endDistance
            endDistance = origin.transform.position + cacheRay * rayCastLength;
        }
    }




    public float getPercentageDiff()
    {
        return hitDistance / rayCastLength * 100;
    }


    /// <summary>
    /// To draw the line of the renderer
    /// </summary>
    /// <param name="_start"></param>
    /// <param name="_end"></param>
    /// <param name="_lineWidth"></param>
    /// <param name="_color"></param>
    /// <param name="_endPointSize"></param>
    private void DrawLineRenderer(Vector3 _start, Vector3 _end, float _lineWidth, Color _color, Vector3 _endPointSize)
    {
        //dont do anything if the draw line is not checked.
        if (!drawLine)
            return;

        //otherwise draw it!
        ActivateLineRenderer(true);
        lineRenderer.SetPosition(0, _start);
        lineRenderer.SetPosition(1, _end);
        lineRenderer.startWidth = _lineWidth;
        lineRenderer.endWidth = _lineWidth;
        lineRenderer.startColor = _color;
        lineRenderer.endColor = _color;
        endPointObj.material.color = _color;
        endPointObj.transform.position = _end;
        endPointObj.transform.localScale = _endPointSize;
    }


    /// <summary>
    /// to activate or deactivate line renderer
    /// </summary>
    /// <param name="_value"></param>
    public void ActivateLineRenderer(bool _value)
    {
        lineRenderer.gameObject.SetActive(_value);
        endPointObj.gameObject.SetActive(_value);
    }


    /// <summary>
    /// To get the position according to the percentage
    /// </summary>
    /// <param name="_percent">0 - 1 </param>
    /// <returns></returns>
    public Vector3 getPos(float _percent)
    {
        Vector3 v = Vector3.Lerp(origin.transform.position, endDistance, _percent);
        return v;
    }

    public Vector3 getEndCoor()
    {
        return hit.point;
    }


    /// <summary>
    /// get the distance between the origin and the hit point
    /// </summary>
    /// <returns></returns>
    public float getDistance()
    {
        return Vector3.Distance(origin.transform.position, hit.point);
    }


    public Quaternion getEndQuaternion()
    {
        Quaternion rot = Quaternion.LookRotation(Vector3.ProjectOnPlane(origin.transform.forward, hit.normal), hit.normal);

        return rot;
    }

}