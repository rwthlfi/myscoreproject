using UnityEngine;
using Mirror;

public class RestaurantButtonsSync : NetworkBehaviour
{
    public GameObject buffetHelp, seasonWinter, fridgeHelp;

    [Header("Syncvar Server variable")]
    [SyncVar(hook = nameof(ChangeBuffetHelpOn))]
    public bool buffetHelpOn = false;
    void ChangeBuffetHelpOn(bool _old, bool _new) { }

    [SyncVar(hook = nameof(ChangeSeasonWinter))]
    public bool seasonWinterOn = false;
    void ChangeSeasonWinter(bool _old, bool _new) { }

    [SyncVar(hook = nameof(ChangeFridgeHelpOn))]
    public bool fridgeHelpOn = false;
    void ChangeFridgeHelpOn(bool _old, bool _new) { }


    private void Start()
    {
        CheckBuffetHelp(buffetHelpOn);
        SetCheckSeason(seasonWinterOn);
        CheckFridgeHelp(fridgeHelpOn);
    }

    // Activates the Buffet Help UI

    public void CheckBuffetHelp(bool buffetHelpValue)
    {
        Cmd_CheckBuffetHelp(buffetHelpValue);
    }

    [Command(requiresAuthority = false)]
    private void Cmd_CheckBuffetHelp(bool buffetHelpValue)
    {
        buffetHelpOn = buffetHelpValue;

        Rpc_CheckBuffetHelp(buffetHelpValue);
    }

    [ClientRpc]
    private void Rpc_CheckBuffetHelp(bool buffetHelpValue)
    {
        SetCheckBuffetHelp(buffetHelpValue);
    }

    private void SetCheckBuffetHelp(bool buffetHelpValue)
    {
        if (buffetHelpValue)
        {
            buffetHelp.SetActive(true);
        }
        else
        {
            buffetHelp.SetActive(false);
        }
    }

    // Activates the Fridge Help UI

    public void CheckFridgeHelp(bool fridgeHelpValue)
    {
        Cmd_CheckFridgeHelp(fridgeHelpValue);
    }

    [Command(requiresAuthority = false)]
    private void Cmd_CheckFridgeHelp(bool fridgeHelpValue)
    {
        fridgeHelpOn = fridgeHelpValue;

        Rpc_CheckFridgeHelp(fridgeHelpValue);
    }

    [ClientRpc]
    private void Rpc_CheckFridgeHelp(bool fridgeHelpValue)
    {
        SetCheckFridgeHelp(fridgeHelpValue);
    }

    private void SetCheckFridgeHelp(bool fridgeHelpValue)
    {
        if (fridgeHelpValue)
        {
            fridgeHelp.SetActive(true);
        }
        else
        {
            fridgeHelp.SetActive(false);
        }
    }

    // Changes between spring and winter season

    public void CheckSeason(bool checkSeasonValue)
    {
        Cmd_CheckSeason(checkSeasonValue);
    }

    [Command(requiresAuthority = false)]
    private void Cmd_CheckSeason(bool checkSeasonValue)
    {
        seasonWinterOn = checkSeasonValue;

        Rpc_CheckSeason(checkSeasonValue);
    }

    [ClientRpc]
    private void Rpc_CheckSeason(bool checkSeasonValue)
    {
        SetCheckSeason(checkSeasonValue);
    }

    private void SetCheckSeason(bool checkSeasonValue)
    {
        if (checkSeasonValue)
        {
            seasonWinter.SetActive(true);
        }
        else
        {
            seasonWinter.SetActive(false);
        }
    }
}
