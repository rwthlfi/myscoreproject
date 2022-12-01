using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_Flood : MonoBehaviour
{
    [Header("Script Reference")]
    public MobileFloodTutorialManager mobileFloodTutor;

    private bool allowDestruction = false;
    private Coroutine floodRoutine;

    private void Start()
    {
        floodRoutine = StartCoroutine(CoroutineExtensions.HideAfterSeconds(this.gameObject, 10f));
    }

    private void OnEnable()
    {
        if (floodRoutine != null)
            StopCoroutine(floodRoutine);

        //if the user already click the "Test flood" button
        //but havent reach the tutorStepFinish
        if (mobileFloodTutor.currentStep != (int)MobileFloodTutorialManager.tutorStep.Finish)
        {
            allowDestruction = true;
        }
        else
            allowDestruction = false;

        //hide after 10 seconds from the enabled
        floodRoutine = StartCoroutine(CoroutineExtensions.HideAfterSeconds(this.gameObject, 10f));
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!allowDestruction) // if destruction is not allowed dont do anything
            return;


        var a = other.gameObject.GetComponent<Component_MobileFloodWall_Ref>();
        if (a == null)
            return;

        a.GetComponent<Rigidbody>().isKinematic = false;
        a.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

    }
}
