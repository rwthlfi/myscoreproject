using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollViewExtensions : MonoBehaviour
{
    [Header("Ui Variable")]
    public Transform content;
    public ScrollRect scrollRect;
    public Scrollbar scrollbar;

    [Header("Extra Variable")]
    public float steps;
    public float extraOffset = 0f;

    [Header("Speed")]
    public float lerpSpeed = 0.25f;

    Coroutine a;


    // Start is called before the first frame update
    void Start()
    {
        //yield return new WaitForEndOfFrame();
        steps = 1f / ((float)content.childCount - 1) + extraOffset;
        //print(content.childCount);
        a = StartCoroutine(LerpingScrollView(scrollbar.value, returnValue =>
            {
                //and Finally lerp to that point
                StartCoroutine(LerpingScrollBar(scrollbar, returnValue, lerpSpeed));
            }));
    }

    // Update is called once per frame
    void Update()
    {
    }



    /// <summary>
    /// to recalculate the steps, in case there is additional content
    /// </summary>
    public void RecalculateSteps()
    {
        int i = 0;
        foreach (Transform trs in content)
            if (trs.gameObject.activeSelf)
                i++;

        if (i == 0)
            return;

        //steps = 1f / ((float)content.childCount-1) + extraOffset;
        steps = 1f / ((float)i - 1) + extraOffset;
    }


    public void Ui_ScrollRectChanged()
    {
        StopCoroutine(a);
        a = StartCoroutine(LerpingScrollView(scrollbar.value, returnValue =>
        {
            //and Finally lerp to that point
            StartCoroutine(LerpingScrollBar(scrollbar, returnValue, lerpSpeed));
        }));
    }

    public void Ui_ScrollRectChanged(float _value)
    {
        StopCoroutine(a);
        a = StartCoroutine(LerpingScrollView(_value, returnValue =>
        {
            //and Finally lerp to that point
            StartCoroutine(LerpingScrollBar(scrollbar, returnValue, lerpSpeed));
        }));
    }


    public void Ui_LeftButtonClicked()
    {
        if (scrollbar.value <= 0)
        {
            scrollbar.value = 0;
            return;
        }

        float z = scrollbar.value - steps;
        StartCoroutine(LerpingScrollView(z, returnValue =>
        {
            //and Finally lerp to that point
            StartCoroutine(LerpingScrollBar(scrollbar, returnValue, lerpSpeed));
        }));
    }

    public void Ui_RightButtonClicked()
    {
        if (scrollbar.value >= 1)
        {
            scrollbar.value = 1;
            return;
        }


        float u = scrollbar.value + steps;
        StartCoroutine(LerpingScrollView(u, returnValue =>
        {
            //and Finally lerp to that point
            StartCoroutine(LerpingScrollBar(scrollbar, returnValue, lerpSpeed));
        }));
    }



    float a0, a1, max, min;
    /// <summary>
    /// For Lerping the scroll View so it "snapped"
    /// </summary>
    /// <param name="a"></param>
    /// <param name="_callBack"></param>
    /// <returns></returns>
    private IEnumerator LerpingScrollView(float a, System.Action<float> _callBack)
    {
        //set the upper parameter
        max = 0f;
        while (max < a)
        {
            max += steps;
            yield return null;
        }

        //set the below parameter
        min = 1;
        while (min > a)
        {
            min -= steps;
            yield return null;
        }

        //get the closer value
        a0 = Mathf.Abs(a - max);
        a1 = Mathf.Abs(a - min);
        if (a0 < a1)
            a = max;
        else
            a = min;


        //print("a " + a);
        _callBack(a);
    }



    /// <summary>
    /// Lerping the scroll bar to the targeted value
    /// </summary>
    /// <param name="_scrollBar"></param>
    /// <param name="_targetValue"></param>
    /// <param name="_overTime"></param>
    /// <returns></returns>
    public IEnumerator LerpingScrollBar(Scrollbar _scrollBar, float _targetValue, float _overTime)
    {
        float startTime = Time.time;
        float ori = _scrollBar.value;

        //print("value " + _scrollBar.value + " target " + _targetValue);

        while(Time.time < startTime + _overTime)
        {
            //print("t " + Time.time);
            _scrollBar.value = Mathf.Lerp(ori, _targetValue, (Time.time - startTime) / _overTime);
            yield return null;
        }

        //set the value target
        _scrollBar.value = _targetValue;

        //to make sure the value is not getting larger than 1 or less than 0
        if (scrollbar.value >= 1)
            scrollbar.value = 1;

        if (scrollbar.value <= 0)
            scrollbar.value = 0;
    }

}
