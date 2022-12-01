using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;
using AvatarTablet;

namespace AvatarCreation
{
    //This is the container for the sync variable. 
    //like the name, height, avatars, admin speaking etc.

    public class NetworkPlayerSyncVar : NetworkBehaviour
    {
        [Header("Player Related")]
        public Transform referenceAvatar;
        public SphereCollider speakingCollider;

        [Header("UI references")]
        public GameObject statusCanvas;
        public TextMeshProUGUI playerNameText;
        public Image nameBg;
        public Image muteIcon;
        public Image streamerIcon;

        [Header("Script Reference")]
        public MainControlTablet mainControlTablet;
        public AvatarSettings avatarSettings;


        //The player properties(like; name, height, avatars)
        [Header("SyncVar - Properties")]
        [SyncVar(hook = nameof(OnNameChanged))]
        public string playerName;
        [SyncVar(hook = nameof(OnUniqueIDChanged))]
        public string playerUniqueID;
        [SyncVar(hook = nameof(OnHeightChanged))]
        public float playerHeight;
        [SyncVar(hook = nameof(OnCustomeChanged))]
        public string playerCustome;
        [SyncVar(hook = nameof(OnBlendShapeChanged))]
        public string playerBlendShape;

        [Header("SyncVar - Status")]
        [SyncVar(hook = nameof(OnAdminStatusChanged))]
        public int adminStatus;
        [SyncVar(hook = nameof(OnAttributeChanged))]
        public string attributeChanged;
        [SyncVar(hook = nameof(OnSpeakRadiusChanged))]
        public float speakRadius;
        private float speakRadCache = 10;
        public Slider speakSlider;


        //Change Name
        void OnNameChanged(string _Old, string _New)
        {
            playerNameText.text = _New;
        }

        //Change UniqueID
        void OnUniqueIDChanged(string _Old, string _New)
        {
            playerUniqueID = _New;
        }

        //Change Height
        void OnHeightChanged(float _Old, float _New)
        {
            referenceAvatar.transform.localScale = new Vector3(_New, _New, _New);
        }

        //Change Custome
        void OnCustomeChanged(string _Old, string _New)
        {
            //change Custome
            print("Custome " + _New);
            avatarSettings.avatarSetting_Renderer.Load_SavedCustome(_New);
        }

        //Change Blendshapes
        void OnBlendShapeChanged(string _Old, string _New)
        {
            //change Custome
            print("Blend " + _New);
            avatarSettings.avatarSetting_Renderer.LoadFace(_New);
        }


        //Change Admin Status
        void OnAdminStatusChanged(int _Old, int _New)
        {
            //change AdminStatus background
            if (adminStatus == (int)GlobalSettings.PlayerRole.normal)
                nameBg.color = new Color32(0, 42, 132, 255);
            else if (adminStatus == (int)GlobalSettings.PlayerRole.admin)
                nameBg.color = new Color32(87, 171, 39, 255);
        }


        //change player Attribute, for example being incognito, death, etc.
        void OnAttributeChanged(string _Old, string _New)
        {
            statusCanvas.SetActive(!attributeChanged.Contains(GlobalSettings.att_Incognito));
            muteIcon.gameObject.SetActive(_New.Contains(GlobalSettings.att_Mute));
            streamerIcon.gameObject.SetActive(_New.Contains(GlobalSettings.att_Streamer));

        }

        //change speaking radius
        void OnSpeakRadiusChanged(float _Old, float _New)
        {
            //change SpeakRadius
            speakingCollider.radius = _New;
            
        }




        public override void OnStartLocalPlayer()
        {
            CmdSetPlayerName(PlayerPrefs.GetString(PrefDataList.savedUsername));
            CmdSetPlayerUniqueID(PlayerPrefs.GetString(PrefDataList.savedCreatorID));
            CmdSetHeight(PlayerPrefs.GetFloat(PrefDataList.avatarHeight));
            CmdSetCustome(PlayerPrefs.GetString(PrefDataList.savedPreviousCustome));
            CmdSetBlendShape(PlayerPrefs.GetString(PrefDataList.avatarBlendshapes));
            CmdSpeakRadius(GlobalSettings.speakRadiusDefault);
        }




        //player info sent to server, then server updates sync vars which handles it on all clients
        [Command]
        public void CmdSetPlayerName(string _name) { playerName = _name; }
        
        [Command]
        public void CmdSetPlayerUniqueID(string _uniqueID) { playerUniqueID = _uniqueID; }

        public void Ui_setHeight(Slider _slider)
        {
            CmdSetHeight(_slider.value);
        }

        [Command]
        public void CmdSetHeight(float _height) { playerHeight = _height; }

        [Command]
        public void CmdSetCustome(string _custome) { playerCustome = _custome; }
        
        [Command]
        public void CmdSetBlendShape(string _blendShapes) { playerBlendShape = _blendShapes; }




        //player status to be sent to the server
        [Command]
        public void CmdAdminStatus(int _admin) { adminStatus = _admin; }


        [Command]
        public void CmdChangeAttribute(string _attributeChanged) { attributeChanged = _attributeChanged; }


        public void Ui_setSpeakingRadius(Slider _slider)
        {
            CmdSpeakRadius(_slider.value);
        }

        public void Ui_setGlobalSpeaking(Toggle _toggle)
        {
            print("Global speaking");
            speakSlider.interactable = !_toggle.isOn; // the opposite of the toggle
            if (_toggle.isOn)
                CmdSpeakRadius(100);
            else
                CmdSpeakRadius(speakRadCache);
        }

        public void Ui_setSpeakingEveryone(float _rad)
        {
            CmdSpeakRadius(_rad);
        }


        [Command]
        public void CmdSpeakRadius(float _speakRadius) { speakRadius = _speakRadius; }
    

    }

}
