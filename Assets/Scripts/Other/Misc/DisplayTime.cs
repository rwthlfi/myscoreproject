using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

public class DisplayTime : MonoBehaviour
{
    public bool isAnalog = true;
    private const float
            hoursToDegrees = 360f / 12f,
            minutesToDegrees = 360f / 60f,
            secondsToDegrees = 360f / 60f;

    public Transform hoursHandPivot, minutesHandPivot, secondsHandPivot;

    public Text textClock;

    TimeSpan timespan;
    DateTime time;
    string hour, minute, second;

    private void Start()
    {
        StartCoroutine(CountTime());
    }

    IEnumerator CountTime()
    {
        if (isAnalog) //analog time measurement
        {
            timespan = DateTime.Now.TimeOfDay;
            hoursHandPivot.localRotation =
                Quaternion.Euler(0f, (float)timespan.TotalHours * -hoursToDegrees, 0f);
            minutesHandPivot.localRotation =
                Quaternion.Euler(0f, (float)timespan.TotalMinutes * -minutesToDegrees, 0f);
            secondsHandPivot.localRotation =
                Quaternion.Euler(0f, (float)timespan.TotalSeconds * -secondsToDegrees, 0f);

        }

        else //digital time measurement
        {
            time = DateTime.Now;
            hour = LeadingZero(time.Hour);
            minute = LeadingZero(time.Minute);
            second = LeadingZero(time.Second);
            textClock.text = hour + ":" + minute + ":" + second;
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(CountTime());
    }

    //adding pre 0 to 1 digit digital time
    string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }
}