using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AvatarCreation
{
    [System.Serializable]
    public class MapVRTrans
    {
        [Header("the VR Target and its ik")]
        public Transform vrTarget;
        public Transform IKTarget;
        public Vector3 trackPosOffset;
        public Vector3 trackRotOffset;


        /// <summary>
        /// to map the avatar
        /// </summary>
        /// <param name="_trackPos">track the position</param>
        /// <param name="_trackRot">track th rotation</param>
        public void MapVRAvatar(Vector3 _trackPos, Vector3 _trackRot)
        {
            IKTarget.position = vrTarget.TransformPoint(_trackPos);
            IKTarget.rotation = vrTarget.rotation * Quaternion.Euler(_trackRot);
        }

    }

    [System.Serializable]
    public class LowerBody
    {
        public Transform footTarget;
        public bool isMoving;
        
        public Vector3 lastFootPos;
        public Vector3 lastFootRot;

        public Transform targetRef;
        public float allowDist = 0.2f;
        public float allowAngle = 10f;
        public Vector2 moveSpeed = new Vector2(0.35f, 0.5f);
        public float speed = 0.35f;
        public float posUp = 0.15f;
        public Vector3 rotUp = new Vector3(0, -30f, 0);



        /// <summary>
        /// to get the initial foot position
        /// </summary>
        public void RecordStartPos()
        {
            //set the speed
            speed = moveSpeed.x;

            lastFootPos = footTarget.position;
            lastFootRot = footTarget.eulerAngles;
        }

        /// <summary>
        /// to update its position
        /// </summary>
        public void StickToGround()
        {
            footTarget.position = lastFootPos;
            footTarget.eulerAngles = lastFootRot;

            lastFootPos = footTarget.position;
            lastFootRot = footTarget.eulerAngles;
        }


        /// <summary>
        /// to check if the leg is far enough in order to move the position back to its root
        /// </summary>
        /// <returns></returns>
        public bool isLegInTolerance()
        {
            //Debug.Log("p " + Vector3.Distance(footTarget.position, targetRef.position));
            //Debug.Log("r " + Quaternion.Angle(footTarget.rotation, targetRef.rotation));

            if (Vector3.Distance(footTarget.position, targetRef.position) >= allowDist
                || Quaternion.Angle(footTarget.rotation, targetRef.rotation) >= allowAngle)
                return false;
            else
                return true;
        }
        
        /// <summary>
        /// get the distance between the current foot position and the target reference
        /// </summary>
        /// <returns></returns>
        public float getDist()
        {
            return Vector3.Distance(footTarget.position, targetRef.position);
        }


        /// <summary>
        /// to move the legs to its actual position
        /// </summary>
        /// <param name="_theBody">attach the ref Avatar here since we need to move the body up and down a bit</param>
        /// <param name="_overTime">how long do the legs need to move to reach the desired target </param>
        /// <returns></returns>
        public IEnumerator MoveLegs(CapsuleCollider _theBody, float _overTime)
        {
            isMoving = true;

            //get the foot to move to the targeted one.
            float startTime = Time.time;
            Vector3 oriPos = footTarget.position;
            Quaternion oriRot = footTarget.rotation;
            float oriBodyHeight = _theBody.height;
            float bodyMoveDownPercent = 0.965f;

            //move it up first
            while (Time.time < startTime + (_overTime /2))
            {
                lastFootPos = targetRef.position;
                lastFootRot = targetRef.eulerAngles;

                //--> Change positiion
                footTarget.position = Vector3.Lerp(oriPos, lastFootPos + new Vector3(0, posUp, 0), (Time.time - startTime) / (_overTime / 2));
                footTarget.rotation = Quaternion.Lerp(oriRot, Quaternion.Euler(lastFootRot + rotUp), (Time.time - startTime) / (_overTime / 2));
                
                //lerp the body a bit -> I mean when people move, they kinda bouncing up and down
                _theBody.height = Mathf.Lerp(oriBodyHeight, oriBodyHeight * bodyMoveDownPercent, (Time.time - startTime) / (_overTime / 2));

                //--> Change Rotation
                //yes there is two condition, because sometimes the angle got so twisted that the rotation doesnt make sense.
                /*
                if (Quaternion.Angle(footTarget.rotation, targetRef.rotation) > allowAngle * 2)
                {
                    footTarget.eulerAngles = lastFootRot;
                    oriRot = targetRef.eulerAngles;
                }
                else
                    footTarget.eulerAngles = Vector3.Lerp(oriRot, lastFootRot + rotUp, (Time.time - startTime) / (_overTime / 2));
                */

                yield return null;
            }


            //Move it to the target
            startTime = Time.time;
            oriPos = footTarget.position;
            oriRot = footTarget.rotation;

            while (Time.time < startTime + (_overTime / 2))
            {
                lastFootPos = targetRef.position;
                lastFootRot = targetRef.eulerAngles;

                //--> Change positiion & rot
                footTarget.position = Vector3.Lerp(oriPos, lastFootPos , (Time.time - startTime) / (_overTime / 2));
                footTarget.rotation = Quaternion.Lerp(oriRot, Quaternion.Euler(lastFootRot), (Time.time - startTime) / (_overTime / 2));
                
                //lerp the body a bit -> I mean when people move, they kinda bouncing up and down
                _theBody.height = Mathf.Lerp(_theBody.height, oriBodyHeight, (Time.time - startTime) / (_overTime / 2));

                /*
                //--> Change Rotation
                //yes there is two condition, because sometimes the angle got so twisted that the rotation doesnt make sense.
                if (Quaternion.Angle(footTarget.rotation, targetRef.rotation) > allowAngle *2)
                {
                    footTarget.eulerAngles = lastFootRot;
                    oriRot = targetRef.eulerAngles;
                }
                else
                    footTarget.eulerAngles = Vector3.Lerp(oriRot, lastFootRot, (Time.time - startTime) / (_overTime / 2));
                */


                yield return null;
            }

            //make sure the legs goes to the target
            footTarget.position = targetRef.position;
            footTarget.rotation = targetRef.rotation;
            _theBody.height = oriBodyHeight;

            //yield return new WaitForSeconds(_overTime * 0.01f);

            isMoving = false;
        }

        /// <summary>
        /// move the legs immediately without any animation
        /// </summary>
        public void MoveLegs()
        {
            lastFootPos = targetRef.position;
            lastFootRot = targetRef.eulerAngles;
            footTarget.position = lastFootPos;
            footTarget.rotation = Quaternion.Euler(lastFootRot);
            isMoving = false;
        }

    }


    public class ProceduralAnim : MonoBehaviour
    {
        public Transform player;
        public Transform refAvatar;
        public Vector3 bodyOffset;
        public float smooth  = 5f;

        [Header("Upper Body")]
        public RaycastingExt headRayCast;
        [SerializeField] public MapVRTrans head;
        [SerializeField] public MapVRTrans leftHand;
        [SerializeField] public MapVRTrans rightHand;

        [Header("Crouching Variable")]
        public bool isCrouch = false;
        public float crouchTriggerHeight = 1f;
        public float crouchTriggerAngle = 135f;
        public float locatorCrouchOffset;


        [Header("Lower Body")]
        public bool isCentered = true;
        bool moveLeftFirst = false;
        [SerializeField] public LowerBody leftLeg;
        [SerializeField] public LowerBody rightLeg;
        public Transform locatorLeg;
        Vector3 locatorPosOffset;
        private IEnumerator legsCoroutine;


        // Start is called before the first frame update
        void Start()
        {
            leftLeg.RecordStartPos();
            rightLeg.RecordStartPos();

            //just a dummy
            legsCoroutine = JustADummy();
        }

        // Update is called once per frame
        void Update()
        {
            #region - - - - - - Head And body
            //mapping the avatar
            head.MapVRAvatar(head.trackPosOffset, head.trackRotOffset);
            leftHand.MapVRAvatar(leftHand.trackPosOffset, leftHand.trackRotOffset);
            rightHand.MapVRAvatar(rightHand.trackPosOffset, rightHand.trackRotOffset);

            //Mapping the body and rotation according to the head.
            // -> pos & rot
            refAvatar.position = head.vrTarget.position + bodyOffset;
            refAvatar.rotation = Quaternion.Lerp(refAvatar.rotation, Quaternion.Euler(0, head.vrTarget.eulerAngles.y, 0), Time.deltaTime * smooth);

            
            #endregion




            #region - - - - - Legs

            isCentered = inCenterOfMass();

            if (inCenterOfMass()) // if the player balanced and not out of the center of mass yet, root the foot
            {
                leftLeg.StickToGround();
                rightLeg.StickToGround();
            }

            else
            {
                //check if legs is currently moving. dont do anything
                if (leftLeg.isMoving || rightLeg.isMoving)
                {
                    return;
                }

                //if none is moving, 
                //check which one has the most distance, then move that farthest leg.
                else
                {
                    if (moveLeftFirst)
                    {
                        moveLeftFirst = false;
                        StopCoroutine(legsCoroutine);
                        legsCoroutine = leftLeg.MoveLegs(player.GetComponent<CapsuleCollider>(), leftLeg.speed);
                        StartCoroutine(legsCoroutine);
                    }

                    else
                    {
                        moveLeftFirst = true;
                        StopCoroutine(legsCoroutine);
                        legsCoroutine = rightLeg.MoveLegs(player.GetComponent<CapsuleCollider>(), rightLeg.speed);
                        StartCoroutine(legsCoroutine);
                    }
                }


            }


            //set the locator position
            locatorLeg.position = head.IKTarget.TransformPoint(locatorPosOffset);
            locatorLeg.eulerAngles = new Vector3(0, head.IKTarget.eulerAngles.y, 0);

            #endregion
            /*
            Debug.Log("left is In Tolerance " + leftLeg.isLegInTolerance());
            Debug.Log("right is In Tolerance " + rightLeg.isLegInTolerance());
            Debug.Log("a " +  inCenterOfMass());
            */

            isCrouch = isCrouching();
            if (isCrouch)
            {
                //change locator position
                locatorPosOffset = new Vector3(0, 0, locatorCrouchOffset);
            }

            else
            {
                //change locator position 
                locatorPosOffset = Vector3.zero;
            }
        }





        #region - - -> helper variable

        /// <summary>
        /// to check if the body is in the center of mass or not
        /// </summary>
        /// <returns></returns>
        public bool inCenterOfMass()
        {
            if (leftLeg.isLegInTolerance() && rightLeg.isLegInTolerance())
                return true;
            else
                return false;
        }



        /// <summary>
        /// Check if the player is crouching, by looking at its head rotation and body position
        /// </summary>
        /// <returns></returns>
        public bool isCrouching()
        {
            // if the head VR target rotation is less than X, and the body position is less than X
            //that means he is crouching
            //print("a " + headRayCast.getDistance() + " b " + head.vrTarget.eulerAngles.x);
            if (headRayCast.getDistance() <= 1f 
                && head.vrTarget.eulerAngles.x >= 30f) 
                return true;
            else
            
                return false;
        }


        /// <summary>
        /// just a dummy coroutine
        /// </summary>
        /// <returns></returns>
        public IEnumerator JustADummy()
        {
            yield return null;
        }




        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(leftLeg.footTarget.position, 0.15f);
            Gizmos.DrawWireSphere(rightLeg.footTarget.position, 0.15f);
            //Debug.DrawLine(transform.position, transform.position + transform.up * 2f);
        }

        #endregion
    }

}
