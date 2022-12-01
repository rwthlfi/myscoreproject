using UnityEngine;
using RoomService;

public class ResetOnlineScenario : MonoBehaviour
{

    public int scenarioID;
    public TheRoomServices theRoomServices;

    public void Ui_ChangeScenario(int scenarioID)
    {
        if (!theRoomServices)
            theRoomServices = (TheRoomServices)FindObjectOfType(typeof(TheRoomServices));

        StartCoroutine(theRoomServices.ChangeScene(scenarioID));
    }
}