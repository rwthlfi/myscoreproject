using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomInfoMessage : MonoBehaviour
{
    [Header("Info Texts")]
    public GameObject warning_emptyUsernameText;
    public GameObject info_tryToConnectText;
    public GameObject error_ConnectText;
    public GameObject info_CreatingGameText;
    public GameObject error_FailedCreatingGameText;
    public GameObject error_ExpireDateIsIncorrect;
    public GameObject error_ExpireDateIsinThePast;
    public GameObject info_roomCreationIsSent;
    public GameObject info_roomIsCreated;

    public void ShowInfo(GameObject _Info)
    {
        warning_emptyUsernameText.SetActive(false);
        info_tryToConnectText.SetActive(false);
        error_ConnectText.SetActive(false);
        info_CreatingGameText.SetActive(false);
        error_FailedCreatingGameText.SetActive(false);
        error_ExpireDateIsIncorrect.SetActive(false);
        error_ExpireDateIsinThePast.SetActive(false);
        info_roomCreationIsSent.SetActive(false);
        info_roomIsCreated.SetActive(false);

        if (_Info == null)
            return;

        StartCoroutine(CoroutineExtensions.HideAfterSeconds(_Info.gameObject, 5f));
    }

}
