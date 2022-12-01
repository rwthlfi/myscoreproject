using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;


namespace WZLExperiment
{
    public class UltraSonicSensor : MonoBehaviour
    {
        [Header("Script Reference")]
        public RaycastingExt raycastingExt;

        [Header("Use Enable Obj")]
        public bool useEnableObj = false;
        public List<GameObject> objToEnabledList = new List<GameObject>();


        // Update is called once per frame
        void Update()
        {
            //if is hit and there is hitTarget, do something
            if (raycastingExt.isHit && raycastingExt.hitTarget)
                EnableObject(true);
            else
                EnableObject(false);



            //print(this.name + " Value: " + raycastingExt.hitDistance);
        }



        private void EnableObject(bool _value)
        {
            //if enable object is not needed, dont do anything
            if (!useEnableObj)
                return;

            //otherwise, enable all object in the list
            else
            {
                foreach (GameObject go in objToEnabledList)
                    go.SetActive(_value);

            }
        }

        /// <summary>
        /// To show the sensor object
        /// </summary>
        /// <param name="_toggle"></param>
        public void Ui_toggleShowSensor(Toggle _toggle)
        {
            raycastingExt.drawLine = _toggle.isOn;
            raycastingExt.ActivateLineRenderer(_toggle.isOn);

            foreach(GameObject go in objToEnabledList)
            {
                go.GetComponent<RaycastingExt>().drawLine = _toggle.isOn;
                go.GetComponent<RaycastingExt>().ActivateLineRenderer ( _toggle.isOn);
            }
        }

    }
}
