using UnityEngine;

public class Painter : MonoBehaviour
{
    [SerializeField]
    private PaintMode paintMode;

    [SerializeField]
    private Transform paintingTransform;

    [SerializeField]
    public float raycastLength = 0.01f;

    [SerializeField]
    private Texture2D brush;

    [SerializeField]
    private float spacing = 1f;
    
    private float currentAngle = 0f;
    private float lastAngle = 0f;

    private PaintReceiver paintReceiver;
    private Collider paintReceiverCollider;


    private Stamp stamp;

    private Color color;

    private Vector2? lastDrawPosition = null;

    [System.NonSerialized]
    public bool isDrawn = false;
    

    public void Initialize(PaintReceiver newPaintReceiver)
    {
        stamp = new Stamp(brush);
        stamp.mode = paintMode;

        if (!newPaintReceiver)
            return;

        paintReceiver = newPaintReceiver;
        paintReceiverCollider = newPaintReceiver.GetComponent<Collider>();
    }

    public void Update()
    {
        currentAngle = -transform.rotation.eulerAngles.z;

        Ray ray = new Ray(paintingTransform.position, paintingTransform.forward);
        RaycastHit hit;

        //Debug.DrawRay(ray.origin, ray.direction * raycastLength);

        if (!paintReceiverCollider)
            return;

        if (paintReceiverCollider.Raycast(ray, out hit, raycastLength))
        {
            if (lastDrawPosition.HasValue && lastDrawPosition.Value != hit.textureCoord)
            {
                paintReceiver.DrawLine(stamp, lastDrawPosition.Value, hit.textureCoord, lastAngle, currentAngle, color, spacing);

                //set the bool to-> the player is drawing.
                isDrawn = true;
            }

            else
            {
                //use this if you want to update every frame when someone doesnt even draw.
                //paintReceiver.CreateSplash(hit.textureCoord, stamp, color, currentAngle);
            }

            lastAngle = currentAngle;

            lastDrawPosition = hit.textureCoord;
        }
        else
        {
            lastDrawPosition = null;
            isDrawn = false;
        }
    }

    public void ChangeColour(Color newColor)
    {
        color = newColor;
    }

    public void SetRotation(float newAngle)
    {
        currentAngle = newAngle;
    }


}
