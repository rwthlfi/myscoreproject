using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is used to follow the player around if the distance is too far.
public class FollowPlayer : MonoBehaviour
{
    public Transform objectToFollow; // the targetobject that this object will follow.
    private float speed = 5f; // how fast the object follow the player.
    public Vector3 offsetDistance = new Vector3(0.5f, 0.5f, 0.5f); // the "rest" gap that the object will maintain when following the player. otherwise the object will collide with the player. 
    public float followDistance = 2f; // the distance between the two objects that will trigger the "follow the player" effet

    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        if (!objectToFollow)
            return;

        if(gapBetween() >= followDistance)
        {
            Vector3 targetPosition = objectToFollow.transform.position - offsetDistance;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    //to check the distance between the two objects
    private float gapBetween()
    {
        float gap = Vector3.Distance(transform.position, objectToFollow.transform.position);
        return gap;
    }
}