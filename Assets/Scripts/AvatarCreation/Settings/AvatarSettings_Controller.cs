using UnityEngine;
using UnityEngine.UI;


namespace AvatarCreation
{
    [System.Serializable]
    public class controllerVar
    {
        public GameObject controllerL;
        public GameObject controllerR;

        public Canvas controllerCanvasL;
        public Canvas controllerCanvasR;

        public void SetControllerActive(bool _value)
        {
            controllerL.SetActive(_value);
            controllerR.SetActive(_value);
        }

        public void SetControllerInfoActive(bool _value)
        {
            controllerCanvasL.gameObject.SetActive(_value);
            controllerCanvasR.gameObject.SetActive(_value);
        }
    }


    public class AvatarSettings_Controller : MonoBehaviour
    {
        public controllerVar oculusVar;
        public controllerVar steamVar;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void LoadShowSetting()
        {
            //Load the controller
            oculusVar.SetControllerActive(ConverterFunction.IntToBool(PlayerPrefs.GetInt(PrefDataList.showController)));
            steamVar.SetControllerActive(ConverterFunction.IntToBool(PlayerPrefs.GetInt(PrefDataList.showController)));

            //load the info as well
            oculusVar.SetControllerInfoActive(ConverterFunction.IntToBool(PlayerPrefs.GetInt(PrefDataList.InfoController)));
            steamVar.SetControllerInfoActive(ConverterFunction.IntToBool(PlayerPrefs.GetInt(PrefDataList.InfoController)));
        }

        public void Ui_ShowController(Toggle _toggle)
        {
            oculusVar.SetControllerActive(_toggle.isOn);
            steamVar.SetControllerActive(_toggle.isOn);
            //save the value
            PlayerPrefs.SetInt(PrefDataList.showController, ConverterFunction.BoolToInt(_toggle.isOn));
        }

        public void Ui_ShowInfo(Toggle _toggle)
        {
            oculusVar.SetControllerInfoActive(_toggle.isOn);
            steamVar.SetControllerInfoActive(_toggle.isOn);

            //save the value
            PlayerPrefs.SetInt(PrefDataList.InfoController, ConverterFunction.BoolToInt(_toggle.isOn));
        }


    }
}
