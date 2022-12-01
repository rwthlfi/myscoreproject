using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AvatarCreation
{
    public class AlwaysGrounded : MonoBehaviour
    {
        public RaycastingExt rayExt;
        public bool trackPos = true;
        public bool trackRot = true;

        [Header("Offset")]
        public Vector3 trackPosOff;
        public Vector3 trackRotOff;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (trackPos)
                this.transform.position = Vector3.Lerp(this.transform.position,
                                                       new Vector3(transform.position.x, rayExt.getEndCoor().y + trackPosOff.y, transform.position.z),
                                                       Time.deltaTime * 10.0f);



            if (trackRot && rayExt.isHit)
            {
                // this is working
                // this.transform.rotation = rayExt.getEndQuaternion() * Quaternion.Euler(trackRotOff);

                this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
                                                         rayExt.getEndQuaternion() * Quaternion.Euler(trackRotOff),
                                                         Time.deltaTime * 10f);
            }

        }

    }

}