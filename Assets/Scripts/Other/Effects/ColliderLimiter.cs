using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderLimiter : MonoBehaviour
{
    [Header("Rigidbody Variable")]
    public Rigidbody rb;
    public float maxVelocity = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude >= maxVelocity)
            rb.velocity = Vector3.zero;
        if (rb.angularVelocity.magnitude >= maxVelocity)
            rb.angularVelocity = Vector3.zero;
    }
}
