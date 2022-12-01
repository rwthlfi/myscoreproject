using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class SliderValueToText : MonoBehaviour
{
    public bool sliderProduction;
    public Slider sliderUI;
    public DesalinationControl desalinationControl;
    public DesalinationNetworkSync desalinationNetworkSync;

    [Header("Slider Freshwater Production")]
    public Text freshwaterVolume;
    public Text freshwaterVolume2, energyNeeded, brineProduced, seawaterNeeded, localPopulation, localPopulation2;

    [Header("Slider Water Share")]
    public Text agriculture;
    public Text sanitation;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);

        if (sliderProduction) 
        {
            sliderUI.value = desalinationNetworkSync.sliderValueProduction;
        }
        else
        {
            sliderUI.value = desalinationNetworkSync.sliderValueShare;
        }
        ShowSliderValue();
    }

    public void ShowSliderValue()
    {
        if (sliderProduction)
        {
            desalinationNetworkSync.ShowSliderValueProduction(sliderUI.value);
        }
        else
        {
            desalinationNetworkSync.ShowSliderValueShare(sliderUI.value);
        }
    }

    public void ChangeSliderValue(float sliderValue)
    {
        if (sliderProduction)
        {
            sliderUI.value = sliderValue;

            string freshwatermessage = "Current Freshwater Output:\n" + (Math.Round((sliderUI.value), 2)) + " m³/day";
            freshwaterVolume.text = freshwatermessage;
            freshwaterVolume2.text = freshwatermessage;

            energyNeeded.text = "Energy Needed:\n" + (Math.Round((sliderUI.value * 8.2), 2)) + " kWh/day" + "\n\n" + "Space needed:\n" + (Math.Round((sliderUI.value * 8.035), 2)) + " m²";

            brineProduced.text = "Brine Produced:\n" + (Math.Round((sliderUI.value * 1.2222), 2)) + " m³/day";

            seawaterNeeded.text = "Seawater Feed:\n" + (Math.Round(((sliderUI.value + (sliderUI.value * 1.2222))), 2)) + " m³/day";

            string populationmessage = "Local Population:\n" + (Math.Round((sliderUI.value * 16.6667), 0));
            localPopulation.text = populationmessage;
            localPopulation2.text = populationmessage;

            desalinationControl.currentWaterOutput = ((float)Math.Round(sliderUI.value, 2));
            desalinationControl.currentPopulation = ((float)Math.Round((sliderUI.value * 16.6667), 0));
        }
        else
        {
            sliderUI.value = sliderValue;
            agriculture.text = sliderUI.maxValue - Math.Round((sliderUI.value), 0) + "% Agriculture";

            sanitation.text = (Math.Round((sliderUI.value), 0)) + "% Sanitation";

            desalinationControl.currentWaterShare = (float)(sliderUI.maxValue - Math.Round((sliderUI.value), 0));
        }

        desalinationControl.ChangeValues();
    }
}