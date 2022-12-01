using UnityEngine;
//using Vuplex.WebView;
using TMPro;
using UnityEngine.UI;

namespace AvatarTablet
{
    public class WebviewWindow : MonoBehaviour
    {
       // public CanvasWebViewPrefab _webViewPrefab;
        public TMP_InputField urlInputField;
        public TextMeshProUGUI progress;

        [Header("Bookmarked Window")]
        public Transform bookmarkedWindow;
        public Transform bookmarkedContent;
        public SimpleObjectPool bookmarkedButton_prefab;

        //Scaling
        private Vector3 canvasWebViewFullSize = new Vector3(1, 1, 1);
        private Vector3 canvasWebViewFullPos = new Vector3(0, 0, 0);
        private Vector3 canvasWebViewHalfSize = new Vector3(0.75f, 1, 1);
        private Vector3 canvasWebViewHalfPos = new Vector3(-110, 0, 0);

        //private bool startProgressTracking = false;

        private enum bookmarkedButtonHierarchy
        {
            bgImage = 0,
            bookmarkedButton = 1,
            deleteButton = 2
        };

        // Start is called before the first frame update
        void Start()
        {
            //default to make it as inactive
            bookmarkedWindow.gameObject.SetActive(false);
            //StartCoroutine(waitLoading());
        }

        /*
        // Update is called once per frame
        void Update()
        {
            _webViewPrefab.WebView.LoadProgressChanged += WebView_LoadProgressChanged;
        }



        //----------- WebView Menu Function--------//

        private void WebView_LoadProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            float value = e.Progress;
            progress.text = (value * 100).ToString() + " %";
            urlInputField.text = _webViewPrefab.WebView.Url;
        }

        public void ButtonBack_isClicked()
        {
            _webViewPrefab.WebView.GoBack();
            urlInputField.text = _webViewPrefab.WebView.Url;
        }

        public void ButtonForward_isClicked()
        {
            _webViewPrefab.WebView.GoForward();
            urlInputField.text = _webViewPrefab.WebView.Url;
        }

        public void ButtonZoomOut_isClicked()
        {
            _webViewPrefab.WebView.ZoomOut();
        }

        public void ButtonZoomIn_isClicked()
        {
            _webViewPrefab.WebView.ZoomIn();
        }

        public void ButtonRefresh_isClicked()
        {
            _webViewPrefab.WebView.Reload();
            urlInputField.text = _webViewPrefab.WebView.Url;
        }

        public void ButtonEnter_isClicked()
        {
            _webViewPrefab.WebView.LoadUrl(urlInputField.text);
            urlInputField.text = _webViewPrefab.WebView.Url;
        }
        */

        //this is to show the bookmarked pages in the website
        public void ShowBookmarked()
        {
            //if the bookmarked window is active, then deactivate them
            if (bookmarkedWindow.gameObject.activeSelf)
            {
                bookmarkedWindow.gameObject.SetActive(false);
                //make the browser full screen again
                //_webViewPrefab.transform.localPosition = canvasWebViewFullPos;
                //_webViewPrefab.transform.localScale = canvasWebViewFullSize;

            }

            else
            {
                bookmarkedWindow.gameObject.SetActive(true);

                //resize the browser to halfsize.
                //_webViewPrefab.transform.localPosition = canvasWebViewHalfPos;
                //_webViewPrefab.transform.localScale = canvasWebViewHalfSize;


                //initialize all the Bookmarked buttons
                InitAllBookmark();
            }
        }
        
        // BookMarked Function // 

        private void InitAllBookmark()
        {
            //destroy all the children in the content page.
            foreach (Transform child in bookmarkedContent)
                Destroy(child.gameObject);

            //get the current saved bookmarked string.
            string bookmarked = PlayerPrefs.GetString(PrefDataList.bookmarkedWebsite);

            //seperate the link and store them in array
            string[] contentsDetail = bookmarked.Split('|');

            //spawn the prefab button.
            //the i equal to 1, is because we are ignoring the empty field at the begginning.
            //it just an empty player preferences
            for (int i = 1; i < contentsDetail.Length; i++)
            {
                //create the bookmarked widget.
                GameObject bookmarkedButton = CreateBookmarkButton(contentsDetail[i]);

                //put it on the content panel pages
                bookmarkedButton.transform.SetParent(bookmarkedContent, false);
            }

            //everyone happy.
        }

        //function to create the bookmarked button in the website
        private GameObject CreateBookmarkButton(string _bookmarkedLink)
        {
            //Debug.Log("Created Bookmark " + _bookmarkedLink);
            //decrypt to normal human readable text
            string decrypt = EncryptionUtil.Decrypt(_bookmarkedLink);

            //get the prefabbutton
            GameObject bookmarkedButton = bookmarkedButton_prefab.GetObject();
            //Assign the Text
            bookmarkedButton.GetComponentInChildren<TextMeshProUGUI>().text = decrypt;

            //assign the link button ability
            bookmarkedButton.transform.GetChild((int)bookmarkedButtonHierarchy.bookmarkedButton)
                                       .GetComponent<Button>().onClick.AddListener(delegate
                                       {
                                           SetBookmarkedLink(decrypt);
                                       }
                                       );

            //assign the delete button ability
            bookmarkedButton.transform.GetChild((int)bookmarkedButtonHierarchy.deleteButton)
                                       .GetComponent<Button>().onClick.AddListener(delegate
                                       {
                                           DeleteBookmarked(_bookmarkedLink);
                                           Destroy(bookmarkedButton);//delete the button after the user click the delet button
                                   }
                                       );

            return bookmarkedButton;
        }


        //bookmarked tab for the Website
        public void BookmarkCurrentLink_isClicked()
        {
            //if the url input is empty.
            //the do nothing
            if (urlInputField.text == "")
                return;

            //get the current saved bookmarked string.
            string bookmarked = PlayerPrefs.GetString(PrefDataList.bookmarkedWebsite);

            //get the current url
            string currentURL = EncryptionUtil.Encrypt(urlInputField.text);
            Debug.Log("asdf " + currentURL);

            //add the string to the saved bookmarked
            string updatedBookmarked = bookmarked + "|" + currentURL;

            //save them to the player prefabs
            PlayerPrefs.SetString(PrefDataList.bookmarkedWebsite, updatedBookmarked);

            InitAllBookmark();
        }

        private void SetBookmarkedLink(string _link)
        {
            //Debug.Log("current URL " + _link);
            //get the link browser
            urlInputField.text = _link;
            //direct the page
            //ButtonEnter_isClicked();
        }

        //this function is to delete the bookmarked pages
        public void DeleteBookmarked(string _linkText)
        {
            //Debug.Log("d  " + _linkText);

            //get the current saved bookmarked string.
            string bookmarked = PlayerPrefs.GetString(PrefDataList.bookmarkedWebsite);

            //delete the saved link
            string newBookmarks = bookmarked.Replace("|" + _linkText, "");
            PlayerPrefs.SetString(PrefDataList.bookmarkedWebsite, newBookmarks);

        }


    }
}
