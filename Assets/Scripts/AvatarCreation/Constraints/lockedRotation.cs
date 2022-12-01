using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarCreation
{

    public class lockedRotation : MonoBehaviour
    {
        [Header("Limit Value")]
        public float minRot = -60f;
        public float maxRot = 60f;

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
                LimitRot_X();

            else if (constraintY)
                LimitRot_Y();

            else if (constraintZ)
                LimitRot_Z();
        }

        private void LimitRot_X()
        {
            cacheValue = transform.rotation.eulerAngles;

            cacheValue.x = cacheValue.x > 180 ? cacheValue.x - 360 : cacheValue.x;
            cacheValue.x = Mathf.Clamp(cacheValue.x, minRot, maxRot);

            this.transform.rotation = Quaternion.Euler(cacheValue);
        }


        private void LimitRot_Y()
        {
            cacheValue = transform.rotation.eulerAngles;

            cacheValue.y = cacheValue.y > 180 ? cacheValue.y - 360 : cacheValue.y;
            cacheValue.y = Mathf.Clamp(cacheValue.y, minRot, maxRot);

            this.transform.rotation = Quaternion.Euler(cacheValue);
        }


        private void LimitRot_Z()
        {
            cacheValue = transform.rotation.eulerAngles;

            cacheValue.z = cacheValue.z > 180 ? cacheValue.z - 360 : cacheValue.z;
            cacheValue.z = Mathf.Clamp(cacheValue.z, minRot, maxRot);

            this.transform.rotation = Quaternion.Euler(cacheValue);
        }
    }

}
