using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float counter;
    private float dist;

    public Transform origin;
    public Transform destination;
    public LayerMask layerMask;

    private float defaultLength = 20.0f;


    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, origin.position);
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
       
        //dist = Vector3.Distance(origin.position, destination.position);
        dist = Vector3.Distance(origin.position, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {

        UpdateLine();
    }



    //LineRenderer Extenstions
    private void UpdateLine()
    {
        //use default or distance
        float targetLength = defaultLength;

        //Raycast
        RaycastHit hit = CreateRaycast(targetLength);

        //default
        Vector3 endPosition = transform.position + (transform.forward * targetLength);

        //or based on hit
        if (hit.collider != null)
            endPosition = hit.point;

       
        /*
        else
            endPosition = origin.position; // to hide the line Renderer.
        */
        //set position of th dot
        destination.transform.position = endPosition;

        //set line renderer
        lineRenderer.SetPosition(0, origin.position);
        lineRenderer.SetPosition(1, endPosition);

    }
    private RaycastHit CreateRaycast(float _length)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, defaultLength, layerMask);

        return hit;
    }

}
