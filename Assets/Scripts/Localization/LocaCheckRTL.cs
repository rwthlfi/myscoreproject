using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using DFTGames.Localization;

public class LocaCheckRTL : MonoBehaviour
{
    // This script checks the current set language from the menu (Toogle Menu with script "LanguageSelectionMenuUIToggle") and converts
    // the textmesh pro text objects and simple UI text objects alignment from LTR to RTL when the language setting demands it

    public TMP_FontAsset _fontAssetArabic;
    public TMP_FontAsset _fontAssetChinese;
    private TMP_FontAsset _fontAssetInitial;
    public Font fontInitial;
    public Font fontArabic;
    public Font fontChinese;
    private Vector3 _TransformLocaleRTLInitial;
    private Vector3 _TransformLocaleRTL;
    private bool _enableRTL;

    private void OnEnable()
    {
        Start();
    }

    private void Start()
    {
        CheckRTLState();
        _enableRTL = true;
    }

    private void GetInitialValues()
    {
        //get initial scale to preserve the LTR configuration
        _TransformLocaleRTLInitial = GetComponent<RectTransform>().localScale;
        if (Mathf.Sign(_TransformLocaleRTLInitial.x) == -1)
            _TransformLocaleRTLInitial.x = _TransformLocaleRTLInitial.x * -1;

        _TransformLocaleRTL = GetComponent<RectTransform>().localScale;
        _TransformLocaleRTL = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        //Check if text components of type text mesh or default unity text are avilable and get alignment as well as initial font asset(tmp), prevent null reference exception and missmatch of font
        if (GetComponent<TextMeshProUGUI>() != null)
        {
            _fontAssetInitial = GetComponent<TextMeshProUGUI>().font;
        }
    }

    public void CheckRTLState()
    {
        GetInitialValues();

        // change settings for RTL
        if (Locale.PlayerLanguage == SystemLanguage.Arabic)
        {
            if (_enableRTL)
            {
                _enableRTL = false;
                GetComponent<RectTransform>().localScale = _TransformLocaleRTL;
            }

            if (GetComponent<TextMeshProUGUI>() != null)
            {
                GetComponent<TextMeshProUGUI>().font = _fontAssetArabic;
            }
            if (GetComponent<Text>() != null)
                GetComponent<Text>().font = fontArabic;
        }
        else if (Locale.PlayerLanguage == SystemLanguage.Chinese)
        {
            _enableRTL = true;
            GetComponent<RectTransform>().localScale = _TransformLocaleRTLInitial;

            if (GetComponent<TextMeshProUGUI>() != null)
            {
                GetComponent<TextMeshProUGUI>().font = _fontAssetChinese;
            }
            if (GetComponent<Text>() != null)
                GetComponent<Text>().font = fontChinese;
        }
        else
        {
            // Set everything to the state before RTL
            _enableRTL = true;
            GetComponent<RectTransform>().localScale = _TransformLocaleRTLInitial;

            if (GetComponent<TextMeshProUGUI>() != null)
            {
                GetComponent<TextMeshProUGUI>().font = _fontAssetInitial;
            }
            if (GetComponent<Text>() != null)
                GetComponent<Text>().font = fontInitial;
        }
    }
}