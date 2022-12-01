using UnityEngine;
using Mirror;
using System.Collections;
using UnityEngine.Networking;

public class Change360ImageButtonNetworkSync : NetworkBehaviour
{
    public GameObject sphereUI0, sphereUI1, sphereUI2, sphereUI3;
    public string uRL0, uRL1, uRL2, uRL3;
    public Enter360Sphere enter360Sphere;
    private string imageName;

    [Header("Syncvar Server variable")]
    [SyncVar(hook = nameof(ChangeCurrentImage))]
    public float currentImageValue = 0;

    void ChangeCurrentImage(float _old, float _new) { }

    private void Start()
    {
        ChangeImage(currentImageValue);
    }

    public void ChangeCurrentImage(float currentImage)
    {
        Cmd_ChangeCurrentImage(currentImage);
    }

    [Command(requiresAuthority = false)]
    private void Cmd_ChangeCurrentImage(float currentImage)
    {
        currentImageValue = currentImage;

        Rpc_ChangeCurrentImage(currentImageValue);
    }

    [ClientRpc]
    private void Rpc_ChangeCurrentImage(float currentImage)
    {
        ChangeImage(currentImage);
    }

    private void ChangeImage(float currentImage)
    {
        sphereUI0.SetActive(false); sphereUI1.SetActive(false); sphereUI2.SetActive(false); sphereUI3.SetActive(false);

        //if (currentImage == 0)
        //{
        //    enter360Sphere.SetNewTexture(imageName);
        //    sphereUI0.SetActive(true);
        //}

        //else if (currentImage == 1)
        //{
        //    enter360Sphere.SetNewTexture(imageName);
        //    sphereUI1.SetActive(true);
        //}

        //else if (currentImage == 2)
        //{
        //    enter360Sphere.SetNewTexture(imageName);
        //    sphereUI2.SetActive(true);
        //}

        //else if (currentImage == 3)
        //{
        //    enter360Sphere.SetNewTexture(imageName);
        //    sphereUI3.SetActive(true);
        //}
    }
}
