using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RadioButton_AddOn : MonoBehaviour
{
    public List<Button> buttonList = new List<Button>();
    public Color activatedColor = new Color(255, 255, 255);
    public Color deactivatedColor = new Color(128, 128, 128);


    public void Ui_ButtonClicked(Button _button)
    {
        ChangeColor(_button);

    }

    public void ChangeColor(Button _button)
    {
        //turn all the button' color to the deactivated color
        foreach (Button btn in buttonList)
            btn.GetComponent<Image>().color = deactivatedColor;


        //and turn the desired button's color to the activated one.
        _button.GetComponent<Image>().color = activatedColor;
    }

}
