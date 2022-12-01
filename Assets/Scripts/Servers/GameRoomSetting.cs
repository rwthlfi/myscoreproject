using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRoomSetting : MonoBehaviour
{
    //GameRoom Variables ->Pulled from the GameRegistration
    public string GameScene;
    public string NetworkAddress;
    public ushort NetworkPort;
    public string UniqueID;
    public string RoomName;
    public string RoomCreator;
    public string CreatorID;
    public string RoomPassword;
    public int RoomMaxPlayers;
    public string RoomExpireDate;


    private  IEnumerator Start()
    {
        yield return new WaitForSeconds(5f);

        Debug.Log("GameScene: " + SceneManager.GetActiveScene().name);
        //Debug.Log("NetworkAddress: " + NetworkAddress);
        //Debug.Log("NetworkPort: " + NetworkPort);
        //Debug.Log("UniqueID: " + UniqueID);
        Debug.Log("RoomName: " + RoomName);
        Debug.Log("RoomCreator: " + RoomCreator);
        Debug.Log("CreatorID: " + CreatorID);
        Debug.Log("RoomPassword: " + RoomPassword);
        Debug.Log("RoomMaxPlayers: " + RoomMaxPlayers);
        Debug.Log("RoomExpireDate: " + RoomExpireDate);
    }
}
