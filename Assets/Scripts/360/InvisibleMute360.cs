using UnityEngine;

public class InvisibleMute360 : MonoBehaviour
{
    public bool invisibleMute;
    public Collider _collider;
    public GameObject gameobject;
    public Collider colliderGameobject;

    private void OnTriggerEnter(Collider other)
    {
        if (invisibleMute)
        {
            foreach (SkinnedMeshRenderer smr in other.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                if (smr.gameObject != gameObject && smr.gameObject.name == "Piller.Mesh")
                {
                    smr.GetComponent<SkinnedMeshRenderer>().enabled = false;
                }
            }

            /*
            if (other.GetComponentInChildren<MicrophoneWindow>(true) != null)
            {
                other.GetComponentInChildren<MicrophoneWindow>(true).MutePlayer();
            }
            */
            foreach (Canvas c in other.GetComponentsInChildren<Canvas>())
            {
                if (c.gameObject.name == "StatusCanvas")
                {
                    c.GetComponent<Canvas>().enabled = false;
                }
            }
        }

        else if (invisibleMute == false)
        {
            foreach (SkinnedMeshRenderer smr in other.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                if (smr.gameObject != gameObject && smr.gameObject.name == "Piller.Mesh")
                {
                    smr.GetComponent<SkinnedMeshRenderer>().enabled = true;
                }
            }

            /*
            if (other.GetComponentInChildren<MicrophoneWindow>(true) != null)
            {
                other.GetComponentInChildren<MicrophoneWindow>(true).UnMutePlayer();
            }

            */
            foreach (Canvas c in other.GetComponentsInChildren<Canvas>())
            {
                if (c.gameObject.name == "StatusCanvas")
                {
                    c.GetComponent<Canvas>().enabled = true;
                }
            }
        }
    }
}