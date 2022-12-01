using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AvatarCreation
{
    public class MovementPC : MonoBehaviour
    {
        [Header("Reference")]
        public ProceduralAnim player;

        [Header("To Move Object")]
        public RaycastingExt rayExt_Head;
        public Transform handTarget;
        Transform cacheObj = null;
        public Vector3 cacheHandPos;
        Vector3 cacheHandRot;


        //Status Variable
        bool isSitting;
        float height;


        //Movment speed variable
        float speed = 0.5f;
        public Vector2 speedLimit = new Vector2(0.5f, 1f);


        // Start is called before the first frame update
        void Start()
        {
            speed = speedLimit.x;
            height = player.GetComponent<CapsuleCollider>().height;

            //cache the hand position and rotation
            cacheHandPos = player.rightHand.vrTarget.localPosition;
            cacheHandRot = player.rightHand.vrTarget.localEulerAngles;

            //Hide and Lock the cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            //Cursor.lockState = CursorLockMode.Confined;
        }

        // Update is called once per frame
        void Update()
        {
            MovementInput();
        }

        private void MovementInput()
        {
            //Movement

            if (Input.GetKey(KeyCode.W))
                player.transform.Translate(Vector3.forward * Time.deltaTime * speed);

            if (Input.GetKey(KeyCode.S))
                player.transform.Translate(-1f * Vector3.forward * Time.deltaTime * speed);

            if (Input.GetKey(KeyCode.A))
                player.transform.Translate(Vector3.left * Time.deltaTime * speed);

            if (Input.GetKey(KeyCode.D))
                player.transform.Translate(Vector3.right * Time.deltaTime * speed);


            //rotate the player using mouse
            player.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed * 1000);

            //rotate the player's head
            player.head.vrTarget.transform.Rotate(new Vector3( -1 *Input.GetAxis("Mouse Y"), 0, 0) * Time.deltaTime * speed * 1000);


            // - - > special Ability
            //Running
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = speedLimit.y;
                player.leftLeg.speed = player.leftLeg.moveSpeed.x; 
                player.rightLeg.speed = player.rightLeg.moveSpeed.x;
            }
            else
            {
                speed = speedLimit.x;
                player.leftLeg.speed = player.leftLeg.moveSpeed.y;
                player.rightLeg.speed = player.rightLeg.moveSpeed.y;
            }

            //Crouch
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                isSitting = !isSitting; // flip the bool
                if (isSitting)
                    player.GetComponent<CapsuleCollider>().height /= 2;
                else
                    player.GetComponent<CapsuleCollider>().height = height;
            }



            //Grab Item
            if (Input.GetKey(KeyCode.Mouse0) && rayExt_Head.isHit)
            {
                //assign the object
                cacheObj = rayExt_Head.hitTarget.transform;
                //move the object to the hand target

                //move the hand to its position & rotation
                cacheObj.position = handTarget.position;
                cacheObj.rotation = player.rightHand.vrTarget.rotation;

                player.rightHand.vrTarget.position = Vector3.Lerp(player.rightHand.vrTarget.position, handTarget.position, Time.deltaTime * 10f);
                player.rightHand.vrTarget.rotation = Quaternion.Lerp(player.rightHand.vrTarget.rotation, handTarget.rotation, Time.deltaTime * 10f);

            }

            else if (!Input.GetKey(KeyCode.Mouse0))
            {
                cacheObj = null;
                player.rightHand.vrTarget.localPosition = Vector3.Lerp(player.rightHand.vrTarget.localPosition, cacheHandPos, Time.deltaTime * 10f);
                player.rightHand.vrTarget.localRotation = Quaternion.Lerp(player.rightHand.vrTarget.localRotation, Quaternion.Euler(cacheHandRot), Time.deltaTime * 10f);
            }

        }


    }
}