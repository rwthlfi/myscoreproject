using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AutoBoundingCollider : MonoBehaviour
{
    private BoxCollider boxCol;
    private Bounds bounds;
    MeshFilter[] filters;

    // Start is called before the first frame update
    void Start()
    {
        boxCol = GetComponent<BoxCollider>();
        bounds = new Bounds(transform.position, Vector3.zero);
        filters = GetComponentsInChildren<MeshFilter>();


    }

    private void Update()
    {
        //AdjustTheCollider();
    }

    public void AdjustTheCollider()
    {
        BoxCollider bc = GetComponent<BoxCollider>();
        if (bc == null) 
        {
            bc = this.gameObject.AddComponent<BoxCollider>();
        }
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        bool hasBounds = false;
        Renderer[] renderers = this.GetComponentsInChildren<Renderer>();
        foreach (Renderer render in renderers)
        {
            if (hasBounds)
            {
                bounds.Encapsulate(render.bounds);
            }
            else
            {
                bounds = render.bounds;
                hasBounds = true;
            }
        }
        if (hasBounds)
        {
            bc.center = bounds.center - this.transform.position;
            bc.size = bounds.size;
        }
        else
        {
            bc.size = bc.center = Vector3.zero;
            bc.size = Vector3.zero;
        }
    
    /*
    boxCol = gameObject.GetComponent<BoxCollider>();
    if (boxCol == null)
    {
        boxCol = gameObject.AddComponent<BoxCollider>();
    }

    bounds = new Bounds(transform.position, Vector3.zero);

    var allDescendants = gameObject.GetComponentsInChildren<Transform>();
    foreach (Transform desc in allDescendants)
    {
        var childRenderer = desc.GetComponent<Renderer>();
        if (childRenderer != null)
        {
            bounds.Encapsulate(childRenderer.bounds);
        }
        boxCol.center = bounds.center - transform.position;
        boxCol.size = bounds.size;
    }
    */

    }
}


