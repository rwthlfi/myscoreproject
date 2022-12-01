using UnityEngine;
using Mirror;


public class DesalinationNetworkSync : NetworkBehaviour
{
    public SliderValueToText sliderValueToTextProduction;
    public SliderValueToText sliderValueToTextShare;

    [Header("Syncvar Server variable")]
    [SyncVar(hook = nameof(ChangeSliderValueProductionFloat))]
    public float sliderValueProduction = 60;

    [SyncVar(hook = nameof(ChangeSliderValueShareFloat))]
    public float sliderValueShare = 50;

    void ChangeSliderValueProductionFloat(float _old, float _new) { }
    void ChangeSliderValueShareFloat(float _old, float _new) { }

    public void ShowSliderValueProduction(float sliderValue)
    {
        Cmd_ChangeSliderValueProduction(sliderValue);
    }

    [Command(requiresAuthority = false)]
    private void Cmd_ChangeSliderValueProduction(float sliderValue)
    {
        sliderValueProduction = sliderValue;

        Rpc_ChangeSliderValueProduction(sliderValue);
    }

    [ClientRpc]
    private void Rpc_ChangeSliderValueProduction(float sliderValue)
    {
        sliderValueToTextProduction.ChangeSliderValue(sliderValue);
    }


    public void ShowSliderValueShare(float sliderValue)
    {
        Cmd_ChangeSliderValueShare(sliderValue);
    }

    [Command(requiresAuthority = false)]
    private void Cmd_ChangeSliderValueShare(float sliderValue)
    {
        sliderValueShare = sliderValue;

        Rpc_ChangeSliderValueShare(sliderValue);
    }

    [ClientRpc]
    private void Rpc_ChangeSliderValueShare(float sliderValue)
    {
        sliderValueToTextShare.ChangeSliderValue(sliderValue);
    }
}