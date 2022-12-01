using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Fresenius
{
    public class CollectorTank : MonoBehaviour
    {
        public Transform water;
        public float currentValue = 0;

        private float tolerance = 0.15f; // to avoid lerping infinitely

        public TextMeshPro textPro;




        // Update is called once per frame
        void Update()
        {
            //check if the scaling is already reach the needed value
            if (water.localScale.y == currentValue - tolerance)
                return;

            water.localScale = Vector3.Lerp(water.localScale,
                                            new Vector3(water.localScale.x,
                                                        currentValue / 15, // -> just for scaling
                                                        water.localScale.z),
                                            Time.deltaTime * 1f
                                            );
        }


        /// <summary>
        /// To assign the tank's name
        /// </summary>
        /// <param name="_str"></param>
        public void AssignTankName(string _str)
        {
            textPro.text = _str;
        }

        /// <summary>
        /// To add the water and update the tank
        /// </summary>
        /// <param name="_value"></param>
        public void AddWater(float _value)
        {
            currentValue += _value;
        }


        /// <summary>
        /// To reduce the water and update the tank
        /// </summary>
        /// <param name="_value"></param>
        public void ReduceWater(float _value)
        {
            currentValue -= _value;

            if (currentValue <= 0)
                currentValue = 0;

        }
        public void activateThisTank(bool _val)
        {
            this.gameObject.SetActive(_val);
        }

        public string getTankName()
        {
            return textPro.text;
        }

        public float getTotalValue()
        {
            return currentValue;
        }

        public float getAverageValue()
        {
            return currentValue / 5;
        }
    }

}