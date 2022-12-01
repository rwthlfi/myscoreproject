using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//A typeWritter effect
public class TypeWritterEffect : MonoBehaviour
{
    private float delay = 0.05f;
    private string currentText = "";

    public TextMeshProUGUI _test;

    public bool typing_isFinish
    {
        get;
        set;
    }


    private void Start()
    {
    }

    /// <summary>
    /// to create an effect that writes the Text from the script
    /// </summary>
    /// <param name="_textMesh"></param>
    /// <param name="_fullText"></param>
    /// <returns></returns>
    public IEnumerator ShowText(TextMeshProUGUI _textMesh, string _fullText)
    {
        typing_isFinish = false;

        currentText = "";
        foreach (char c in _fullText)
        {
            currentText += c;
            _textMesh.text = currentText;
            yield return new WaitForSeconds(delay);
        }
        typing_isFinish = true;
    }

    /// <summary>
    /// to create the effect then the Text is already inside the TextMeshProUGUI. Usefull for localization
    /// </summary>
    /// <param name="_textMesh"></param>
    /// <returns></returns>
    public IEnumerator DisplayText(TextMeshProUGUI _textMesh)
    {
        //set the typing to false;
        typing_isFinish = false;

        //get the text from the TextMeshProUGUI
        string str = _textMesh.text;


        _textMesh.text = "";

        foreach (char c in str)
        {
            _textMesh.text += c;
            yield return new WaitForSeconds(delay);
        }


        yield return new WaitForSeconds(1f);
        typing_isFinish = true;
    }

    
}