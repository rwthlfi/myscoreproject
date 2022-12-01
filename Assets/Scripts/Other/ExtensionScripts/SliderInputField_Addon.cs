using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Globalization;
using System.Text.RegularExpressions;

//Add-on to synchronize the inputfield and the Slider
public class SliderInputField_Addon : MonoBehaviour
{
    public Slider slider;
    public TMP_InputField inputField;

    public float minVal = 0;
    public float maxVal = 10;


    private void Start()
    {
        //init the Slider's min and max value
        slider.minValue = minVal;
        slider.maxValue = maxVal;

        slider.value = minVal;
        inputField.text = minVal.ToString("f1");
    }


    //attach this to the slider
    public void UpdateFromSliderToInputField()
    {
        inputField.text = slider.value.ToString("f1");
    }

    //attach this to the inputField
    public void UpdateFromInputFieldToSlider()
    {
        CheckTheValue();
        slider.Set(float.Parse(inputField.text), false);
    }



    private void CheckTheValue()
    {
        if (inputField.text == "")
            inputField.text = "0";
        if (float.Parse(inputField.text) <= minVal)
            inputField.text = minVal.ToString("f1");

        if (float.Parse(inputField.text) > maxVal)
            inputField.text = maxVal.ToString("f1");
    }


}
