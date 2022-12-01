using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System;

public class MediaVideoSync : NetworkBehaviour
{
    [Header("Target GameObject")]
    public Transform mediaPlane;
    public VideoPlayer mediaVideoPlayer;
    public AudioSource mediaAudioSource;
    public GameObject videosMenu;
    public Button buttonPlay;
    public Button buttonPause;
    public Button buttonUnmute;
    public Button buttonMute;
    public Slider mediaVideoSlider;
    //public TextMeshProUGUI currentVideoText;
    public TextMeshProUGUI timelineText;
    private double maxTimeLine;

    [Header("SyncVar Variable")]
    [SyncVar(hook = nameof(OnVideoLinkChanged))]
    public string videoLink;

    [SyncVar(hook = nameof(OnTimeStamp))]
    public double timeLineDouble;
    public TimeSpan timeLineTS = new TimeSpan(0, 0, 0);

    [SyncVar(hook = nameof(OnMediaIsPlay))]
    public bool mediaIsPlayed = true;

    [SyncVar(hook = nameof(OnAudioIsMute))]
    public bool audioIsMute = false;

    [SyncVar(hook = nameof(OnMediaStatus))]
    public int mediaStatus = 0;
    public enum mediaStatusEnum { doingNothing, currentlyPreparing, errorNoLink };

    [Header("Info Text")]
    public TextMeshProUGUI info_pleaseWait;
    public TextMeshProUGUI error_LinkNotExist;




    void OnVideoLinkChanged(string _Old, string _New)
    {
        StartCoroutine(waitPreparingVideo());

    }

    void OnTimeStamp(double _Old, double _New)
    {
    }

    void OnMediaIsPlay(bool _Old, bool _New)
    {
        buttonPause.gameObject.SetActive(_New);
        buttonPlay.gameObject.SetActive(!_New);

    }

    void OnAudioIsMute(bool _Old, bool _New)
    {
        buttonUnmute.gameObject.SetActive(_New);
        buttonMute.gameObject.SetActive(!_New);

    }

    //to show the status of whats happening for the user.
    void OnMediaStatus(int _Old, int _New)
    {
        if (_New == (int)mediaStatusEnum.doingNothing)
        {
            info_pleaseWait.gameObject.SetActive(false);
            error_LinkNotExist.gameObject.SetActive(false);
        }
        else if (_New == (int)mediaStatusEnum.currentlyPreparing)
        {
            info_pleaseWait.gameObject.SetActive(true);
            error_LinkNotExist.gameObject.SetActive(false);
        }
        else if (_New == (int)mediaStatusEnum.errorNoLink)
        {
            info_pleaseWait.gameObject.SetActive(false);
            error_LinkNotExist.gameObject.SetActive(true);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        if (videoLink != "")
        {
            StartCoroutine(waitPreparingVideo());
        }

    }



    private float nextActionTime = 0.0f;
    private float period = 1f; // update every x seconds
    private void Update()
    {

        //print("vidLink " + videoLink + " bool " +mediaIsPlayed);
        // Since there is a problem with video shader issue, therefore the server will count the time manually
        // The player will periodically check with the Server if his/her time match with the server.
        // otherwise fast forward or fast backward.
        // the time count to become 00:00:00 will be executed by the client whenever the video is finish or a new video is played.
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
            // execute block of code here

            //dont need to do anything
            if (videoLink == "")
                return;

            if (!mediaIsPlayed) // -> media is currently paused/not playing. so dont need to do anything
            {
                // due to video shader not working on the server.,
                // therefore we paused the video directly on the client
                if (isClient)
                    mediaVideoPlayer.Pause();
                return;
            }



            // SERVER ONLY 
            if (isServer)
            {
                //Adding one second.
                timeLineTS = timeLineTS.Add(TimeSpan.FromSeconds(1));
                ServerChangeTimeStamp(ConverterFunction.timeToDouble(timeLineTS));


                //if it goes beyond the timeline, reset it.
                if (timeLineTS.TotalSeconds >= (float)maxTimeLine)
                {
                    ServerChangeTimeStamp(0); // Reset the count again
                    //print("too long.. restarting");
                }

                 //print("sec " + timeLineDouble);
            }

            // CLIENT ONLY
            else if (isClient)
            {
                //keep playing it if it is already prepared
                if (!mediaPlane.gameObject.activeSelf)
                {
                    StartCoroutine(waitPreparingVideo());
                }

                else
                {
                    //set the state of the audio
                    mediaAudioSource.mute = audioIsMute;

                    if (!mediaVideoPlayer.isPlaying)
                        mediaVideoPlayer.Play();

                    timelineText.text = ConverterFunction.doubleToTimeString(timeLineDouble);
                }



                //Find the time difference between local's media timeline and the server's timeline
                var timeDiff = Mathf.Abs((float)(mediaVideoPlayer.time - timeLineDouble));

                //if the current video time is less or bigger than X seconds compared to the synchronizetimeLine
                //then kinda fast/back forward it.
                if (timeDiff >= 3 && mediaVideoPlayer.isPrepared)
                {
                    mediaVideoPlayer.time = timeLineDouble;
                    //update the slider too
                    mediaVideoSlider.Set((float)timeLineDouble, false);
                }
            }

        }
    }


    /// <summary>
    /// to Change a new video which will sync everything to other client.
    /// </summary>
    /// <param name="_str">Give the link to the video</param>
    public void Ui_ChangeNewVideo(string _str)
    {
        //show the preparing message
        CmdUpdatediaStatus(mediaStatusEnum.currentlyPreparing);

        StartCoroutine(SQLloader.LoadLinkFromWeb(_str, returnValue =>
        {
            if (!returnValue)
            {
                CmdUpdatediaStatus(mediaStatusEnum.errorNoLink);
                print("Video Link Does not exist -> proceed to do Nothing");
            }
            else
            {
                CmdUpdatediaStatus(mediaStatusEnum.doingNothing);
                print("Video Link exist -> Do something");
                //assign the url

                //Change the link in the server
                CmdSyncVideoLink(_str);

                //Reset the Timeline and MaxLength in the server
                CmdChangeTimeStamp(0);
            }
        }));

    }

    //Player change the timeStamp on the server, due to new video viewing, or maybe slider being slided.
    [Command(requiresAuthority = false)]
    public void CmdChangeTimeStamp(double _time)
    {
        ChangeTimeStamp(_time);
    }

    [Server] // Server changes its own timestamp
    public void ServerChangeTimeStamp(double _time)
    {
        ChangeTimeStamp(_time);
    }

    //timeStamp Logic change
    private void ChangeTimeStamp(double _time)
    {
        //change the timelineString and the dateTime
        timeLineDouble = _time;
        timeLineTS = ConverterFunction.doubleToTime(_time);
        //refresh the maxTimeLine
        maxTimeLine = mediaVideoPlayer.length;
    }

    //change the url from client's input to the server
    [Command(requiresAuthority = false)]
    public void CmdSyncVideoLink(string _link)
    {
        videoLink = _link;
        StartCoroutine(waitPreparingVideo());
        print("currentLink " + _link);
    }


    //Only run this in the server
    public IEnumerator waitPreparingVideo()
    {
        if (!mediaPlane.gameObject.activeSelf)
        {
            yield break;
        }


        //assign the url
        mediaVideoPlayer.url = videoLink;
        //wait a bit till video is ready.
        while (!mediaVideoPlayer.isPrepared)
        {
            mediaVideoPlayer.Prepare();
            yield return null;
        }

        //assign the video Time
        maxTimeLine = mediaVideoPlayer.length;

        //assign the slider max value
        mediaVideoSlider.maxValue = (float)mediaVideoPlayer.length;
        yield return null;

    }


    //Player change the timeStamp on the server, due to new video viewing, or maybe slider being slided.
    [Command(requiresAuthority = false)]
    public void CmdUpdatediaStatus(mediaStatusEnum _value)
    {
        mediaStatus = (int)_value;
    }




    //Video UI

    /// <summary>
    /// To Show the video Menu 
    /// </summary>
    /// <param name="_value"></param>
    public void Ui_ShowVideoMenu(bool _value)
    {
        videosMenu.SetActive(_value);
    }

    //pause the video
    public void Ui_PauseVideo()
    {
        mediaVideoPlayer.Pause();
        Cmd_PlayVideo(false);
    }

    //play the video
    public void Ui_PlayVideo()
    {
        mediaVideoPlayer.Play();
        Cmd_PlayVideo(true);
    }

    /// <summary>
    /// for timestamp slider which will also sync it to the Server and clients
    /// </summary>
    public void Ui_MediaSliderFunction()
    {
        CmdChangeTimeStamp(mediaVideoSlider.value);
    }

    /// <summary>
    /// for synchronize the video state to play or pause
    /// </summary>
    /// <param name="_bool">play -> true && pause -> false</param>
    [Command(requiresAuthority = false)]
    public void Cmd_PlayVideo(bool _bool)
    {
        mediaIsPlayed = _bool;
    }


    [Server]
    public void Server_PlayVideo(bool _bool)
    {
        mediaIsPlayed = _bool;
    }

    //To Play the audio
    public void Ui_PlayAudio()
    {
        mediaAudioSource.mute = false;
        //mediaVideoPlayer.SetDirectAudioMute(0, false);
        Cmd_PlayAudio(false);

    }

    //To Mute the Audio
    public void Ui_MuteAudio()
    {
        mediaAudioSource.mute = true;
        //mediaVideoPlayer.SetDirectAudioMute(0, true);
        Cmd_PlayAudio(true);
    }


    /// <summary>
    /// for synchronize the video state to play or pause
    /// </summary>
    /// <param name="_bool">play -> true && pause -> false</param>
    [Command(requiresAuthority = false)]
    public void Cmd_PlayAudio(bool _bool)
    {
        audioIsMute = _bool;
    }

}
