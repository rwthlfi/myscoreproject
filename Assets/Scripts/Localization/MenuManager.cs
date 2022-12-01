using UnityEngine;
using DFTGames.Localization;

public class MenuManager : MonoBehaviour
{
    #region Public Methods

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

    public void SetFrench()
    {
        Localize.SetCurrentLanguage(SystemLanguage.French);
        LocalizeImage.SetCurrentLanguage();
        //LocalizeAudio.SetCurrentLanguage();  // audio loca needs to be added later
    }

    public void SetSpanish()
    {
        Localize.SetCurrentLanguage(SystemLanguage.Spanish);
        LocalizeImage.SetCurrentLanguage();
        //LocalizeAudio.SetCurrentLanguage();  // audio loca needs to be added later
    }

    public void SetItalian()
    {
        Localize.SetCurrentLanguage(SystemLanguage.Italian);
        LocalizeImage.SetCurrentLanguage();
        //LocalizeAudio.SetCurrentLanguage();  // audio loca needs to be added later
    }

    public void SetChinese()
    {
        Localize.SetCurrentLanguage(SystemLanguage.Chinese);
        LocalizeImage.SetCurrentLanguage();
        //LocalizeAudio.SetCurrentLanguage();  // audio loca needs to be added later
    }

    public void SetArabic()
    {
        Localize.SetCurrentLanguage(SystemLanguage.Arabic);
        LocalizeImage.SetCurrentLanguage();
        //LocalizeAudio.SetCurrentLanguage();  // audio loca needs to be added later
    }

    public void SetRussian()
    {
        Localize.SetCurrentLanguage(SystemLanguage.Russian);
        LocalizeImage.SetCurrentLanguage();
        //LocalizeAudio.SetCurrentLanguage();  // audio loca needs to be added later
    }
    #endregion Public Methods
}