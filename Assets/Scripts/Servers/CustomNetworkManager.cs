using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using RoomService;

public class CustomNetworkManager : NetworkManager
{
    public GameObject info_Disconnected;
    public LoadingScreenManager lsm;

    //Variable for loading
    [System.NonSerialized] public bool sceneloaded = false;




    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        //will be called after the clientChangeScene
        base.OnClientSceneChanged(conn);

        EnableContinue();
        sceneloaded = true;
        //wait for a sec

        //enable the xr rig
        //enableXR();
    }


    private void EnableContinue()
    {
        if (!lsm)
            lsm = FindObjectOfType<LoadingScreenManager>();
        if (lsm)
        {
            print("cannot found the loading screen. Do you need to assign it bro?");
            lsm.EnableContinueButton();
        }
            
        
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        
        base.OnServerDisconnect(conn);
        // Remove authority to players that disconnect the match so they don't disappear
        List<NetworkIdentity> networkIds = new List<NetworkIdentity>(conn.clientOwnedObjects);

        foreach (NetworkIdentity netId in networkIds)
        {
            netId.RemoveClientAuthority();
        }
        

    }
	
	//To show a warning message when the client got disconnected
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        SceneManager.LoadScene(GlobalSettings.WelcomeScene);
        print("disconnected due to inactivity.");
        StartCoroutine(show(7));
    }

    
    private IEnumerator show(int _sec)
    {
        if(info_Disconnected)
        {
            info_Disconnected.gameObject.SetActive(true);
            yield return new WaitForSeconds(_sec);
            info_Disconnected.gameObject.SetActive(false);
        }

        else
            yield return null;
        
    }
    

}
