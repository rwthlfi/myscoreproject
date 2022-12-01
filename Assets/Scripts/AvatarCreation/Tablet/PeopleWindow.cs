using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RoomService;
using AvatarCreation;
using Mirror;

namespace AvatarTablet
{
    public class PeopleWindow : MonoBehaviour
    {
        [Header("UI Reference")]
        public Transform content;
        public Button btn_muteEveryone;
        public Button btn_TurnOffShare;
        public Button btn_RecallEveryone;

        [Header("Arrow Pointer")]
        public GameObject arrowPointer;

        [Header("Script Reference")]
        private TheRoomServices theRoomServices;
        public ShareScreenWindow shareScreenWindow;
        public NetworkPlayerSyncVar netPlayerSyncVar;
        public SimpleObjectPool people_Prefab;


        void Start()
        {
            EnableExtraMenu(false);
            arrowPointer.gameObject.SetActive(false);
            InitAttendanceList();
        }


        private float nextActionTime = 0.0f;
        public float period = 5f;
        void Update()
        {
            if (Time.time > nextActionTime)
            {
                nextActionTime = Time.time + period;
                //Code here
                InitAttendanceList();
            }
        }


        /// <summary>
        /// to enable the Extra Menu -> usually it is used for the admin
        /// </summary>
        /// <param name="_value"></param>
        public void EnableExtraMenu(bool _value)
        {
            btn_muteEveryone.gameObject.SetActive(_value);
            btn_TurnOffShare.gameObject.SetActive(_value);
            btn_RecallEveryone.gameObject.SetActive(_value);

            //the give admin and kick
            foreach(RefPeople _rp in content.GetComponentsInChildren<RefPeople>())
            {
                _rp.toggle_giveAdmin.gameObject.SetActive(_value);
                _rp.btn_kickPeople.gameObject.SetActive(_value);
            }
        }


        /// <summary>
        /// init all the gameobject according to all the people in the room.
        /// </summary>
        public void InitAttendanceList()
        {
            foreach (Transform child in content)
                Destroy(child.gameObject);

            //Get all the NetworkIdentity (the member) in the scenario
            //Get the roomManager
            if(!theRoomServices)
                theRoomServices = (TheRoomServices)FindObjectOfType(typeof(TheRoomServices));

            //Get the playerSyncVar name and assign them
            foreach (NetworkPlayerSyncVar _ps in theRoomServices.NetworkPlayerList)
            {
                
                /*
                //dont create the attendance button for yourself
                if (ni.netId == this.GetComponentInParent<NetworkIdentity>().netId)
                    continue;
                */

                //create the attendance list button
                GameObject attendance = CreatePeopleButton(_ps);
                attendance.transform.SetParent(content, false);
            }
        }

        /// <summary>
        /// function to create the people button 
        /// </summary>
        /// <param name="_ni"></param>
        /// <returns></returns>
        private GameObject CreatePeopleButton(NetworkPlayerSyncVar _ni)
        {
            //get the Object and the reference
            GameObject peopleObj = people_Prefab.GetObject();
            RefPeople refPeople = peopleObj.GetComponent<RefPeople>();

            //assign the name by getting it from the PlayerSyncVARHook
            refPeople.theName.text = _ni.playerName;


            //assign the showLocationButton
            refPeople.toggle_showLoc.onValueChanged.AddListener(delegate
            {
                //put your function to show location here.
                arrowPointer.SetActive(refPeople.toggle_showLoc.isOn);
                arrowPointer.GetComponent<AlwaysPointing>().target = _ni.transform;
            }
            );

            //assign the teleportation
            refPeople.btn_teleport.onClick.AddListener(delegate
            {
                //put your function to teleport here
                Ui_teleportToTheirLocation(_ni.GetComponent<NetworkIdentity>());
            });



            //assign the "MakeAdmin" 
            refPeople.toggle_giveAdmin.onValueChanged.AddListener(delegate
            {
                //put your function to makeAdmin here.
                Ui_MakeAdmin(_ni.GetComponent<NetworkIdentity>(), refPeople.toggle_giveAdmin.isOn);
            });



            //assign the kickoutButton
            refPeople.btn_kickPeople.onClick.AddListener(delegate
                                    {
                                        //put your function to kick here
                                        //Debug.Log("Kick network ID " + ConverterFunction.StringToInt(_attendanceNI.netId.ToString()));
                                        Ui_KickOut(_ni.GetComponent<NetworkIdentity>());
                                    });

            //check if the make admin and kick should be shown
            if (netPlayerSyncVar.adminStatus == (int)GlobalSettings.PlayerRole.admin)
            {
                refPeople.toggle_giveAdmin.gameObject.SetActive(true);
                refPeople.btn_kickPeople.gameObject.SetActive(true);
            }

            return peopleObj;
        }


        /// <summary>
        /// to mute everyone
        /// </summary>
        public void Ui_MuteEveryone()
        {
            theRoomServices.MuteEveryone();
        }


        /// <summary>
        /// to recall everyone to his/her position
        /// </summary>
        public void Ui_RecallEveryone(Transform _parent)
        {
            theRoomServices.RecallEveryone(_parent.position - new Vector3(0.2f, 0, 0.2f));
        }

        public void Ui_teleportToTheirLocation(NetworkIdentity _netId)
        {
            netPlayerSyncVar.transform.position = _netId.transform.position - new Vector3(0.2f, 0, 0.2f);
        }

        public void Ui_MakeAdmin(NetworkIdentity _netId, bool _value)
        {
            theRoomServices.MakeAdmin(_netId, _value);
        }

        /// <summary>
        /// To kick someone out
        /// </summary>
        /// <param name="_ni"></param>
        public void Ui_KickOut(NetworkIdentity _ni)
        {
            theRoomServices.KickSomeone(_ni);
        }

    }
}
