using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using TMPro;
using Mirror;
//using Vuplex.WebView;
using System.Threading.Tasks;
using RoomService;
using AvatarCreation;

public class MediaLinksLoader : NetworkBehaviour
{
    [Header("UI element")]
    public TextMeshProUGUI titleText_0;
    public TextMeshProUGUI titleText_1;
    public Image titleLogo;
    public TextMeshProUGUI currentInfo_Video;
    public TextMeshProUGUI currentInfo_Picture;
    public TextMeshProUGUI currentInfo_Website;
    public TextMeshProUGUI currentController;
    public GameObject currentInfo_UserViewingWebsite;
    public TextMeshProUGUI currentInfo_WebsiteUser;
    public GameObject currentInfo_NoOneViewing;

    public Button leftScrollButton;
    public Button rightScrollButton;

    [Header("File Links")]
    public string fileLink = "https://files.lfi.rwth-aachen.de/myscore/MyScoreAssets/UniExpo/Booth_1/";
    public string fileText_Research = "LinkList_Research.txt";
    public string fileText_Teaching = "LinkList_Teaching.txt";
    public string serverFolder_Media = "Media";
    public string serverFolder_Thumbnail = "Thumbnail";
    public string serverFolder_Website = "Website";
    public string serverFolder_Word = "Word";

    public List<string> linksList = new List<string>();
    public Transform linkContent;
    public List<GameObject> linkButtonsList_Research = new List<GameObject>();
    public List<GameObject> linkButtonsList_Teaching = new List<GameObject>();



    [Header("SyncVar Variable")]
    [SyncVar(hook = nameof(OnLinkChanged))]
    public string currentLink;

    [SyncVar(hook = nameof(OnTopTabChanged))]
    public int currentTopTab; // Research = 0 & Teaching = 1
    public enum topMenuTab { Research, teaching };

    [SyncVar(hook = nameof(OnGivenAuthorityIDChanged))]
    public int authorityIDChanged;


    [SyncVar(hook = nameof(OnScrollViewChanged))]
    public float scrollViewValue;


    [Header("Script Reference")]
    public MediaVideoSync mediaVideoSync;
    public MediaImageSync mediaImageSync;
    public GameObject mediaWebsite;
   // public CanvasWebViewPrefab webviewWebsite;
    public MediaAuthorityTrigger mediaAuthorityTrigger;
    public SimpleObjectPool buttonLink_Prefab;
    public TheRoomServices theRoomService;
    public RadioButton_AddOn radioButton;
    public ScrollViewExtensions scrollViewExt;
    public BoothEnabler boothEnabler;

    private enum ButtonLinkHierarchy
    {
        imageThumbnail,
        buttonShow,
        textPreview
    };

    [NonSerialized] public string videoFormat = ".mp4";
    [NonSerialized] public string imageJpgFormat = ".jpg";
    [NonSerialized] public string textFormat = ".txt";

    void OnLinkChanged(string _Old, string _New)
    {
        if (SQLloader.GetFileExtenstion(_New) == videoFormat)
        {
            //check if it is server
            if(isServer)
                mediaVideoSync.mediaPlane.gameObject.SetActive(true);
            else
            {
                //check if local player already enter the trigger area
                if(boothEnabler.cachePlayer)
                {
                    //if yes activate them. otherwise deactivate to save performance
                    mediaVideoSync.mediaPlane.gameObject.SetActive(true);
                }
                else
                    mediaVideoSync.mediaPlane.gameObject.SetActive(false);
            }

            mediaVideoSync.Ui_ShowVideoMenu(true);
            mediaImageSync.mediaPlane.gameObject.SetActive(false);
            WebsiteEnabled(false);

            //display which media being selected.
            EnableCurrentInfo(currentInfo_Video);
        }

        else if (SQLloader.GetFileExtenstion(_New) == imageJpgFormat)
        {
            mediaVideoSync.mediaPlane.gameObject.SetActive(false);
            mediaVideoSync.Ui_ShowVideoMenu(false);
            mediaImageSync.mediaPlane.gameObject.SetActive(true);
            WebsiteEnabled(false);
            EnableCurrentInfo(currentInfo_Picture);
        }

        else // Website
        {
            mediaVideoSync.mediaPlane.gameObject.SetActive(false);
            mediaVideoSync.Ui_ShowVideoMenu(false);
            mediaImageSync.mediaPlane.gameObject.SetActive(false);
            EnableCurrentInfo(currentInfo_Website);

            WebsiteEnabled(true);
        }
    }

    void OnTopTabChanged(int _Old, int _New)
    {
        Ui_MenuTabChanged((topMenuTab)_New);
    }



    private NetworkIdentity currentAuthority;
    void OnGivenAuthorityIDChanged(int _Old, int _New)
    {

        //Debug.Log("Assign a new authority to Player with ID: " + _New);
        if (_New == mediaAuthorityTrigger.noAuthority) // which mean no one there)
        {
            Debug.Log("no authority");
            currentController.text = "-";
            currentAuthority = null; // deassign the current authority
            EnableMonitorControl(false);
            return; // dont do anything anymore.
        }

        //OTHERWISE! 
        //assign the current Authority
        currentAuthority = theRoomService.getLocalPlayer().netIdentity;

        //show who has Authority in the big panel
        if (_New == currentAuthority.netId)
        {
            currentController.text = currentAuthority.GetComponent<NetworkPlayerSyncVar>().playerName;
        }

        //check if you are the new given authority.
        //if yes -> enable the Control
        if (_New == currentAuthority.netId)
        {
            Debug.Log("enable MontorControl");
            EnableMonitorControl(true);
        }
        //otherwise -> disable the Control
        else
        {
            Debug.Log("disable MontorControl");
            EnableMonitorControl(false);
        }

    }


    //sync the scrollView value when changed
    void OnScrollViewChanged(float _Old, float _New)
    {
        scrollViewExt.Ui_ScrollRectChanged(_New);
    }



    [Server]
    public void ChangeAuthority(int _netID)
    {

        authorityIDChanged = _netID;
        Debug.Log("Change authority to playerID of " + authorityIDChanged);
    }


    // Start is called before the first frame update
    void Start()
    {
        //Init title and Logo
        InitTitle();
        InitLogo();

        //theRoomService = FindObjectOfType<TheRoomServices>();

        //Init the content
        linkButtonsList_Research = InitLinkList(fileText_Research); //fileText_Research -> LinkList_Research.txt
        linkButtonsList_Teaching = InitLinkList(fileText_Teaching); //fileText_Research -> LinkList_Teaching.txt

        //activated the trigerring function only in the Server.
        if (isServer)
        {
            mediaAuthorityTrigger.isAllow = true;
            print("I am server So I will allow the media Authority Trigger");
        }

        if (isClient)
        {
            //just a small checking
            //StartCoroutine(JustStartChecking(3f));

        }

    }


    // Update is called once per frame
    void Update()
    {
    }


    private void InitTitle()
    {
        StartCoroutine(SQLloader.LoadURL(fileLink + "Title.txt", returnValue =>
        {
            //clean up the string
            var strClean = SQLloader.stringCleaner(returnValue);

            //store the link in the list
            string[] a = strClean.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

            if (a.Length < 2) // to avoid index out of Range
                return;

            titleText_0.text = a[0];
            titleText_1.text = a[1];
        }));
    }


    private void InitLogo()
    {
        //assign Button thumbnail
        StartCoroutine(SQLloader.LoadThumbnailFromWeb(fileLink + "Title.jpg",
                                                      titleLogo));

    }



    //init all the link 
    private List<GameObject> InitLinkList(string _extraURL)
    {
        //clean up the child in the content
        /*
        foreach (Transform child in linkContent)
            Destroy(child.gameObject);
        */


        var tempList = new List<GameObject>();

        //Get the button URL
        StartCoroutine(SQLloader.LoadURL(fileLink + _extraURL, returnValue =>
        {
            //clean up the string
            var strClean = SQLloader.stringCleaner(returnValue);

            //store the link in the list
            linksList = strClean.Split(new string[] { "||" }, StringSplitOptions.None).ToList();

            //Remove empty list
            linksList = linksList.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

            //create the button Link with its thumbnail and put them in the content
            foreach (string str in linksList)
            {
                //get the file name and determine if its *.mp4 or *.jpg
                if (SQLloader.GetFileExtenstion(str) == videoFormat)
                {
                    GameObject videoButton = CreateVideoLinkButton(fileLink + serverFolder_Media + "/" + str);
                    videoButton.transform.SetParent(linkContent, false);
                    tempList.Add(videoButton);
                }

                else if (SQLloader.GetFileExtenstion(str) == imageJpgFormat)
                {
                    GameObject imageButton = CreateImageLinkButton(fileLink + serverFolder_Media + "/" + str);
                    imageButton.transform.SetParent(linkContent, false);
                    tempList.Add(imageButton);
                }

                else // -> Webseite
                {
                    //print("str " + str); // return website1 and website2
                    GameObject webButton = CreateWebButton(fileLink + serverFolder_Website + "/" + str);
                    webButton.transform.SetParent(linkContent, false);
                    tempList.Add(webButton);
                    //print("Create Website");
                }

            }

            //recalculate the steps
            scrollViewExt.RecalculateSteps();

            //disable all control
            EnableMonitorControl(false);

            //change the menu accordingly
            Ui_MenuTabChanged((topMenuTab)currentTopTab);


            //only in server
            if (isServer)
                Server_SyncTheFirstLink();

        }));


        //return the list of all the button
        return tempList;
    }






    /// <summary>
    /// function to create a video button with Link
    /// </summary>
    /// <param name="_link"></param>
    /// <returns></returns>
    private GameObject CreateVideoLinkButton(string _link)
    {
        //get the button
        GameObject linkButton = buttonLink_Prefab.GetObject();

        //Assign button ability
        linkButton.GetComponent<ButtonMediaRWTHHierarchy>().buttonShow.onClick.AddListener(delegate
        {
            print(_link);
            UpdateLink(_link);
            mediaVideoSync.Ui_ChangeNewVideo(_link);
            mediaVideoSync.Ui_PlayVideo();
        });


        //get file name
        var a = SQLloader.GetOnlyFileName(_link);

        //assign Button thumbnail
        StartCoroutine(SQLloader.LoadThumbnailFromWeb(fileLink + serverFolder_Thumbnail + "/" + a + imageJpgFormat,
                              linkButton.GetComponent<ButtonMediaRWTHHierarchy>().imageThumbnail));

        //assign Preview Words
        StartCoroutine(SQLloader.LoadURL(fileLink + serverFolder_Word + "/" + a + textFormat,
                              linkButton.GetComponent<ButtonMediaRWTHHierarchy>().textPreviewWord));

        //just for caching the link
        //might come in handy though
        linkButton.GetComponent<ButtonMediaRWTHHierarchy>().textFullLink.text = _link;

        //return the link
        return linkButton;
    }


    /// <summary>
    /// function to create an image button with Link
    /// </summary>
    /// <param name="_link"></param>
    /// <returns></returns>
    private GameObject CreateImageLinkButton(string _link)
    {
        //get the button
        GameObject linkButton = buttonLink_Prefab.GetObject();

        //Assign button ability
        linkButton.GetComponent<ButtonMediaRWTHHierarchy>().buttonShow.onClick.AddListener(delegate
        {
            print(_link);
            UpdateLink(_link);
            mediaImageSync.Ui_ChangeNewImage(_link);
        });

        //get file name
        var a = SQLloader.GetOnlyFileName(_link);

        //assign Button thumbnail
        StartCoroutine(SQLloader.LoadThumbnailFromWeb(fileLink + serverFolder_Thumbnail + "/" + a + imageJpgFormat,
                              linkButton.GetComponent<ButtonMediaRWTHHierarchy>().imageThumbnail));

        //assign Preview Words
        StartCoroutine(SQLloader.LoadURL(fileLink + serverFolder_Word + "/" + a + textFormat,
                              linkButton.GetComponent<ButtonMediaRWTHHierarchy>().textPreviewWord));

        //just for caching the link
        //might come in handy though
        linkButton.GetComponent<ButtonMediaRWTHHierarchy>().textFullLink.text = _link;

        //return the link
        return linkButton;
    }


    /// <summary>
    /// to Create the Web button
    /// </summary>
    /// <param name="_link"></param>
    /// <returns></returns>
    private GameObject CreateWebButton(string _link)
    {
        //_link -> https://files.lfi.rwth-aachen.de/myscore/MyScoreAssets/UniExpo/Booth_1/Website/Website1
        //get the button
        GameObject linkButton = buttonLink_Prefab.GetObject();


        //Assign button ability
        linkButton.GetComponent<ButtonMediaRWTHHierarchy>().buttonShow.onClick.AddListener(delegate
        {
            StartCoroutine(SQLloader.LoadURL(_link + textFormat, returnValue =>
            {
                print("returnValue " + returnValue + " Link " + _link);
                UpdateLink(returnValue);
                StartCoroutine(Ui_WebsiteShow(returnValue)); // currently it just locally.

            }));
            //need to get the content of website1.txt
            //Change the link for the webseite

        });

        /*
        //assign Button thumbnail
        StartCoroutine(SQLloader.LoadThumbnailFromWeb(fileLink + serverFolder_Thumbnail + "/" + a + imageJpgFormat,
                              linkButton.GetComponent<ButtonMediaRWTHHierarchy>().imageThumbnail));
        
        */

        //assign Preview Words
        StartCoroutine(SQLloader.LoadURL(fileLink + serverFolder_Word + "/"
                                                  + SQLloader.GetOnlyFileName(_link)
                                                  + textFormat,

                                                  linkButton.GetComponent<ButtonMediaRWTHHierarchy>().textPreviewWord));


        //assign the text preview
        linkButton.GetComponent<ButtonMediaRWTHHierarchy>().textPreviewWord.text = _link;

        //just for caching the link
        //might come in handy though
        linkButton.GetComponent<ButtonMediaRWTHHierarchy>().textFullLink.text = _link;
        //return the link
        return linkButton;
    }



    /// <summary>
    /// To Update the link for the synchronization.
    /// </summary>
    /// <param name="_link"></param>
    private void UpdateLink(string _link)
    {
        CmdSyncLink(_link);
    }

    [Command(requiresAuthority = false)]
    public void CmdSyncLink(string _link)
    {
        currentLink = _link;
    }

    [Server]
    private void Server_SyncTheFirstLink()
    {

        string currentFormat = "";

        //get the current menuTab
        if (currentTopTab == (int)topMenuTab.Research)
        {
            if (linkButtonsList_Research.Count == 0)
                return;

            //get the format from the link store in the Button game object
            currentFormat = SQLloader.GetFileExtenstion(linkButtonsList_Research[0]
                                     .GetComponent<ButtonMediaRWTHHierarchy>().textFullLink.text);

            currentLink = linkButtonsList_Research[0].GetComponent<ButtonMediaRWTHHierarchy>().textFullLink.text;
        }

        else if (currentTopTab == (int)topMenuTab.teaching)
        {
            if (linkButtonsList_Teaching.Count == 0)
                return;

            //get the format from the link store in the Button game object
            currentFormat = SQLloader.GetFileExtenstion(linkButtonsList_Teaching[0]
                                     .GetComponent<ButtonMediaRWTHHierarchy>().textFullLink.text);

            currentLink = linkButtonsList_Teaching[0].GetComponent<ButtonMediaRWTHHierarchy>().textFullLink.text;
        }

        if (currentFormat == "")
            print("no extension exist");

        //update the link according to the format file
        if (currentFormat == videoFormat)
        {
            mediaVideoSync.videoLink = currentLink;
            mediaVideoSync.Server_PlayVideo(true);
            StartCoroutine(mediaVideoSync.waitPreparingVideo());
        }

        else if (currentFormat == imageJpgFormat)
            mediaImageSync.imageLink = currentLink;

        else //its a website
        {
            WebsiteEnabled(true);
            StartCoroutine(Ui_WebsiteShow(linkButtonsList_Teaching[0].GetComponent<ButtonMediaRWTHHierarchy>().textFullLink.text));
        }
    }


    public void SyncTheScrollViewValue()
    {
        if (isServer)
            return;
        if (!theRoomService.theLocalPlayer)
            return;
        //check if you have authority // if not, dont do anything
        if (authorityIDChanged != theRoomService.getLocalPlayer().netId)
            return;

        CmdSyncScrollViewValue(scrollViewExt.scrollbar.value);
    }

    [Command(requiresAuthority = false)]
    public void CmdSyncScrollViewValue(float _value)
    {
        scrollViewValue = _value;
    }

    /// <summary>
    /// to enable or disable the monitor control 
    /// </summary>
    /// <param name="_value"></param>
    public void EnableMonitorControl(bool _value)
    {
        //dont do anything if he is not a client
        /*
        if (!isClient)
            return;
        */

        //disable the Top menu
        foreach (Button btn in radioButton.buttonList)
            btn.interactable = _value;

        //en/di-sable the buttons to change media in the link content
        foreach (Button btn in linkContent.GetComponentsInChildren<Button>(true))
            btn.interactable = _value;

        //en/di-sable the buttons and slider to control video 
        mediaVideoSync.buttonPlay.interactable = _value;
        mediaVideoSync.buttonPause.interactable = _value;
        mediaVideoSync.buttonMute.interactable = _value;
        mediaVideoSync.buttonUnmute.interactable = _value;
        mediaVideoSync.mediaVideoSlider.interactable = _value;

        //refresh the scrollbar value
        scrollViewExt.scrollbar.Set(scrollViewValue, false);

        //en/di-sable the scrollView
        scrollViewExt.scrollRect.horizontal = _value;
        scrollViewExt.scrollbar.interactable = _value;
        leftScrollButton.interactable = _value;
        rightScrollButton.interactable = _value;


    }



    /// <summary>
    /// To enable the current info according to the file format given
    /// </summary>
    /// <param name="_text"></param>
    private void EnableCurrentInfo(TextMeshProUGUI _text)
    {
        currentInfo_Video.gameObject.SetActive(false);
        currentInfo_Picture.gameObject.SetActive(false);
        currentInfo_Website.gameObject.SetActive(false);

        _text.gameObject.SetActive(true);
    }


    public void Ui_TopMenu_Research()
    {
        Ui_MenuTabChanged(topMenuTab.Research);
        //sent value to server
        CmdSyncTopMenu((int)topMenuTab.Research);
    }

    public void Ui_TopMenu_Teaching()
    {

        Ui_MenuTabChanged(topMenuTab.teaching);
        //sent value to server
        CmdSyncTopMenu((int)topMenuTab.teaching);
    }

    /// <summary>
    /// to change the menu
    /// </summary>
    /// <param name="_menuTab"></param>
    private void Ui_MenuTabChanged(topMenuTab _menuTab)
    {
        if (_menuTab == topMenuTab.Research)
        {
            radioButton.ChangeColor(radioButton.buttonList[0]);
            foreach (GameObject go in linkButtonsList_Research) go.SetActive(true);
            foreach (GameObject go in linkButtonsList_Teaching) go.SetActive(false);
        }

        else if (_menuTab == topMenuTab.teaching)
        {
            radioButton.ChangeColor(radioButton.buttonList[1]);
            foreach (GameObject go in linkButtonsList_Research) go.SetActive(false);
            foreach (GameObject go in linkButtonsList_Teaching) go.SetActive(true);
        }

        //recalculate the steps
        scrollViewExt.RecalculateSteps();
    }

    [Command(requiresAuthority = false)]
    public void CmdSyncTopMenu(int _menuID)
    {
        currentTopTab = _menuID;
    }


    private void WebsiteEnabled(bool _enable)
    {
        //dont do anything in the server
        if (isServer)
            return;


        if (_enable && theRoomService.theLocalPlayer) // if enabling && the local player exist
        {
            //Disable all the info message First
            currentInfo_UserViewingWebsite.SetActive(false);
            currentInfo_NoOneViewing.SetActive(false);


            //check if you have authority // if not, show info message and thats it
            if (authorityIDChanged != theRoomService.theLocalPlayer.netId)
            {
                if (authorityIDChanged == mediaAuthorityTrigger.noAuthority) // which means no one is controlling them
                    currentInfo_NoOneViewing.SetActive(true); // show the info that no one is viewing

                else
                {
                    //show info message.
                    //en/di-sable the website
                    mediaWebsite.SetActive(true);
                    currentInfo_UserViewingWebsite.SetActive(true);
                    currentInfo_WebsiteUser.text = currentController.text;
                    return;
                }

            }
            else // but disable the info message if you are the user
                currentInfo_UserViewingWebsite.SetActive(false);
        }

        // in case of disable... well., no special case... just disable it

        //en/di-sable the website
        mediaWebsite.SetActive(_enable);

    }



    public IEnumerator Ui_WebsiteShow(string _link)
    {

        yield return new WaitForSeconds(1f);
        /*
        webviewWebsite.enabled = true;

        //Disable all the info message First
        currentInfo_UserViewingWebsite.SetActive(false);
        currentInfo_NoOneViewing.SetActive(false);

        yield return new WaitForSeconds(1f);

        //Load the url directly
        webviewWebsite.InitialUrl = _link;
        webviewWebsite.WebView.LoadUrl(_link);*/
    }


    private IEnumerator JustStartChecking(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        if (SQLloader.GetFileExtenstion(currentLink) == videoFormat)
        {
            mediaVideoSync.mediaPlane.gameObject.SetActive(true);
            mediaVideoSync.Ui_ShowVideoMenu(true);
            mediaImageSync.mediaPlane.gameObject.SetActive(false);
            WebsiteEnabled(false);

            //display which media being selected.
            EnableCurrentInfo(currentInfo_Video);
        }

        else if (SQLloader.GetFileExtenstion(currentLink) == imageJpgFormat)
        {
            mediaVideoSync.mediaPlane.gameObject.SetActive(false);
            mediaVideoSync.Ui_ShowVideoMenu(false);
            mediaImageSync.mediaPlane.gameObject.SetActive(true);
            WebsiteEnabled(false);
            EnableCurrentInfo(currentInfo_Picture);
        }

        else // Website
        {
            mediaVideoSync.mediaPlane.gameObject.SetActive(false);
            mediaVideoSync.Ui_ShowVideoMenu(false);
            mediaImageSync.mediaPlane.gameObject.SetActive(false);
            EnableCurrentInfo(currentInfo_Website);

            WebsiteEnabled(true);
        }
    }

}
