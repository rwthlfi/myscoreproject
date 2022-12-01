using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


//to know which object currently grabbed by the hand ..
public class XRDirectObject : MonoBehaviour
{
    private XRDirectInteractor xrDirectInteractor;
    // Start is called before the first frame update
    void Start()
    {
        xrDirectInteractor = GetComponent<XRDirectInteractor>();

    }

    // Update is called once per frame
    void Update()
    {
        if(xrDirectInteractor && xrDirectInteractor.selectTarget)
            Debug.Log("asdf " + xrDirectInteractor.selectTarget.name);
    }
}
