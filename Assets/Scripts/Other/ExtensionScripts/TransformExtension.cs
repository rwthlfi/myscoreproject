using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//references
//https://answers.unity.com/questions/589983/using-mathfround-for-a-vector3.html

//usage: 
/*
    vector3 = new Vector3(1.23243f, 2.213124f, 12.4123f);
    var roundedVector3 = vector3.Round(1);
*/
static class TransformExtension
{
    /// <summary>
    /// Rounds Vector3 and Quaternion.
    /// </summary>
    /// <param name="vector3"></param>
    /// <param name="decimalPlaces"></param>
    /// <returns></returns>
    public static Vector3 Round(this Vector3 vector3, int decimalPlaces = 2)
    {
        float multiplier = 1;
        for (int i = 0; i < decimalPlaces; i++)
        {
            multiplier *= 10f;
        }
        return new Vector3(
            Mathf.Round(vector3.x * multiplier) / multiplier,
            Mathf.Round(vector3.y * multiplier) / multiplier,
            Mathf.Round(vector3.z * multiplier) / multiplier);
    }

    public static Quaternion Round(this Quaternion quaternion, int decimalPlaces = 2)
    {
        float multiplier = 1;
        for (int i = 0; i < decimalPlaces; i++)
        {
            multiplier *= 10f;
        }

        return new Quaternion(
            Mathf.Round(quaternion.x * multiplier) / multiplier,
            Mathf.Round(quaternion.y * multiplier) / multiplier,
            Mathf.Round(quaternion.z * multiplier) / multiplier,
            Mathf.Round(quaternion.w * multiplier) / multiplier

            );
    }
}
