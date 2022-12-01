using UnityEngine;
using Mirror;

public class WaterDamNetworkSync : NetworkBehaviour
{
    public WaterDamControl waterDamControl;

    //Server variable
    [Header("Syncvar Server variable")]
    [SyncVar(hook = nameof(ChangeHADBool))] // we are coupling the function to the variable
    public bool HADOpen = false;

    [SyncVar(hook = nameof(ChangeMeroweBool))]
    public bool MeroweOpen = false;

    [SyncVar(hook = nameof(ChangeSennarBool))]
    public bool SennarOpen = false;

    [SyncVar(hook = nameof(ChangeRoseiresBool))]
    public bool RoseiresOpen = false;

    [SyncVar(hook = nameof(ChangeGERDBool))]
    public bool GERDOpen = false;

    [SyncVar(hook = nameof(ChangeWeatherInt))]
    public int Weather = 0;

    [SyncVar(hook = nameof(ChangeGERDInt))]
    public int GERD = 0;

    [SyncVar(hook = nameof(ChangeHADInt))]
    public int HAD = 0;

    void ChangeHADBool(bool _old, bool _new) { }

    void ChangeMeroweBool(bool _old, bool _new) { }

    void ChangeSennarBool(bool _old, bool _new) { }

    void ChangeRoseiresBool(bool _old, bool _new) { }

    void ChangeGERDBool(bool _old, bool _new) { }
    
    void ChangeWeatherInt(int _old, int _new) { }

    void ChangeGERDInt(int _old, int _new) { }

    void ChangeHADInt(int _old, int _new) { }

    //
    // reset button of WaterDamControl

    public void ButtonWaterDamControlRestart()
    {
        Cmd_WaterDamControlRestart();
    }

    [Command(requiresAuthority = false)]
    private void Cmd_WaterDamControlRestart()
    {
        HADOpen = MeroweOpen = SennarOpen = RoseiresOpen = GERDOpen = false;
        Rpc_WaterDamControlRestart();
    }

    [ClientRpc]
    private void Rpc_WaterDamControlRestart()
    {
        HADOpen = MeroweOpen = SennarOpen = RoseiresOpen = GERDOpen = false;
        waterDamControl.Restart();
    }

    //
    // close / open had dam
    public void ButtonChangeHAD()
    {
        if (HADOpen)
        {
            Cmd_ChangeHAD(false);
        }
        else
            Cmd_ChangeHAD(true);
    }

    [Command(requiresAuthority = false)]
    private void Cmd_ChangeHAD(bool _value)
    {
        HADOpen = _value;
        Rpc_ChangeHAD(_value);
    }
    [ClientRpc]
    private void Rpc_ChangeHAD(bool _value)
    {
        waterDamControl.ChangeHAD();
    }

    //
    // close / open Merowe dam
    public void ButtonChangeMerowe()
    {
        if (MeroweOpen)
        {
            Cmd_ChangeMerowe(false);
        }
        else
            Cmd_ChangeMerowe(true);
    }

    [Command(requiresAuthority = false)]
    private void Cmd_ChangeMerowe(bool _value)
    {
        MeroweOpen = _value;
        Rpc_ChangeMerowe(_value);
    }
    [ClientRpc]
    private void Rpc_ChangeMerowe(bool _value)
    {
        waterDamControl.ChangeMerowe();
    }

    //
    // close / open Sennar dam
    public void ButtonChangeSennar()
    {
        if (SennarOpen)
        {
            Cmd_ChangeSennar(false);
        }
        else
            Cmd_ChangeSennar(true);
    }

    [Command(requiresAuthority = false)]
    private void Cmd_ChangeSennar(bool _value)
    {
        SennarOpen = _value;
        Rpc_ChangeSennar(_value);
    }
    [ClientRpc]
    private void Rpc_ChangeSennar(bool _value)
    {
        waterDamControl.ChangeSennar();
    }

    //
    // close / open Roseires dam
    public void ButtonChangeRoseires()
    {
        if (RoseiresOpen)
        {
            Cmd_ChangeRoseires(false);
        }
        else
            Cmd_ChangeRoseires(true);
    }

    [Command(requiresAuthority = false)]
    private void Cmd_ChangeRoseires(bool _value)
    {
        RoseiresOpen = _value;
        Rpc_ChangeRoseires(_value);
    }
    [ClientRpc]
    private void Rpc_ChangeRoseires(bool _value)
    {
        waterDamControl.ChangeRoseires();
    }

    //
    // close / open GERD dam
    public void ButtonChangeGERD()
    {
        if (GERDOpen)
        {
            Cmd_ChangeGERD(false);
        }
        else
            Cmd_ChangeGERD(true);
    }

    [Command(requiresAuthority = false)]
    private void Cmd_ChangeGERD(bool _value)
    {
        GERDOpen = _value;
        Rpc_ChangeGERD(_value);
    }
    [ClientRpc]
    private void Rpc_ChangeGERD(bool _value)
    {
        waterDamControl.ChangeGERD();
    }

    //
    // Button Start Simulation
    public void ButtonStartSimulation()
    {
        Cmd_StartSimulation(Weather, GERD, HAD);
    }

    [Command(requiresAuthority = false)]
    private void Cmd_StartSimulation(int _weather, int _gerd, int _had)
    {
        Rpc_StartSimulation(_weather, _gerd, _had);
    }
    [ClientRpc]
    private void Rpc_StartSimulation(int _weather, int _gerd, int _had)
    {
        waterDamControl.StartSimulation(_weather, _gerd, _had);
    }
}