using UnityEngine;

public class Eraser : MonoBehaviour
{
    [SerializeField]
    private Color color;

    [SerializeField]
    public Painter painter;

    [SerializeField]
    public PaintReceiver paintReceiver;

    public LayerMask ValidLayers;
    public string tagName = "Painter";

    void Awake()
    {
        painter.Initialize(paintReceiver);
        painter.ChangeColour(color);
    }

    public virtual void LateUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(painter.transform.position, painter.transform.forward, out hit, painter.raycastLength, ValidLayers, QueryTriggerInteraction.Ignore))
        {
            if(hit.transform.gameObject.tag == tagName && hit.transform.gameObject != paintReceiver)
            {
                painter.Initialize(hit.transform.GetComponent<PaintReceiver>());
                painter.ChangeColour(color);
                paintReceiver = hit.transform.GetComponent<PaintReceiver>();
            }

        }
    }
}
