using UnityEngine;

public class BuffetPlaceManager : MonoBehaviour
{
    public void CheckTriggerNames(GameObject sourceObject, string objectName, string triggerName)
    {
        if (objectName == triggerName)
        {
            sourceObject.GetComponent<BuffetPlaceTrigger>().ActivateResult();
        }
        else
        {
            sourceObject.GetComponent<BuffetPlaceTrigger>().ResultWrong();
        }
    }
}
