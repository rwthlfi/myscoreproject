using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderFish : Wander
{
    private string areaTag ="";

    public override void Start()
    {
        base.Start();
        areaTag = GlobalSettings.TagSwimable;
        rb.useGravity = true;
    }


    private void OnTriggerStay(Collider other)
    {
        //cause the OnTriggerStay will get called, even if the script is disabled
        if (!this.enabled)
            return;

        //if this fish gameobject is touching the areaTag Collider
        //allow the wandering and disable its gravity.
        if (other.gameObject.tag == areaTag )
        {
            //print("Stay - true");
            allowWander_disablePhysic(true);
        }

        
        else // otherwise, disable wandering and reenable gravity
        {
            //print("Stay - false");
            allowWander_disablePhysic(false);
        }
        
    }

    //if the fish exit the area, then disable wandering and reenable gravity
    private void OnTriggerExit(Collider other)
    {
        //cause the OnTriggerStay will get called, even if the script is disabled
        if (!this.enabled)
            return;

        if (other.gameObject.tag == areaTag)
        {
            //print("Exit - false");
            allowWander_disablePhysic(false);
        }
            
    }
    
    /// <summary>
    /// To enable the wandering, but the physic will be disabled. otherwise the wander and physic will fight for dominance.
    /// </summary>
    /// <param name="_enable">enable the wander? but physic will be disabled</param>
    private void allowWander_disablePhysic(bool _enable)
    {
        allowWander = _enable;
        rb.useGravity = !_enable; // the opposite.

        if (!_enable) // if the wander need to be disabled, then disable the wandering directly.
            StopWanderingImmediately();


    }

    
    //take the update from the inherit class.
    public override void Update()
    {
        base.Update();
       
    }
}
