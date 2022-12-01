using UnityEngine;

public class DoorActiveCollider : MonoBehaviour
{
    public Rigidbody doorRigidbody;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<LeftHandControllerReference>())
        {
            other.GetComponent<SphereCollider>().isTrigger = false;
            doorRigidbody.velocity = Vector3.zero;
            doorRigidbody.angularVelocity = Vector3.zero;
        }
        else if (other.GetComponent<RightHandControllerReference>())
        {
            other.GetComponent<SphereCollider>().isTrigger = false;
            doorRigidbody.velocity = Vector3.zero;
            doorRigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<LeftHandControllerReference>())
        {
            other.GetComponent<SphereCollider>().isTrigger = true;
            doorRigidbody.velocity = Vector3.zero;
            doorRigidbody.angularVelocity = Vector3.zero;
        }
        else if (other.GetComponent<RightHandControllerReference>())
        {
            other.GetComponent<SphereCollider>().isTrigger = true;
            doorRigidbody.velocity = Vector3.zero;
            doorRigidbody.angularVelocity = Vector3.zero;
        }
    }
}
