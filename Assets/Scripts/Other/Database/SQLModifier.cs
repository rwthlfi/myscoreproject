using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;


//this script is responsible for Inserting, Removing and Modifying all data in the database
public static class SQLModifier
{
    public static IEnumerator modifierURL(string _uri, Dictionary<string, string> _dict, System.Action<string> _callback)
    {
        //Create a posting form.
        WWWForm form = new WWWForm();
        for (int i = 0; i < _dict.Count; i++)
        {
            string key = _dict.Keys.ElementAt(i);
            string value = _dict.Values.ElementAt(i);

            form.AddField(key, value);
        }

        //call and send the php script
        using (UnityWebRequest www = UnityWebRequest.Post(_uri, form))
        {
            yield return www.SendWebRequest();

            string result = www.downloadHandler.text;

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Throws error: " + www.error);
            }

            else //connection success
            {
                string errorMsg = SQLChecker.isError(result);
                if (errorMsg != null) //checking if there is any error appearing.
                    _callback("Data Error: " + errorMsg);


                else
                    //return the value back for later usage.
                    _callback(result);
            }
        }


    }

}
