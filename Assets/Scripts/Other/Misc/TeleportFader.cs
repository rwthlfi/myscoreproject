using Fungus;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TeleportFader : MonoBehaviour
{
    public float fadeTime;
    public float waitTimer;
    public GameObject AvatarFBX;
    public GameObject ParticleSystem;
    public GameObject PointLeftHand;
    public GameObject PointRightHand;
    private ParticleSystem particleObject;
    private AudioSource audioObject;
    private LineRenderer pointrightlinerenderer;
    private LineRenderer pointleftlinerenderer;
    public Vector3 AvatarScaleStart;
    public Vector3 AvatarScaleFinish;

    void Start()
    {
        particleObject = GetComponent<ParticleSystem>();
        audioObject = GetComponent<AudioSource>();
        pointleftlinerenderer = PointLeftHand.GetComponent<LineRenderer>();
        pointrightlinerenderer = PointRightHand.GetComponent<LineRenderer>();
    }

    public void startFading()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        pointleftlinerenderer.enabled = false;
        pointrightlinerenderer.enabled = false;

        particleObject.Play();
        audioObject.Play();
        iTween.ScaleTo(AvatarFBX, AvatarScaleStart, fadeTime);

        yield return new WaitForSeconds((float)waitTimer);

        iTween.ScaleTo(AvatarFBX, AvatarScaleFinish, fadeTime);
        pointleftlinerenderer.enabled = true;
        pointrightlinerenderer.enabled = true;
    }
}