using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System;

public static class SQLloader 
{
    public static IEnumerator LoadURL(string _uri, TextMeshProUGUI _text)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            //store to the text
            _text.text = webRequest.downloadHandler.text;
            //Debug.Log(_uri + " webRequest.downloadHandler.text");

        }
    }


    /// <summary>
    /// To Load all the text in the given URL
    /// </summary>
    /// <param name="uri">the url of the php request</param>
    /// <param name="_callback">return the value</param>
    /// <returns></returns>
    public static IEnumerator LoadURL(string _uri, System.Action<string> _callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log("Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log("HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    //Debug.Log("" + webRequest.downloadHandler.text);
                    break;
            }

            //return the value back for later usage.
            _callback(webRequest.downloadHandler.text);
        }
    }

    /// <summary>
    /// To load the URL with specific keys
    /// </summary>
    /// <param name="_uri">the url of the php request</param>
    /// <param name="_dict">put your string and key value here</param>
    /// <param name="_callback">return the value</param>
    /// <returns></returns>
    public static IEnumerator LoadURL(string _uri, Dictionary<string, string> _dict, System.Action<string> _callback)
    {
        //Create a posting form.
        WWWForm form = new WWWForm();
        for(int i = 0; i < _dict.Count; i++)
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
                Debug.Log("Throws error: " + www.error);

            else //connection success
            {
                string errorMsg = SQLChecker.isError(result);
                if (errorMsg != null) _callback("Data Error: " + errorMsg);
                
                else
                    //return the value back for later usage.
                    _callback(result);
            }
        }
    }


    /// <summary>
    /// To load the URL with specific keys
    /// </summary>
    /// <param name="_uri">the url of the php request</param>
    /// <param name="_dict">put your string and key value here</param>
    /// <param name="_callback">return the value</param>
    /// <returns></returns>
    public static IEnumerator LoadMedia(string _uri, Dictionary<string, string> _dict, System.Action<string> _callback)
    {
        //Create a posting form.
        WWWForm form = new WWWForm();
        for (int i = 0; i < _dict.Count; i++)
        {
            string key = _dict.Keys.ElementAt(i);
            string value = _dict.Values.ElementAt(i);

            form.AddField(key, value);
        }

        string result;
        //call and send the php script in order to receive the link to the media
        using (UnityWebRequest www = UnityWebRequest.Post(_uri, form))
        {
            yield return www.SendWebRequest();

            result = www.downloadHandler.text;

            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log("Throws error: " + www.error);

            else //connection success
            {
                string errorMsg = SQLChecker.isError(result);
                if (errorMsg != null) _callback("Data Error: " + errorMsg);


                else
                {
                    //return the value back for later usage.
                    if (result == "")
                        Debug.Log("No link has been retrieved");
                    else
                        _callback(result);

                }
            }
        }
    }


    /// <summary>
    /// to Load tHumbnail from the given url
    /// </summary>
    /// <param name="_url"></param>
    /// <param name="_image"></param>
    /// <returns></returns>
    public static IEnumerator LoadThumbnailFromWeb(string _url, Image _image)
    {
        //send webrequest according to the given url
        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(_url);
        yield return webRequest.SendWebRequest();

        //get the downloaded asset bundle
        var texture = DownloadHandlerTexture.GetContent(webRequest);

        //create the texture and assin them to the given image.
        _image.overrideSprite = Sprite.Create(texture,
                                              new Rect(0, 0, texture.width, texture.height),
                                              new Vector3(0.5f, 0.5f)
                                              );

    }

    /// <summary>
    /// to Load the image from the webServer
    /// </summary>
    /// <param name="_url">the URL</param>
    /// <param name="_renderer">Where should the picture be renderered</param>
    /// <returns></returns>
    public static IEnumerator LoadImageFromWeb(string _url, Renderer _renderer)
    {
        //send webrequest according to the given url
        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(_url);
        yield return webRequest.SendWebRequest();

        //get the downloaded asset bundle
        var texture = DownloadHandlerTexture.GetContent(webRequest);
        _renderer.gameObject.SetActive(true);
        _renderer.material.SetTexture("_MainTex", texture);
    }


    /// <summary>
    /// To Load the image once and assign them to multiple Renderer
    /// </summary>
    /// <param name="_url"></param>
    /// <param name="_rendererList"></param>
    /// <returns></returns>
    public static IEnumerator LoadImageFromWeb(string _url, List<Renderer> _rendererList)
    {
        //send webrequest according to the given url
        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(_url);
        yield return webRequest.SendWebRequest();

        //get the downloaded asset bundle
        var texture = DownloadHandlerTexture.GetContent(webRequest);

        foreach(Renderer rdr in _rendererList)
        {
            rdr.material.SetTexture("_MainTex", texture);
        }
    }




    /// <summary>
    /// load image from the given Url with callback..
    /// </summary>
    /// <param name="_url">the url to load</param>
    /// <param name="_callback">the Texture2D</param>
    /// <returns></returns>
    public static IEnumerator LoadImageFromWeb(string _url, System.Action<Texture2D> _callback)
    {
        //send webrequest according to the given url
        /*UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(_url);
        yield return webRequest.SendWebRequest();

        //get the downloaded asset bundle
        Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);

        //return the Texture2D as callback
        _callback(texture);
        */
        var result = false;
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(_url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    result = false; break;
                case UnityWebRequest.Result.DataProcessingError:
                    result = false; break;
                case UnityWebRequest.Result.ProtocolError:
                    result = false; break;
                case UnityWebRequest.Result.Success:
                    result = true; break;
            }

            Texture2D texture2D;
            //if the result, resulting in error, then throw a null texture
            if (!result)
                texture2D = null;

            //Otherwise get the downloaded asset bundle
            else
                texture2D = DownloadHandlerTexture.GetContent(webRequest);

            //return the Texture2D as callback
            _callback(texture2D);
        }
    }




    /// <summary>
    /// to load the video from the url. AND PLAY IT!
    /// </summary>
    /// <param name="_url">give the url</param>
    /// <param name="_videoPlayer">the video player holder</param>
    /// <returns></returns>
    public static IEnumerator LoadVideoFromWeb(string _url, VideoPlayer _videoPlayer, System.Action<bool> _callback)
    {
        //_videoPlayer.source = VideoSource.Url;
        
        var result = false;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    result = false; break;
                case UnityWebRequest.Result.DataProcessingError:
                    result = false; break;
                case UnityWebRequest.Result.ProtocolError:
                    result = false; break;
                case UnityWebRequest.Result.Success:
                    result = true; break;
            }


            //return the result
            _callback(result);
        }

        if (result)
        {
            //if the link is ok..., Do something here?

        }

        yield return null;
    }



    /// <summary>
    /// to Load the 3D model from the given URL. with callback.
    /// if the 3D model already exist in the device.., then load the one in the device.
    /// otherwise load it from the given url.
    /// </summary>
    /// <param name="_url"></param>
    /// <param name="_callback"></param>
    /// <returns></returns>
    public static IEnumerator Load3DModelFromWeb(string _url, System.Action<string> _callback)
    {
        //The json content from the given link
        UnityWebRequest jsonReq = UnityWebRequest.Get(_url);
        yield return jsonReq.SendWebRequest();
        string content = jsonReq.downloadHandler.text;

        //save the json content to the persistant data for later usage.
        //1. get the file and format name
        string savePath = string.Format("{0}/{1}", Application.persistentDataPath, GetFileName(_url));
        
        File.WriteAllText(savePath, content);
        Debug.Log("Saved " + savePath);
        _callback(savePath);

    }

    /// <summary>
    /// To check if the link is ok or not
    /// </summary>
    /// <param name="_url">Given Link</param>
    /// <param name="_callback"></param>
    /// <returns></returns>
    public static IEnumerator LoadLinkFromWeb(string _url, System.Action<bool> _callback)
    {
        var result = false;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    result = false; break;
                case UnityWebRequest.Result.DataProcessingError:
                    result = false; break;
                case UnityWebRequest.Result.ProtocolError:
                    result = false; break;
                case UnityWebRequest.Result.Success:
                    result = true; break;
            }


            //return the result
            _callback(result);
        }

        if (result)
        {
            //if the link is ok..., Do something here?

        }

        yield return null;
    }


    /// <summary>
    /// to get the name of the data and its Extensions
    /// </summary>
    /// <param name="_url">url</param>
    /// <returns>example.gltf</returns>
    public static string GetFileName(string _url)
    {
        //get the name of the object by removing the unnneded link, ex. "http://localhost/myscore/generalMedia/avatars/"
        string[] pieces = _url.Split('/');
        string filename = pieces[pieces.Length - 1];

        //Debug.Log("name " + filename); // this will return, ex. "randomAvatar.gltf"
        return filename;
    }

    /// <summary>
    /// to Get the name of the Data and without extensions
    /// </summary>
    /// <param name="_url">given the url</param>
    /// <returns>this will return "picture" not "picture.jpg" </returns>
    public static string GetOnlyFileName(string _url)
    {
        return Path.GetFileNameWithoutExtension(_url);
    }

    /// <summary>
    /// To get the file extensions
    /// </summary>
    /// <param name="_url">url given</param>
    /// <returns>this function will return the extenstion something like *.mp4</returns>
    public static string GetFileExtenstion(string _url)
    {
        //Convert to lower case
        return Path.GetExtension(_url).ToLower();
    }


    /// <summary>
    /// to split the given string using || as seperator
    /// </summary>
    /// <param name="_str">give the strings</param>
    /// <returns></returns>
    public static List<string> splitGivenString(string _str)
    {
        return _str.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries).ToList();
    }


    /// <summary>
    /// To split the string with seperator
    /// </summary>
    /// <param name="_str"></param>
    /// <param name="_seperator"></param>
    /// <returns></returns>
    public static List<string> splitGivenString(string _str, string _seperator)
    {
        return _str.Split(new string[] { _seperator }, StringSplitOptions.RemoveEmptyEntries).ToList();
    }


    /// <summary>
    /// Just a cleaner of the string., cause sometimes in German they use char like & or \
    /// </summary>
    /// <param name="_temp"></param>
    /// <returns></returns>
    public static string stringCleaner(string _temp)
    {
        //Decode the html signs into normal readable human text(Plain Text)s
        string cleanStr = _temp;
        cleanStr = cleanStr.Replace("&lt;", "<");
        cleanStr = cleanStr.Replace("&gt;", ">");
        cleanStr = cleanStr.Replace("&amp;", "&");
        cleanStr = cleanStr.Replace("&quot;", "\"");

        //might called later
        cleanStr = cleanStr.Replace("<br>", "");
        cleanStr = cleanStr.Replace("<br/>", "");
        cleanStr = cleanStr.Replace("<br />", "");
        cleanStr = cleanStr.Replace("\n", "");

        //cleanStr = cleanStr.Replace(" ", "");
        cleanStr = cleanStr.Replace("\n", "");
        cleanStr = cleanStr.Replace("\r", "");
        cleanStr = cleanStr.Replace("\x0A", "");
        cleanStr = cleanStr.Replace("\xA", "");
        

        return cleanStr;
    }

    /// <summary>
    /// To clean only enter stuff
    /// </summary>
    /// <param name="_temp"></param>
    /// <returns></returns>
    public static string stringCleaner_OnlyEnter(string _temp)
    {
        //Decode the html signs into normal readable human text(Plain Text)s
        string cleanStr = _temp;

        //might called later
        cleanStr = cleanStr.Replace("<br>", "");
        cleanStr = cleanStr.Replace("<br/>", "");
        cleanStr = cleanStr.Replace("<br />", "");
        cleanStr = cleanStr.Replace("\n", "");
        cleanStr = cleanStr.Replace("\r", "");


        return cleanStr;
    }

}
