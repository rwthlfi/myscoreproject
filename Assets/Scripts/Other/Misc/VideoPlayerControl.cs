using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Video;

public class VideoPlayerControl : MonoBehaviour
{
    [Header("Sources")]
    public VideoClip videoClip;
    public string videoPlayerURL;
    public bool sourceIsVideoClip;

    [Header("Misc")]
    public VideoPlayer videoPlayer;
    public Slider videoTimeSlider;
    public AudioSource videoAudioSource;

    public GameObject playImage;
    public GameObject pauseImage;
    public GameObject unMuteImage;
    public GameObject muteImage;
    public RectTransform fillArea;
    public RectTransform handle;
    public Text timeText;
    public GameObject hideImage;
    public GameObject unhideImage;
    public GameObject menu;
    public Image hideButtonBackground;
    public GameObject loading;

    private bool _mute;
    private bool _isPlaying;
    private bool _stop;
    private float _framesPerSecond;
    private float _currentSeconds;
    private bool _hidePlayer;

    private void Awake()
    {
        if (videoClip == null && string.IsNullOrEmpty(videoPlayerURL))
        {
            timeText.text = ("Video clip or URL not found!");
            timeText.color = Color.red;
            enabled = false;
            videoPlayer.enabled = false;
        }
    }

    private void Start()
    {
        videoTimeSlider.interactable = false;

        if (sourceIsVideoClip)
            videoPlayer.clip = videoClip;
        else if (!sourceIsVideoClip)
            videoPlayer.url = videoPlayerURL;

        videoPlayer.prepareCompleted += Prepared;
        _currentSeconds = 0.01f;
        _hidePlayer = false;
    }

    void Prepared(VideoPlayer videoPlayer)
    {
        double videoLength = videoPlayer.length;
        videoTimeSlider.maxValue = (float)videoPlayer.length;
        videoTimeSlider.interactable = true;
    }

    void SeekCompleted(VideoPlayer videoPlayer)
    {
        loading.SetActive(false);
    }

    public void PlayResume()
    {
        _stop = false;

        if (!_isPlaying)
        {
            loading.SetActive(true);
            videoPlayer.Play();
            _isPlaying = true;
            playImage.SetActive(false);
            pauseImage.SetActive(true);
            StartCoroutine(UpdateSliderPosition());

        }
        else
        {
            videoPlayer.Pause();
            _isPlaying = false;
            pauseImage.SetActive(false);
            playImage.SetActive(true);
        }
    }

    public void Stop()
    {
        videoPlayer.Stop();
        _isPlaying = false;
        _stop = true;
        videoTimeSlider.interactable = false;
        videoTimeSlider.value = 0;
        _currentSeconds = 0.01f;
        pauseImage.SetActive(false);
        playImage.SetActive(true);
        timeText.text = "00:00:00 / 00:00:00";
        loading.SetActive(false);

        StopCoroutine(UpdateSliderPosition());
    }

    public void SliderValue()
    {
        loading.SetActive(true);
        _framesPerSecond = videoPlayer.frameRate;
        StopCoroutine(UpdateSliderPosition());

        if (videoTimeSlider.value < 1)
        {
            videoPlayer.frame = 0;
        }
        else if (videoTimeSlider.value >= videoTimeSlider.maxValue - 1)
        {
            videoTimeSlider.value = videoTimeSlider.maxValue - 1;
        }
        else
        {
            videoPlayer.frame = (long)(videoTimeSlider.value * _framesPerSecond);
        }

        if (!_stop)
        {
            videoPlayer.Play();
            _isPlaying = true;

            playImage.SetActive(false);
            pauseImage.SetActive(true);
        }

        videoPlayer.seekCompleted += SeekCompleted;

        StartCoroutine(UpdateSliderPosition());
    }

    public void MuteUnmute()
    {
        if (!_mute)
        {
            videoAudioSource.mute = true;
            muteImage.SetActive(false);
            unMuteImage.SetActive(true);
            _mute = true;
        }
        else
        {
            videoAudioSource.mute = false;
            muteImage.SetActive(true);
            unMuteImage.SetActive(false);
            _mute = false;
        }
    }

    IEnumerator UpdateSliderPosition()
    {
        while (_isPlaying)
        {
            _currentSeconds = videoPlayer.frame / videoPlayer.frameRate;
            float maxSeconds = videoTimeSlider.maxValue;

            float normalizeCurrentSeconds = (_currentSeconds / maxSeconds);

            if (_currentSeconds >= 0.01)
            {
                fillArea.anchorMax = new Vector2(normalizeCurrentSeconds, 1);
                handle.anchorMin = new Vector2(normalizeCurrentSeconds, 0);
                handle.anchorMax = new Vector2(normalizeCurrentSeconds, 1);

            }

            Timer();

            yield return new WaitForSeconds(1);

            if (videoPlayer.isPlaying)
                loading.SetActive(false);
        }
    }

    void Timer()
    {
        if (_currentSeconds >= 0)
        {
            float currentHours = TimeSpan.FromSeconds(_currentSeconds).Hours;
            float currentMinutes = TimeSpan.FromSeconds(_currentSeconds).Minutes;
            float currentSeconds = TimeSpan.FromSeconds(_currentSeconds).Seconds;

            float maxHours = TimeSpan.FromSeconds(videoTimeSlider.maxValue).Hours;
            float maxMinutes = TimeSpan.FromSeconds(videoTimeSlider.maxValue).Minutes;
            float maxSeconds = TimeSpan.FromSeconds(videoTimeSlider.maxValue).Seconds;

            timeText.text = (currentHours.ToString("00") + ":" + currentMinutes.ToString("00") + ":" + currentSeconds.ToString("00")) + " / " + (maxHours.ToString("00") + ":" + maxMinutes.ToString("00") + ":" + maxSeconds.ToString("00"));
        }
    }

    public void HidePlayer()
    {
        if (_hidePlayer)
        {
            menu.transform.localScale = new Vector3(1, 1, 1);
            hideImage.SetActive(false);
            unhideImage.SetActive(true);
            var color = hideButtonBackground.color;
            color.a = 1f;
            hideButtonBackground.color = color;
            _hidePlayer = false;
        }
        else
        {
            menu.transform.localScale = new Vector3(0, 0, 0);
            unhideImage.SetActive(false);
            hideImage.SetActive(true);
            var color = hideButtonBackground.color;
            color.a = 0.3f;
            hideButtonBackground.color = color;
            _hidePlayer = true;
        }
    }
}