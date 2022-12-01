using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BuffetPlaceTrigger : MonoBehaviour
{
    public BuffetPlaceManager buffetPlaceManager;
    public AudioSource audioSourceCheck, audioSourceWrong;
    public GameObject spriteCheck, collidedGameObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<XRGrabInteractable>())
        {
            buffetPlaceManager.CheckTriggerNames(gameObject, other.gameObject.name, gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<XRGrabInteractable>())
        {
            DeactivateResult();
        }
    }

    public void ActivateResult()
    {
        audioSourceCheck.Play();
        spriteCheck.SetActive(true);
    }

    public void DeactivateResult()
    {
        spriteCheck.SetActive(false);
    }

    public void ResultWrong()
    {
        audioSourceWrong.Play();
    }
}
