using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AutoBoundingCharacterController : MonoBehaviour
{
    public CharacterController charController;
    public Transform targetCentering;

    Vector3 cachePos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateTheCharControllerCenter();
    }

    private void UpdateTheCharControllerCenter()
    {
        targetCentering.localPosition = new Vector3(0, targetCentering.localPosition.y, 0);
        //charController.center = new Vector3(targetCentering.localPosition.x, 1f, targetCentering.localPosition.z);
    }
}
