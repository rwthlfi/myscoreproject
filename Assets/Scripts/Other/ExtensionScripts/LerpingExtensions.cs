using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This script is used as an extension of the lerping.
// the lerping that is happening in here can be used outside the "Update" function
public static class LerpingExtensions
{
    //to move In the worldPosition
    public static IEnumerator MoveTo(Transform _objectToMove, Vector3 _target, float _overTime)
    {
        float startTime = Time.time;
        Vector3 origin = _objectToMove.position;
        while (Time.time < startTime + _overTime)
        {
            _objectToMove.position = Vector3.Lerp(origin, _target, (Time.time - startTime) / _overTime);
            yield return null;
        }
        _objectToMove.position = _target;
    }


    public static IEnumerator MoveToLocal(Transform _objectToMove, Vector3 _target, float _overTime)
    {
        float startTime = Time.time;
        Vector3 origin = _objectToMove.localPosition;
        while (Time.time < startTime + _overTime)
        {
            _objectToMove.localPosition= Vector3.Lerp(origin, _target, (Time.time - startTime) / _overTime);
            yield return null;
        }
        _objectToMove.localPosition = _target;
    }

    //to Scale
    public static IEnumerator ScaleTo(Transform _objectToScale, Vector3 _target, float _overTime)
    {
        float startTime = Time.time;
        Vector3 originScale = _objectToScale.localScale;
        while (Time.time < startTime + _overTime)
        {
            //Debug.Log("scaling");
            _objectToScale.localScale = Vector3.Lerp(originScale, _target, (Time.time - startTime) / _overTime);
            yield return null;
        }
        _objectToScale.localScale = _target;
    }

    //to Rotate
    public static IEnumerator RotateTo(Transform _objectToRotate, Quaternion _target, float _overTime)
    {
        float startTime = Time.time;
        Quaternion oriRot = _objectToRotate.rotation;
        while (Time.time < startTime + _overTime)
        {
            _objectToRotate.rotation = Quaternion.Slerp(oriRot, _target, (Time.time - startTime) / _overTime);
            yield return null;
        }
        _objectToRotate.rotation = _target;
    }


    //to Rotate Locally
    public static IEnumerator RotateToLocal(Transform _objectToRotate, Quaternion _target, float _overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + _overTime)
        {
            _objectToRotate.localRotation = Quaternion.Slerp(_objectToRotate.localRotation, _target, (Time.time - startTime) / _overTime);
            yield return null;
        }
        _objectToRotate.localRotation = _target;
    }

    //rotate local Euler
    public static IEnumerator RotateToLocalEuler(Transform _objectToRotate, Vector3 _target, float _overTime)
    {
        float startTime = Time.time;
        Vector3 origin = _objectToRotate.localEulerAngles;

        while (Time.time < startTime + _overTime)
        {
            _objectToRotate.localEulerAngles = Quaternion.Slerp(Quaternion.Euler(origin),
                                                                Quaternion.Euler(_target), 
                                                                (Time.time - startTime) / _overTime).eulerAngles;
            yield return null;
        }
        _objectToRotate.localEulerAngles = _target;
    }



    //to return Object Transform to its original Value
    public static IEnumerator ReturnToOriginal(Transform _objectToReturn, 
                                            Vector3 _posTarget,
                                            Quaternion _rotTarget,
                                            Vector3 _scaleTarget, 
                                            float _overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + _overTime)
        {
            _objectToReturn.localPosition = Vector3.Lerp(_objectToReturn.localPosition, _posTarget, (Time.time - startTime) / _overTime);
            _objectToReturn.localRotation = Quaternion.Slerp(_objectToReturn.localRotation, _rotTarget, (Time.time - startTime) / _overTime);
            _objectToReturn.localScale = Vector3.Lerp(_objectToReturn.localScale, _scaleTarget, (Time.time - startTime) / _overTime);
            yield return null;
        }
        _objectToReturn.localPosition = _posTarget;
        _objectToReturn.localRotation = _rotTarget;
        _objectToReturn.localScale = _scaleTarget;
    }


    //to return Object Transform to its original Value
    public static IEnumerator ReturnToWorldCoor(Transform _objectToReturn,
                                            Vector3 _posTarget,
                                            Quaternion _rotTarget,
                                            Vector3 _scaleTarget,
                                            float _overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + _overTime)
        {
            _objectToReturn.position = Vector3.Lerp(_objectToReturn.position, _posTarget, (Time.time - startTime) / _overTime);
            _objectToReturn.rotation = Quaternion.Slerp(_objectToReturn.rotation, _rotTarget, (Time.time - startTime) / _overTime);
            _objectToReturn.localScale = Vector3.Lerp(_objectToReturn.localScale, _scaleTarget, (Time.time - startTime) / _overTime);
            yield return null;
        }
        _objectToReturn.position = _posTarget;
        _objectToReturn.rotation = _rotTarget;
        _objectToReturn.localScale = _scaleTarget;

    }


    /// <summary>
    /// To lerp the blendshapes
    /// </summary>
    /// <param name="_blend">the skinned mesh renderer</param>
    /// <param name="_blendIdx">the id of the blendshapes</param>
    /// <param name="_target">the target blendshapes </param>
    /// <param name="_overTime">how long the lerping time</param>
    /// <returns></returns>
    public static IEnumerator LerpBlendshapes(SkinnedMeshRenderer _blend, int _blendIdx, float _target, float _overTime)
    {
        float startTime = Time.time;
        float origin = _blend.GetBlendShapeWeight(_blendIdx);
        while (Time.time < startTime + _overTime)
        {
            float a = Mathf.Lerp(origin, _target, (Time.time - startTime) / _overTime);

            _blend.SetBlendShapeWeight(_blendIdx, a);
            yield return null;
        }

        _blend.SetBlendShapeWeight(_blendIdx, _target);
    }

}