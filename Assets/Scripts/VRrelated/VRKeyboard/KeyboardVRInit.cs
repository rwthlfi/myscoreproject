using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VRKeys;
using UnityEngine.UI;

public class KeyboardVRInit : MonoBehaviour
{
    public XRRig xrRig;
    private Keyboard keyboard;

    [Header("Extra component")]
    public Slider keySlider;
    public Transform moveKeyboard;
    private Vector3 moveKeyboardScale;
    public Button showKeyButton;

    [Header("Script Reference")]
    public AvatarCreation.VRPlatformDetector vrPlatformDetector;
    public Transform refOculus;
    public Transform refSteam;

    [System.NonSerialized] public bool snap = true;


    // Start is called before the first frame update
    void Start()
    {
        //init all the component. 
        keyboard = GetComponent<Keyboard>();
        showKeyButton.gameObject.SetActive(false);

        moveKeyboardScale = moveKeyboard.localScale;

        // Improves event system performance
        Canvas canvas = keyboard.canvas.GetComponent<Canvas>();
        canvas.worldCamera = xrRig.GetComponentInChildren<Camera>();

        keyboard.Enable();

        
        Invoke("Ui_hideKeyboard", 3.0f);
        //Ui_hideKeyboard();
        /*
        keyboard.OnUpdate.AddListener(HandleUpdate);
        keyboard.OnSubmit.AddListener(HandleSubmit);
        keyboard.OnCancel.AddListener(HandleCancel);
        */

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //for keyboard scaling
    public void KeyboardScaling()
    {
        float scaling = getScaling();

        this.transform.localScale = new Vector3(scaling, scaling, scaling);
    }

    public void KeyboardSnap(Toggle _snapToggle)
    {
        snap = _snapToggle.isOn;
        //later it will be taken care by the keyboard Listener
    }


    //to get the scaling by the _keySlider using linear interpolation.
    private float getScaling()
    {
        float minS = 0.6f, maxS = 1.2f;
        float scaleResult;

        //simple linear interpolation
        scaleResult = (((maxS - minS) / (keySlider.maxValue - keySlider.minValue)) * (keySlider.value - keySlider.minValue)) + minS;
        return scaleResult;
    }


    public void Ui_hideKeyboard()
    {
        //set the scaling to zero
        this.transform.localScale = Vector3.zero;
        //hide the keyboard mover as well
        moveKeyboard.localScale = Vector3.zero;

        //afterwards attached it to the left hand.
        showKeyButton.gameObject.SetActive(true);

    }

    //for hiding the keyboard
    public void Ui_ShowKeyboard()
    {
        //show the keyboard
        float scaling = getScaling();

        this.transform.localScale = new Vector3(scaling, scaling, scaling);
        moveKeyboard.localScale = moveKeyboardScale;

        //deactivate the showKeyboardCanvas
        showKeyButton.gameObject.SetActive(false);

    }


    //for hiding the keyboard
    public void Ui_ShowKeyboard(bool _value)
    {
        //show the keyboard
        float scaling = getScaling();

        this.transform.localScale = new Vector3(scaling, scaling, scaling);
        moveKeyboard.localScale = moveKeyboardScale;

        //set the position

        if (vrPlatformDetector.useSteamVR)
        {
            moveKeyboard.transform.position = refSteam.position;
            moveKeyboard.transform.rotation = refSteam.rotation;
        }
        else
        {
            moveKeyboard.transform.position = refOculus.position;
            moveKeyboard.transform.rotation = refOculus.rotation;

        }


        //deactivate the showKeyboardCanvas
        showKeyButton.gameObject.SetActive(false);

    }
}
