using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
//using Dissonance;
using AvatarCreation;

namespace AvatarTablet
{
    public class MicrophoneWindow : MonoBehaviour
    {
        [Header("Microphone UI")]
        public Toggle speakingToggle;
        public TextMeshProUGUI microphoneCurrentName;
        public Transform microphoneContent;
        //private DissonanceComms dc;

        [Header("Player Variable")]
        public NetworkPlayerSyncVar networkPlayerSyncVar;

        [Header("Prefab Reference")]
        public SimpleObjectPool microphone_Prefab;



        void Start()
        {
            //find the DissonanceComms in the scene
            //dc = FindObjectOfType<DissonanceComms>();
            //load the mic name
            string mic = PlayerPrefs.GetString(PrefDataList.savedMicrophone);
            if (mic != "")
                microphoneCurrentName.text = mic;

            InitMicrophoneList();
        }


        /// <summary>
        /// This is the toggle to Mute or unmute yourself.
        /// </summary>
        public void Ui_SpeakingToggle()
        {
            bool allowSpeaking = speakingToggle.isOn;
            bool alreadyMute = networkPlayerSyncVar.attributeChanged.Contains(GlobalSettings.att_Mute);
            /*
            //decide if you wanna speak or not speaking.
            if (dc != null)
                dc.IsMuted = !allowSpeaking;
            */
            if (allowSpeaking && alreadyMute) // allow speaking 
            {
                //remove the mute attribute
                string currenAtt = networkPlayerSyncVar.attributeChanged;
                string convert = currenAtt.Replace(GlobalSettings.att_Mute, "");
                networkPlayerSyncVar.CmdChangeAttribute(convert);
            }

            // Muting
            else if (!allowSpeaking && !alreadyMute)
            {
                networkPlayerSyncVar.CmdChangeAttribute(networkPlayerSyncVar.attributeChanged + GlobalSettings.att_Mute);
            }
        }

        /// <summary>
        /// initialize the microphone list.
        /// </summary>
        private void InitMicrophoneList()
        {
            //destroy all the child first
            foreach (Transform child in microphoneContent)
                Destroy(child.gameObject);

            //get and store all microphone available in the Sytem
            List<string> microphoneList = Microphone.devices.ToList();

            //create microphone button foreach of the microphone devices available in the list.
            foreach (string str in microphoneList)
            {
                GameObject microphone = CreateMicrophoneButton(str);
                //set it to the content
                microphone.transform.SetParent(microphoneContent, false);

            }
        }

        //create the microphone button gameObject
        public GameObject CreateMicrophoneButton(string _microphoneName)
        {
            //get the prefab
            GameObject microphoneButton = microphone_Prefab.GetObject();

            //assign the name to the microphonebutton
            microphoneButton.GetComponentInChildren<TextMeshProUGUI>().text = _microphoneName;

            //add the Microphone button capabilites
            microphoneButton.GetComponentInChildren<Button>().onClick.AddListener(delegate
            {
                //put the function
                //dc.MicrophoneName = _microphoneName;
                //change the name in the UI
                microphoneCurrentName.text = _microphoneName;
                //save the name in the player prefs
                PlayerPrefs.SetString(PrefDataList.savedMicrophone, _microphoneName);
            });

            return microphoneButton;
        }

        public void MutePlayer()
        {
            speakingToggle.isOn = false;
            Ui_SpeakingToggle();
        }

        public void UnMutePlayer()
        {
            speakingToggle.isOn = true;
            Ui_SpeakingToggle();
        }
    }

}
