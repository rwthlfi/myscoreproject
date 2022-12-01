using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRKeys;

public class RaycastingKeyboard : MonoBehaviour
{
    private SphereLauncher sphereLauncher;
    public float MaxRange = 25f;
    public LayerMask ValidLayers;

    [System.NonSerialized] public bool allowClick = true;


    // Start is called before the first frame update
    void Start()
    {
        sphereLauncher = GetComponent<SphereLauncher>();
    }


    void LateUpdate()
    {
        //after the user dont press anymore, the re-enable the clicking.
        if (!sphereLauncher.isPressed)
            allowClick = true;

        //the user press, but not hit anything disable the clicking
        if (sphereLauncher.isPressed && allowClick)
        {
            RaycastHit hit;

            //if it doesnt hit anything, disable the clicking.
            if (!Physics.Raycast(transform.position, transform.up, out hit, MaxRange, ValidLayers, QueryTriggerInteraction.Ignore))
            {
                allowClick = false;
            }
            
            else if (allowClick)
            {
                allowClick = false;
                Key vrKey = hit.transform.gameObject.GetComponent<Key>();
                if(vrKey) // null reference checker
                    vrKey.PressKey();
            }
        }
    }
}
        


