using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Slider_AddOn : MonoBehaviour
{
    public enum Unit 
    { 
        Percentage, Cm, Meter, Radius, Reaction, heightCm, PercentageInvert
    };

    public Unit unit;

    public Slider slider;
    public TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh.text = getValue().ToString("F2") + " " + getUnit();
        
    }



    public void PercentageSlider_isSlided()
    {
        textMesh.text = getValue().ToString("F2") + " " + getUnit();
    }

    //return the desired value
    private float getValue()
    {
        float value = 0f;
        switch (unit)
        {
            case Unit.Percentage: value = (slider.value - slider.minValue) / (slider.maxValue - slider.minValue) * 100;  break;
            case Unit.PercentageInvert: value = Mathf.Abs(((slider.value - slider.minValue)/ (slider.maxValue - slider.minValue) * 100) -100);  break;

            case Unit.Cm: value = slider.value ; break;
            case Unit.heightCm: value = GlobalSettings.heightScaleConverter(slider.value) ; break;
            case Unit.Radius: value = slider.value; break;
            case Unit.Reaction: value = slider.value; break;
            default: value = slider.value; break;
        }
        return value;
    }


    //return the selected unit
    private string getUnit()
    {
        string a = "%"; // this is by default
        switch (unit)
        {
            case Unit.Percentage: a = "%"; break;
            case Unit.Cm: a = "Cm"; break;
            case Unit.heightCm: a = "Cm"; break;            
            case Unit.Meter: a = "Meter"; break;
            case Unit.Radius: a = "Radius"; break;
            
            default:  a = "";  break;
        }

        return a;
    }

}
