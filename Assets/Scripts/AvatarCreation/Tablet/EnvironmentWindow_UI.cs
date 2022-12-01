using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using TMPro;


namespace AvatarTablet
{
    public class EnvironmentWindow_UI : MonoBehaviour
    {
        [Header("Surrounding Variable")]
        public string surLinkRoot;
        public string surLinkContent;

        [Header("Logo Variable")]
        public string logoLinkRoot;
        public string logoLinkContent;


        [Header("Ui Reference")]
        public Transform contentSur;
        public Transform contentLogo;


        [Header("Script Reference")]
        public EnvironmentWindow environmentWindow;
        public SimpleObjectPool objPool;


        // Start is called before the first frame update
        void Start()
        {
            foreach (Transform child in contentSur)
                Destroy(child.gameObject);
            InitList(surLinkRoot, surLinkContent, false);


            //init the logo
            foreach (Transform child in contentLogo)
                Destroy(child.gameObject);
            InitList(logoLinkRoot, logoLinkContent, true);

        }



        private void Update()
        {
        }


        /// <summary>
        /// to initialize the button with its link
        /// </summary>
        /// <param name="_link"></param>
        /// <param name="_extra"></param>
        /// <param name="_renderer"></param>
        private void InitList(string _link, string _extra, bool _isLogo)
        {
            var tempList = new List<string>();
            StartCoroutine(SQLloader.LoadURL(_link + _extra, returnValue =>
            {
                //clean up the string
                var strClean = SQLloader.stringCleaner(returnValue);

                //store the link in the list
                tempList = strClean.Split(new string[] { "||" }, StringSplitOptions.None).ToList();

                //Remove empty list
                tempList = tempList.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();


                //Check if it is a logo
                if (_isLogo)
                {
                    //create buttons for logo
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        GameObject btn = CreateLogoButton(_link + tempList[i]);
                        btn.transform.SetParent(contentLogo, false);
                    }
                }

                else
                {
                    //create buttons for the surrounding
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        GameObject btn = CreateSurButton(_link + tempList[i]);
                        btn.transform.SetParent(contentSur, false);
                    }
                }

            }));

        }


        /// <summary>
        /// create the button to change link logo in the server.
        /// </summary>
        /// <param name="_link"></param>
        /// <returns></returns>
        private GameObject CreateLogoButton(string _link)
        {
            //get the button
            GameObject linkButton = objPool.GetObject();

            //get file name
            var fileName = SQLloader.GetOnlyFileName(_link);
            //print("filename " + fileName);

            // assign button ability
            linkButton.GetComponent<Button>().onClick.AddListener(delegate
            {
                print(_link);
                //Change the Sync Value in the Environment Window
                environmentWindow.CmdChangeLogo(_link);

            });

            //assign text name
            linkButton.GetComponentInChildren<TextMeshProUGUI>().text = fileName;


            return linkButton;
        }


        /// <summary>
        /// create the button to change Surrounding in the server.
        /// </summary>
        /// <param name="_link"></param>
        /// <returns></returns>
        private GameObject CreateSurButton(string _link)
        {
            //get the button
            GameObject linkButton = objPool.GetObject();

            //get file name
            var fileName = SQLloader.GetOnlyFileName(_link);
            //print("filename " + fileName);

            // assign button ability
            linkButton.GetComponent<Button>().onClick.AddListener(delegate
            {
                print(_link);
                //Change the Sync Value in the Environment Window
                environmentWindow.CmdChangeSur(_link);

            });

            //assign text name
            linkButton.GetComponentInChildren<TextMeshProUGUI>().text = fileName;


            return linkButton;
        }

        public void Ui_CloseWindow(GameObject _window)
        {
            _window.SetActive(false);
        }
    }
}
