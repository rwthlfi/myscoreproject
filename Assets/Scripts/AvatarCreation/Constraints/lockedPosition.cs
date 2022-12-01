using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AvatarCreation
{

    public class lockedPosition : MonoBehaviour
    {
        [Header("Limit localValue")]
        public float minLoc = -5f;
        public float maxLoc = 5f;


        [Header("ConstraintAxes")] // you are only allowed to choose one of the axis
        public bool constraintX;
        public bool constraintY;
        public bool constraintZ;
        private Vector3 cacheValue;


        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (constraintX)
                LimitLoc_X();

            else if (constraintY)
                LimitLoc_Y();

            else if (constraintZ)
                LimitLoc_Z();
        }

        private void LimitLoc_X()
        {
            //Get the local position
            cacheValue = transform.localPosition;

            cacheValue.x = Mathf.Clamp(cacheValue.x, minLoc, maxLoc);
            this.transform.localPosition = cacheValue;
        }

        private void LimitLoc_Y()
        {
            //Get the local position
            cacheValue = transform.localPosition;

            cacheValue.y = Mathf.Clamp(cacheValue.y, minLoc, maxLoc);
            this.transform.localPosition = cacheValue;
        }

        private void LimitLoc_Z()
        {
            //Get the local position
            cacheValue = transform.localPosition;

            cacheValue.z = Mathf.Clamp(cacheValue.z, minLoc, maxLoc);
            this.transform.localPosition = cacheValue;
        }
    }

}
