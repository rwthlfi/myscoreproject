using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// This script is to only allow one Toggle to remain "on" 
// The rest of the toggle in the RadioToggleList group will be deactivated without triggering the "OnValueChanged"
public class RadioToggle_AddOn : MonoBehaviour
{
    public List<Toggle> RadioToggleList = new List<Toggle>();

    private void Awake()
    {
        //set the first toggle to be true
        if (RadioToggleList.Count <= 0)
            return;
        RadioToggleList[0].Set(true, false);
    }

    //attach this to each of the toggle.
    public void Toggle_IsClicked()
    {
        Toggle clickedToggle = EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>();

        foreach (Toggle tg in RadioToggleList)
            tg.Set(false, false);

        clickedToggle.Set(true, false);
        //Debug.Log("name " + clickedToggle.name);
    }

    //get the activated toggle in the group
    public Toggle getActiveToggle()
    {
        foreach (Toggle tg in RadioToggleList)
        {
            if (tg.isOn)
                return tg;
        }

        //if there is none activated, which is impossible then activated the first toggle in the group and return it.
        RadioToggleList[0].Set(true, false);
        return RadioToggleList[0];
    }
}
