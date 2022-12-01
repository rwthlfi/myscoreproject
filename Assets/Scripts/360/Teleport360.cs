using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using AvatarCreation;

public class Teleport360 : MonoBehaviour
{
    private float _xpos, _ypos, _zpos, _xrot, _yrot, _zrot;
    public NetworkPlayerSetup playerLocalVar;
    public SphereCollider sphereCollider;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    public GameObject mainPlayer;
    private FirstPersonController _firstPersonController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.isTrigger == false && other.GetComponent<NetworkPlayerSetup>().isLocalPlayer)
        {
            mainPlayer = other.gameObject;

            //GetComponent<SphereCollider>().enabled = false;

            TriggerColliderFinish();
        }
    }

    public void TriggerColliderFinish()
    {
        if (mainPlayer.GetComponent<NetworkPlayerSetup>() != null)
        {
            playerLocalVar = mainPlayer.GetComponent<NetworkPlayerSetup>();

            if (playerLocalVar)
            {
                StartCoroutine(ChangeMovement());
            }

            if (GlobalSettings.DeviceType() == GlobalSettings.Device.WindowsNonVR)
            {
                _firstPersonController = mainPlayer.GetComponent<FirstPersonController>();
            }
        }
    }

    IEnumerator ChangeMovement()
    {
        yield return new WaitForSeconds(0.4f);

        /*
        playerLocalVar.ZeroPlayerAccelerationSpeed();
        playerLocalVar.ZeroPlayerRotationSpeed();
        */
        if (GlobalSettings.DeviceType() == GlobalSettings.Device.WindowsNonVR)
        {
            _firstPersonController.m_WalkSpeed = 0;
            _firstPersonController.m_RunSpeed = 0;
        }
    }

    public void TeleportPosition(float xpos, float ypos, float zpos, float xrot, float yrot, float zrot, bool inside3DRoom)
    {
        if (inside3DRoom == true)
        {
            /*
            playerLocalVar.LoadPlayerAccelerationSpeed();
            playerLocalVar.LoadPlayerRotationSpeed();
            */

            if (GlobalSettings.DeviceType() == GlobalSettings.Device.WindowsNonVR)
            {
                _firstPersonController.m_WalkSpeed = 1;
                _firstPersonController.m_RunSpeed = 3;
            }
        }
        else
        {
            /*
            playerLocalVar.ZeroPlayerAccelerationSpeed();
            playerLocalVar.ZeroPlayerRotationSpeed();
            */

            if (GlobalSettings.DeviceType() == GlobalSettings.Device.WindowsNonVR)
            {
                _firstPersonController.m_WalkSpeed = 0;
                _firstPersonController.m_RunSpeed = 0;
            }
        }

        _xpos = xpos;
        _ypos = ypos;
        _zpos = zpos;
        _xrot = xrot;
        _yrot = yrot;
        _zrot = zrot;

        StartCoroutine(TeleportPositonCoroutine());
    }

    IEnumerator TeleportPositonCoroutine()
    {
        if (mainPlayer.GetComponentInChildren<CameraFade>() != null)
        {
            mainPlayer.GetComponentInChildren<CameraFade>().FadeCameraIn(0.1f);
        }

        yield return new WaitForSeconds(0.2f);

        mainPlayer.transform.localPosition = new Vector3(_xpos, _ypos, _zpos);
        mainPlayer.transform.localEulerAngles = new Vector3(_xrot, _yrot, _zrot);

        if (GlobalSettings.DeviceType() == GlobalSettings.Device.WindowsNonVR)
        {
            _firstPersonController.enabled = false;
        }

        yield return new WaitForSeconds(0.2f);

        if (mainPlayer.GetComponentInChildren<CameraFade>() != null)
        {
            mainPlayer.GetComponentInChildren<CameraFade>().FadeCameraOut(0.2f);
        }

        if (GlobalSettings.DeviceType() == GlobalSettings.Device.WindowsNonVR)
        {
            _firstPersonController.enabled = true;
        }
    }
}