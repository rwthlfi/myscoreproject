using System;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using System.Collections;
using Mirror;

public class MTL2_1CaliperAnimation : NetworkBehaviour
{
    public XRController controller = null;
    public GameObject caliperPivot;
    public bool isPress, meassurementObjectEnter;
    public float caliperNumber, meassurementObjectNumber;
    public Text caliperNumberText;
    public MTL2_1ManagerNetworkSync mTL2_1ManagerMTL2_1ManagerNetworkSync;
    public XRHapticsUISound xRHapticsUISound;

    // Start is called before the first frame update
    void Start()
    {
        caliperNumber = 40.00f;
        caliperNumberText.text = caliperNumber.ToString();
        meassurementObjectEnter = false;
        isPress = false;

        if (!isServer)
        {
            StartCoroutine(FindXRController());
        }
    }

    public void UseCaliper()
    {
        isPress = true;

        StartCoroutine(StartEstimationTool());
    }

    public void StopCaliper()
    {
        isPress = false;
        caliperNumber = 40.00f;
        caliperNumberText.text = caliperNumber.ToString();

        StopCoroutine(StartEstimationTool());
    }

    // Update is called once per frame
    void Update()
    {
        if (isPress)
        {
            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float pointerValue))
            {
                Cmd_StartCaliperMovement(pointerValue);
            }
        }
    }

    IEnumerator StartEstimationTool()
    {
        while (true)
        {
            if (isPress)
            {
                if (controller.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float pointerValue))
                {
                    caliperNumber = ((float)Math.Round((40 - (20 * pointerValue) + UnityEngine.Random.Range(-.15f, 0.15f)), 2));
                    Cmd_ChangeCaliperText(caliperNumber.ToString());

                    if (pointerValue >= 0.99 && meassurementObjectEnter)
                    {
                        meassurementObjectEnter = false;
                        xRHapticsUISound.TriggerHapticsRight(0.3f, 0.1f);

                        mTL2_1ManagerMTL2_1ManagerNetworkSync.GetEstimationValues(caliperNumber);

                        {
                            // only needed if it needs to be specified which object you need to measure
                            //if(meassurementObjectNumber == 1)
                            //{
                            //    Debug.Log("measure value 1");
                            //}
                            //else if (meassurementObjectNumber == 2)
                            //{
                            //    Debug.Log("measure value 2");
                            //}
                            //else if (meassurementObjectNumber == 3)
                            //{
                            //    Debug.Log("measure value 3");
                            //}
                            //else if (meassurementObjectNumber == 4)
                            //{
                            //    Debug.Log("measure value 4");
                            //}
                            //else if (meassurementObjectNumber == 5)
                            //{
                            //    Debug.Log("measure value 5");
                            //}
                        }
                    }
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator FindXRController()
    {
        yield return new WaitForSeconds(2f);

        controller = GameObject.Find("RightHand Controller").GetComponent<XRController>();
    }

    ////////////////////////////////////////////////////////////////

    [Command(requiresAuthority = false)]
    private void Cmd_StartCaliperMovement(float pointerValue)
    {
        Server_moveCaliper(pointerValue);
    }

    [Server]
    private void Server_moveCaliper(float pointerValue)
    {
        caliperPivot.transform.localPosition = new Vector3(caliperPivot.transform.localPosition.x, caliperPivot.transform.localPosition.y, Mathf.Lerp(0.1f, 0, (float)Math.Round(pointerValue, 1)));
    }

    ////////////////////////////////////////////////////////////////

    [Command(requiresAuthority = false)]
    private void Cmd_ChangeCaliperText(string caliperNumberTextValue)
    {
        Rpc_ChangeCaliperText(caliperNumberTextValue);
    }

    [ClientRpc]
    private void Rpc_ChangeCaliperText(string caliperNumberTextValue)
    {
        caliperNumberText.text = caliperNumberTextValue;
    }
}