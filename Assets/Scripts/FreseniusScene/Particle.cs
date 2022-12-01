using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Fresenius
{
    public class Particle : MonoBehaviour
    {
        public List<Transform> objList = new List<Transform>();
        private List<Vector3> oriPosList = new List<Vector3>();
        private List<Quaternion> oriRotList = new List<Quaternion>();

        public float liveTime = 1f;
        private void Start()
        {
            //record the pos and rot
            foreach(Transform trans in objList)
            {
                oriPosList.Add(trans.localPosition);
                oriRotList.Add(trans.localRotation);
            }
        }



        private void OnEnable()
        {
            //just in case, cancel the invoke first
            CancelInvoke("DisableGameObject");
            //start disabling after 2 seconds
            Invoke("DisableGameObject", liveTime);
            
        }


        //To disable this gameObject
        private void DisableGameObject()
        {
            this.gameObject.SetActive(false);
        }


        private void OnDisable()
        {
            int i = 0;
            //reset the position and rotation back
            foreach(Transform tran in objList)
            {
                tran.localPosition = oriPosList[i];
                tran.localRotation = oriRotList[i];
                i++;
            }
        }
    }

}