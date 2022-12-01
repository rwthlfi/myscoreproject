
using TMPro;
using UnityEngine;

public class Component_Stopwatch : MonoBehaviour
{
    float timer;
    float seconds;
    float minutes;
    float hours;

    bool startCounting;

    
    private void Start()
    {
        startCounting = false;
        timer = 0;
    }

    private void Update()
    {
        TimerCalculation();
    }

    public string TimerCalculation()
    {
        if (startCounting)
        {
            timer += Time.deltaTime;
            seconds = (int)(timer % 60);
            minutes = (int)((timer / 60) % 60);
            hours = (int)(timer / 3600);

            return hours.ToString("00") + ":"
                   + minutes.ToString("00") + ":"
                   + seconds.ToString("00");
        }

        else
            return "00:00:00";
    }

    public void StartTimer()
    {
        startCounting = true;
    }

    public void stopTimer()
    {
        startCounting = false;
        timer = 0;
    }

    public void resetTimer()
    {
        startCounting = false;
        timer = 0;
    }
}
