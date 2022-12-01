using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    //Rigidbody
    public Rigidbody rb;

    //Movement Variable
    public bool allowWander = false;
    public float moveSpeed = 1.5f;
    public float rotSpeed = 100f;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;

    //Randomize variable
    float rotTime;
    float rotateWait;
    int rotateLorR;
    float walkWait ;
    float walkTime;
    public Vector3 randomDirection;
    /*
    float forceValue;
    int forceActivate;
    */

    //for the enumarator
    // keep a copy of the executing script
    private IEnumerator coroutine;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        coroutine = Wandering();
    }

    public virtual void Update()
    {
        if (!allowWander) // dont do anything if the wandering is not allowed.
            return;

        if (isWandering == false)
        {
            StartCoroutine(Wandering());
        }
        if (isRotatingRight == true)
        {
            transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(randomDirection), 2f * Time.deltaTime);
            //transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed);
        }
        if (isRotatingLeft == true)
        {
            transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(randomDirection), 2f * Time.deltaTime);
            //transform.Rotate(Vector3.up * Time.deltaTime * -rotSpeed);
        }
        if (isWalking == true)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }
    IEnumerator Wandering()
    {
        isWandering = true;

        rotTime = Random.Range(0, 5);
        rotateWait = Random.Range(0, 5);
        rotateLorR = Random.Range(0, 3);
        walkWait = Random.Range(0, 5);
        walkTime = Random.Range(0, 6);
        randomDirection = new Vector3(Random.Range(-60f, 60f), Random.Range(-180f, 180f), 0);

        /*
        forceValue = Random.Range(25, 100);
        forceActivate = Random.Range(0, 2);
        */
        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotateWait);


        if (rotateLorR == 1)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingRight = false;
        }
        if (rotateLorR == 2)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingLeft = false;
        }

        /*
        if (forceActivate == 1)
        {
            //print("Explode " + name);
            rb.AddForce(forceValue, forceValue, forceValue);
        }
        */

        isWandering = false;
    }

    /// <summary>
    /// to stop the wandering script immediately.
    /// </summary>
    public void StopWanderingImmediately()
    {
        if (coroutine == null)
            coroutine = Wandering();

        StopCoroutine(coroutine);
        allowWander = false;


    }
}
