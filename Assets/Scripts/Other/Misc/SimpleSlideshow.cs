using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleSlideshow : MonoBehaviour
{

    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public float delay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ImageChange());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ImageChange()
    {
        while (true)
        {
            gameObject.GetComponent<UnityEngine.UI.Image>().sprite = sprite2;
            yield return new WaitForSeconds(delay);
            gameObject.GetComponent<UnityEngine.UI.Image>().sprite = sprite3;
            yield return new WaitForSeconds(delay);
            gameObject.GetComponent<UnityEngine.UI.Image>().sprite = sprite1;
            yield return new WaitForSeconds(delay);
        }
    }
}