using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
//using Vuplex.WebView;
using TMPro;
/*
public class WebViewKeyboard : MonoBehaviour
{
    [Header("WebView Variable")]
    public Transform parentObject;
    //public CanvasWebViewPrefab _webViewPrefab;
    public TMP_InputField urlInputField;
    public TextMeshProUGUI progress;
    
    
    [Header("keyboard instantiated variable")]
    private Vector3 localScale = new Vector3(2000f, 2000f, 1f);
    public Vector3 localPos = new Vector3(0f, -265f, 0f);
    private Vector3 localEangles = new Vector3(0, 180, 0);
    Keyboard _keyboard;

    [Header("InputField Variable")]
    public TMP_InputField _inputField;

    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.LinuxPlayer)
            return;

        // Add the keyboard under the main webview.
        _keyboard = Keyboard.Instantiate();
        _keyboard.transform.parent = parentObject;
        _keyboard.transform.localScale = localScale;
        _keyboard.transform.localPosition = localPos;
        _keyboard.transform.localEulerAngles = localEangles;
        // Hook up the keyboard so that characters are routed to the main webview.
        _keyboard.InputReceived += (sender, e) => _webViewPrefab.WebView.HandleKeyboardInput(e.Value);
        _keyboard.InputReceived += Testing;
        //_keyboard.InputReceived += (sender, e) => InputField_isEntered(e.Value);

    }

    void Update()
    {
        //yes, I know it checks about linux., and its weird. but 'till now hasn't find a way to disabled it on the server.
        //if the webview is not disabled, then the sever will be flooded with a bunch of "null references" error message.
        if (Application.platform == RuntimePlatform.LinuxPlayer)
            return;
        
        transform.position = Camera.main.transform.position + new Vector3(0, 0.2f, 0);
        //_webViewPrefab.WebView.LoadProgressChanged += WebView_LoadProgressChanged;

    }

    private void Testing (object sender, EventArgs<string> e)
    {
        if (_inputField == null)
            return;

        //get the caret position.
        int currentCaretPos = _inputField.caretPosition;

        switch (e.Value)
        {
            case "Backspace":
                _inputField.text = _inputField.text.Remove(_inputField.text.Length - 1); return;
            case "Enter":
                _inputField.text += "\n"; return;
            case "ArrowLeft":
                _inputField.caretPosition = currentCaretPos - 1; return;
            case "ArrowRight":
                _inputField.caretPosition = currentCaretPos + 1; return;
            case "ArrowUp":
                _inputField.text += ""; return;
            case "ArrowDown":
                _inputField.text += ""; return;
        }

        _inputField.text += e.Value;
    }

    private void WebView_LoadProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        float value = e.Progress;
        progress.text = (value * 100).ToString() + " %";
    }

    public void InputField_isClicked()
    {
        //Register the Input Field.
        _inputField = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();
    }



    //----------- WebView Menu Function--------//

    public void ButtonBack_isClicked()
    {
        _webViewPrefab.WebView.GoBack();
        urlInputField.text = _webViewPrefab.WebView.Url;
    }

    public void ButtonForward_isClicked()
    {
        _webViewPrefab.WebView.GoForward();
        urlInputField.text = _webViewPrefab.WebView.Url;
    }
    public void ButtonRefresh_isClicked()
    {
        _webViewPrefab.WebView.Reload();
        urlInputField.text = _webViewPrefab.WebView.Url;
    }

    public void ButtonEnter_isClicked()
    {
        _webViewPrefab.WebView.LoadUrl(urlInputField.text);
    }

}
*/