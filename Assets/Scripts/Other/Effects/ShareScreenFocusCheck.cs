using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShareScreenFocusCheck : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler /*ISelectHandler, IDeselectHandler*/  // require interface for OnSelect.
{
    public ShareScreenFocusList sslist;
    public Transform anchor;

    [Header("The Share Screen Toggle")]
    //public Toggle toggle;
    public Vector3 toggleOffset = Vector3.zero;
    public Vector3 toggleRotOffset = Vector3.zero;

    [Header("The Share Screen Window")]
    //public Transform window;
    public Vector3 windowOffset = Vector3.zero;
    public Vector3 windowRotOffset = Vector3.zero;

    void Start()
    {
        anchor = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //attach this to the "EventTrigger" -> pointerEnter
    public void OnSelect(BaseEventData _eventData)
    {
        OnObjectGrab();
    }

    //attached this to the "EventTrigger" -> pointer exit
    public void OnDeselect(BaseEventData _eventData)
    {
        //reset the coroutine to hide the toggle
        if (sslist)
        {
            StopCoroutine(sslist.hideCoroutine);
            sslist.hideCoroutine = sslist.waitForShare();
            StartCoroutine(sslist.hideCoroutine);
        }
        
    }



    //attached this to the XRGrab event
    public void OnObjectGrab()
    {
        UpdatePosRot();
        if (!sslist || !sslist.shareToggle)
            return;
        sslist.shareToggle.gameObject.SetActive(true);
        //update the ssList with this current transform.
        sslist.currentFocusedWindow = this.transform;
    }

    private void UpdatePosRot()
    {
        if (sslist && !sslist._allowShifting(this.transform))
            return;

        /*
        if (!anchor)
            anchor = transform.parent;
        */
        //dont do anything if the toggle is on..., cause the user might again "accidentantly" grab the ui.

        if (!sslist || !sslist.shareToggle || !anchor)
            return;

        sslist.shareToggle.position = anchor.TransformPoint(toggleOffset);
        sslist.shareToggle.rotation = anchor.rotation * Quaternion.Euler(toggleRotOffset);

        //make the windows as a child of this object.
        sslist.shareWindow.position = anchor.TransformPoint(windowOffset);
        sslist.shareWindow.rotation = anchor.rotation * Quaternion.Euler(windowRotOffset);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //print("enter");
        OnObjectGrab();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //print("exit");
        //reset the coroutine to hide the toggle
        if (sslist)
        {
            StopCoroutine(sslist.hideCoroutine);
            sslist.hideCoroutine = sslist.waitForShare();
            StartCoroutine(sslist.hideCoroutine);
        }
        //throw new System.NotImplementedException();
    }

}
