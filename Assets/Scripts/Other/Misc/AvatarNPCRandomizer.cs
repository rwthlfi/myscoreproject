using UnityEngine;
using Mirror;
using AvatarCreation;

public class AvatarNPCRandomizer : NetworkBehaviour
{
    public Ui_AvatarCustomeSelection ui_AvatarCustomeSelection;
    public AvatarSetting_Renderer avatarSetting_Renderer;

    [SyncVar(hook = nameof(ChangeNPCString))]
    public string npcString = string.Empty;
    void ChangeNPCString(string _old, string _new) { }

    void Start()
    {
        if (isServer)
        {
            RandomizeCustome();
        }
        Cmd_ChangeNPCLook(npcString);
    }
    private void Update()
    {
        avatarSetting_Renderer.Load_SavedCustome(npcString);
    }

    public void RandomizeCustome()
    {
        string strType = Type.customize + "|";
        string strArms = 0.ToString() + "|";
        string strEyes = Random.Range(0, ui_AvatarCustomeSelection.Eyes.meshs.Count).ToString() + "|";
        string strGlasses = Random.Range(0, ui_AvatarCustomeSelection.Glass.meshs.Count).ToString() + "|";
        string strHair = Random.Range(0, ui_AvatarCustomeSelection.Hair.meshs.Count).ToString() + "|";
        string strHands = 0.ToString() + "|";
        string strHead = Random.Range(0, ui_AvatarCustomeSelection.Human.meshs.Count).ToString() + "|";
        string strRig = 0 + "|";
        string strPants = Random.Range(0, ui_AvatarCustomeSelection.Pants.meshs.Count).ToString() + "|";
        string strClothes = Random.Range(0, ui_AvatarCustomeSelection.Clothes.meshs.Count).ToString() + "|";
        string strShoes = Random.Range(0, ui_AvatarCustomeSelection.Shoes.meshs.Count).ToString() + "|";

        npcString = strType + strArms + strEyes + strGlasses + strHair + strHands + strHead + strRig + strPants + strClothes + strShoes;
    }

    [Command(requiresAuthority = false)]
    private void Cmd_ChangeNPCLook(string npcString)
    {
        Rpc_ChangeNPCLook(npcString);
    }

    [ClientRpc]
    private void Rpc_ChangeNPCLook(string npcString)
    {
        avatarSetting_Renderer.Load_SavedCustome(npcString);
    }
}
