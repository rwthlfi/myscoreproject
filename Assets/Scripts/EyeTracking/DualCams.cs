using UnityEngine;

public class DualCams : MonoBehaviour
{

    private bool _otherCamGameViewShowing;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (_otherCamGameViewShowing == false)
            {
                _otherCamGameViewShowing = true;
                UnityEngine.XR.XRSettings.gameViewRenderMode = UnityEngine.XR.GameViewRenderMode.None;
            }
            else
            {
                _otherCamGameViewShowing = false;
                UnityEngine.XR.XRSettings.gameViewRenderMode = UnityEngine.XR.GameViewRenderMode.LeftEye;
            }
        }
    }
}