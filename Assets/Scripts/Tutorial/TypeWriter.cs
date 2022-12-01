using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriter : MonoBehaviour
{
    private float _delay = 0.1f;
    private string _fullText;
    private string _currentText = "";
    int textlenght;

    private void Start()
    {
        _fullText = GetComponent<Text>().text;
        textlenght = 0;

        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        for (textlenght = 0; textlenght < _fullText.Length; textlenght++)
        {
            _currentText = _fullText.Substring(0, textlenght);
            this.GetComponent<Text>().text = _currentText;
            yield return new WaitForSeconds(_delay);
        }
    }
}
