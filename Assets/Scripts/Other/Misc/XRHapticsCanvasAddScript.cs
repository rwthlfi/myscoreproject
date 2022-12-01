using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class XRHapticsCanvasAddScript : MonoBehaviour
{

    //This script adds the XRHapticsUISound and Audio Source to an interactable UI element on runtime

    private void Start()
    {
        List<GameObject> gameObjects = new List<GameObject>();
        gameObjects.AddRange(gameObject.GetComponentsInChildren<Button>().Select(x => x.gameObject));
        gameObjects.AddRange(gameObject.GetComponentsInChildren<Toggle>().Select(x => x.gameObject));
        gameObjects.AddRange(gameObject.GetComponentsInChildren<Slider>().Select(x => x.gameObject));
        gameObjects.AddRange(gameObject.GetComponentsInChildren<Dropdown>().Select(x => x.gameObject));

        foreach (var item in gameObjects)
        {
            item.AddComponent<XRHapticsUISound>();
            var audioSource = item.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 0.5f;
            audioSource.clip = Resources.Load<AudioClip>("Artefacts/Audio/UI/UIConfirmSound");
        }
    }
}
