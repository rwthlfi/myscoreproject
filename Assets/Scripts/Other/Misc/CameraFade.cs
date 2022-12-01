using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraFade : MonoBehaviour
{
    public Image FadeCanvasImage;

     IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);

        FadeCanvasImage.CrossFadeAlpha(0, 1f, false);
    }

    public void FadeCameraOut(float fadetime)
    {
        FadeCanvasImage.CrossFadeAlpha(0, fadetime, false);
    }

    public void FadeCameraIn(float fadetime)
    {
        FadeCanvasImage.CrossFadeAlpha(1, fadetime, false);
    }
}