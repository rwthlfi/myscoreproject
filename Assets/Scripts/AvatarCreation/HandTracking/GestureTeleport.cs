using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AvatarCreationHandTracking
{
    [RequireComponent(typeof(LineRenderer))]
    //To trigger the teleporting function after certain gestern is performed
    public class GestureTeleport : MonoBehaviour
    {
        [Header("Teleporting Variable")]
        public Transform attachPoint;
        public float offset = -1;
        public Vector3 offsetRot= new Vector3(0, 0, 1);
        private RaycastHit hit;
        public LayerMask layerMask;
        public string layerTag = "Teleportable";
        public bool allowTeleport = false;
        public float maxDistance = 15f;

        [Header("Teleport Time")]
        public float castingTime = 3f;
        private float timeRemaining;

        [Header("Teleport UI")]
        public TextMeshProUGUI teleportTimeText;

        [Header("Teleport Char Variable")]
        public GameObject charToTeleport;

        [Header("Teleport Effect ")]
        public LineRenderer lineRenderer;
        private Vector3 middlePoint;
        private Vector3 endPoint;
        public float vertexCount = 7;
        public float point2YPosition = 2;

        //debug
        //public GameObject debugGameObj;

        private void Start()
        {
            timeRemaining = castingTime;
            //lineRenderer = GetComponent<LineRenderer>();
            ClearCurvedLineRenderer();
            this.enabled = false;
        }

        void Update()
        {
        }

        private void OnDisable()
        {
            ClearCurvedLineRenderer();
        }

        public void ActivatedTeleport(bool _actived)
        {
            allowTeleport = _actived;
            this.enabled = _actived;
        }


        private void FixedUpdate()
        {
            //set the allow teleport to allow char to use teleport
            if (!allowTeleport)
                return;


            // Does the ray intersect any objects excluding the player layer
            //if (Physics.Raycast(attachPoint.position, attachPoint.TransformDirection(Vector3.forward * offset), out hit, maxDistance, layerMask))
            if (Physics.Raycast(attachPoint.position, attachPoint.TransformDirection(offsetRot * offset), out hit, maxDistance, layerMask)
                )
            {
                //endPoint = hit.point;
                //debugGameObj = hit.transform.gameObject;
                //print("yes" + hit.transform.name);
                if (hit.transform.tag == layerTag)
                {
                    //Only for Debugging
                    //Debug.DrawRay(attachPoint.position, attachPoint.TransformDirection(Vector3.forward * offset) * hit.distance, Color.yellow);
                    //Debug.Log("Did Hit: " + hit.transform);
                    
                    endPoint = hit.point;
                    SetCurvedLineRenderer(Color.green);

                    //if there is still time; then reduce the time
                    if (timeRemaining > 0)
                    {
                        timeRemaining -= Time.deltaTime;
                        ShowCastingText(true);
                    }

                    //otherwise teleport the char and reset the time
                    if (timeRemaining <= 0)
                    {
                        //teleport the player to the hit point coordinate.
                        charToTeleport.transform.position = hit.point;
                        //clear the curve line renderer and
                        ClearCurvedLineRenderer();
                    }

                }
            }

            else
            {
                endPoint = attachPoint.position + attachPoint.TransformDirection(offsetRot * offset) * maxDistance;
                SetCurvedLineRenderer(Color.red);
                //Only for Debugging
                //Debug.DrawRay(attachPoint.position, attachPoint.TransformDirection(Vector3.forward * offset) * maxDistance, Color.white);
                //Debug.Log("Did not Hit" + hit.transform);
                //ClearCurvedLineRenderer();

            }
        }


        private void SetCurvedLineRenderer(Color _color)
        {
            lineRenderer.enabled = true;

            //change line renderer color
            lineRenderer.material.color = _color;

            middlePoint = new Vector3((attachPoint.position.x + endPoint.x) / 2,
                                       point2YPosition,
                                       (attachPoint.position.z + endPoint.z) / 2
                                      );



            var pointList = new List<Vector3>();

            for (float ratio = 0; ratio <= 1; ratio += 1 / vertexCount)
            {
                var tangent1 = Vector3.Lerp(attachPoint.position, middlePoint, ratio);
                var tangent2 = Vector3.Lerp(middlePoint, endPoint, ratio);
                var curve = Vector3.Lerp(tangent1, tangent2, ratio);
                pointList.Add(curve);
            }

            lineRenderer.positionCount = pointList.Count;
            lineRenderer.SetPositions(pointList.ToArray());
        }



        private void ClearCurvedLineRenderer()
        {
            lineRenderer.enabled = false;
            timeRemaining = castingTime;
            ShowCastingText(false);
        }


        private void ShowCastingText(bool _show)
        {
            if (!teleportTimeText)
            {
                print("Are you sure you dont need the Casting Text ? cause it is not assigned");
                return;
            }

            //set the telportation text to be active/not
            teleportTimeText.gameObject.SetActive(_show);

            if (_show) // if need to be shown, which means that the teleport ray is hitting something
                teleportTimeText.text = timeRemaining.ToString("F1");
            else //which means it doesnt need to be shown, then reset the timer
                teleportTimeText.text = castingTime.ToString("F1");

        }
    }
}