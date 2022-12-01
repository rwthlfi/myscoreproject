using UnityEngine;
using Mirror;


//it is a reference to find the Netobj easily
//attach this object to the "NetObj" gameObject
public class NetObjReference : NetworkBehaviour
{
    private NetObjLibraryLoader netObjLibraryLoader;
    private GameObject objInList;
    private Rigidbody objInList_RigidBody;
    private AutoMeshCollider autoMeshCollider;


    [Header("SyncVar Variable")]
    [SyncVar(hook = nameof(OnObjLinkChanged))]
    public string objlink; // this is used if the object is loaded from the database

    [SyncVar(hook = nameof(OnObjIDChanged))]
    public int objID;

    [SyncVar(hook = nameof(OnOwnerIdChanged))]
    public string ownerID;


    [SyncVar(hook = nameof(OnPhysicAttribute))]
    public string physicAttribute; // "Gravity" use Gravity || "Kinematic" -> use kinematic




    private void Start()
    {

    }

    void OnObjLinkChanged(string _Old, string _New)
    {
        //change Link
        //playerNameText.text = link;
        //do Here something with the newly changed Link
        //load an object perhaps
    }

    void OnObjIDChanged(int _Old, int _New)
    {
        //Load the Object from the Library
        //print("objId changed: Old = " + _Old + " ,new= " + _New);
        SetupNetObjectMesh(_New);
    }


    void OnOwnerIdChanged(string _Old, string _New)
    {
        //Do nothing. this is just needed because the Owner will have a limit on spawning thing.
        //print("Change owner ID " + _New);
    }


    //do we need this. ?
    void OnPhysicAttribute(string _Old, string _New)
    {
        //change the physic attribute
        //print("objPhysic Changed: Old = " + _Old + " ,new= " + _New);
        SetupNetObjPhysic(_New);
    }

    //player info sent to server, then server updates sync vars which handles it on all clients

    [Command(requiresAuthority = false)]
    public void CmdSetupObjLink(string _newLink) { objlink = _newLink; }

    [Command(requiresAuthority = false)]
    public void CmdSetupObjID(int _id) { objID = _id; }

    [Command(requiresAuthority = false)]
    public void CmdOwnerIDChanged(string _id) { ownerID = _id; }

    [Command(requiresAuthority = false)]
    public void CmdSetupPhysicAttribute(string _att) { physicAttribute = _att; }


    /// <summary>
    /// To set the owner ID from somewhere else which will avoid "no active client" error
    /// </summary>
    /// <param name="_str"></param>
    [Server]
    public void Server_SetOwnerID(string _str)
    {
        ownerID = _str;
    }

    

    private void SetupNetObjectMesh(int _listId)
    {
        //find the net obj
        netObjLibraryLoader = (NetObjLibraryLoader)FindObjectOfType<NetObjLibraryLoader>();

        //instantiate the GameObject as a child
        objInList = Instantiate(netObjLibraryLoader.netObjList[_listId]);

        //Change the object name
        this.name = netObjLibraryLoader.netObjList[_listId].name;

        //set it as a child of the netobjList and zeroing pos and rot
        objInList.transform.SetParent(this.transform, false);
        objInList.transform.localPosition = Vector3.zero;
        objInList.transform.localRotation = Quaternion.Euler(Vector3.zero);
        
        //Adjust the collider to encapsulate the given model
        autoMeshCollider = objInList.GetComponentInParent<AutoMeshCollider>();
        autoMeshCollider.AutoAdjustCollider(GlobalSettings.layerGrabbable);
        

        //change layer
        objInList.layer = GlobalSettings.layerGrabbable;


        //get the rigidbody in the parents. (Remember the child is only for visualization.
        objInList_RigidBody = objInList.GetComponentInParent<Rigidbody>();
        //disable physics, to avoid conflict with the Game Server physic attribute.
        objInList_RigidBody.isKinematic = true;

        

    }


    //Do we need this ?
    private void SetupNetObjPhysic(string _physicAttribute)
    {
        //get the rigidbody in the parents. (Remember the child is only for visualization.
        objInList_RigidBody = objInList.GetComponentInParent<Rigidbody>();
        
        //disable all physic first
        /*
        objInList_RigidBody.useGravity = false;
        objInList_RigidBody.isKinematic = false;
        */

        objInList_RigidBody.velocity = Vector3.zero;
        objInList_RigidBody.angularVelocity = Vector3.zero;


        //set the physic according to the attribute.
        if (_physicAttribute.Contains(GlobalSettings.netObj_att_useGravity))
            objInList_RigidBody.useGravity = true;
        
        if (_physicAttribute.Contains(GlobalSettings.netObj_att_isKinematic))
            objInList_RigidBody.isKinematic = true;

            
    }

    
}
