using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DesalinationControl : MonoBehaviour
{
    public SpriteMeshChangeColor[] spriteMeshChangeColor;
    public GameObject[] solarPanels;
    public GameObject[] buildings;
    public GameObject[] nPCSprites;
    public SpriteRenderer[] nPCWindowsSchool, nPCWindowsHospital;
    public float waterBrineFillLevel, currentWaterOutput, currentWaterShare, currentPopulation;
    public SliderValueToText sliderValueWaterShare;
    private float resultWaterShare;
    public SWEMNPCAnimations sWEMNPCAnimations;
    public GameObject water, brine;
    public Vector3 waterTransformOri, brineTransformOri;
    public Vector3 waterTransformMax, brineTransformMax;



    private void Start()
    {
        ChangePlantColors();
        ChangeSolarPanelVolume();
        ChangeNPCBuildings();
        WaterBrineVolume();
        sWEMNPCAnimations.NPCAnimationControl(currentPopulation, currentWaterShare);
    }

    public void ChangeValues()
    {
        resultWaterShare = (float)(currentWaterOutput * currentWaterShare * 0.01f);
        waterBrineFillLevel = (float)(Math.Round(currentWaterOutput / 6000, 2));

        ChangePlantColors();
        ChangeSolarPanelVolume();
        ChangeNPCBuildings();
        WaterBrineVolume();
        sWEMNPCAnimations.NPCAnimationControl(currentPopulation, currentWaterShare);
    }

    private void WaterBrineVolume()
    {
        water.transform.localPosition = new Vector3(16.77f, Mathf.Lerp(waterTransformOri.y, waterTransformMax.y, (float)Math.Round(waterBrineFillLevel, 2)), 2.44f);
        brine.transform.localPosition = new Vector3(16.77f, Mathf.Lerp(brineTransformOri.y, brineTransformMax.y, (float)Math.Round(waterBrineFillLevel, 2)), 14.8f);
    }

    private void ChangeSolarPanelVolume()
    {
        if (currentWaterOutput >= 0)
            solarPanels[0].SetActive(true);
        else
            solarPanels[0].SetActive(false);

        if (currentWaterOutput >= 100)
            solarPanels[1].SetActive(true);
        else
            solarPanels[1].SetActive(false);

        if (currentWaterOutput >= 200)
            solarPanels[2].SetActive(true);
        else
            solarPanels[2].SetActive(false);

        if (currentWaterOutput >= 300)
            solarPanels[3].SetActive(true);
        else
            solarPanels[3].SetActive(false);

        if (currentWaterOutput >= 400)
            solarPanels[4].SetActive(true);
        else
            solarPanels[4].SetActive(false);

        if (currentWaterOutput >= 500)
            solarPanels[5].SetActive(true);
        else
            solarPanels[5].SetActive(false);

        if (currentWaterOutput >= 600)
            solarPanels[6].SetActive(true);
        else
            solarPanels[6].SetActive(false);

        if (currentWaterOutput >= 700)
            solarPanels[7].SetActive(true);
        else
            solarPanels[7].SetActive(false);

        if (currentWaterOutput >= 800)
            solarPanels[8].SetActive(true);
        else
            solarPanels[8].SetActive(false);

        if (currentWaterOutput >= 900)
            solarPanels[9].SetActive(true);
        else
            solarPanels[9].SetActive(false);

        if (currentWaterOutput >= 1000)
            solarPanels[10].SetActive(true);
        else
            solarPanels[10].SetActive(false);

        if (currentWaterOutput >= 1100)
            solarPanels[11].SetActive(true);
        else
            solarPanels[11].SetActive(false);

        if (currentWaterOutput >= 1200)
            solarPanels[12].SetActive(true);
        else
            solarPanels[12].SetActive(false);

        if (currentWaterOutput >= 1300)
            solarPanels[13].SetActive(true);
        else
            solarPanels[13].SetActive(false);

        if (currentWaterOutput >= 1400)
            solarPanels[14].SetActive(true);
        else
            solarPanels[14].SetActive(false);

        if (currentWaterOutput >= 1500)
            solarPanels[15].SetActive(true);
        else
            solarPanels[15].SetActive(false);

        if (currentWaterOutput >= 1600)
            solarPanels[16].SetActive(true);
        else
            solarPanels[16].SetActive(false);

        if (currentWaterOutput >= 1700)
            solarPanels[17].SetActive(true);
        else
            solarPanels[17].SetActive(false);

        if (currentWaterOutput >= 1800)
            solarPanels[18].SetActive(true);
        else
            solarPanels[18].SetActive(false);

        if (currentWaterOutput >= 1900)
            solarPanels[19].SetActive(true);
        else
            solarPanels[19].SetActive(false);

        if (currentWaterOutput >= 2000)
            solarPanels[20].SetActive(true);
        else
            solarPanels[20].SetActive(false);

        if (currentWaterOutput >= 2100)
            solarPanels[21].SetActive(true);
        else
            solarPanels[21].SetActive(false);

        if (currentWaterOutput >= 2200)
            solarPanels[22].SetActive(true);
        else
            solarPanels[22].SetActive(false);

        if (currentWaterOutput >= 2300)
            solarPanels[23].SetActive(true);
        else
            solarPanels[23].SetActive(false);

        if (currentWaterOutput >= 2400)
            solarPanels[24].SetActive(true);
        else
            solarPanels[24].SetActive(false);

        if (currentWaterOutput >= 2500)
            solarPanels[25].SetActive(true);
        else
            solarPanels[25].SetActive(false);

        if (currentWaterOutput >= 2600)
            solarPanels[26].SetActive(true);
        else
            solarPanels[26].SetActive(false);

        if (currentWaterOutput >= 2700)
            solarPanels[27].SetActive(true);
        else
            solarPanels[27].SetActive(false);

        if (currentWaterOutput >= 2800)
            solarPanels[28].SetActive(true);
        else
            solarPanels[28].SetActive(false);

        if (currentWaterOutput >= 2900)
            solarPanels[29].SetActive(true);
        else
            solarPanels[29].SetActive(false);

        if (currentWaterOutput >= 3000)
            solarPanels[30].SetActive(true);
        else
            solarPanels[30].SetActive(false);

        if (currentWaterOutput >= 3100)
            solarPanels[31].SetActive(true);
        else
            solarPanels[31].SetActive(false);

        if (currentWaterOutput >= 3200)
            solarPanels[32].SetActive(true);
        else
            solarPanels[32].SetActive(false);

        if (currentWaterOutput >= 3300)
            solarPanels[33].SetActive(true);
        else
            solarPanels[33].SetActive(false);

        if (currentWaterOutput >= 3400)
            solarPanels[34].SetActive(true);
        else
            solarPanels[34].SetActive(false);

        if (currentWaterOutput >= 3500)
            solarPanels[35].SetActive(true);
        else
            solarPanels[35].SetActive(false);

        if (currentWaterOutput >= 3600)
            solarPanels[36].SetActive(true);
        else
            solarPanels[36].SetActive(false);

        if (currentWaterOutput >= 3700)
            solarPanels[37].SetActive(true);
        else
            solarPanels[37].SetActive(false);

        if (currentWaterOutput >= 3800)
            solarPanels[38].SetActive(true);
        else
            solarPanels[38].SetActive(false);

        if (currentWaterOutput >= 3900)
            solarPanels[39].SetActive(true);
        else
            solarPanels[39].SetActive(false);

        if (currentWaterOutput >= 4000)
            solarPanels[40].SetActive(true);
        else
            solarPanels[40].SetActive(false);

        if (currentWaterOutput >= 4100)
            solarPanels[41].SetActive(true);
        else
            solarPanels[41].SetActive(false);

        if (currentWaterOutput >= 4200)
            solarPanels[42].SetActive(true);
        else
            solarPanels[42].SetActive(false);

        if (currentWaterOutput >= 4300)
            solarPanels[43].SetActive(true);
        else
            solarPanels[43].SetActive(false);

        if (currentWaterOutput >= 4400)
            solarPanels[44].SetActive(true);
        else
            solarPanels[44].SetActive(false);

        if (currentWaterOutput >= 4500)
            solarPanels[45].SetActive(true);
        else
            solarPanels[45].SetActive(false);

        if (currentWaterOutput >= 4600)
            solarPanels[46].SetActive(true);
        else
            solarPanels[46].SetActive(false);

        if (currentWaterOutput >= 4700)
            solarPanels[47].SetActive(true);
        else
            solarPanels[47].SetActive(false);

        if (currentWaterOutput >= 4800)
            solarPanels[48].SetActive(true);
        else
            solarPanels[48].SetActive(false);

        if (currentWaterOutput >= 4900)
            solarPanels[49].SetActive(true);
        else
            solarPanels[49].SetActive(false);

        if (currentWaterOutput >= 5000)
            solarPanels[50].SetActive(true);
        else
            solarPanels[50].SetActive(false);

        if (currentWaterOutput >= 5100)
            solarPanels[51].SetActive(true);
        else
            solarPanels[51].SetActive(false);

        if (currentWaterOutput >= 5200)
            solarPanels[52].SetActive(true);
        else
            solarPanels[52].SetActive(false);

        if (currentWaterOutput >= 5300)
            solarPanels[53].SetActive(true);
        else
            solarPanels[53].SetActive(false);

        if (currentWaterOutput >= 5400)
            solarPanels[54].SetActive(true);
        else
            solarPanels[54].SetActive(false);

        if (currentWaterOutput >= 5500)
            solarPanels[55].SetActive(true);
        else
            solarPanels[55].SetActive(false);

        if (currentWaterOutput >= 5600)
            solarPanels[56].SetActive(true);
        else
            solarPanels[56].SetActive(false);

        if (currentWaterOutput >= 5700)
            solarPanels[57].SetActive(true);
        else
            solarPanels[57].SetActive(false);

        if (currentWaterOutput >= 5800)
            solarPanels[58].SetActive(true);
        else
            solarPanels[58].SetActive(false);

        if (currentWaterOutput >= 6000)
            solarPanels[59].SetActive(true);
        else
            solarPanels[59].SetActive(false);
    }

    private void ChangePlantColors()
    {
        //if (resultWaterShare >= 20) // this is inactive cause having no agriculture would be unrealistic
        //    spriteMeshChangeColor[0].ChangeToGreen();
        //else
        //    spriteMeshChangeColor[0].ChangeToBrown();

        if (resultWaterShare >= 20) // last value 120
            spriteMeshChangeColor[1].ChangeToGreen();
        else
            spriteMeshChangeColor[1].ChangeToBrown();

        if (resultWaterShare >= 600)
            spriteMeshChangeColor[2].ChangeToGreen();
        else
            spriteMeshChangeColor[2].ChangeToBrown();

        if (resultWaterShare >= 1200)
            spriteMeshChangeColor[3].ChangeToGreen();
        else
            spriteMeshChangeColor[3].ChangeToBrown();

        if (resultWaterShare >= 1500)
            spriteMeshChangeColor[4].ChangeToGreen();
        else
            spriteMeshChangeColor[4].ChangeToBrown();

        if (resultWaterShare >= 1800)
            spriteMeshChangeColor[5].ChangeToGreen();
        else
            spriteMeshChangeColor[5].ChangeToBrown();

        if (resultWaterShare >= 2100)
            spriteMeshChangeColor[6].ChangeToGreen();
        else
            spriteMeshChangeColor[6].ChangeToBrown();

        if (resultWaterShare >= 2400)
            spriteMeshChangeColor[7].ChangeToGreen();
        else
            spriteMeshChangeColor[7].ChangeToBrown();

        if (resultWaterShare >= 2700)
            spriteMeshChangeColor[8].ChangeToGreen();
        else
            spriteMeshChangeColor[8].ChangeToBrown();

        if (resultWaterShare >= 3000)
            spriteMeshChangeColor[9].ChangeToGreen();
        else
            spriteMeshChangeColor[9].ChangeToBrown();

        if (resultWaterShare >= 3300)
            spriteMeshChangeColor[10].ChangeToGreen();
        else
            spriteMeshChangeColor[10].ChangeToBrown();

        if (resultWaterShare >= 3600)
            spriteMeshChangeColor[11].ChangeToGreen();
        else
            spriteMeshChangeColor[11].ChangeToBrown();

        if (resultWaterShare >= 3900)
            spriteMeshChangeColor[12].ChangeToGreen();
        else
            spriteMeshChangeColor[12].ChangeToBrown();

        if (resultWaterShare >= 4200)
            spriteMeshChangeColor[13].ChangeToGreen();
        else
            spriteMeshChangeColor[13].ChangeToBrown();

        if (resultWaterShare >= 4500)
            spriteMeshChangeColor[14].ChangeToGreen();
        else
            spriteMeshChangeColor[14].ChangeToBrown();

        if (resultWaterShare >= 4800)
            spriteMeshChangeColor[15].ChangeToGreen();
        else
            spriteMeshChangeColor[15].ChangeToBrown();

        if (resultWaterShare >= 5100)
            spriteMeshChangeColor[16].ChangeToGreen();
        else
            spriteMeshChangeColor[16].ChangeToBrown();

        if (resultWaterShare >= 5400)
            spriteMeshChangeColor[17].ChangeToGreen();
        else
            spriteMeshChangeColor[17].ChangeToBrown();

        if (resultWaterShare >= 5700)
            spriteMeshChangeColor[18].ChangeToGreen();
        else
            spriteMeshChangeColor[18].ChangeToBrown();

        if (resultWaterShare >= 6000)
            spriteMeshChangeColor[19].ChangeToGreen();
        else
            spriteMeshChangeColor[19].ChangeToBrown();
    }

    private void ChangeNPCBuildings()
    {
        // change height of school and hospital with 0, 2000, 4000, 6000 water
        // change shown 2d npcs inside building regarding population

        // active buildings
        if (currentPopulation >= 0)
        {
            buildings[0].SetActive(true);
            buildings[1].SetActive(true);
            nPCSprites[0].SetActive(true);
            nPCSprites[1].SetActive(true);
        }

        if (currentPopulation >= 33000)
        {
            buildings[2].SetActive(true);
            buildings[3].SetActive(true);
            nPCSprites[2].SetActive(true);
            nPCSprites[3].SetActive(true);
        }
        else
        {
            buildings[2].SetActive(false);
            buildings[3].SetActive(false);
            nPCSprites[2].SetActive(false);
            nPCSprites[3].SetActive(false);
        }

        if (currentPopulation >= 67000)
        {
            buildings[4].SetActive(true);
            buildings[5].SetActive(true);
            nPCSprites[4].SetActive(true);
            nPCSprites[5].SetActive(true);
        }
        else
        {
            buildings[4].SetActive(false);
            buildings[5].SetActive(false);
            nPCSprites[4].SetActive(false);
            nPCSprites[5].SetActive(false);
        }

        if (currentPopulation >= 100000)
        {
            buildings[6].SetActive(true);
            buildings[7].SetActive(true);
            nPCSprites[6].SetActive(true);
            nPCSprites[7].SetActive(true);
        }
        else
        {
            buildings[6].SetActive(false);
            buildings[7].SetActive(false);
            nPCSprites[6].SetActive(false);
            nPCSprites[7].SetActive(false);
        }

        ChangeWindowSchool();
        ChangeWindowHospital();
    }

    private void ChangeWindowSchool()
    {
        //active npc 2d graphics in buildings
        // index up to 35,   activate 22/23/24/25 && 32/33/34/35 together

        if (currentWaterShare <= 75)
        {
            nPCWindowsSchool[0].color = Color.white;
            nPCWindowsSchool[1].color = Color.white;
            nPCWindowsSchool[2].color = Color.white;
            nPCWindowsSchool[3].color = Color.white;
        }
        else if (currentWaterShare >= 75.1)
        {
            nPCWindowsSchool[0].color = Color.black;
            nPCWindowsSchool[1].color = Color.black;
            nPCWindowsSchool[2].color = Color.black;
            nPCWindowsSchool[3].color = Color.black;
        }

        if (currentWaterShare <= 50)
        {
            nPCWindowsSchool[4].color = Color.white;
            nPCWindowsSchool[5].color = Color.white;
            nPCWindowsSchool[6].color = Color.white;
            nPCWindowsSchool[7].color = Color.white;
        }
        else if (currentWaterShare >= 50.1)
        {
            nPCWindowsSchool[4].color = Color.black;
            nPCWindowsSchool[5].color = Color.black;
            nPCWindowsSchool[6].color = Color.black;
            nPCWindowsSchool[7].color = Color.black;
        }

        if (currentWaterShare <= 25)
        {
            nPCWindowsSchool[8].color = Color.white;
            nPCWindowsSchool[9].color = Color.white;
            nPCWindowsSchool[10].color = Color.white;
            nPCWindowsSchool[11].color = Color.white;
        }
        else if (currentWaterShare >= 25.1)
        {
            nPCWindowsSchool[8].color = Color.black;
            nPCWindowsSchool[9].color = Color.black;
            nPCWindowsSchool[10].color = Color.black;
            nPCWindowsSchool[11].color = Color.black;
        }

        if (currentWaterShare <= 0)
        {
            nPCWindowsSchool[12].color = Color.white;
            nPCWindowsSchool[13].color = Color.white;
            nPCWindowsSchool[14].color = Color.white;
            nPCWindowsSchool[15].color = Color.white;
            nPCWindowsSchool[16].color = Color.white;
            nPCWindowsSchool[17].color = Color.white;
        }
        else if (currentWaterShare >= 0.1)
        {
            nPCWindowsSchool[12].color = Color.black;
            nPCWindowsSchool[13].color = Color.black;
            nPCWindowsSchool[14].color = Color.black;
            nPCWindowsSchool[15].color = Color.black;
            nPCWindowsSchool[16].color = Color.black;
            nPCWindowsSchool[17].color = Color.black;
        }
    }

    private void ChangeWindowHospital()
    {
        if (currentWaterShare >= 0.1)
        {
            nPCWindowsHospital[0].color = Color.white;
            nPCWindowsHospital[1].color = Color.white;
            nPCWindowsHospital[2].color = Color.white;
            nPCWindowsHospital[3].color = Color.white;
        }
        else if (currentWaterShare <= 0)
        {
            nPCWindowsHospital[0].color = Color.black;
            nPCWindowsHospital[1].color = Color.black;
            nPCWindowsHospital[2].color = Color.black;
            nPCWindowsHospital[3].color = Color.black;
        }

        if (currentWaterShare >= 25.1)
        {
            nPCWindowsHospital[4].color = Color.white;
            nPCWindowsHospital[5].color = Color.white;
            nPCWindowsHospital[6].color = Color.white;
            nPCWindowsHospital[7].color = Color.white;
        }
        else if (currentWaterShare <= 25)
        {
            nPCWindowsHospital[4].color = Color.black;
            nPCWindowsHospital[5].color = Color.black;
            nPCWindowsHospital[6].color = Color.black;
            nPCWindowsHospital[7].color = Color.black;
        }

        if (currentWaterShare >= 50.1)
        {
            nPCWindowsHospital[8].color = Color.white;
            nPCWindowsHospital[9].color = Color.white;
            nPCWindowsHospital[10].color = Color.white;
            nPCWindowsHospital[11].color = Color.white;
        }
        else if (currentWaterShare <= 50)
        {
            nPCWindowsHospital[8].color = Color.black;
            nPCWindowsHospital[9].color = Color.black;
            nPCWindowsHospital[10].color = Color.black;
            nPCWindowsHospital[11].color = Color.black;
        }

        if (currentWaterShare >= 75.1)
        {
            nPCWindowsHospital[12].color = Color.white;
            nPCWindowsHospital[13].color = Color.white;
            nPCWindowsHospital[14].color = Color.white;
            nPCWindowsHospital[15].color = Color.white;
            nPCWindowsHospital[16].color = Color.white;
            nPCWindowsHospital[17].color = Color.white;
        }
        else if (currentWaterShare <= 75)
        {
            nPCWindowsHospital[12].color = Color.black;
            nPCWindowsHospital[13].color = Color.black;
            nPCWindowsHospital[14].color = Color.black;
            nPCWindowsHospital[15].color = Color.black;
            nPCWindowsHospital[16].color = Color.black;
            nPCWindowsHospital[17].color = Color.black;
        }
    }
}
