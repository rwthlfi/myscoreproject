using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWithClamp : MonoBehaviour
{
    public Transform toFollow;
    public Vector3 originToFollowLocalPos; // has to be the same with the to Follow position

    public bool xClamp, yClamp, zClamp = true;

    float maxPos;
    

    // Start is called before the first frame update
    void Start()
    {
        //originToFollowLocalPos = toFollow.localPosition;
        maxPos = (this.transform.parent.localScale.x / 15 
                 + this.transform.parent.localScale.x / 15 ) /2;
    }

    // Update is called once per frame
    void Update()
    {
        //follow the targets
        this.transform.position = toFollow.position;

        ClampPosition();
        /*
        //clamp the position so it doest goes out of boundary
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -maxPos, maxPos), 
                                              0,
                                              Mathf.Clamp(transform.localPosition.z, -maxPos, maxPos));
        */
    }

    public void ClampPosition()
    {
        Vector3 result = Vector3.zero;

        if (xClamp)
            result = new Vector3(Mathf.Clamp(transform.localPosition.x, -maxPos, maxPos), result.y, result.z);

        if (yClamp) 
            result = new Vector3(result.x, Mathf.Clamp(transform.localPosition.y, -maxPos, maxPos), result.z);

        if (zClamp)
            result = new Vector3(result.x, result.y, Mathf.Clamp(transform.localPosition.z, -maxPos, maxPos));

        transform.localPosition = result;
    }


    //when release
    public void ReleaseActualGrabbing()
    {
        StartCoroutine(LerpingExtensions.MoveToLocal(toFollow, originToFollowLocalPos, 0.5f));
    }

}
