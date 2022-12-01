using UnityEngine;

/// <summary>
/// Class managing UI art localization. Language specific art shall be saved following this folder structure:
///
///     Resources/localization/UI/English 
///     Resources/localization/UI/Italian 
///     Resources/localization/UI/Japanese
///
/// ... and so on, where the folder name is the string version of the SystemLanguage enumeration.
/// </summary>
/// 

namespace DFTGames.Localization
{
    public class LocalizeAudio : MonoBehaviour
    {
        #region Public Fields

        public string localizationKey_AudioClipName;

        #endregion Public Fields

        #region Private Fields

        private const string STR_LOCALIZATION_PREFIX = "Localization/Audio/";

        private AudioClip audioClip;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// This is to set the Sprite according to the selected language by code. It update all the Image
        /// components in the scene. This MUST be called AFTER the language has been set in Localize class.
        /// </summary>
        public static void SetCurrentLanguage()
        {
            LocalizeAudio[] allTexts = GameObject.FindObjectsOfType<LocalizeAudio>();
            for (int i = 0; i < allTexts.Length; i++)
                allTexts[i].UpdateLocale();
        }

        /// <summary>
        /// Update the Sprite of the Image we are attached to. It has a 100ms delay to allow Start operations.
        /// </summary>
        public void UpdateLocale()
        {
            if (!audioClip) return; // catching race condition
            Invoke("_updateLocale", 0.1f);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Update the Sprite of the Image we are attached to. If language has not been set yet try again
        /// in 100ms.
        /// </summary>
        private void _updateLocale()
        {
            if (Locale.CurrentLanguageHasBeenSet)
            {
                Debug.Log(Resources.Load(STR_LOCALIZATION_PREFIX + Locale.PlayerLanguage.ToString() + "/" + localizationKey_AudioClipName, typeof(AudioClip)) as AudioClip);
                
                AudioClip tmp = Resources.Load(STR_LOCALIZATION_PREFIX + Locale.PlayerLanguage.ToString() + "/" + localizationKey_AudioClipName, typeof(AudioClip)) as AudioClip;
                if (tmp != null)
                    audioClip = tmp;
                return;
            }
            UpdateLocale();
        }

        private void Start()
        {
            _updateLocale();
            GetComponent<AudioSource>().clip = audioClip;
            UpdateLocale();
        }

        private void OnEnable()
        {
            Start();
        }
        #endregion Private Methods
    }
}