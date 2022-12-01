using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using TMPro;
using Mirror;

namespace WZLExperiment
{
    public class ScannerSensor : NetworkBehaviour
    {
        [Header("GameObject Variable")]
        public Transform mainObj;
        public Transform objParent;
        public Transform turningPlate;

        //Synchro this with array later
        [Header("SyncVar - Properties")]
        [SyncVar(hook = nameof(OnScannedChanged))]
        public string scannedIndexList;
        public List<string> tempList;

        [Header("UI variable")]
        public TextMeshProUGUI textPercent;
        public TextMeshProUGUI textComplete;
        public Button flipButton;

        [Header("Script Reference")]
        public RaycastingExt raycastingExt;


        //Networking things

        void OnScannedChanged(string _Old, string _New)
        {
            //get the mainObj
            scannedIndexList = _New;

            //now the server need to check which one has been on
            if (scannedIndexList == "")
                EnableAllMeshRend(false);

        }

        private void EnableAllMeshRend(bool _val)
        {
            foreach (MeshRenderer _mr in mainObj.GetComponentsInChildren<MeshRenderer>())
                _mr.enabled = _val;
        }

        // Start is called before the first frame update
        void Start()
        {
        }


        private float nextActionTime = 0.0f;
        public float period = 1f;
        // Update is called once per frame
        void Update()
        {
            if (Time.time > nextActionTime) 
            { 
                nextActionTime = Time.time + period;
                //code here
                CheckScanning();
                textPercent.text = getPercentage().ToString() + "%";
                //print("percent " + getPercentage());

                //show or hide the "complete" sign.
                if (textPercent.text == "100%")
                    textComplete.gameObject.SetActive(true);
                else
                    textComplete.gameObject.SetActive(false);
                
            }
        }


        /// <summary>
        /// To Check the scanning. if the index is there, enabled the mesh renderer
        /// </summary>
        private void CheckScanning()
        {
            //split string
            tempList = scannedIndexList.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            
            //loop through the list of string
            foreach(string _str in tempList)
            {
                //print("a " + _str);
                //activate the gameObject child hierarchy
                mainObj.GetChild(ConverterFunction.StringToInt(_str)).GetComponent<MeshRenderer>().enabled = true;
            }

            

        }


        /// <summary>
        /// to Start Scanning the target
        /// </summary>
        public void UI_StartScan()
        {
            //string a = getScannedIdx();
            string a = getRandomScanned();
            //get the scanned index
            if (a == "")
                return;

            //send the scanned index to Server
            else
            {
                //for offline debuggging
                //SendScannedIndex(a);
                //end of offline debugging

                Cmd_SendScannedIndex(a);

            }
        }

        [Command(requiresAuthority = false)]
        private void Cmd_SendScannedIndex(string _index)
        {
            SendScannedIndex(_index);
        }

        public void SendScannedIndex(string _index)
        {
            //print("index " + _index);
            //activate the renderer
            mainObj.GetChild(ConverterFunction.StringToInt(_index)).GetComponent<MeshRenderer>().enabled = true;

            string a = "";
            //now the server need to check which one has been on
            foreach (MeshRenderer _mr in mainObj.GetComponentsInChildren<MeshRenderer>())
            {
                if (_mr.enabled)
                    a += _mr.transform.GetSiblingIndex() + "|";
            }

            //NOTE: Change this to the CMD
            scannedIndexList = a;
        }


        /// <summary>
        /// To get the scanned index when the start scan is called
        /// </summary>
        /// <returns></returns>
        private string getScannedIdx()
        {
            if (raycastingExt.hitTarget)
            {
                print("object scanned " + raycastingExt.hitTarget);
                //get the child position
                int child = raycastingExt.hitTarget.transform.GetSiblingIndex();

                return ConverterFunction.IntToString(child);
            }

            else
                return "";
        }


        public List<int> tmp = new List<int>();
        /// <summary>
        /// to get a random scanned index
        /// </summary>
        /// <returns></returns>
        private string getRandomScanned()
        {
            if (raycastingExt.hitTarget)
            {
                print("object scanned " + raycastingExt.hitTarget);
                tmp.Clear();
                //tmp = tempList;
                //get the child position
                int child = 0;
                foreach (MeshRenderer _mr in mainObj.GetComponentsInChildren<MeshRenderer>(true))
                {
                    if (_mr.enabled)
                    {
                        tmp.Add(child);
                    }
                    child++;
                }

                int a = ConverterFunction.getNewNumberOutsideGivenList(tmp, 0, mainObj.GetComponentsInChildren<MeshRenderer>(true).Length -1);
                tmp.Add(a);
                if (a == -1)
                    a = 0;
                //print("valu " + a);
                return ConverterFunction.IntToString(a);
            }

            else
                return "";
        }


        private int getPercentage()
        {
            //get the total child of the main object
            int total = mainObj.GetComponentsInChildren<MeshRenderer>(true).Count();

            
            int current = 0;
            //check how many of the mesh renderer is enabled
            //now the server need to check which one has been on
            foreach (MeshRenderer _mr in mainObj.GetComponentsInChildren<MeshRenderer>(true))
            {
                if (_mr.enabled)
                    current++;
            }
            //print("total " + total + " current " + current);

            return current * 100 / total ;
        }



        /// <summary>
        /// To flip the object and disable the interaction for X seconds
        /// </summary>
        /// <param name="_flipButton"></param>

        public void Ui_FlipObject_Again()
        {
            Cmd_FlipObject();
        }


        //to tell the server to start the tutorial.
        [Command(requiresAuthority = false)]
        public void Cmd_FlipObject()
        {
            FlipObject();
        }

        public void FlipObject()
        {
            float angle = ConverterFunction.getAngleInspector(objParent).x;
            print("angle " + angle);

            if (objParent.localEulerAngles.x != 0)
            {
                StartCoroutine(LerpingExtensions.RotateToLocalEuler(objParent, Vector3.zero, 1f));
                StartCoroutine(CoroutineExtensions.InteractableButtonAfterSeconds(flipButton, 1f));
                objParent.localEulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                StartCoroutine(LerpingExtensions.RotateToLocalEuler(objParent, new Vector3(180f, 0, 0), 1f));
                StartCoroutine(CoroutineExtensions.InteractableButtonAfterSeconds(flipButton, 1f));
                //objParent.localEulerAngles = new Vector3(180, 0, 0);
            }
        }



        /// <summary>
        /// To Reset the experiment of the scanning
        /// </summary>
        public void Ui_ResetExperiment_Scanner()
        {
            Cmd_ResetExp_Sensor();
        }


        //to tell the server to start the tutorial.
        [Command(requiresAuthority = false)]
        public void Cmd_ResetExp_Sensor()
        {
            ResetExp_Scanner();
        }


        public void ResetExp_Scanner()
        {
            scannedIndexList = "";
            foreach (MeshRenderer _mr in mainObj.GetComponentsInChildren<MeshRenderer>())
            {
                _mr.enabled = false;
            }
            Debug.Log("scanned " + scannedIndexList);
        }


        /// <summary>
        /// To show the sensor object
        /// </summary>
        /// <param name="_toggle"></param>
        public void Ui_toggleShowSensor(Toggle _toggle)
        {
            raycastingExt.drawLine = _toggle.isOn;
            raycastingExt.ActivateLineRenderer(_toggle.isOn);
        }
    }

}