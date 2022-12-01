using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShareScreenFocusList : MonoBehaviour
{
    [Header("Sharing Element")]
    public Transform shareToggle;
    public Transform shareWindow;
    private Toggle toggle;


    [System.NonSerialized]
    public Transform currentFocusedWindow;

    //[System.NonSerialized]
    public Transform currentSharedWindow;

    public IEnumerator hideCoroutine;

    //public GameObject debug;
    private void Start()
    {
        // cause the shareToggle is not actually contains the toggle.
        //however we do need the share screentoggle.. therefore we cache this toggle value
        toggle = shareToggle.GetComponentInChildren<Toggle>(); 

        //set the Dummycoroutine
        hideCoroutine = waitForShare();
    }


    public void ui_WindowOpener_IsClicked(GameObject _window)
    {
        //debug = _window.GetComponentInChildren<ShareScreenFocusCheck>().gameObject;
        //logic:
        //if window is active, then put the focus there
        if (_window.activeSelf )
        {
            _window.GetComponentInChildren<ShareScreenFocusCheck>().OnObjectGrab();
            shareToggle.gameObject.SetActive(true);
            print("I am in ");
        }

        //if not, remove the focus, and turn off share screen
        else
        {
            if(currentSharedWindow == null)
            {
                print("no sharing right, I will close the share screen");
                toggle.GetComponent<Toggle>().isOn = false;
                shareToggle.gameObject.SetActive(false);
                return;
            }

            //if the recently closed window is not the current shared window, dont do anything
            else if (_window.GetComponentInChildren<ShareScreenFocusCheck>().transform
                != currentSharedWindow
                )
            {
                print("should not be the same");
                return;
            }

            else
            {
                print("Its the same? ");
            }

            toggle.GetComponent<Toggle>().isOn = false;
            shareToggle.gameObject.SetActive(false);




        }
    }


    //when the schare screen is clicked, then store the current focused Window.,
    //therefore to lock it to the focus.. and not giving the share screen to other.
    public void ui_SharedIsClicked()
    {
        if (toggle.isOn)
            currentSharedWindow = currentFocusedWindow;
        else
            currentSharedWindow = null;
    
    }


    //to remove the share screen button when it is not on focus anymore.
    public IEnumerator waitForShare()
    {
        StopCoroutine(waitForShare());
        yield return new WaitForSeconds(3f);
        //if it is currently on, dont do anything
        if (toggle.isOn)
            yield return null;
        else
            shareToggle.gameObject.SetActive(false);
    }




    public bool _allowShifting(Transform _window)
    {
        //if there is no shared window, then feel free to shift it.
        if (!currentSharedWindow)
            return true;

        if (_window == currentSharedWindow)
            return true;
        else
            return false;
    }



}
