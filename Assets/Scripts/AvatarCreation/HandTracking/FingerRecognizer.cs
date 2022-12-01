using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRKeys;

//attach this script to each and every finger Tip available
//that means 10 fingers...., shit.. is there realy no way around it...?
namespace AvatarCreationHandTracking
{

    [RequireComponent(typeof(Collider))]  // dont forget to set it as Trigger too
    [RequireComponent(typeof(Rigidbody))] // dont forget to make it kinematic
    public class FingerRecognizer : MonoBehaviour
    {
        private float cooldown = 0.5f;
        private bool allowType = true;
        public Key currentKey;

        public bool useEvent = false;

        private void OnTriggerEnter(Collider other)
        {
            //if allow typing and it does hit the key
            if (other.GetComponent<Key>())
            {
                //cache the key
                currentKey = other.GetComponent<Key>();
                //if no using event, then click it directly
                if (!useEvent)
                {
                    currentKey.PressKey();
                }
                
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<Key>())
                currentKey = other.GetComponent<Key>();

        }


        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Key>() == currentKey)
            {
                currentKey = null;
                //allowType = true;
                //StartCoroutine(TypeCooldown());
            }
        }

    
        private void OnDisable()
        {
            allowType = true;
            currentKey = null;
        }


        //attach this function to the Event press on the HandGesture
        public void EventPress()
        {
            if(currentKey && allowType)
            {
                currentKey.PressKey();
                StartCoroutine(TypeCooldown());
            }
        }

        //attach this function to the Event release on the HandGesture
        public void EventRelease()
        {
            allowType = true;
        }

        private IEnumerator TypeCooldown()
        {
            allowType = false;
            yield return new WaitForSeconds(cooldown);
            allowType = true;
        }
    }

}
