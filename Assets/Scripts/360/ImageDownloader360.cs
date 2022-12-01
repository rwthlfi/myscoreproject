using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Threading;

public class ImageDownloader360 : MonoBehaviour
{

    private string imageFolderName, imageDownloadPath, activeSceneName, path;
    private int index = 0;
    public List<string> uRLImageList = new List<string>();
    public List<Enter360Sphere> uRLGameobject = new List<Enter360Sphere>();
    public Texture[] image360textures;
    private byte[] bytes;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.2f);

        activeSceneName = SceneManager.GetActiveScene().name;

        if (GlobalSettings.DeviceType() == GlobalSettings.Device.Android)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/DownloadedMedia/" + activeSceneName);
            imageFolderName = Application.persistentDataPath + "/DownloadedMedia/" + activeSceneName;
        }
        else
        {
            Directory.CreateDirectory(Application.dataPath + "/Resources/DownloadedMedia/" + activeSceneName);
            imageFolderName = Application.dataPath + "/Resources/DownloadedMedia/" + activeSceneName;
        }

        imageDownloadPath = "DownloadedMedia/" + activeSceneName;

        //get all sphere URLs in child gameObjects first and add to the URL list
        Transform[] childObject360 = GetComponentsInChildren<Transform>();
        foreach (Transform child in childObject360)
        {
            if (child.GetComponent<Enter360Sphere>() != null)
            {
                uRLImageList.Add(child.GetComponent<Enter360Sphere>().uRL);
                uRLGameobject.Add(child.GetComponent<Enter360Sphere>());
            }
        }

        var urlLenght = uRLImageList.Count;
        image360textures = new Texture[urlLenght];

        StartCoroutine(LoadImageFromWeb(uRLImageList[index]));
    }

    public IEnumerator LoadImageFromWeb(string _url)
    {
        yield return new WaitForSeconds(.2f);

        // check if file has beed downloaded, if not download
        if (!(File.Exists(imageFolderName + "/" + SQLloader.GetOnlyFileName(uRLImageList[index]))) && GlobalSettings.DeviceType() == GlobalSettings.Device.Android || !(File.Exists(imageFolderName + "/" + SQLloader.GetFileName(uRLImageList[index]))))
        {
            UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(_url);
            yield return webRequest.SendWebRequest();

            //get the downloaded asset bundle
            image360textures[index] = DownloadHandlerTexture.GetContent(webRequest);

            // sets the texture after the download, will not be used when the texture is already downloaded
            uRLGameobject[index].GetComponent<Enter360Sphere>().SetNewTextureOnlineSource((Texture2D)image360textures[index]);
        }

        yield return new WaitForSeconds(.2f);

        StartCoroutine(Save360(image360textures[index]));
    }

    private IEnumerator Save360(Texture _tex)
    {
        yield return new WaitForSeconds(.2f);

        // check if file has beed downloaded, if not continue with saving the download
        //if (!(File.Exists(imageFolderName + "/" + SQLloader.GetOnlyFileName(uRLImageList[index]))))
        if (!(File.Exists(imageFolderName + "/" + SQLloader.GetOnlyFileName(uRLImageList[index]))) && GlobalSettings.DeviceType() == GlobalSettings.Device.Android || !(File.Exists(imageFolderName + "/" + SQLloader.GetFileName(uRLImageList[index]))))
        {
            Texture2D _texture = (Texture2D)image360textures[index];

            bytes = _texture.EncodeToPNG();

            if (GlobalSettings.DeviceType() == GlobalSettings.Device.Android)
            {
                string urlfilename = SQLloader.GetOnlyFileName(uRLImageList[index]);
                path = Application.persistentDataPath + "/DownloadedMedia/" + activeSceneName + "/" + urlfilename;
            }
            else
            {
                string urlfilename = SQLloader.GetFileName(uRLImageList[index]);
                path = Application.dataPath + "/Resources/DownloadedMedia/" + activeSceneName + "/" + urlfilename;
            }

            string filename_current = path;

            File.WriteAllBytes(filename_current, bytes);
        }
        else
        {
            // gets the gameobject from the URL and sets the texture appropriatly
            string textureName = SQLloader.GetOnlyFileName(uRLImageList[index]);
            uRLGameobject[index].SetNewTexture(imageDownloadPath, textureName);
        }

        yield return new WaitForSeconds(.2f);

        //counts up the list index in case the file is not downloaded
        if (index < uRLImageList.Count - 1)
        {
            index++;

            StartCoroutine(LoadImageFromWeb(uRLImageList[index]));
        }
    }
}