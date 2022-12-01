using UnityEngine;

//add this script to a game object that you want to scale (on runtime) with a UI Slider and reference the slider afterwards

public class ResizeBySlider : MonoBehaviour
{
    private float objScale;
    public UnityEngine.UI.Slider slider;

    public void Resize()
    {
        objScale = slider.value;
        this.transform.localScale = new Vector3(objScale, objScale, objScale);
    }
}