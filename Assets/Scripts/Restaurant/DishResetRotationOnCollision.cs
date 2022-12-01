using UnityEngine;

public class DishResetRotationOnCollision : MonoBehaviour
{
    private Vector3 _localrotation;
    private Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _localrotation = transform.localEulerAngles;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Breakable" && collision.gameObject.name == "Ground")
        {
            _rigidbody.velocity = new Vector3(0,0,0);
            _rigidbody.angularVelocity = new Vector3(0, 0, 0);
            transform.localEulerAngles = _localrotation;

            /*
            if (GetComponentInChildren<Wobble>())
            {
                var liquid = GetComponentInChildren<Wobble>().gameObject;
                liquid.transform.localPosition = new Vector3(0, -0.07f, 0);
                liquid.transform.localScale = new Vector3(10, 10, 0.01f);
            }*/
        }
    }
}