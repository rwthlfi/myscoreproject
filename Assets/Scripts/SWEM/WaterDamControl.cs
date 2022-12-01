using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaterDamControl : MonoBehaviour
{
    //public bool HADOpen, MeroweOpen, SennarOpen, RoseiresOpen, GERDOpen;
    public float HADFillLevel, MeroweFillLevel, SennarFillLevel, RoseiresFillLevel, GERDFillLevel;
    public float NileFillLevelMax, HADFillLevelMax, MeroweFillLevelMax, SennarFillLevelMax, RoseiresFillLevelMax, GERDFillLevelMax;
    public Button HADButton, MeroweButton, SennarButton, RoseiresButton, GERDButton, DAMRestartButton, StartButton;
    public MeshRenderer HADMesh, MeroweMesh, SennarMesh, RoseiresMesh, GERDMesh;
    public Material WhiteDam, WhiteGreenDam, WhiteRedDam;
    public GameObject NileOceanWater, HADMeroweWater, MeroweSennarWater, SennarRoseiresWater, RoseiresGERDWater, GERDTanaWater;
    public SpriteRenderer HADLock, MeroweLock, SennarLock, RoseiresLock, GERDLock;
    public Sprite DamLock, DamUnlock;
    Vector3 HADMeroweWaterTransOri, MeroweSennarWaterTransOri, SennarRoseiresWaterTransOri, RoseiresGERDWaterTransOri, GERDTanaWaterTransOri;
    public Vector3 NileOceanWaterMin, HADMeroweWaterMin, MeroweSennarWaterMin, SennarRoseiresWaterMin, RoseiresGERDWaterMin, GERDTanaWaterMin;
    public Toggle WeatherWet, WeatherAverage, WeatherDry, GERDOn, GERDOff, HADWaterHigh, HADWaterLow;
    public CascadeSimulationScreens cascadeSimulationScreens;
    public int _weather, _gerd, _had;
    public WaterDamNetworkSync waterDamNetworkSync;
    
    void Start()
    {
        HADMeroweWaterTransOri = HADMeroweWater.transform.position;
        MeroweSennarWaterTransOri = MeroweSennarWater.transform.position;
        SennarRoseiresWaterTransOri = SennarRoseiresWater.transform.position;
        RoseiresGERDWaterTransOri = RoseiresGERDWater.transform.position;
        GERDTanaWaterTransOri = GERDTanaWater.transform.position;
        cascadeSimulationScreens = GetComponent<CascadeSimulationScreens>();
        _weather = _gerd = _had = 0;

        Restart();
        StartCoroutine(FillControl());

        if (waterDamNetworkSync.HADOpen == true) { ChangeHAD(); }
        if (waterDamNetworkSync.MeroweOpen == true) { ChangeMerowe(); }
        if (waterDamNetworkSync.SennarOpen == true) { ChangeSennar(); }
        if (waterDamNetworkSync.RoseiresOpen == true) { ChangeRoseires(); }
        if (waterDamNetworkSync.GERDOpen == true) { ChangeGERD(); }
    }

    private IEnumerator FillControl()
    {
        while (true)
        {
            if (waterDamNetworkSync.HADOpen)
            {
                if (HADFillLevel >= 0.1f)
                {
                    HADFillLevel -= 0.1f;
                    //NileFillLevel += 0.15f;
                }
                //if (HADFillLevel <= 0.11f && NileFillLevel >= 0.1f)
                //{
                //    NileFillLevel -= 0.1f;
                //}
            }
            else
            {
                if (HADFillLevel <= HADFillLevelMax && MeroweFillLevel >= 0.1f && waterDamNetworkSync.MeroweOpen)
                {
                    HADFillLevel += 0.1f;
                }
                //if (NileFillLevel >= 0.1f)
                //{
                //    NileFillLevel -= 0.1f;
                //}
            }

            if (waterDamNetworkSync.MeroweOpen)
            {
                if (MeroweFillLevel >= 0.1f)
                {
                    MeroweFillLevel -= 0.1f;
                    if (HADFillLevel <= HADFillLevelMax)
                    {
                        HADFillLevel += 0.15f;
                    }
                }
                if (waterDamNetworkSync.HADOpen && MeroweFillLevel <= 0.11f && HADFillLevel >= 0.1f)
                {
                    HADFillLevel -= 0.1f;
                }
            }
            else
            {
                if (MeroweFillLevel <= MeroweFillLevelMax && SennarFillLevel >= 0.1f && waterDamNetworkSync.SennarOpen)
                {
                    MeroweFillLevel += 0.1f;
                }
            }

            if (waterDamNetworkSync.SennarOpen)
            {
                if (SennarFillLevel >= 0.1f)
                {
                    SennarFillLevel -= 0.1f;
                    if (MeroweFillLevel <= MeroweFillLevelMax)
                    {
                        MeroweFillLevel += 0.15f;
                    }
                }
                if (waterDamNetworkSync.MeroweOpen && SennarFillLevel <= 0.11f && MeroweFillLevel >= 0.1f)
                {
                    MeroweFillLevel -= 0.1f;
                }
            }
            else
            {
                if (SennarFillLevel <= SennarFillLevelMax && RoseiresFillLevel >= 0.1f && waterDamNetworkSync.RoseiresOpen)
                {
                    SennarFillLevel += 0.1f;
                }
            }

            if (waterDamNetworkSync.RoseiresOpen)
            {
                if (RoseiresFillLevel >= 0.1f)
                {
                    RoseiresFillLevel -= 0.1f;
                    if (SennarFillLevel <= SennarFillLevelMax)
                    {
                        SennarFillLevel += 0.15f;
                    }
                }
                if (waterDamNetworkSync.SennarOpen && RoseiresFillLevel <= 0.11f && SennarFillLevel >= 0.1f)
                {
                    SennarFillLevel -= 0.1f;
                }
            }
            else
            {
                if (RoseiresFillLevel <= RoseiresFillLevelMax && GERDFillLevel >= 0.1f && waterDamNetworkSync.GERDOpen)
                {
                    RoseiresFillLevel += 0.1f;
                }
            }

            if (waterDamNetworkSync.GERDOpen)
            {
                if (GERDFillLevel >= 0.1f)
                {
                    GERDFillLevel -= 0.1f;
                    if (RoseiresFillLevel <= RoseiresFillLevelMax)
                    {
                        RoseiresFillLevel += 0.15f;
                    }
                }
                if (waterDamNetworkSync.RoseiresOpen && GERDFillLevel <= 0.11f && RoseiresFillLevel >= 0.1f)
                {
                    RoseiresFillLevel -= 0.1f;
                }
            }
            else
            {
                if (GERDFillLevel <= GERDFillLevelMax)
                {
                    GERDFillLevel += 0.1f;
                }
            }

            WaterMovement();

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void WaterMovement()
    {
        //
        // start water movement up and down depending on specific dam and fill level
        //

        HADMeroweWater.transform.position = new Vector3(HADMeroweWater.transform.position.x, Mathf.Lerp(HADMeroweWaterMin.y, HADMeroweWaterTransOri.y, (float)Math.Round(HADFillLevel, 2)), HADMeroweWater.transform.position.z);
        MeroweSennarWater.transform.position = new Vector3(MeroweSennarWater.transform.position.x, Mathf.Lerp(MeroweSennarWaterMin.y, MeroweSennarWaterTransOri.y, (float)Math.Round(MeroweFillLevel, 2)), MeroweSennarWater.transform.position.z);
        SennarRoseiresWater.transform.position = new Vector3(SennarRoseiresWater.transform.position.x, Mathf.Lerp(SennarRoseiresWaterMin.y, SennarRoseiresWaterTransOri.y, (float)Math.Round(SennarFillLevel, 2)), SennarRoseiresWater.transform.position.z);
        RoseiresGERDWater.transform.position = new Vector3(RoseiresGERDWater.transform.position.x, Mathf.Lerp(RoseiresGERDWaterMin.y, RoseiresGERDWaterTransOri.y, (float)Math.Round(RoseiresFillLevel, 2)), RoseiresGERDWater.transform.position.z);
        GERDTanaWater.transform.position = new Vector3(GERDTanaWater.transform.position.x, Mathf.Lerp(GERDTanaWaterMin.y, GERDTanaWaterTransOri.y, (float)Math.Round(GERDFillLevel, 2)), GERDTanaWater.transform.position.z);
    }

    public void ToggleChange(Toggle toggle)
    {
        if (toggle == WeatherWet && WeatherWet.isOn)
        {
            waterDamNetworkSync.Weather = 0;
            WeatherAverage.isOn = false;
            WeatherDry.isOn = false;
        }
        else if (toggle == WeatherAverage && WeatherAverage.isOn)
        {
            waterDamNetworkSync.Weather = 1;
            WeatherWet.isOn = false;
            WeatherDry.isOn = false;
        }
        else if (toggle == WeatherDry && WeatherDry.isOn)
        {
            waterDamNetworkSync.Weather = 2;
            WeatherWet.isOn = false;
            WeatherAverage.isOn = false;
        }
        if (toggle == GERDOff && GERDOff.isOn)
        {
            waterDamNetworkSync.GERD = 0;
            GERDOn.isOn = false;
        }
        else if (toggle == GERDOn && GERDOn.isOn)
        {
            waterDamNetworkSync.GERD = 1;
            GERDOff.isOn = false;
        }
        if (toggle == HADWaterHigh && HADWaterHigh.isOn)
        {
            waterDamNetworkSync.HAD = 0;
            HADWaterLow.isOn = false;
        }
        else if (toggle == HADWaterLow && HADWaterLow.isOn)
        {
            waterDamNetworkSync.HAD = 1;
            HADWaterHigh.isOn = false;
        }
    }

    //
    // from here network functions / will be called by WaterDamNetworkSync
    //

    public void Restart() // will be called by WaterDamNetworkSync
    {
        HADFillLevel = MeroweFillLevel = SennarFillLevel = RoseiresFillLevel = GERDFillLevel = 1f;
        HADFillLevelMax = MeroweFillLevelMax = SennarFillLevelMax = RoseiresFillLevelMax = GERDFillLevelMax = 0.9f;
        HADLock.sprite = MeroweLock.sprite = SennarLock.sprite = RoseiresLock.sprite = GERDLock.sprite = DamLock;
        HADMesh.material.color = MeroweMesh.material.color = SennarMesh.material.color = RoseiresMesh.material.color = GERDMesh.material.color = Color.white;
        GERDFillLevelMax = 0.9f;
        GERDMesh.gameObject.SetActive(true);
        HADButton.gameObject.SetActive(true); MeroweButton.gameObject.SetActive(true); SennarButton.gameObject.SetActive(true); RoseiresButton.gameObject.SetActive(true); GERDButton.gameObject.SetActive(true);

        StartCoroutine(LerpingExtensions.MoveTo(HADMeroweWater.transform, HADMeroweWaterTransOri, 0.2f));
        StartCoroutine(LerpingExtensions.MoveTo(MeroweSennarWater.transform, MeroweSennarWaterTransOri, 0.2f));
        StartCoroutine(LerpingExtensions.MoveTo(SennarRoseiresWater.transform, SennarRoseiresWaterTransOri, 0.2f));
        StartCoroutine(LerpingExtensions.MoveTo(RoseiresGERDWater.transform, RoseiresGERDWaterTransOri, 0.2f));
        StartCoroutine(LerpingExtensions.MoveTo(GERDTanaWater.transform, GERDTanaWaterTransOri, 0.2f));

        cascadeSimulationScreens.Restart();
        WeatherWet.interactable = WeatherAverage.interactable = WeatherDry.interactable = GERDOff.interactable = GERDOn.interactable = HADWaterHigh.interactable = HADWaterLow.interactable = StartButton.interactable = true;
        HADButton.gameObject.SetActive(true); MeroweButton.gameObject.SetActive(true); SennarButton.gameObject.SetActive(true); RoseiresButton.gameObject.SetActive(true); GERDButton.gameObject.SetActive(true);
    }

    public void ChangeHAD() // will be called by WaterDamNetworkSync
    {
        if (waterDamNetworkSync.HADOpen)
        {
            HADLock.sprite = DamLock;
            HADMesh.material.color = Color.red;
        }
        else
        {
            HADLock.sprite = DamUnlock;
            HADMesh.material.color = Color.green;
        }
        WaterMovement();
    }

    public void ChangeMerowe() // will be called by WaterDamNetworkSync
    {
        if (waterDamNetworkSync.MeroweOpen)
        {
            MeroweLock.sprite = DamLock;
            MeroweMesh.material.color = Color.red;
        }
        else
        {
            MeroweLock.sprite = DamUnlock;
            MeroweMesh.material.color = Color.green;
        }
        WaterMovement();
    }

    public void ChangeSennar() // will be called by WaterDamNetworkSync
    {
        if (waterDamNetworkSync.SennarOpen)
        {
            SennarLock.sprite = DamLock;
            SennarMesh.material.color = Color.red;
        }
        else
        {
            SennarLock.sprite = DamUnlock;
            SennarMesh.material.color = Color.green;
        }
        WaterMovement();
    }

    public void ChangeRoseires() // will be called by WaterDamNetworkSync
    {
        if (waterDamNetworkSync.RoseiresOpen)
        {
            RoseiresLock.sprite = DamLock;
            RoseiresMesh.material.color = Color.red;
        }
        else
        {
            RoseiresLock.sprite = DamUnlock;
            RoseiresMesh.material.color = Color.green;
        }
        WaterMovement();
    }

    public void ChangeGERD() // will be called by WaterDamNetworkSync
    {
        if (waterDamNetworkSync.GERDOpen)
        {
            GERDLock.sprite = DamLock;
            GERDMesh.material.color = Color.red;
        }
        else
        {
            GERDLock.sprite = DamUnlock;
            GERDMesh.material.color = Color.green;
        }
        WaterMovement();
    }

    public void StartSimulation(int _weather, int _gerd, int _had) // will be called by WaterDamNetworkSync
    {
        Restart();

        WeatherWet.interactable = WeatherAverage.interactable = WeatherDry.interactable = GERDOff.interactable = GERDOn.interactable = HADWaterHigh.interactable = HADWaterLow.interactable = StartButton.interactable = DAMRestartButton.interactable = false;
        HADButton.gameObject.SetActive(false); MeroweButton.gameObject.SetActive(false); SennarButton.gameObject.SetActive(false); RoseiresButton.gameObject.SetActive(false); GERDButton.gameObject.SetActive(false);
        cascadeSimulationScreens.SimulateCascade(_weather, _gerd, _had);
    }
}