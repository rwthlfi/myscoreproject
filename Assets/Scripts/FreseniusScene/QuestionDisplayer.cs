using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Fresenius
{
    public class QuestionDisplayer : MonoBehaviour
    {
        [Header("Text mesh display")]
        public Transform mainDisplay;
        public TextMeshProUGUI textDisplay;

        public string currentText = "";
        public string fullText = "";
        public bool finishType = false;

        // Start is called before the first frame update
        void Start()
        {
            //StartCoroutine(DisplayText("Something2asfasfdasfsafsafd ", 0.1f));
        }


        void Update()
        {

        }

        public void TurnOnDisplay(bool _value)
        {
            mainDisplay.gameObject.SetActive(_value);
        }


        /// <summary>
        /// display the text with callback
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_speed"></param>
        /// <param name="_callback"></param>
        /// <returns></returns>
        public IEnumerator DisplayText(string _text, float _speed, System.Action<bool> _callback)
        {
            //reset the Text first
            textDisplay.text = "";
            TurnOnDisplay(true);


            //start typing one by one.
            foreach (char c in _text)
            {
                textDisplay.text += c;
                yield return new WaitForSeconds(_speed);
            }


            yield return new WaitForSeconds(3f);


            TurnOnDisplay(false);
            _callback (true);
        }


        /// <summary>
        /// TO display the text without turning off the display
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_speed"></param>
        /// <returns></returns>
        public IEnumerator DisplayText_NoAutoTurnOff(string _text, float _speed, System.Action<bool> _callback)
        {
            //reset the Text first
            textDisplay.text = "";
            TurnOnDisplay(true);


            //start typing one by one.
            foreach (char c in _text)
            {
                textDisplay.text += c;
                yield return new WaitForSeconds(_speed);
            }

            //TurnOnDisplay(false);
            _callback(true);
        }

    }
}
