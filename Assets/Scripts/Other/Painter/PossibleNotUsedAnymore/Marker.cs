using UnityEngine;

public class Marker : MonoBehaviour
{
    [SerializeField]
    private Color color;
    [SerializeField]
    [Tooltip("If you leave them empty, it will use the default color in editor")]
    private ColorPicker colorPicker;


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

                //If there is a color picker
                if (colorPicker)
                    color = colorPicker.color;

                painter.ChangeColour(color);
                paintReceiver = hit.transform.GetComponent<PaintReceiver>();
            }
        }
    }



}
