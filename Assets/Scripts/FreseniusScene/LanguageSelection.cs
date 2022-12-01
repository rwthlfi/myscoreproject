using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DFTGames.Localization;


namespace Fresenius
{

    public class LanguageSelection : MonoBehaviour
    {
        public void SetEnglish()
        {
            Localize.SetCurrentLanguage(SystemLanguage.English);
            LocalizeImage.SetCurrentLanguage();
            //LocalizeAudio.SetCurrentLanguage();  // audio loca needs to be added later
        }

        public void SetGerman()
        {
            Localize.SetCurrentLanguage(SystemLanguage.German);
            LocalizeImage.SetCurrentLanguage();
            //LocalizeAudio.SetCurrentLanguage();  // audio loca needs to be added later
        }
    }
}
