using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using RoomService;
using AvatarCreation;

namespace AvatarTablet
{
    public class RolePlayWindow : NetworkBehaviour
    {
        public Transform RolePlayWindowUI;
        [Header("Ui variable")]
        public Slider reactionSlider;
        public Transform presenterContent;
        public GameObject presenterBG; 
        public TextMeshProUGUI presenterText;
        public Button presenterStopFollowButton;
        public GameObject info_executed;

        [Header("NPC Related")]
        public NpcGroupController npcGroup;

        [Header("Script Reference")]
        public SimpleObjectPool presenter_Prefab;
        public TheRoomServices theRoomService;


        private void Start()
        {
            //npcGroup = (NpcGroupController)FindObjectOfType(typeof(NpcGroupController));
            //disable usless info
            info_executed.SetActive(false);
            presenterBG.SetActive(false);
            presenterText.text = "";
            presenterStopFollowButton.gameObject.SetActive(false);

            InitAvailableTarget();
        }



        // Update is called once per frame
        void Update()
        {

        }



        //Triggered when the Reaction's slider is slided.
        public void Ui_ReactionIsSlided()
        {
            //the CMD Function
            Cmd_Npc_ReactionLevel((int)reactionSlider.value);
            print("Executed " + (int)reactionSlider.value);
        }

        [Command(requiresAuthority = false)]
        void Cmd_Npc_ReactionLevel(int _reaction)
        {
            //Rpc_NpcReactionLevel(_reaction); 
            NpcReaction(_reaction);
        }

        private void NpcReaction(int _reaction)
        {
            if (!npcChecker()) return;
            //change to appropriate reaction
            if (_reaction == 0)
                npcGroup.NoReacting();
            else
                npcGroup.StartReaction(_reaction);
        }






        //To Do Animation of Raising a hands.
        public void Ui_RaiseHands()
        {
            int anim = 0;
            Cmd_RaiseHand(anim);
        }

        [Command(requiresAuthority = false)]
        void Cmd_RaiseHand(int _anim) { Rpc_RaiseHand(_anim); }

        [ClientRpc]
        void Rpc_RaiseHand(int _anim) { RaiseHand(_anim); }

        private void RaiseHand(int _anim)
        {
            //Insert your logic here.
        }




        //for the npc to throw something.
        public void Ui_NpcThrows(bool _goodObj)
        {
            Cmd_NpcThrows(_goodObj);
            StartCoroutine(CoroutineExtensions.HideAfterSeconds(info_executed, 2));
        }

        [Command(requiresAuthority = false)]
        void Cmd_NpcThrows(bool _goodObj) { Rpc_NpcThrows(_goodObj); }

        [ClientRpc]
        void Rpc_NpcThrows(bool _goodObj) { NpcThrows(_goodObj); }

        private void NpcThrows(bool _goodObj)
        {
            //Insert your logic here.
            if (!npcChecker()) return;

            npcGroup.CrowdThrowsGoodObj(_goodObj);
        }






        public void Ui_doorBanging()
        {
            Cmd_doorBanging();
            StartCoroutine(CoroutineExtensions.HideAfterSeconds(info_executed, 2));
        }

        [Command(requiresAuthority = false)]
        void Cmd_doorBanging() { Rpc_doorBanging(); }

        [ClientRpc]
        void Rpc_doorBanging() { DoorBanging(); }

        private void DoorBanging()
        {
            //Insert your logic here.
            if (!npcChecker()) return;

            npcGroup.RandomDoorBanging();
        }


        public void Ui_TelephoneRing()
        {
            Cmd_TelephoneRing();
            StartCoroutine(CoroutineExtensions.HideAfterSeconds(info_executed, 2));
        }

        [Command(requiresAuthority = false)]
        void Cmd_TelephoneRing() { Rpc_TelephoneRing(); }

        [ClientRpc]
        void Rpc_TelephoneRing() { TelephoneRing(); }

        private void TelephoneRing()
        {
            //Insert your logic here.
            if (!npcChecker()) return;

            npcGroup.RandomTelephoneRinging();
        }




        // For cheering
        public void Ui_CheeringOnce(bool _cheer)
        {
            Cmd_NpcCheering(_cheer);
            StartCoroutine(CoroutineExtensions.HideAfterSeconds(info_executed, 2));
        }

        [Command(requiresAuthority = false)]
        void Cmd_NpcCheering(bool _cheer) { Rpc_NpcCheering(_cheer); }

        [ClientRpc]
        void Rpc_NpcCheering(bool _cheer) { NpcCheering(_cheer); }

        private void NpcCheering(bool _cheer)
        {
            //Insert your logic here.
            if (!npcChecker()) return;

            npcGroup.CrowdCheering(_cheer, false);
        }







        //Just a null Reference Checker.
        private bool npcChecker()
        {
            if (!npcGroup)
            {
                Debug.Log("there is no NPc Group found here");
                npcGroup = (NpcGroupController)FindObjectOfType(typeof(NpcGroupController));
                return false;
            }
            return true;
        }



        public void InitAvailableTarget()
        {
            foreach (Transform child in presenterContent)
                Destroy(child.gameObject);

            //Get all the NetworkIdentity (the member) in the scenario
            //Get the roomManager
            theRoomService = (TheRoomServices)FindObjectOfType(typeof(TheRoomServices));
            //Get the playerSyncVar name and assign them
            foreach (NetworkPlayerSyncVar _ps in theRoomService.NetworkPlayerList)
            {
                NetworkIdentity ni = _ps.GetComponent<NetworkIdentity>();

                //dont create the attendance button for yourself
                /*
                if (ni.netId == this.GetComponentInParent<NetworkIdentity>().netId)
                    continue;
                */
                //create the attendance list button
                GameObject attendance = CreatePresenterTargetButton(ni);
                attendance.transform.SetParent(presenterContent, false);
            }
        }


        private GameObject CreatePresenterTargetButton(NetworkIdentity _attendanceNI)
        {
            //get the button
            GameObject presenterTargetButton = presenter_Prefab.GetObject();

            //assign the name by getting it from the PlayerSyncVARHook
            presenterTargetButton.transform
                                 .GetComponentInChildren<TextMeshProUGUI>().text = _attendanceNI.GetComponent<NetworkPlayerSyncVar>().playerName;

            //assign the Forced turn off Screenbutton
            presenterTargetButton.transform
                                 .GetComponentInChildren<Button>().onClick.AddListener(delegate
                                 {
                                 //put your function 
                                 Ui_startFollowing(_attendanceNI);
                                 });


            return presenterTargetButton;
        }

        /// <summary>
        /// the slider to set the total Follower to Follow the Player
        /// </summary>
        /// <param name="_slider"></param>
        public void Ui_SetTotalFollower(Slider _slider)
        {
            npcGroup.CmdRenewingFollower((int)_slider.value);
            //print("Slider value " + (int)_slider.value);
        }



        /// <summary>
        /// To Refresh the Player list that is in the game
        /// </summary>
        public void Ui_refresh_isClicked()
        {
            InitAvailableTarget();
        }

        public void Ui_startFollowing(NetworkIdentity _netId)
        {
            Cmd_FollowPresenter(_netId);
        }

        [Command(requiresAuthority = false)]
        void Cmd_FollowPresenter(NetworkIdentity _netId)
        {
            Rpc_FollowPresenter(_netId);
            //set the npc to follow people.
            if (!_netId)
                npcGroup.UnFollowTarget();

            else
                npcGroup.FollowTarget(_netId);
        }

        [ClientRpc]
        void Rpc_FollowPresenter(NetworkIdentity _netId)
        {
            if (!_netId)
            {
                presenterBG.gameObject.SetActive(false);
                presenterText.text = "";
                presenterStopFollowButton.gameObject.SetActive(false);
            }

            else
            {
                presenterBG.gameObject.SetActive(true);
                presenterText.text = _netId.GetComponent<NetworkPlayerSyncVar>().playerName;
                presenterStopFollowButton.gameObject.SetActive(true);
            }

        }

        public void Ui_stopFollowing()
        {
            Cmd_FollowPresenter(null);
        }

        public void Ui_CloseWindow(GameObject _window)
        {
            _window.SetActive(false);
        }
    }
}
