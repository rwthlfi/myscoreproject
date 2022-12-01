using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace WZLExperiment
{
    public class LensHolder : MonoBehaviour
    {
        [Header("Detection Lens")]
        public string detectionTag = "Set the Gameobject tag in here.";
        public GameObject currentCollider;
        public Transform attachPoint;

        [Header("Lens Variable")]
        public GameObject LensA;
        public GameObject LensB;
        public GameObject LensC;
        public Rigidbody LensA_rigidBdy;
        public Rigidbody LensB_rigidBdy;
        public Rigidbody LensC_rigidBdy;
        public RaycastingExt lensC_raycastExt;

        [Header("Light Variable")]
        public RaycastingExt lightDiffuse;
        public RaycastingExt lightRing;
        public RaycastingExt lightDome;

        [Header("Sprite Renderer")]
        public SpriteRenderer spriteRenderer;
        public Sprite spriteNull;
        public Sprite spriteA;
        public Sprite spriteB;
        public Sprite spriteC;

        public Sprite spriteL_Diffuse;
        public Sprite spriteL_Ring;
        public Sprite spriteL_Dome;

        [Header("UI Info")]
        public TextMeshPro info_selectLens;
        public TextMeshPro info_selectLight;



        // Start is called before the first frame update
        void Start()
        {
            ChangeInfo(spriteNull, false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == detectionTag && !currentCollider)
            {
                //print("Enter");

                //register it to the system so the OnTriggerExit will be called if the GameObject is disabled.
                ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
                currentCollider = other.gameObject;
                CheckLens(currentCollider);
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == currentCollider)
            {
                //print("Exit");

                //register the exit
                ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
                currentCollider = null;

                CheckLens(currentCollider);
            }
        }


        private void Update()
        {
            /*
            if (currentCollider == LensC)
            {
                LensC.transform.position = Vector3.MoveTowards(LensC.transform.position,
                                                               attachPoint.position,
                                                               Time.deltaTime * 0.5f);
                LensC.transform.rotation = Quaternion.RotateTowards(LensC.transform.rotation,
                                                                   attachPoint.rotation,
                                                                   Time.deltaTime * 60f);
            }
            */

            if (currentCollider == LensA)
                SnapPosRot(LensA);
            else if (currentCollider == LensB)
                SnapPosRot(LensB);

            else if(currentCollider == LensC)
            {
                SnapPosRot(LensC);
                CheckLight();
            }
        }





        /// <summary>
        /// checking the lens
        /// </summary>
        /// <param name="_go"></param>
        private void CheckLens(GameObject _go)
        {
            if (_go == LensA)
            {
                ChangeInfo(spriteA, false);
                LensA_rigidBdy.isKinematic = true;
                LensB_rigidBdy.isKinematic = false;
                LensC_rigidBdy.isKinematic = false;
            }

            else if (_go == LensB)
            {

                ChangeInfo(spriteB, false);
                LensA_rigidBdy.isKinematic = false;
                LensB_rigidBdy.isKinematic = true;
                LensC_rigidBdy.isKinematic = false;
            }

            else if (_go == LensC)
            {
                ChangeInfo(spriteC, true);
                LensA_rigidBdy.isKinematic = false;
                LensB_rigidBdy.isKinematic = false;
            }

            else
            {
                ChangeInfo(spriteNull, false);
                LensA_rigidBdy.isKinematic = false;
                LensB_rigidBdy.isKinematic = false;

            }
        }




        /// <summary>
        /// when the lens is changed 
        /// </summary>
        /// <param name="_sprite"></param>
        /// <param name="_rightLens"></param>
        private void ChangeInfo(Sprite _sprite, bool _rightLens)
        {
            if (_rightLens) // if it is given a right lens
            {
                ShowInfo(info_selectLight);
                LensC_rigidBdy.isKinematic = true;
                spriteRenderer.sprite = spriteC;
            }

            else
            {
                ShowInfo(info_selectLens);
                LensC_rigidBdy.isKinematic = false;
                spriteRenderer.sprite = _sprite;
            }

            lensC_raycastExt.enabled = _rightLens;
        }


        private void CheckLight()
        {
            //if raycasting not enabled -> dont do anything.
            if (!lensC_raycastExt.enabled)
                return;

            //otherwise check what the Raycast hit.
            if (lensC_raycastExt.hitTarget == lightDiffuse.gameObject)
            {
                //print("light Diffuse");
                lightDiffuse.enabled = true;
                if (lightDiffuse.hitTarget)
                    spriteRenderer.sprite = spriteL_Diffuse;
            }

            else if (lensC_raycastExt.hitTarget == lightRing.gameObject)
            {
                //print("light Ring");
                lightRing.enabled = true;
                if (lightRing.hitTarget)
                    spriteRenderer.sprite = spriteL_Ring;

            }

            else if (lensC_raycastExt.hitTarget == lightDome.gameObject)
            {
                //print("light Dome");
                lightDome.enabled = true;
                if (lightDome.hitTarget)
                    spriteRenderer.sprite = spriteL_Dome;

            }


            //if none fit, then disable the light
            else
            {
                lightDiffuse.enabled = false;
                lightRing.enabled = false;
                lightDome.enabled = false;
                spriteRenderer.sprite = spriteC;
            }


        }


        public void SnapPosRot(GameObject _lens)
        {

            _lens.transform.position = Vector3.MoveTowards(_lens.transform.position,
                                                           attachPoint.position,
                                                           Time.deltaTime * 0.5f);
            _lens.transform.rotation = Quaternion.RotateTowards(_lens.transform.rotation,
                                                               attachPoint.rotation,
                                                               Time.deltaTime * 60f);

        }


        /// <summary>
        /// show the info
        /// </summary>
        /// <param name="_text"></param>
        private void ShowInfo(TextMeshPro _text)
        {
            info_selectLens.gameObject.SetActive(false);
            info_selectLight.gameObject.SetActive(false);

            _text.gameObject.SetActive(true);
        }
    }
}