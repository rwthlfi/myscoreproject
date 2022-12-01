using UnityEngine;
using UnityEngine.UI;

//extension to the Button 
public class Button_AddOn : MonoBehaviour
{
    [Header("Button Var")]
    public Image imgButton;

    [Header("Object Var")]
    public Transform _pivot;
    public float allowedDist = 0.1f;

    [Header("Color Var")]
    public bool active = false;
    public Color color_deactivate = new Color();
    public Color color_activate = new Color();

    /// <summary>
    /// to activate or deactivate a gameObject
    /// </summary>
    /// <param name="_go"></param>
    public void ActivateObj(GameObject _go)
    {
        _go.SetActive(!_go.activeSelf);
    }

    /// <summary>
    /// to call back the object in case its wandering to far.
    /// </summary>
    /// <param name="_go"></param>
    public void CallObject(Transform _go)
    {
        if(Vector3.Distance(_go.position, _pivot.position) > allowedDist)
        {
            StartCoroutine(LerpingExtensions.MoveTo(_go, _pivot.position, 1f));
            StartCoroutine(LerpingExtensions.RotateTo(_go, _pivot.rotation, 1f));
        }
    }


    /// <summary>
    /// To change the color of a button
    /// </summary>
    public void ChangeColor()
    {
        if(!active)
        {
            active = true;
            imgButton.color = color_activate;
        }
        else
        {
            active = false;
            imgButton.color = color_deactivate;

        }
    }

}
