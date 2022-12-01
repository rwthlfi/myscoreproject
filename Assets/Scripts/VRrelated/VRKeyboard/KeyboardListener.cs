using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
//using Vuplex.WebView;
using System;

public class KeyboardListener : MonoBehaviour
{
    [Header("UI Variable")]
    public TMP_InputField previewText;

    [Header("VuplexVariable")]
    //public CanvasWebViewPrefab _webViewPrefab;

    [Header("Keyboard Variable")]
    //get the reference to the keyboard listener
    private KeyboardVRInit keyboardVRInit;
    public VRKeys.Keyboard keyboard;
    public Transform moveKeyboard;
    //public Vector3 snapPositionOffset; // to snap the keyboard so that when the user click an inputfield, it will show up in front of the user.


    private void Start()
    {
        keyboard = GetComponent<VRKeys.Keyboard>();
        //moveKeyboard = this.transform.parent.GetChild(0);
        keyboardVRInit = GetComponent<KeyboardVRInit>();
    }

    /// <summary>
    /// To Assign the keyboard, in order the input field to be able to be field
    /// </summary>
    
    public void InputField_isClicked()
    {
        //show the Keyboard
        keyboardVRInit.Ui_ShowKeyboard();

        //Register the Input Field to the VrKeyboard
        keyboard.displayText = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();
        MoveKeyboard();
        previewText.text = keyboard.displayText.text;
    }


    //to use with the input Field in the Browser.
    public void VuplexListener(string _str)
    {
        /*
        string value = _str;
        if (!_webViewPrefab)
            return;

        //if web view is Exist an the webview is active 
        if (_webViewPrefab.gameObject.activeInHierarchy)
        {
            if (_str == "Clear")
                _webViewPrefab.WebView.SelectAll();

            _webViewPrefab.WebView.HandleKeyboardInput(value);
            //Debug.Log("asdf " + value);
        }
        */
        //Debug.Log("String" + _str);
    }

    private void MoveKeyboard()
    {
        //if the keyboard has the "snap" component turn off, then dont do anything.
        if (!keyboardVRInit.snap)
            return;

        //move the keyboard a bit in front of the User eyes

        if (keyboardVRInit.vrPlatformDetector.useSteamVR)
        {
            moveKeyboard.transform.position = keyboardVRInit.refSteam.position;
            moveKeyboard.transform.rotation = keyboardVRInit.refSteam.rotation;
        }
        else
        {
            moveKeyboard.transform.position = keyboardVRInit.refOculus.position;
            moveKeyboard.transform.rotation = keyboardVRInit.refOculus.rotation;

        }

    }
}
