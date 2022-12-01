using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class FPSMeasurement : MonoBehaviour
{
    public TextMeshProUGUI m_Text;
    private float timelapse;
    public float timer, refresh, avgFramerate;
    string display = "{0} FPS";

    private void Start()
    {
    }

    private float nextActionTime = 0.0f;
    public float period = 0.2f;


    private void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
            // execute block of code here

            //Change smoothDeltaTime to deltaTime or fixedDeltaTime to see the difference
            timelapse = Time.smoothDeltaTime;
            timer = timer <= 0 ? refresh : timer -= timelapse;

            if (timer <= 0) avgFramerate = (int)(1f / timelapse);
            m_Text.text = string.Format(display, avgFramerate.ToString());
        }
    }
}