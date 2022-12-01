using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
   
    public void SetColorRed()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public void SetColorBlue()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}
