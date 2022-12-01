using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class XRHapticsUISound : MonoBehaviour, IPointerUpHandler, IPointerEnterHandler
{
    private List<InputDevice> _foundControllers;
    private int _pointerID1 = 1;
    private int _pointerID2 = 2;

    public AudioSource _audioSource;

    private void Start()
    {
        _foundControllers = new List<InputDevice>();

        if (GameObject.Find("UIAudioSource"))
        {
            _audioSource = GameObject.Find("UIAudioSource").GetComponent<AudioSource>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GlobalSettings.haptics == true/* || SceneManager.GetActiveScene().buildIndex == 0*/)
        {
            if (GlobalSettings.DeviceType() == GlobalSettings.Device.Android || Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (eventData.pointerId == _pointerID1) // for oculus quest "ID 2" is the left controller, "ID 1" is right
                    TriggerHapticsRight(0.1f, 0.1f);
                else
                    TriggerHapticsLeft(0.1f, 0.1f);
            }
            else if (GlobalSettings.DeviceType() == GlobalSettings.Device.WindowsVR)
            {
                if (eventData.pointerId == _pointerID2) // for htc "ID 2" is the right controller, "ID 1" is left
                    TriggerHapticsRight(0.1f, 0.1f);
                else
                    TriggerHapticsLeft(0.1f, 0.1f);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GlobalSettings.DeviceType() == GlobalSettings.Device.Android || Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (eventData.pointerId == _pointerID1) // for oculus quest "ID 2" is the left controller, "ID 1" is right
            {
                if (GlobalSettings.haptics == true)
                    TriggerHapticsRight(0.2f, 0.2f);

                if (GlobalSettings.uIAudio == true)
                    UIAudio();
            }
            else
            {
                if (GlobalSettings.haptics == true)
                    TriggerHapticsLeft(0.2f, 0.2f);

                if (GlobalSettings.uIAudio == true)
                    UIAudio();
            }
        }
        else if (GlobalSettings.DeviceType() == GlobalSettings.Device.WindowsVR)
        {
            if (eventData.pointerId == _pointerID2) // for htc "ID 2" is the right controller, "ID 1" is left
            {
                if (GlobalSettings.haptics == true)
                    TriggerHapticsRight(0.2f, 0.2f);

                if (GlobalSettings.uIAudio == true)
                    UIAudio();
            }
            else
            {
                if (GlobalSettings.haptics == true)
                    TriggerHapticsLeft(0.2f, 0.2f);

                if (GlobalSettings.uIAudio == true)
                    UIAudio();
            }
        }
    }

    public void TriggerHapticsRight(float amplitude, float duration) // UI OnPointerEnter 0.1f, 0.1f / UI Button Press 0.2f, 0.2f / 3D Object Grab 0.3f, 0.2f
    {
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, _foundControllers);
        foreach (var device in _foundControllers)
        {
            if (device.TryGetHapticCapabilities(out HapticCapabilities capabilities))
            {
                if (capabilities.supportsImpulse)
                {
                    device.SendHapticImpulse(0, amplitude, duration);
                }
            }
        }
    }

    public void TriggerHapticsLeft(float amplitude, float duration) // UI OnPointerEnter 0.1f, 0.1f / UI Button Press 0.2f, 0.2f / 3D Object Grab 0.3f, 0.2f
    {
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left, _foundControllers);
        foreach (var device in _foundControllers)
        {
            if (device.TryGetHapticCapabilities(out HapticCapabilities capabilities))
            {
                if (capabilities.supportsImpulse)
                {
                    device.SendHapticImpulse(0, amplitude, duration);
                }
            }
        }
    }

    public void UIAudio()
    {
        if (GlobalSettings.uIAudio == true/* || SceneManager.GetActiveScene().buildIndex == 0*/)
        {
            if (_audioSource != null)
                _audioSource.Play();
        }
    }
}