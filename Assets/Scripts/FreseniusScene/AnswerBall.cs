using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fresenius
{
    public class AnswerBall : MonoBehaviour
    {
        [Header("Answer Variable")]
        public float answerVal;

        //tank variable
        public CollectorTank collectorTank;


        [Header("Coordinate")]
        public bool useCoordinate = false;
        public float smoothness;
        public Vector3 initPos;
        public Vector3 finalPos;

        [Header("Scaled up")]
        public bool useAutoScale = false;


        private void FixedUpdate()
        {
            if (useCoordinate)
            {

                if (this.transform.localPosition != finalPos)
                {
                    this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, finalPos, Time.deltaTime * smoothness);
                    this.transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.one, Time.deltaTime * smoothness);
                }

                else
                {
                    this.transform.localPosition = initPos;
                    this.transform.localScale = Vector3.zero;
                }
            }

            if (useAutoScale)
            {
                this.transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.one, Time.deltaTime * smoothness);
            }

        }


        private void OnDisable()
        {
            //this.transform.localPosition = Vector3.zero;
            this.transform.localScale = Vector3.zero;
        }


        /// <summary>
        /// To assign which tanks to fill
        /// </summary>
        /// <param name="_val"></param>
        public void AssignTankID(CollectorTank _tank)
        {
            collectorTank = _tank;
        }


        /// <summary>
        /// Fill the Tank
        /// </summary>
        /// <param name="_val"></param>
        public void FillTheTank()
        {
            collectorTank.AddWater(answerVal);
        }


    }
}