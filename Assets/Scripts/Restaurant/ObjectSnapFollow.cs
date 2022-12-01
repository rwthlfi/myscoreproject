using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectSnapFollow : MonoBehaviour
{
    public BoxCollider _childCollider;
    private bool isTilting = false;
    private int tiltThreshold = 90;

    public GameObject _child;
    public bool _isSnapped;

    private void Update()
    {
        if (_child != null && _isSnapped)
        {
            if (_child.GetComponent<XRGrabInteractable>().isSelected)
            {
                StartReset();
            }

            if (_childCollider != null)
            {
                var _childPosition = _childCollider.bounds.center.y - _childCollider.bounds.extents.y;
                _child.transform.position = new Vector3(transform.position.x, transform.position.y + _childCollider.bounds.extents.y + 0.02f, transform.position.z);
                _child.transform.rotation = transform.rotation;
            }

            bool pourCheck = CalculateTiltAngle() < tiltThreshold;

            if (isTilting != pourCheck)
            {
                isTilting = pourCheck;
                if (isTilting)
                {
                    StartReset();
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.position.y > transform.position.y)
        {
            if (collision.gameObject.GetComponent<GameObjectSnapReference>() && !_isSnapped)
            {
                _childCollider = collision.gameObject.GetComponent<BoxCollider>();
                _childCollider.enabled = false;
                _child = collision.gameObject;
                _child.GetComponent<Rigidbody>().isKinematic = true;
                _isSnapped = true;
                _childCollider.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _child)
            StartReset();
    }

    private float CalculateTiltAngle()
    {
        return transform.up.y * Mathf.Rad2Deg;
    }

    private void StartReset()
    {
        StartCoroutine(ResetObject());
    }

    private IEnumerator ResetObject()
    {
        _childCollider = null;
        _child.GetComponent<Rigidbody>().isKinematic = false;
        _child = null;

        yield return new WaitForSeconds(0.5f);

        _isSnapped = false;
    }
}