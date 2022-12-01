using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoomService;
using AvatarCreation;
using Mirror;
using UnityEngine.UI;

namespace AvatarTablet
{
    //It is used to manage all the item in the scenario
    public class ItemManagerWindow : MonoBehaviour
    {
        public int maxAllowedItem = 10;

        [Header("UI Reference")]
        public Transform content;
        public GameObject info_noGameObjectfound;
        public Button button_DeleteAllItemInScene;
        public Toggle toggle_showAllItems;   

        [Header("ArrowPointer")]
        public GameObject arrowPointer;

        [Header("Info Error")]
        public GameObject info_MaxItemReached;

        [Header("Script references")]
        private TheRoomServices theRoomServices;
        public SimpleObjectPool item_prefab;



        private void Start()
        {
            //disable the button that doesnt belong for admin
            EnableExtraMenu(false);
        }


        private float nextActionTime = 0.0f;
        public float period = 5f;
        void Update()
        {
            if (Time.time > nextActionTime)
            {
                nextActionTime = Time.time + period;
                //Code here
                InitItemsList();
            }
        }

        /// <summary>
        /// init all the gameobject according to the item in the room
        /// </summary>
        public void InitItemsList()
        {
            foreach (Transform child in content)
                Destroy(child.gameObject);


            //Get the roomManager
            if (!theRoomServices)
                theRoomServices = (TheRoomServices)FindObjectOfType(typeof(TheRoomServices));


            //check if it is empty or not //-> if yes then show no gameObject found.
            if (theRoomServices.netObjList.Length <= 0)
            {
                info_noGameObjectfound.SetActive(true);
                return;
            }
            else
                info_noGameObjectfound.SetActive(false);


            //Get the NetObj reference  and spawned it in the UI
            foreach (NetObjReference _netObj in theRoomServices.netObjList)
            {

                //check if the object belongs to you
                //if yes -> check if the toggle is on
                //otherwise -> create the ui

                //print("_netobj " + _netObj.ownerID + " localID " + theRoomServices.theLocalPlayer.playerUniqueID + deleteLater);

                //if it is belong to you,
                if (_netObj.ownerID == theRoomServices.theLocalPlayer.playerUniqueID)
                {
                    //create the ui
                    GameObject items = CreateItemUi(_netObj);
                    
                    //set to parent if the items still exist
                    if (items)
                        items.transform.SetParent(content, false);

                }

                //BUTT if it is not belong to you
                else
                {
                    //if yes -> check if the toggle is on,
                    //NOTE -> Default client will always have the toggle as off.
                    if(toggle_showAllItems.isOn)
                    {
                        //create the ui
                        GameObject items = CreateItemUi(_netObj);

                        //set to parent if the items still exist
                        if (items)
                            items.transform.SetParent(content, false);
                    }

                }


            }
        }



        /// <summary>
        /// To create the Item UI
        /// </summary>
        /// <param name="_netObj"></param>
        /// <returns></returns>
        private GameObject CreateItemUi(NetObjReference _netObj)
        {
            //get the Object and the reference
            GameObject itemObj = item_prefab.GetObject();
            RefItem refItem = itemObj.GetComponent<RefItem>();

            if (_netObj == null)
                return null;

            //assign the name 
            refItem.itemName.text = _netObj.name.Replace("(Clone)", "");

            //assign the owner
            refItem.itemOwner.text = theRoomServices.getPlayerName(_netObj.ownerID);
            refItem.ownerID = _netObj.ownerID;

            //assign the showLocationButton
            refItem.buttonShow.onClick.AddListener(delegate
            {
                //put your function to show location here.
                ActivateArrow(true);
                arrowPointer.GetComponent<AlwaysPointing>().target = _netObj.transform;
            }
            );

            //assign the delete button
            refItem.buttonDelete.onClick.AddListener(delegate
                                {
                                    //if you are deleting your own, you dont need to do anything
                                    //otherwise if you delete other players object, send him the news.
                                    if (refItem.ownerID != theRoomServices.theLocalPlayer.playerUniqueID)
                                    {
                                        theRoomServices.Show_MessageItemRemoved(theRoomServices.getPlayerNetIdentity(refItem.ownerID));
                                    }

                                    
                                    theRoomServices.Ui_DeSpawnedNetworkObject(_netObj.GetComponent<NetworkIdentity>());
                                });



            //return the gameObject
            return itemObj;
        }


        /// <summary>
        /// to activate or deactivate the arrow pointer
        /// </summary>
        /// <param name="_value"></param>
        public void ActivateArrow(bool _value)
        {
            arrowPointer.SetActive(_value);
        }


        /// <summary>
        /// Attached to the UI -> to show/Hide all the items
        /// </summary>
        /// <param name="_toggle"></param>
        public void Ui_Toggle_ShowAll(Toggle _toggle)
        {
            foreach (RefItem _refItem in content.GetComponentsInChildren<RefItem>(true))
            {
                _refItem.gameObject.SetActive(_toggle.isOn);
            }
        }

        /// <summary>
        /// To Delete all of the item that belongs to you.
        /// </summary>
        public void DeleteAllYourItem()
        {
            foreach (RefItem _refItem in content.GetComponentsInChildren<RefItem>())
            {
                //check if item belongs to you
                if (_refItem.ownerID == theRoomServices.theLocalPlayer.playerUniqueID)
                {
                    // Trigger the button delete. Therefore this item will be deleted
                    _refItem.buttonDelete.onClick.Invoke();
                }
            }
        }


        /// <summary>
        /// to delete all items in the scenario
        /// </summary>
        public void DeleteAllItems()
        {
            foreach (RefItem _refItem in content.GetComponentsInChildren<RefItem>(true))
            {
                // Trigger the button delete. Therefore this item will be deleted
                _refItem.buttonDelete.onClick.Invoke();
            }
        }



        /// <summary>
        /// to enable the Extra Menu -> usually it is used for the admin
        /// </summary>
        /// <param name="_value"></param>
        public void EnableExtraMenu(bool _value)
        {
            button_DeleteAllItemInScene.gameObject.SetActive(_value);
            toggle_showAllItems.gameObject.SetActive(_value);

            //if true -> which means most likely you are the admin
            if (_value)
            {
                foreach (RefItem _refItem in content.GetComponentsInChildren<RefItem>(true))
                {
                    if (toggle_showAllItems.isOn)
                    {
                        _refItem.gameObject.SetActive(true);
                    }

                    else // if not showing all items -> 
                    {
                        //check if you are the owner
                        if (_refItem.ownerID == theRoomServices.theLocalPlayer.playerUniqueID)
                        {
                            _refItem.gameObject.SetActive(true);
                        } // if not the owner, then dont show it
                        else
                            _refItem.gameObject.SetActive(false);
                    }
                }
            }
        }


        /// <summary>
        /// To check if the item reached maximum allowed or not.
        /// </summary>
        /// <returns></returns>
        public bool isMaximum()
        {
            if (getItemCount() >= maxAllowedItem)
                return true;
            else
                return false;
        }


        /// <summary>
        /// To get the item Count before spawning
        /// </summary>
        public int getItemCount()
        {
            int i = 0;

            //Get the roomManager
            if (!theRoomServices)
                theRoomServices = (TheRoomServices)FindObjectOfType(typeof(TheRoomServices));

            foreach (NetObjReference netItem in FindObjectsOfType<NetObjReference>())
            {
                if (netItem.ownerID == theRoomServices.theLocalPlayer.playerUniqueID)
                {
                    i++;
                } 
            }
            return i;
        }
    }

}