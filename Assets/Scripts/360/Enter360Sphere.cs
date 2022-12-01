using AvatarCreation;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enter360Sphere : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject VRGround;
    public GameObject VRTerrain;
    Renderer m_Renderer;
    public Vector3 upScaleVector3 = new Vector3(5, 5, 5);
    private MeshRenderer _groundMeshRenderer;
    private Terrain _terrainRenderer;
    public Vector3 initScale = new Vector3(.75f, .75f, .75f);
    public string uRL;
    private byte[] bytes;
    public bool isOffline;
    public Texture offlineTexture;
    public float yValue = 1, yValueOrigin, xValueOrigin, zValueOrigin;

    private void Start()
    {
        yValueOrigin = transform.parent.gameObject.transform.localPosition.y;
        xValueOrigin = transform.parent.gameObject.transform.localPosition.x;
        zValueOrigin = transform.parent.gameObject.transform.localPosition.z;
        m_Renderer = GetComponent<Renderer>();

        // get ground of scene to make invisible when inside sphere
        if (VRGround != null)
            _groundMeshRenderer = VRGround.GetComponent<MeshRenderer>();
        if (VRTerrain != null)
            _terrainRenderer = VRTerrain.GetComponent<Terrain>();

        if (isOffline)
            SetNewTextureOffline(offlineTexture);

    }

    private void OnTriggerEnter(Collider other)
    {
        // check the current local player, so no other players trigger the spheres
        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
            if ((other.gameObject.layer == 6 && other.isTrigger == false && other.GetComponent<NetworkPlayerSetup>().isLocalPlayer)) // online
            {
                EnterSphere();
            }
        }
        else if (other.gameObject.layer == 6 && other.isTrigger == false) // offline
        {
            EnterSphere();
        }
    }

    private void EnterSphere()
    {
        // make ground invisible
        if (VRGround != null)
            _groundMeshRenderer.enabled = false;
        if (VRTerrain != null)
            _terrainRenderer.enabled = false;

        // size up the sphere
        transform.localScale = upScaleVector3;
        transform.parent.gameObject.transform.localPosition = new Vector3(xValueOrigin, yValue, zValueOrigin);
        Canvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        // check the current local player, so no other players trigger the spheres
        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
            if ((other.gameObject.layer == 6 && other.isTrigger == false && other.GetComponent<NetworkPlayerSetup>().isLocalPlayer)) // online
            {
                ExitSphere();
            }
        }
        else if (other.gameObject.layer == 6 && other.isTrigger == false) // offline
        {
            ExitSphere();
        }
    }

    private void ExitSphere()
    {
        // scale down the sphere to init level
        Canvas.SetActive(false);
        transform.localScale = initScale;
        transform.parent.gameObject.transform.localPosition = new Vector3(xValueOrigin, yValueOrigin, zValueOrigin);

        // make ground visible again
        if (VRGround != null)
            _groundMeshRenderer.enabled = true;
        if (VRTerrain != null)
            _terrainRenderer.enabled = true;
    }

    public void SetNewTexture(string imageDownloadPath, string textureName) // sets the downloaded texture from resource folder if available
    {
        if (m_Renderer == null)
        {
            m_Renderer = GetComponent<Renderer>();
        }

        if (GlobalSettings.DeviceType() == GlobalSettings.Device.Android)
        {
            Texture2D texture = new Texture2D(2, 2);

            bytes = File.ReadAllBytes(Application.persistentDataPath + "/" + imageDownloadPath + "/" + textureName);

            texture.LoadImage(bytes);
            m_Renderer.material.mainTexture = texture;
        }
        else
        {
            m_Renderer.material.mainTexture = (Texture2D)Resources.Load(imageDownloadPath + "/" + textureName);
        }
    }

    public void SetNewTextureOnlineSource(Texture texture) // sets the downloaded texture directly after download
    {
        if (m_Renderer == null)
        {
            m_Renderer = GetComponent<Renderer>();
        }

        m_Renderer.material.mainTexture = texture;
    }

    public void SetNewTextureOffline(Texture texture) // sets the downloaded texture from resource folder if available
    {
        m_Renderer.material.mainTexture = texture;
    }
}