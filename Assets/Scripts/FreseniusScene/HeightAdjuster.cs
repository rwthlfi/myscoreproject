using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightAdjuster : MonoBehaviour
{
    public Camera _mainCamera;
    public float yOff = 0.5f;

    [Header("Object to adjust in Y")]
    public Transform questionDisplay;
    public Transform answerMain;
    public Transform resultCanvas;

    [Header("Answer Value")]

    public float yAnswerOff = 0.5f;


    // Use this for initialization
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);

        _mainCamera = Camera.main;
    }


    // Update is called once per frame
    void Update()
    {
        //if main camera exist
        if (_mainCamera )
        {
            //lerp the y position
            AdjustYPos(questionDisplay);
            AdjustYPos(resultCanvas);
            AdjustYPosAnswer(answerMain);

        }


    }

    /// <summary>
    /// to adjust the postion of the given Object but only in Y
    /// </summary>
    /// <param name="_obj"></param>
    private void AdjustYPos(Transform _obj)
    {
        //lerp the y position
        _obj.position = Vector3.Lerp(_obj.position,
                                     new Vector3(_obj.position.x,
                                                 _mainCamera.transform.position.y + yOff,
                                                 _obj.position.z),
                                     Time.deltaTime);
    }

    private void AdjustYPosAnswer(Transform _obj)
    {
        //lerp the y position
        _obj.position = Vector3.Lerp(_obj.position,
                                     new Vector3(_obj.position.x,
                                                 _mainCamera.transform.position.y + yAnswerOff,
                                                 _obj.position.z),
                                     Time.deltaTime);

    }

}
