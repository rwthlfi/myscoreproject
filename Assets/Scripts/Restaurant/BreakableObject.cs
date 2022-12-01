using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BreakableObject : MonoBehaviour
{
    enum blendShapes { breaking };

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == gameObject.tag || collision.gameObject.transform.childCount > 0 && collision.gameObject.transform.GetChild(0).tag == gameObject.tag)
        {
            //break the stuff
            //this is with animation

            if (collision.gameObject.GetComponent<SkinnedMeshRenderer>() != null)
            {
                StartCoroutine(breakingThings(collision.gameObject.GetComponent<SkinnedMeshRenderer>(), 0.2f));
            }
            else if (collision.gameObject.GetComponentInChildren<SkinnedMeshRenderer>() != null)
            {
                StartCoroutine(breakingThings(collision.gameObject.GetComponentInChildren<SkinnedMeshRenderer>(), 0.2f));
            }
            //this is without animation
            //collision.gameObject.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight((int)blendShapes.breaking, 100f);

            //disble the objectAuthority // therefore he cannot grab anymore
            collision.gameObject.GetComponent<ObjectAuthorities>().enabled = false;
            collision.gameObject.GetComponent<XRGrabInteractable>().enabled = false;

            StartCoroutine(BreakDelay(collision));
        }
    }

    private IEnumerator breakingThings(SkinnedMeshRenderer _object, float _overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + _overTime)
        {
            _object.SetBlendShapeWeight((int)blendShapes.breaking,
            Mathf.Lerp(0, 100, (Time.time - startTime) / _overTime));

            yield return null;
        }

        _object.SetBlendShapeWeight((int)blendShapes.breaking, 100f);
    }

    private IEnumerator BreakDelay(Collision collision)
    {
        yield return new WaitForSeconds(0.2f);
        collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        var Colliders = collision.gameObject.GetComponents(typeof(Collider));
        var CollidersChild = collision.gameObject.GetComponentsInChildren(typeof(Collider));

        foreach (Collider col in Colliders)
        {
            col.enabled = false;
        }
        foreach (Collider colChil in CollidersChild)
        {
            colChil.enabled = false;
        }
    }
}