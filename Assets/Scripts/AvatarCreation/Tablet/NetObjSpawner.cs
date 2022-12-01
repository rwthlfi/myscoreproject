using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using AvatarCreation;

namespace AvatarTablet
{
    //This is to spawn network object 
    public class NetObjSpawner : NetworkBehaviour
    {
        //list of the object
        public List<GameObject> networkObjList;
        public Transform posRef;

        //the Player unique ID -> which is used to assign them to the network object
        public string playerUniqueID = "";

        [Header("Script Reference")]
        [Tooltip("You can leave this empty if you are not spawning object using 'Ui_SpawnedNetworkObject' function")]
        public NetworkPlayerSyncVar networkPlayerSyncVar;
        public ItemManagerWindow itemManagerWindow;

        // Start is called before the first frame update
        void Start()
        {
            if (isServer)
                return;

            playerUniqueID = PlayerPrefs.GetString(PrefDataList.savedCreatorID);
        }


        /// <summary>
        /// attach this to the UI to spawned object
        /// </summary>
        /// <param name="_listId"></param>
        public void Ui_SpawnedNetworkObject(int _listId)
        {
            //check if you are NOT the admin and Items already maximum, Dont spawn anything
            if (networkPlayerSyncVar.adminStatus != (int)GlobalSettings.PlayerRole.admin &&
                itemManagerWindow.isMaximum())
            {
                //show the warning message
                StartCoroutine(CoroutineExtensions.HideAfterSeconds(itemManagerWindow.info_MaxItemReached, 5f));
                //dont do anything
                return;
            }


            print("Total Item " + itemManagerWindow.getItemCount());
            Cmd_SpawnedNetworkObj(_listId, playerUniqueID, posRef.position, posRef.rotation);
            //print("spawn -> " + playerUniqueID);
        }




        [Command(requiresAuthority = false)]
        void Cmd_SpawnedNetworkObj(int _listId, string _ownerID, Vector3 _pos, Quaternion _rot)
        {
            SpawnedNetworkObj(_listId, _ownerID, _pos, _rot);
        }



        /// <summary>
        /// spawn the network object according to the item in the list
        /// </summary>
        /// <param name="_listId"> the id of the object</param>
        private void SpawnedNetworkObj(int _listId, string _ownerID, Vector3 _pos, Quaternion _rot)
        {
            //Your Logic here.
            GameObject netObj = Instantiate(networkObjList[_listId], _pos, _rot);

            //set the name
            netObj.name = networkObjList[_listId].gameObject.name;

            //Rest the velocity
            netObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            NetworkServer.Spawn(netObj);

            netObj.GetComponent<NetObjReference>().Server_SetOwnerID(_ownerID);
            //set the owner id of the object
            //StartCoroutine(assignOwnerId(netObj.GetComponent<NetworkIdentity>(), _ownerID));
        }


        private IEnumerator assignOwnerId(NetworkIdentity _netID, string _ownerID)
        {
            yield return new WaitForSeconds(5);
            _netID.GetComponent<NetObjReference>().Server_SetOwnerID(_ownerID);
        }
    }
}
