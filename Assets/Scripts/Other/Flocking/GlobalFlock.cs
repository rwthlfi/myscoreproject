using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour
{
    public float worldSize = 10f; // just the world size, or how far the fish are allowed to travel.
    public GameObject goalPrefab; // the target for flocking
    [System.NonSerialized] public Vector3 goalPos;
    public List<GameObject> allAnimals = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        AssignFlockingTarget();
    }

    private float nextActionTime = 0.0f;
    private float period = 0.1f;
    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            period = Random.Range(1, 5); // will execute randomly every x seconds
            goalPos = goalPrefab.transform.position; //change the goal Position since the "Flocking" is determine by the position.

            if (Random.Range(0, worldSize*2) < 50)
            {
                goalPos = new Vector3(Random.Range(-worldSize, worldSize),
                                      Random.Range(-worldSize, worldSize),
                                      Random.Range(-worldSize, worldSize));
                goalPrefab.transform.position = goalPos;
            }
        }

    }

    /// <summary>
    /// assign the flocking target. Therefore you dont need to assign them one by one in inspector
    /// </summary>
    private void AssignFlockingTarget()
    {
        foreach (GameObject flock in allAnimals)
        {
            flock.GetComponent<Flock>().globalFlock = this;
        }
    }

    /// <summary>
    /// Activating the "Wander" script while deactivating the Flocking
    /// </summary>
    /// <param name="_value"></param>
    public void DoAllWander(bool _value)
    {
        foreach (GameObject flock in allAnimals)
        {
            flock.GetComponent<Wander>().enabled = _value;
            flock.GetComponent<Flock>().enabled = !_value;
        }
    }

    /// <summary>
    /// only allowed from 0 until 100
    /// </summary>
    /// <param name="_percentage">percentage from 0 till 10</param>
    public void DoRandomWander(int _percentage)
    {
        if (_percentage == 0) // dont need to do things anymore
            return;
        var wander = (float)_percentage / 100f * allAnimals.Count;

        for(int i = 0; i < wander; i++)
        {
            allAnimals[i].GetComponent<Wander>().enabled = true;
        }

        //the rest of the animals
        for(int i = (int)wander; i< allAnimals.Count; i++ )
        {

            allAnimals[i].GetComponent<Flock>().enabled = true;
        }
    }

    /// <summary>
    /// Activating ALL the flocking, while deactivating the Wandering script
    /// </summary>
    /// <param name="_value"></param>
    public void DoAllFlock(bool _value)
    {
        foreach (GameObject flock in allAnimals)
        {
            flock.GetComponent<Wander>().enabled = !_value;
            flock.GetComponent<Flock>().enabled = _value;
        }
    }


    /// <summary>
    /// increase the neighbouring distance. Therefore more fish will group together
    /// </summary>
    /// <param name="_neighbourDistance"></param>
    public void SetFlockingNeighbourDist(float _neighbourDistance)
    {
        foreach(GameObject flock in allAnimals)
        {
            flock.GetComponent<Flock>().neighbourDistance = _neighbourDistance;
        }
    }
}
