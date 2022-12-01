using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CascadeSimulationScreens : MonoBehaviour
{
    public GameObject Timeline1, Timeline2, WetPeriodOverlay, DryPeriodOverlay, HADWaterLevel;
    public GameObject[] Cascades;
    private float _time = 1f;
    Vector3 Timeline1Ori, Timeline2Ori;
    public Vector3 Timeline1Max, Timeline2Max;
    public WaterDamControl _waterDamControl;
    private int _currentCascade, _cascadeIndex, MOLDays, DSLdays;
    public Text Year, Period, CascadeResult1, CascadeResult2;
    public SpriteRenderer Power, Water;
    public float[] CascadeWetOffHighValue, CascadeWetOffLowValue, CascadeWetOnHighValue, CascadeWetOnLowValue, CascadeAverageOffHighValue, CascadeAverageOffLowValue, CascadeAverageOnHighValue, CascadeAverageOnLowValue, CascadeDryOffHighValue, CascadeDryOffLowValue, CascadeDryOnHighValue, CascadeDryOnLowValue;

    void Start() // get original timeline gameobject position
    {
        Timeline1Ori = Timeline1.transform.position;
        Timeline2Ori = Timeline2.transform.position;
    }

    public void Restart() // restart of timeline position and inactivate graphs
    {
        Timeline1.SetActive(false);
        Timeline2.SetActive(false);
        HADWaterLevel.SetActive(false);
        WetPeriodOverlay.SetActive(false);
        DryPeriodOverlay.SetActive(false);
        CascadeResult1.gameObject.SetActive(false); CascadeResult2.gameObject.SetActive(false);
        Power.color = Color.green;
        Water.color = Color.green;
        _cascadeIndex = -1;
        MOLDays = DSLdays = 0;

        foreach (GameObject cascades in Cascades)
        {
            cascades.SetActive(false);
        }
    }

    public void SimulateCascade(int _weather, int _gerd, int _had) // activate corresponding graphs
    {
        Restart();

        _waterDamControl.HADFillLevel = _waterDamControl.MeroweFillLevel = _waterDamControl.SennarFillLevel = _waterDamControl.RoseiresFillLevel = 0f;
        _waterDamControl.GERDFillLevelMax = 0.0f; _waterDamControl.GERDFillLevel = 0.0f;
        _waterDamControl.GERDMesh.gameObject.SetActive(false);

        if (_weather == 0 && _gerd == 0 && _had == 0) // CascadeWetOffHigh
        {
            Cascades[0].SetActive(true);
            _currentCascade = 0;

            MOLDays = 0;
            DSLdays = 0;
        }
        if (_weather == 0 && _gerd == 0 && _had == 1) // CascadeWetOffLow
        {
            Cascades[1].SetActive(true);
            _currentCascade = 1;

            MOLDays = 0;
            DSLdays = 0;
        }
        if (_weather == 0 && _gerd == 1 && _had == 0) // CascadeWetOnHigh
        {
            _waterDamControl.GERDMesh.gameObject.SetActive(true);
            _waterDamControl.GERDFillLevel = 0.4f; // Gerd available 40% fill level
            _waterDamControl.GERDFillLevelMax = 0.4f; // Gerd available 40% max fill level
            Cascades[2].SetActive(true);
            _currentCascade = 2;

            MOLDays = 0;
            DSLdays = 0;
        }
        if (_weather == 0 && _gerd == 1 && _had == 1) // CascadeWetOnLow
        {
            _waterDamControl.GERDMesh.gameObject.SetActive(true);
            _waterDamControl.GERDFillLevel = 0.4f; // Gerd available 40% fill level
            _waterDamControl.GERDFillLevelMax = 0.4f; // Gerd available 40% max fill level
            Cascades[3].SetActive(true);
            _currentCascade = 3;

            MOLDays = 0;
            DSLdays = 0;
        }
        if (_weather == 1 && _gerd == 0 && _had == 0) // CascadeAverageOffHigh
        {
            Cascades[4].SetActive(true);
            _currentCascade = 4;

            MOLDays = 0;
            DSLdays = 0;
        }
        if (_weather == 1 && _gerd == 0 && _had == 1) // CascadeAverageOffLow
        {
            Cascades[5].SetActive(true);
            _currentCascade = 5;

            MOLDays = 720;
            DSLdays = 0;
        }
        if (_weather == 1 && _gerd == 1 && _had == 0) // CascadeAverageOnHigh
        {
            _waterDamControl.GERDMesh.gameObject.SetActive(true);
            _waterDamControl.GERDFillLevel = 0.4f; // Gerd available 40% fill level
            _waterDamControl.GERDFillLevelMax = 0.4f; // Gerd available 40% max fill level
            Cascades[6].SetActive(true);
            _currentCascade = 6;

            MOLDays = 675;
            DSLdays = 0;
        }
        if (_weather == 1 && _gerd == 1 && _had == 1) // CascadeAverageOnLow
        {
            _waterDamControl.GERDMesh.gameObject.SetActive(true);
            _waterDamControl.GERDFillLevel = 0.4f; // Gerd available 40% fill level
            _waterDamControl.GERDFillLevelMax = 0.4f; // Gerd available 40% max fill level
            Cascades[7].SetActive(true);
            _currentCascade = 7;

            MOLDays = 5355;
            DSLdays = 0;
        }
        if (_weather == 2 && _gerd == 0 && _had == 0) // CascadeDryOffHigh
        {
            Cascades[8].SetActive(true);
            _currentCascade = 8;

            MOLDays = 4590;
            DSLdays = 1080;
        }
        if (_weather == 2 && _gerd == 0 && _had == 1) // CascadeDryOffLow
        {
            Cascades[9].SetActive(true);
            _currentCascade = 9;

            MOLDays = 6030;
            DSLdays = 2430;
        }
        if (_weather == 2 && _gerd == 1 && _had == 0) // CascadeDryOnHigh
        {
            _waterDamControl.GERDMesh.gameObject.SetActive(true);
            _waterDamControl.GERDFillLevel = 0.4f; // Gerd available 40% fill level
            _waterDamControl.GERDFillLevelMax = 0.4f; // Gerd available 40% max fill level
            Cascades[10].SetActive(true);
            _currentCascade = 10;

            MOLDays = 5985;
            DSLdays = 2115;
        }
        if (_weather == 2 && _gerd == 1 && _had == 1) // CascadeDryOnLow
        {
            _waterDamControl.GERDMesh.gameObject.SetActive(true);
            _waterDamControl.GERDFillLevel = 0.4f; // Gerd available 40% fill level
            _waterDamControl.GERDFillLevelMax = 0.4f; // Gerd available 40% max fill level
            Cascades[11].SetActive(true);
            _currentCascade = 11;

            MOLDays = 7065;
            DSLdays = 3195;
        }

        if (_weather == 0)
        {
            WetPeriodOverlay.SetActive(true);
            DryPeriodOverlay.SetActive(false);
            Period.text = "(Wet)";
        }
        else if (_weather == 1)
        {
            WetPeriodOverlay.SetActive(false);
            DryPeriodOverlay.SetActive(false);
            Period.text = "(Average)";
        }
        else if (_weather == 2)
        {
            WetPeriodOverlay.SetActive(false);
            DryPeriodOverlay.SetActive(true);
            Period.text = "(Dry)";
        }

        StartCoroutine(StartTimeLine(_time));
    }

    private IEnumerator StartTimeLine(float _time) // simulate green line moving
    {
        _time = 1f;
        float year = 2021;
        Timeline1.transform.position = Timeline1Ori;
        Timeline2.transform.position = Timeline2Ori;
        Timeline1.SetActive(true);
        Timeline2.SetActive(true);
        HADWaterLevel.SetActive(true);
        StartCoroutine(CountCascadeIndex());

        while (_time >= 0.001)
        {
            _time -= .005f;
            Timeline1.transform.position = new Vector3(Timeline1.transform.position.x, Timeline1.transform.position.y, Mathf.Lerp(Timeline1Max.z, Timeline1Ori.z, (float)Math.Round(_time, 2)));
            Timeline2.transform.position = new Vector3(Timeline2.transform.position.x, Timeline2.transform.position.y, Mathf.Lerp(Timeline2Max.z, Timeline2Ori.z, (float)Math.Round(_time, 2)));

            year += 0.125f;
            Year.text = Math.Round(year, 0).ToString();

            if (_cascadeIndex <= 25)
                CascadeCalculate();
            else
                break;

            yield return new WaitForSeconds(0.1f);
        }

        _waterDamControl.WeatherWet.interactable = _waterDamControl.WeatherAverage.interactable = _waterDamControl.WeatherDry.interactable = _waterDamControl.GERDOff.interactable = _waterDamControl.GERDOn.interactable = _waterDamControl.HADWaterHigh.interactable = _waterDamControl.HADWaterLow.interactable = _waterDamControl.StartButton.interactable = _waterDamControl.DAMRestartButton.interactable = true;

        WetPeriodOverlay.SetActive(false);
        DryPeriodOverlay.SetActive(false);

        CascadeResult1.gameObject.SetActive(true); CascadeResult2.gameObject.SetActive(true);
        CascadeResult1.text = "Result: '" + Cascades[_currentCascade].name + "'\n" + MOLDays + " days in Minimum Operating Level (MOL)\n" + DSLdays + " days in Dead Storage Level (DSL)";
        CascadeResult2.text = "Result: '" + Cascades[_currentCascade].name + "'\n" + MOLDays + " days in Minimum Operating Level (MOL)\n" + DSLdays + " days in Dead Storage Level (DSL)";
    }

    private IEnumerator CountCascadeIndex()
    {
        _cascadeIndex = -1;

        while (_cascadeIndex <= 25)
        {
            _cascadeIndex += 1;
            yield return new WaitForSeconds(.8f);
        }
        StopCoroutine(CountCascadeIndex());
    }

    private void CascadeCalculate() // use list of possible % fill here
    {
        // 183m is the maximum height of the HAD dam
        if (_currentCascade == 0)
        {
            _waterDamControl.HADFillLevel = (float)Math.Round(((CascadeWetOffHighValue[_cascadeIndex]) / 183), 2);
        }
        else if (_currentCascade == 1)
        {
            _waterDamControl.HADFillLevel = (float)Math.Round(((CascadeWetOffLowValue[_cascadeIndex]) / 183), 2);
        }
        else if (_currentCascade == 2)
        {
            _waterDamControl.HADFillLevel = (float)Math.Round(((CascadeWetOnHighValue[_cascadeIndex]) / 183), 2);
        }
        else if (_currentCascade == 3)
        {
            _waterDamControl.HADFillLevel = (float)Math.Round(((CascadeWetOnLowValue[_cascadeIndex]) / 183), 2);
        }
        else if (_currentCascade == 4)
        {
            _waterDamControl.HADFillLevel = (float)Math.Round(((CascadeAverageOffHighValue[_cascadeIndex]) / 183), 2);
        }
        else if (_currentCascade == 5)
        {
            _waterDamControl.HADFillLevel = (float)Math.Round(((CascadeAverageOffLowValue[_cascadeIndex]) / 183), 2);
        }
        else if (_currentCascade == 6)
        {
            _waterDamControl.HADFillLevel = (float)Math.Round(((CascadeAverageOnHighValue[_cascadeIndex]) / 183), 2);
        }
        else if (_currentCascade == 7)
        {
            _waterDamControl.HADFillLevel = (float)Math.Round(((CascadeAverageOnLowValue[_cascadeIndex]) / 183), 2);
        }
        else if (_currentCascade == 8)
        {
            _waterDamControl.HADFillLevel = (float)Math.Round(((CascadeDryOffHighValue[_cascadeIndex]) / 183), 2);
        }
        else if (_currentCascade == 9)
        {
            _waterDamControl.HADFillLevel = (float)Math.Round(((CascadeDryOffLowValue[_cascadeIndex]) / 183), 2);
        }
        else if (_currentCascade == 10)
        {
            _waterDamControl.HADFillLevel = (float)Math.Round(((CascadeDryOnHighValue[_cascadeIndex]) / 183), 2);
        }
        else if (_currentCascade == 11)
        {
            _waterDamControl.HADFillLevel = (float)Math.Round(((CascadeDryOnLowValue[_cascadeIndex]) / 183), 2);
        }

        // changes the UI over HAD corresponding to the water level
        if (_waterDamControl.HADFillLevel <= 0.8709f && _waterDamControl.HADFillLevel >= 0.81f)
        {
            Power.color = Color.red;
            Water.color = Color.yellow;
            //MOLDays += 45;
        }
        else if (_waterDamControl.HADFillLevel <= 0.809f)
        {
            Power.color = Color.red;
            Water.color = Color.red;
            //MOLDays += 45;
            //DSLdays += 45;
        }
        else if (_waterDamControl.HADFillLevel >= 0.871)
        {
            Power.color = Color.green;
            Water.color = Color.green;
        }

        _waterDamControl.HADFillLevel = Mathf.Pow(_waterDamControl.HADFillLevel, 10);
        Debug.Log(_waterDamControl.HADFillLevel);
    }
}