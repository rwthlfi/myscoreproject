using UnityEngine;

public class OpenWristMenu : MonoBehaviour
{
    public GameObject Wristmenu;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "WristMenu")
        {
            Wristmenu.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "WristMenu")
        {
            Wristmenu.SetActive(false);
        }
    }
}