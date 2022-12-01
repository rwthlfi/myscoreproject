using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//this is the boss of all the scattered globalflock.
public class GlobalFlockManager : MonoBehaviour
{
    public List<GlobalFlock> globalFlockList = new List<GlobalFlock>();
    public int flockPercentage = 50;

    [Header("only for debugging")]
    public bool debug = false;
    private void Start()
    {
        globalFlockList = GetComponentsInChildren<GlobalFlock>().ToList();

        foreach (GlobalFlock gf in globalFlockList)
            gf.DoRandomWander(flockPercentage);//about x percent will do wander, while the rest do flocking
    }

    private void Update()
    {
        if(debug)
        {
            debug = false;
            globalFlockList[0].SetFlockingNeighbourDist(100);
        }
    }

}
