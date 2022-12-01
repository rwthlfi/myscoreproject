using System.Collections;
using UnityEngine;

public class PourDetector : MonoBehaviour
{
    public int pourThreshold = 45;
    public Transform origin = null;
    public GameObject streamPrefab = null;

    private bool isPouring = false;
    private BottleStream currentStream = null;

    private void Update()
    {
        bool pourCheck = CalculatePourAngle() < pourThreshold;
        
        if (isPouring != pourCheck)
        {
            isPouring = pourCheck;
            if (isPouring)
            {
                StartPour();
            }

            else
            {
                EndPour();
            }
        }
    }

    private void StartPour()
    {
        //print("start");
        currentStream = CreateStream();
        currentStream.Begin();
    }

    private void EndPour()
    {
        //print("end");
        currentStream.End();
        currentStream = null;
    }

    private float CalculatePourAngle()
    {
        
        return transform.up.y * Mathf.Rad2Deg;
    }

    private BottleStream CreateStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
        return streamObject.GetComponent<BottleStream>();
    }


}