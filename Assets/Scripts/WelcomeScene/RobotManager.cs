using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    public Transform ovrLeftHand;
    public Transform ovrRightHand;

    public Transform robotLeftHand;
    public Transform robotRightHand;

    public bool isGettingHighFive = false;
    public float breakDistance = 0.2f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance(ovrLeftHand, robotLeftHand);
        CheckDistance(ovrRightHand, robotLeftHand);
    }

    private void CheckDistance(Transform _hand, Transform _point)
    {
        float dist = Vector3.Distance(_hand.transform.position, _point.position);
        //Debug.Log("Distance " + dist);
        //if distance is less than "breakDistance"; then break the grabbing
        if (dist <= breakDistance)
        {
            isGettingHighFive = true;
        }
        else
        {
            isGettingHighFive = false;
        }
    }
}
