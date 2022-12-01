using UnityEngine;

public class RaycastUIButtonClick : MonoBehaviour
{

    private bool isClicked = false;

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.z < 0.01f && !isClicked)
        {
            isClicked = true;
            Debug.Log("Click Button");
        }
        else
            isClicked = false;
    }
}
