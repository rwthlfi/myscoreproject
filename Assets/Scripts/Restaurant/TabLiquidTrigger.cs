using AvatarCreation;
using UnityEngine;

public class TabLiquidTrigger : MonoBehaviour
{
    public ParticleSystem particleSystemObject;
    public AudioSource audioSourcePour;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<HandsAnimator>())
        {
            particleSystemObject.Play();
            audioSourcePour.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<HandsAnimator>())
        {
            particleSystemObject.Stop();
            audioSourcePour.enabled = false;
        }
    }
}
