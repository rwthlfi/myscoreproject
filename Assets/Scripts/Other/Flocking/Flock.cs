using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    [Header("Animal Behaviour")]
    public float speed = 0.001f;
    public float minSpeed = 1;
    public float maxSpeed = 10f;
    public float rotationSpeed = 2.0f;
    public float neighbourDistance = 1.0f;
    public bool turning = false;
    private Rigidbody rb;

    [Header("Flock Reference")]
    public GlobalFlock globalFlock;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = Random.Range(minSpeed, maxSpeed);    
    }


    private float nextActionTime = 0.0f;
    public float period = 0.1f;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            period = Random.Range(1, 10); // will execute randomly every 3 seconds
            if (Random.Range(0, 2) == 1) //25% chance the fish will turn
                turning = true;
            else
                turning = false;

            // execute block of code here
        }

        
        if(turning)
        {
            //Vector3 direction = Vector3.zero - transform.position;
            Vector3 direction =  globalFlock.goalPos - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(direction),
                                                  rotationSpeed * Time.deltaTime);
            
            speed = Random.Range(minSpeed, maxSpeed);
        }

        else
        {
            if (Random.Range(0, 5) < 1)
                ApplyRules();
            
        }

        transform.Translate(0, 0, Time.deltaTime * speed);

        //to kinda remove some odd behaviour from the rigidbody
        if (rb.velocity.magnitude >= maxSpeed)
            rb.velocity = Vector3.zero;
        if (rb.angularVelocity.magnitude >= maxSpeed)
            rb.angularVelocity = Vector3.zero;
        
    }


    Vector3 vCenter, vavoid;
    float gSpeed = 0.1f;
    Vector3 goalPos;
    float dist;
    int groupSize = 0;
    private void ApplyRules()
    {
        /*
        GameObject[] gos;
        gos = globalFlock.allFish;
        */
        vCenter = globalFlock.goalPos;
        vavoid = globalFlock.goalPos;

        gSpeed = 0.1f;

        goalPos = globalFlock.goalPos;

        groupSize = 0;
        
        foreach (GameObject go in globalFlock.allAnimals)
        {
            if (go != this.gameObject)
            {
                
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if (dist <= neighbourDistance) 
                {
                    vCenter += go.transform.position;
                    groupSize++;
                    
                    if (dist < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed += anotherFlock.speed;
                }
                
            }

            
            if (groupSize > 0) 
            {
                vCenter = vCenter / groupSize + (goalPos - this.transform.position);
                speed = gSpeed / groupSize;

                Vector3 direction = (vCenter + vavoid) - transform.position;


                if (direction != Vector3.zero)
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                                                          Quaternion.LookRotation(direction),
                                                          rotationSpeed * Time.deltaTime);
            }
        }
    }

}
