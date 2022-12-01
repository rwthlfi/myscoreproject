using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DFTGames.Localization;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LocaMenuButtonActivation : MonoBehaviour
{
    public GameObject english, german, french, spanish, italian, chinese, arabic, russian;

    // Start is called before the first frame update

    private void OnEnable()
    {
        Start();
    }

    void Start()
    {
        CheckSelection();
    }

    public void CheckSelection()
    {
        if (Locale.PlayerLanguage == SystemLanguage.English)
        {
            EventSystem.current.SetSelectedGameObject(english);
        }

        if (Locale.PlayerLanguage == SystemLanguage.German)
        {
            EventSystem.current.SetSelectedGameObject(german);
        }

        if (Locale.PlayerLanguage == SystemLanguage.French)
        {
            EventSystem.current.SetSelectedGameObject(french);
        }

        if (Locale.PlayerLanguage == SystemLanguage.Spanish)
        {
            EventSystem.current.SetSelectedGameObject(spanish);
        }

        if (Locale.PlayerLanguage == SystemLanguage.Italian)
        {
            EventSystem.current.SetSelectedGameObject(italian);
        }

        if (Locale.PlayerLanguage == SystemLanguage.Chinese)
        {
            EventSystem.current.SetSelectedGameObject(chinese);
        }

        if (Locale.PlayerLanguage == SystemLanguage.Arabic)
        {
            EventSystem.current.SetSelectedGameObject(arabic);
        }

        if (Locale.PlayerLanguage == SystemLanguage.Russian)
        {
            EventSystem.current.SetSelectedGameObject(russian);
        }
    }
}