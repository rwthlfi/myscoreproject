using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLookAtCamera : MonoBehaviour
{
    public Camera _mainCamera;
    public float damping = 3f;
    private bool _active = false;

    public bool VerticalLook;

    // Use this for initialization
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);

        _mainCamera = Camera.main;
        _active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_active)
        {
            //null reference check
            if (!_mainCamera)
            {
                return;
            }

            //always face the camera
            //we want to transfer the position of the canvas
            if (!VerticalLook)
            {
                var var = Quaternion.LookRotation(_mainCamera.transform.rotation * Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, var, Time.deltaTime * damping);

            }

            //for vertical looks
            else
            {
                //Debug.Log("Vertical");
                Quaternion var = Quaternion.LookRotation(_mainCamera.transform.rotation * Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, var, Time.deltaTime * damping);
                transform.rotation = Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, 0f));

                //transform.rotation = Quaternion.Slerp(var_Y, var, Time.deltaTime * damping);
            }


            /*
            //this one is without damping.
            transform.LookAt( transform.position + main_camera.transform.rotation * Vector3.forward
                            , main_camera.transform.rotation * Vector3.up);
            */
        }
    }
}