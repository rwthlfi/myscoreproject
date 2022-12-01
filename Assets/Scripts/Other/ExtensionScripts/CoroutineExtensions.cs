using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class CoroutineExtensions
{
    /// <summary>
    /// to Show an object after x seconds
    /// </summary>
    /// <param name="_obj">the game Object</param>
    /// <param name="_seconds">showing for how long</param>
    /// <returns></returns>
    public static IEnumerator ShowAfterSeconds(GameObject _obj, float _seconds)
    {
        _obj.SetActive(false);
        yield return new WaitForSeconds(_seconds);
        _obj.SetActive(true);
    }

    /// <summary>
    /// To hide an object after x seconds
    /// </summary>
    /// <param name="_obj">the game Object</param>
    /// <param name="_seconds">Hide for how long</param>
    /// <returns></returns>
    public static IEnumerator HideAfterSeconds(GameObject _obj, float _seconds)
    {
        _obj.SetActive(true);
        //Debug.Log("Start Hideing");
        yield return new WaitForSeconds(_seconds);
        _obj.SetActive(false);
    }

    /// <summary>
    /// to disable interaction of an object after x seconds
    /// </summary>
    /// <param name="_button">the button element </param>
    /// <param name="_seconds">not iteractable for how long</param>
    /// <returns></returns>
    public static IEnumerator InteractableButtonAfterSeconds(Button _button, float _seconds)
    {
        _button.interactable = false;
        yield return new WaitForSeconds(_seconds);
        _button.interactable = true;
    }
    public static IEnumerator AnimationOverTime(Animator _anim, string _animParam,
                                               float _targetValue, float _overTime)
    {
        float startTime = Time.time;
        float _currentValue = 0;
        while (_currentValue != _targetValue) // This is your target size of object.
        {
            _currentValue = _anim.GetFloat(_animParam);
            _currentValue = Mathf.MoveTowards(_currentValue, _targetValue, (Time.time - startTime) / _overTime /* *0.01f*/);
            _anim.SetFloat(_animParam, _currentValue);
            yield return null;
        }

    }


    /// <summary>
    /// to animate the given blendshapes value
    /// </summary>
    /// <param name="_mesh"></param>
    /// <param name="_blendID"></param>
    /// <param name="_initValue"></param>
    /// <param name="_targetValue"></param>
    /// <param name="_duration"></param>
    /// <returns></returns>
    public static IEnumerator blendShapesOverTime(SkinnedMeshRenderer _mesh, int _blendID, float _initValue, float _targetValue, float _duration)
    {
        float startTime = Time.time;
        float _currentValue = _initValue;
        float value = 0;
        while (Time.time < startTime + _duration) // This is your target size of object.
        {
            value = Mathf.MoveTowards(_currentValue, _targetValue, Mathf.Abs(_targetValue - _initValue) * (Time.time - startTime) / _duration);
            _mesh.SetBlendShapeWeight(_blendID, value);
            yield return null;
        }
        _mesh.SetBlendShapeWeight(_blendID, _targetValue);
    }


    /// <summary>
    /// To enable the script again after x seconds
    /// </summary>
    /// <param name="_mono"></param>
    /// <param name="_seconds"></param>
    /// <returns></returns>
    public static IEnumerator EnableScriptAfter(MonoBehaviour _mono, float _seconds)
    {
        _mono.enabled = false;
        yield return new WaitForSeconds(_seconds);
        _mono.enabled = true;
    }

}
