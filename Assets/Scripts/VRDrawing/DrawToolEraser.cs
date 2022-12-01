using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawToolEraser : MonoBehaviour
{
    bool startCollision = false;

    public void startEraser()
    {
        startCollision = true;
        this.transform.Rotate(Vector3.forward * Time.deltaTime * 1000);
    }

    public void stopEraser()
    {
        startCollision = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (startCollision)
        {
            if (collision.gameObject.tag == "3DLine")
            {
                GameObject.Destroy(collision.gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (startCollision)
        {
            if (collision.gameObject.tag == "3DLine")
            {
                GameObject.Destroy(collision.gameObject);
            }
        }
    }
}
