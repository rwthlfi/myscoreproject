using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AutoMeshCollider : MonoBehaviour
{
    [Header("Collider Variable")]
    public GameObject parentObj;
    Component[] meshrenderer;

    [Header("Grab Interactable")]
    public XRGrabInteractable xRGrabInteractable;

    // Start is called before the first frame update
    void Start()
    {
        //AutoAddCollider();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AutoAdjustCollider(int _layer)
    {
        xRGrabInteractable.enabled = false;
        
        //for mesh renderer...
        meshrenderer = parentObj.GetComponentsInChildren<Renderer>();
        if (meshrenderer != null)
        {
            foreach (Renderer rend in meshrenderer)
            {
                //set the assigned layer
                rend.gameObject.layer = _layer;

                var mc = rend.gameObject.AddComponent<MeshCollider>();
                var smr = mc.GetComponent<SkinnedMeshRenderer>();
                // the skinned mesh renderer are unable to assign the mesh directly to the collider. 
                //therefore we need to assign them manually.
                if (smr) 
                    mc.sharedMesh = smr.sharedMesh;

                mc.convex = true;
                xRGrabInteractable.colliders.Add(mc);
            }
        }


        //however sometime there is also so called skinned mesh renderer. which ..
        xRGrabInteractable.enabled = true;
    }

}
