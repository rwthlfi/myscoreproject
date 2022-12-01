using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class VersionManager : MonoBehaviour
{
    private string serverVersion = "";
    public TextMeshProUGUI versionText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://files.lfi.rwth-aachen.de/myscore/MyScoreAssets/version.txt");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            //Debug.Log(www.downloadHandler.text);

            string content = www.downloadHandler.text;
            //split the content
            string[] contentArray = content.Split('|');

            serverVersion = contentArray[0];
            if (serverVersion != GlobalSettings.appVersion)
            {
                versionText.gameObject.SetActive(true);
                //versionText.text = contentArray[1];
                print("asdf " + contentArray[0] + contentArray[1]);
            }

            // Or retrieve results as binary data
            
            //byte[] results = www.downloadHandler.data;
        }
    }
}
