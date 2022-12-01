using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawAoE : MonoBehaviour
{

    [Header("Player Variable")]
    public SphereCollider sphereCollider;

    [Header("Effect Variable")]
    public GameObject meshEffect;



    //dummy enumerator
    private IEnumerator hidingEnumerator;

    private void Start()
    {
        meshEffect.SetActive(false);
        hidingEnumerator = CoroutineExtensions.HideAfterSeconds(meshEffect, 2f);
        
    }

    /// <summary>
    /// To change the Effect scaling according to the slider value
    /// </summary>
    /// <param name="_slider"></param>
    public void Ui_sliderChange(Slider _slider)
    {
        //activate the mesh
        meshEffect.SetActive(true);
        sphereCollider.radius = _slider.value;

        DrawAreaOfEffect(_slider.value);
    }


    /// <summary>
    /// Draw the AOE in radius Value
    /// </summary>
    /// <param name="_value"></param>
    public void DrawAreaOfEffect(float _value)
    {
        meshEffect.transform.localScale = new Vector3(_value , _value, _value) ;
        TurnOnEffect();
    }


    public void TurnOnEffect()
    {
        StopCoroutine(hidingEnumerator);
        hidingEnumerator = CoroutineExtensions.HideAfterSeconds(meshEffect, 2f);
        StartCoroutine(hidingEnumerator);
    }
}
