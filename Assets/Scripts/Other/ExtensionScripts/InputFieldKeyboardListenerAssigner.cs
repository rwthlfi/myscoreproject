using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace UiExtensioner
{
    //this is to assign the "keyboardListener" to an input field when it is clicked.
    //therefore the inputfield can be typed by the keyboard.
    public class InputFieldKeyboardListenerAssigner : MonoBehaviour
    {
        public KeyboardListener keyboardListener;

        //assign the keyboard Listener to the inputfield
        public void Ui_AssignKeyboardListener()
        {
            //get the keyboardListener
            keyboardListener = (KeyboardListener)FindObjectOfType(typeof(KeyboardListener));

            //if keyboard listener exist -> start typing
            if (keyboardListener)
            {
                //assign the inputField where the key stroke will be recorded.
                keyboardListener.keyboard.displayText = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();
            }

        }
    }
}
