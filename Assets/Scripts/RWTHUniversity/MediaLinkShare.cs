using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class MediaLinkShare : MonoBehaviour
{
    [Header("Current Link")]
    public TMP_InputField emailField;
    public string currentViewedLink;

    [Header("Info gameObject")]
    public GameObject info_succesfullySent;
    public GameObject info_errorNotSent;
    public TextMeshProUGUI email_Subject;
    public TextMeshProUGUI email_Opening;
    public TextMeshProUGUI email_Closing;

    //cache variable
    private List<string> cacheKey;

    [Header("Script Reference")]
    public MediaLinksLoader mediaLinkLoader;

    private void Start()
    {
        //assign the email if there is one in the cache
        if(PlayerPrefs.GetString(PrefDataList.cacheEmail) != "")
        {
            emailField.text = PlayerPrefs.GetString(PrefDataList.cacheEmail);
        }
        info_succesfullySent.SetActive(false);
        info_errorNotSent.SetActive(false);

        
        /*
        string a = EncryptionUtil.Encrypt("myscore@lfi.rwth-aachen.de||smarthost-tls.rwth-aachen.de||25||");
        print("val " + a);

        StartCoroutine(SQLloader.LoadURL("https://files.lfi.rwth-aachen.de/myscore/MyScoreAssets/UniExpo/fileSchluessel.txt", returnValue =>
        {
            a = EncryptionUtil.Decrypt(returnValue);

            cacheKey = SQLloader.splitGivenString(EncryptionUtil.Decrypt(returnValue));
            string from = cacheKey[0];
            //string host = "a";
            //int port = 25;
            string host = cacheKey[1];
            int port = ConverterFunction.StringToInt(cacheKey[2]);
            print("f " + from + " host " + host + " port " + port);
        }));
        */
    }


    public void ShareLinkToEmail()
    {
        currentViewedLink = mediaLinkLoader.currentLink;

        //load key data
        StartCoroutine(SQLloader.LoadURL("https://files.lfi.rwth-aachen.de/myscore/MyScoreAssets/UniExpo/fileSchluessel.txt", returnValue => 
        {
            //split the given string
            cacheKey = SQLloader.splitGivenString(EncryptionUtil.Decrypt(returnValue));

            print(EncryptionUtil.Decrypt(returnValue));
            if(MailRelay.SendEmail_NoPass(cacheKey, emailField.text,
                                          email_Subject.text,
                                          email_Opening.text + currentViewedLink + email_Closing.text)
              )

            {
                //Show success message
                StartCoroutine(CoroutineExtensions.HideAfterSeconds(info_succesfullySent, 3f));
            }
            else //show error message
                StartCoroutine(CoroutineExtensions.HideAfterSeconds(info_errorNotSent, 3f));

            SaveTheEmail();
        }));
    }

    //save the email
    private void SaveTheEmail()
    {
        PlayerPrefs.SetString(PrefDataList.cacheEmail, emailField.text);
    }


    /// <summary>
    /// To Check if there is email saved in the preferences. if yes use it.
    /// attach this to the inputField OnSelect
    /// </summary>
    public void Ui_checkEmail()
    {
        //assign the email if there is one in the cache
        if (PlayerPrefs.GetString(PrefDataList.cacheEmail) != "")
        {
            emailField.text = PlayerPrefs.GetString(PrefDataList.cacheEmail);
        }
    }
}
