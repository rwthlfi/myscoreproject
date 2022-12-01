using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace WZLExperiment
{
    //to insert the data to the line Renderer

    public class UltrasonicGraphInserter : MonoBehaviour
    {
        [Header("Graph Variable")]
        public int graphSizeX = 1000;
        public float scalingX = 5;

        [Header("UltrasonicSensor Reference")]
        public bool useReadSensor = false;
        public float sensorValueScaling = 100f;
        public List<UltraSonicSensor> ultraSonicSensorList = new List<UltraSonicSensor>();


        [Header("Extra Effect")]
        public bool useNoise = false; // if true, the graph will not have a flat line, but rather fluctuating randomly according to the noiseValue
        public Vector2 noiseValue = new Vector2(0, 1); //-> how much random noise you want to get

        [Header("Reference")]
        public LineRenderer lineRenderer;


        //Put here the SyncVar noise sensor things
        public string sensorValue = "";
        private string[] sensorByte;


        //Cache variable

        double depth;
        float percentage;
        int lineRenderPos;

        void Start()
        {
            InitGraph();
        }


        private float nextActionTime = 0.0f;
        public float period = 1f; // every x seconds
        void Update()
        {
            //to limit the update since updating every frame is expensive
            if (Time.time > nextActionTime)
            {
                nextActionTime = Time.time + period;
                // execute block of code here/ 

                //for noise randomize
                if (useNoise)
                {
                    NoiseEffect();
                }

                //set total position
                lineRenderer.positionCount = graphSizeX;

                if (useReadSensor)
                {
                    sensorValue = "";
                    //get the values from the sensor in sensorList.
                    foreach (UltraSonicSensor us in ultraSonicSensorList)
                    {
                        //if it doesnt hit anything or the raycast is not active, dont do anything
                        if (!us.raycastingExt.isHit || !us.gameObject.activeInHierarchy)
                            continue;

                        //otherwise parse the value
                        sensorValue += us.raycastingExt.hitDistance * sensorValueScaling + "||" +
                                       us.raycastingExt.getPercentageDiff() + "||";
                    }

                    if (sensorValue == "")
                        sensorValue = "0||100"; //->well the customer demanded so.

                    //if no sensor is readed, then do nothing
                    if (sensorValue == "")
                        return;
                    else // otherwise parse the value to the graphs
                        ReadingGraph(sensorValue);
                }
            }

        }


        /// <summary>
        /// Set the initial graph
        /// </summary>
        private void InitGraph()
        {
            //set total position
            lineRenderer.positionCount = graphSizeX;
            //set te position
            for (int i = 0; i < graphSizeX; i++)
                lineRenderer.SetPosition(i, new Vector2(i / scalingX, 0));
        }


        /// <summary>
        /// To modify the value of the graph
        /// (we are not using byte array or int, because the byte [] is not supported, and with int we will have to many variable and not able to be scaled dynamically
        /// </summary>
        /// <param name="_str"></param>
        private void ReadingGraph(string _str)
        {
            sensorByte = _str.Split(new string[] { "||" }, StringSplitOptions.None);

            for (int i = 0; i < sensorByte.Length - 1; i += 2)
            {
                depth = Convert.ToDouble(sensorByte[i]);
                percentage = (float)Convert.ToDouble(sensorByte[i + 1]);


                //for every "scalingX" (in this case 5 unit) -> is equal to 1mm.
                //thereforeto get the position of the linerenderer you need to multiply them by the "scalingX"
                //example: if given sensorPos is 3mm that means 3*scalingX = 15
                lineRenderPos = (int)(depth * scalingX);

                /*
                //check if position exist -> to prevent the index out of bounds message!
                if (lineRenderer.positionCount >= lineRenderPos)
                {
                    print("its out of bounds");
                    return;
                }*/

                if (lineRenderPos <= lineRenderer.positionCount)
                {
                    //get the position from the distance
                    lineRenderer.SetPosition(lineRenderPos, new Vector2((float)depth, percentage));

                }

            }
        }


        /// <summary>
        /// To Generate noise instead of a flat line
        /// </summary>
        private void NoiseEffect()
        {
            for (int i = 0; i < graphSizeX; i++)
                lineRenderer.SetPosition(i, new Vector2(i / scalingX, UnityEngine.Random.Range(noiseValue.x, noiseValue.y)));
        }



    }

}