using UnityEngine;

public class MTL2_1TriggerCollider : MonoBehaviour
{
    public MTL2_1CaliperAnimation mTL2_1CaliperAnimation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "MTL2.1_MeasuringCylinder")
        {
            mTL2_1CaliperAnimation.meassurementObjectEnter = true;
        }

        {
            // only needed if it needs to be specified which object you need to measure
            //if (other.gameObject.name == "MTL2.1_MeasuringCylinder(1)")
            //{
            //    mTL2_1CaliperAnimation.meassurementObjectNumber = 1;
            //}
            //else if (other.gameObject.name == "MTL2.1_MeasuringCylinder(2)")
            //{
            //    mTL2_1CaliperAnimation.meassurementObjectNumber = 2;
            //}
            //else if (other.gameObject.name == "MTL2.1_MeasuringCylinder(3)")
            //{
            //    mTL2_1CaliperAnimation.meassurementObjectNumber = 3;
            //}
            //else if (other.gameObject.name == "MTL2.1_MeasuringCylinder(4)")
            //{
            //    mTL2_1CaliperAnimation.meassurementObjectNumber = 4;
            //}
            //else if (other.gameObject.name == "MTL2.1_MeasuringCylinder(5)")
            //{
            //    mTL2_1CaliperAnimation.meassurementObjectNumber = 5;
            //}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        mTL2_1CaliperAnimation.meassurementObjectEnter = false;
        mTL2_1CaliperAnimation.meassurementObjectNumber = 0;
    }
}
