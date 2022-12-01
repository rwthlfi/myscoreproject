using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarCreationHandTracking
{
    //to stablize things when following other gameObject
    //for example our head/hand moves alot.. 
    //and there are some game Object attached to the head/hand that doesnt need to follow camera perfectly 1 to 1
    //Therefore here is the script to give a bit of stability during the moving
    public class Stabilizer : MonoBehaviour
    {

        [Header("Object Reference")]
        public bool allowStabilizer = true;
        public Transform obj;
        public Transform objToFollow;
        private Transform cacheObjToFollow;


        [Header("Tolerance - PositionVariable")]
        [Range(0, 1)]
        public float tolerancePos = 0.1f;
        public float dampingPos = 5f;
        public Vector3 posOffset = Vector3.zero;
        float cachePos = 0f;

        [Header("Tolerance - RotationVariable")]
        [Range(0, 180)]
        public float toleranceRot = 0.1f;
        public float dampingRot = 5f;
        float cacheRot = 0f;

        public Vector3 rotOffset = Vector3.zero;



        private void Start()
        {
            cacheObjToFollow = objToFollow;
        }


        private float nextActionTime = 0.0f;
        public float period = 0.1f;

        private void Update()
        {

            if (!allowStabilizer)
                return;

            if (Time.time > nextActionTime) 
            { 
                nextActionTime = Time.time + period;


                //obj.position = objToFollow.TransformPoint(posOffset);
                //obj.rotation = objToFollow.rotation * Quaternion.Euler(rotOffset);
                cachePos = Vector3.Distance(obj.position, objToFollow.TransformPoint(posOffset));
                cacheRot = Quaternion.Angle(obj.rotation, objToFollow.rotation * Quaternion.Euler(rotOffset));

            }


            
            //save the position
            //cachePos = Vector3.Distance(obj.position, objToFollow.TransformPoint(posOffset));
            //check the position of how far the difference is.
            //if the distance is larger, then start moving it

            //we make the dampingPos & Rot faster the farther the linerenderer is


            if (cachePos > tolerancePos)
            {
                //print("Lol");
                obj.position = Vector3.MoveTowards(obj.position, objToFollow.TransformPoint(posOffset), dampingPos * Time.deltaTime);

            }



            //save the rotation
            //cacheRot = Quaternion.Angle(obj.rotation, objToFollow.rotation * Quaternion.Euler(rotOffset));
            //check the rotation of how far the difference is
            if (cacheRot > toleranceRot)
                obj.rotation = Quaternion.Lerp(obj.rotation, objToFollow.rotation * Quaternion.Euler(rotOffset), dampingRot * Time.deltaTime);
         
            //*/
        }


    }
}