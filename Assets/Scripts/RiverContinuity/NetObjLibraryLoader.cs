using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using RoomService;

//This is to spawn the predetermined network object
//therefore It will always use the [0] position of the networking object
public class NetObjLibraryLoader : NetworkBehaviour
{
    //this is the networkObject that has been registerd to the server.
    //it is an empty network without any Mesh.
    //however once you put the Mesh as a child of this gameObject, the box collider will expand automatically
    //which then, you are allowed to grab the object with the XR Rig.
    //Of course this GameObject is synchronizeable automatically to every client.
    public GameObject registeredNetObj;
    //put the pre determined gameObject in here.
    public List<GameObject> netObjList = new List<GameObject>();
    public Transform spawnPos;


    public string extraAtrribute = ""; // set this in the Editor, the list of extra attribute, you can see it in the GlobalSettings.cs, just search for "Net object attribute"

    [Header("Spawned Limit Reference")]
    public TheRoomServices theRoomService = null;
    [Tooltip("this will include the network object that has been set up in the Scene. therefore increase this max value if your have some pre-set object in the scene.")]
    public int maxNetObj = 20; // 
    public GameObject info_MaxLimitReach;

    /// <summary>
    /// use to spawned the network object by given the id position in the netObjList.
    /// Its has extra attribute that you can defined in the editor. the list of defined attribute is in the GlobalSettings.cs
    /// </summary>
    /// <param name="_listId">the ID position from the netObjList</param>
    public void Ui_SpawnedNetObj_ExtraAttribute(int _listId)
    {
        Cmd_SpawnedNetworkObj(_listId, extraAtrribute);
    }


    /// <summary>
    /// use to spawned the network object by given the id position in the netObjList.
    /// BUT With limitation to check
    /// Its has extra attribute that you can defined in the editor. the list of defined attribute is in the GlobalSettings.cs
    /// </summary>
    /// <param name="_listId">the ID position from the netObjList</param>
    public void Ui_SpawnedNetObj_Limitation(int _listId)
    {
        //refresh the netobject count
        //theRoomService.RefreshNetObjCount();
        //check if it reach the maximum length
        if (theRoomService.netObjList.Length > 20)
        {
            //show maxinfo reached
            StartCoroutine(CoroutineExtensions.HideAfterSeconds(info_MaxLimitReach, 5f));

            //dont do anything
            return;
        }

        //if everything ok then spawned the netobject
        Cmd_SpawnedNetworkObj(_listId, extraAtrribute);
    }

    //Client sending the command to server to spawn the networked object.
    [Command(requiresAuthority = false)]
    void Cmd_SpawnedNetworkObj(int _listId, string _extraAtrribute)
    {
        SpawnedNetworkObj(_listId, _extraAtrribute);
    }


    //the logic that needs to be executed
    private void SpawnedNetworkObj(int _listId, string _attribute)
    {
        //spawn the empty network object
        var netObj = Instantiate(registeredNetObj, spawnPos.position, spawnPos.rotation);

        var netRb = netObj.GetComponent<Rigidbody>();

        //instantiate the GameObject as a child
        var objInList = Instantiate(netObjList[_listId]);

        //change layer
        objInList.layer = GlobalSettings.layerGrabbable;

        // -- this setup only happened in the SERVER -- //
        //set it as a child of the netobjList and zeroing pos and rot
        objInList.transform.SetParent(netObj.transform, false);
        objInList.transform.localPosition = Vector3.zero;
        objInList.transform.localRotation = Quaternion.Euler(Vector3.zero);


        // zeroing the physic first.
        netRb.velocity = Vector3.zero;
        netRb.angularVelocity = Vector3.zero;

        //set the physic of the Parents! Remember the child is only for Visualization
        //Defined extra setting for the given attribute.
        // - Gravity
        if (_attribute.Contains(GlobalSettings.netObj_att_useGravity))
        {
            netRb.useGravity = true;
            print("use gravity");
        }

        // - Kinematic
        if (_attribute.Contains(GlobalSettings.netObj_att_isKinematic))
        {
            netRb.isKinematic = true;
            print("use Kinematic");
        }

        // - Wandering Fish?
        if (_attribute.Contains(GlobalSettings.netObj_att_isWandering))
        {
            var wandering = netObj.AddComponent<WanderFish>();
            wandering.enabled = true;
            wandering.rb = netRb;
            wandering.allowWander = false;
            print("use Wanderfish");
        }

        //Adjust the collider to encapsulate the given model
        netObj.GetComponent<AutoMeshCollider>().AutoAdjustCollider(GlobalSettings.layerGrabbable);


        //spawn the network object to all clients
        NetworkServer.Spawn(netObj);

        // -- End Setup Server -- //

        Server_ChangeNetObjID(netObj.GetComponent<NetObjReference>(), _listId, _attribute);

        // DONT FORGET TO SET IT UP IN THE NetObjReference TOO 
        // therefore client will have the object too
        
        //print("spawned " + netObjList[_listId].name);
    }




    //let the server changes the object id. and its physical attribute
    //this id and attribute will be sync to other client
    [Server]
    private void Server_ChangeNetObjID(NetObjReference _netObj, int _ID, string _attribute)
    {
        _netObj.objID = _ID;
        //_netObj.physicAttribute = _attribute;
    }

    /// <summary>
    /// to freeze the button for x seconds
    /// </summary>
    /// <param name="_seconds"></param>
    public void Ui_FreezeButtonFor(int _seconds)
    {
        var button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        StartCoroutine(CoroutineExtensions.InteractableButtonAfterSeconds(button, _seconds));
    }

}
