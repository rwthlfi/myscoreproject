using UnityEngine;
using UnityEngine.UI;

//This script is to make the Toggle goes on and off with a text /Image
public class ToggleOnOff_AddOn : MonoBehaviour
{

    public GameObject textOn;
    public GameObject textOff;

    public Toggle _toggleObj;

    private void Awake()
    {
        //start at the off Position first
        //ShowTextOn(false);
    }

    public void OnToggleClicked(Toggle _toggle)
    {
        ShowTextOn(_toggle.isOn);
    }

    private void ShowTextOn(bool _value)
    {
        textOn.SetActive(!_value);
        textOff.SetActive(_value);
    }

    public void ActivateObj(GameObject _obj)
    {
        _obj.SetActive(_toggleObj.isOn);
    }

}
