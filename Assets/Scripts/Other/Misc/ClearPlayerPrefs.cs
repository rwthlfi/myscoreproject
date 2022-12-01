using UnityEditor;
using UnityEngine;

public class ClearPlayerPrefs : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void ClearPlayerPrefabs()
    {
        PlayerPrefs.DeleteAll();
    }
}