using UnityEngine;

public class Rotator : MonoBehaviour
{

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 12) * Time.deltaTime);
    }
}
