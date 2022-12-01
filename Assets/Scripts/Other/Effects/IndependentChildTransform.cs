using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndependentChildTransform : MonoBehaviour
{

    //[Header("Anchor position")]
    public bool isAnchoring;
    public Vector3 anchorPosition;
    public Quaternion anchorRotation;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if the anchor is enable then;
        if (isAnchoring)
        {
            //anchor the position and rotation according to the stored gameobject.
            transform.position = anchorPosition;
            transform.rotation = anchorRotation;
        }
        
    }


    //set the anchor of this game Object
    private void SetAnchorTransform()
    {
        anchorPosition = this.transform.position;
        anchorRotation = this.transform.rotation;
    }

    //to enable anchor, therefore the position and rotation become independent
    public void EnableAnchor()
    {
        SetAnchorTransform();
        isAnchoring = true;

    }

    //to disable the anchor
    public void DisableAnchor()
    {
        isAnchoring = false;
    }
}
