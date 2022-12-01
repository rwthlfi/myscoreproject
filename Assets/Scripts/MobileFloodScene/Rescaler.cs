using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rescaler : MonoBehaviour
{
    public GameObject go_1;
    public GameObject go_2;

    private void Awake()
    {
    }


    private void Start()
    {
        //ShowGameObject_1();
        go_1.transform.localScale = new Vector3(1, 1, 1);
        go_2.transform.localScale = new Vector3(1, 1, 1);
    }

    public void ShowGameObject_1()
    {
        if(!go_1 || !go_2)
        {
            Debug.Log("go_1 & go_2 is empty");
            return;
        }
        go_1.gameObject.SetActive(true);
        go_2.gameObject.SetActive(true);
        go_1.transform.localScale = new Vector3(1, 0, 0);
        StartCoroutine(LerpingExtensions.ScaleTo(go_1.transform, new Vector3(1, 1, 1), 50f));
        StartCoroutine(LerpingExtensions.ScaleTo(go_2.transform, new Vector3(1, 0, 0), 50f));
        
    }

    public void ShowGameObject_2()
    {
        if(!go_1 || !go_2)
        {
            Debug.Log("go_1 & go_2 is empty");
            return;
        }
        go_1.gameObject.SetActive(true);
        go_2.gameObject.SetActive(true);
        StartCoroutine(LerpingExtensions.ScaleTo(go_2.transform, new Vector3(1, 1, 1), 50f));
        StartCoroutine(LerpingExtensions.ScaleTo(go_1.transform, new Vector3(1, 0, 0), 50f));
    }
}
