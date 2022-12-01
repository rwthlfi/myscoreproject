using UnityEngine;
using UnityEngine.Rendering;

public class CameraFadeThroughObject : MonoBehaviour
{
    private Camera _mainCamera;
    public bool IndicatorLeft;
    public bool IndicatorRight;
    public GameObject Indicator;

    private void Start()
    {
        _mainCamera = GetComponent<Camera>();
        _mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider collision)
    {
        // layer 11 is layerGrabbableObject, layer 13 is fadeObject, layer 15 is RealTimeLightning
        if ((collision.gameObject.layer == 13))
        {
            if (IndicatorLeft)
            {
                Indicator.SetActive(true);
            }

            else if (IndicatorRight)
            {
                Indicator.SetActive(true);
            }

            else
            {
                _mainCamera.GetComponent<CameraFade>().FadeCameraIn(0.2f);
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (IndicatorLeft)
        {
            Indicator.SetActive(false);
        }

        else if (IndicatorRight)
        {
            Indicator.SetActive(false);
        }

        else
        {
            _mainCamera.GetComponent<CameraFade>().FadeCameraOut(0.2f);
        }
    }
}