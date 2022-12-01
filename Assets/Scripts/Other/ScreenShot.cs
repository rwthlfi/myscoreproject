using UnityEngine;

// Generate a screenshot and save to disk with the name SomeLevel.png.

public class ScreenShot : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ScreenCapture.CaptureScreenshot("SWF_Screenshot", 3);
        }
    }
}