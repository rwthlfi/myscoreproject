using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Video;

namespace AvatarTablet
{
    public class EnvironmentWindow : NetworkBehaviour
    {
        [Header("Sky Variable")]
        [SyncVar(hook = nameof(OnSurLinkChanged))]
        public string currentSurLink;
        public List<Renderer> surRends;

        [Header("Logo Variable")]
        [SyncVar(hook = nameof(OnLogoLinkChanged))]
        public string currentLogoLink;
        public List<Renderer> logoRends;

        public GameObject environment_Ui;

        private void Start()
        {
            if(environment_Ui)
                environment_Ui.SetActive(false);
        }

        //For when the Sky Changes
        void OnSurLinkChanged(string _Old, string _New)
        {
            if (isServer)
                return;

            if (SQLloader.GetOnlyFileName(_New) == GlobalSettings.formatNone)
                foreach (Renderer rdr in surRends)
                    rdr.gameObject.SetActive(false);


            //Check if the link is video 
            else if (SQLloader.GetFileExtenstion(_New) == GlobalSettings.formatVideo)
            {
                foreach (Renderer rdr in surRends)
                {
                    var vp = rdr.GetComponent<VideoPlayer>();
                    vp.source = VideoSource.Url;
                    vp.url = _New;
                    vp.enabled = true;
                    vp.gameObject.SetActive(true);
                    vp.Play();
                }
            }

            //or image
            else if (SQLloader.GetFileExtenstion(_New) == GlobalSettings.formatImage)
            {
                foreach (Renderer rdr in surRends)
                {
                    var vp = rdr.GetComponent<VideoPlayer>();
                    vp.Stop();
                    vp.enabled = false;
                    vp.gameObject.SetActive(true);
                    StartCoroutine(SQLloader.LoadImageFromWeb(_New, rdr));
                }
            }
        }

        //change the url from client's input to the server
        [Command(requiresAuthority = false)]
        public void CmdChangeSur(string _link)
        {
            currentSurLink = _link;
        }




        //Change Logo when the link is changed
        void OnLogoLinkChanged(string _Old, string _New)
        {
            if (isServer)
                return;

            //give the ability to get the link and change logo on the renderer
            if (SQLloader.GetOnlyFileName(_New) == GlobalSettings.formatNone)
                foreach (Renderer rdr in logoRends)
                    rdr.gameObject.SetActive(false);
            else
            {
                foreach (Renderer rdr in logoRends)
                    rdr.gameObject.SetActive(true);
                StartCoroutine(SQLloader.LoadImageFromWeb(_New, logoRends));
            }
            
        }

        //change the url from client's input to the server
        [Command(requiresAuthority = false)]
        public void CmdChangeLogo(string _link)
        {
            currentLogoLink = _link;
        }

    }
}
