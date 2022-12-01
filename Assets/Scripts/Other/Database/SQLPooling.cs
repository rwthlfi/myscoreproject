using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class SQLPooling : MonoBehaviour
{
    //To place all the ui content
    public Transform UIContent;

    [Header("Script References")]
    public SimpleObjectPool sopButton_Prefabs;
    private enum sopHierarchy { thumbnail, text }; // make this hierarchy the same like the sopbutton_prefab's Child order


    // Start is called before the first frame update
    void Start()
    {
        //string encrypt = EncryptionUtil.Encrypt("http://localhost/myscore/generalmedia/pic360/citybeach.jpg");
        //Debug.Log("Encrypt " + encrypt);
        SetupAllLink();
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetupAllLink()
    {
        //Get all the Logo first.
        //set up the form to be posted
        Dictionary<string, string> formDict = new Dictionary<string, string>()
        {
            {SQLurlConfig.dbPass_Post, SQLurlConfig.password},
            {SQLurlConfig.mediaTable_post, SQLurlConfig.tableLogo }
        };


        //load specific data
        StartCoroutine(SQLloader.LoadURL(SQLurlConfig.mediaLoadAllLinksPHP, formDict, returnValue =>
        {
            //print(returnValue); // this will return the "callback"

            //Split the return value
            string[] linkList = returnValue.Split('|');
            for(int i = 0; i < linkList.Length -1; i++)
            {
                //Debug.Log("Link: " + linkList[i]);
                //Create button with an encryption
                CreateSopButton(EncryptionUtil.Encrypt(linkList[i])).transform.SetParent(UIContent, false);
            }


        }));


    }





    //function to create the bookmarked button in the website
    private GameObject CreateSopButton(string _encryptedlink)
    {
        //decrypt the link first
        string decrypt = EncryptionUtil.Decrypt(_encryptedlink);
        string[] parts = decrypt.Split('/');
        string itemThumbnail = parts[parts.Length - 1].Split('.')[0] + "_thumbnail.jpg"; // take the address and the name + "_thumbnail.jpg"
        string itemName = parts[parts.Length - 1];
        string linkAddress = decrypt.Replace(itemName, "");


        Debug.Log("Decrypt: " + decrypt);
        /*
        Debug.Log("linkAddress:" + linkAddress);
        Debug.Log("linkThumbnail: " + itemThumbnail);
        Debug.Log("linkName: " + itemName);
        */

        //get the button
        GameObject sopButton = sopButton_Prefabs.GetObject();

        //assign the thumbnail
        Image img = sopButton.transform.GetChild((int)sopHierarchy.thumbnail)
                             .GetComponent<Image>();

        StartCoroutine(SQLloader.LoadThumbnailFromWeb(linkAddress + itemThumbnail, img));


        //assign the text name
        Text txt = sopButton.transform.GetChild((int)sopHierarchy.text)
                                      .GetComponent<Text>();
        txt.text = itemName.Split('.')[0]; //to remove the data format(e.x. *.jpg or *.png)



        //assign button capabilites
        Button btn = sopButton.GetComponent<Button>();
        btn.onClick.AddListener(delegate
        {
            //assign the button capabilites
            //Send the encrypted link address to the server.
            //the server will then send still-the encrypted link to the client,
            //Which later the clients will decrypt it by themselves.
            Ui_SendLink_isClicked(_encryptedlink);
        });

        return sopButton;
    }

    public void Ui_SendLink_isClicked(string _encryptedLink)
    {
        Cmd_Button_isClicked(_encryptedLink);
    }

    //[Command]
    void Cmd_Button_isClicked(string _encryptedLink)
    {
        //ChangeLogo(_logoID);
        Rpc_LogoChanged(_encryptedLink);
    }


    //change the value on the client Side
    //[ClientRpc]
    void Rpc_LogoChanged(string _encryptedLink)
    {
        //put your logic here.
        ChangeLogo(_encryptedLink);
    }

    private void ChangeLogo(string _encryptedLink)
    {
        //Decrypt the link
        Debug.Log("Link " + EncryptionUtil.Decrypt(_encryptedLink));
        string decrypt = EncryptionUtil.Decrypt(_encryptedLink);


        //insert defined data
        StartCoroutine(SQLloader.LoadImageFromWeb(decrypt, returnedTexture2D =>
        {
            /*
            //assign all Logo
            foreach (LogoContainerReference lcr in logoRef)
            {
                //get the image
                lcr.GetComponent<Renderer>().material.mainTexture = returnedTexture2D;
            }
            */

        }));

    }
}
