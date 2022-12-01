using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using System.Collections;

public class DrawManager : MonoBehaviour
{
    public GameObject LineObject;
    public GameObject GrabbedObject;
    public GameObject Canvas;
    public GameObject Eraser;
    public GameObject Pen;
    public GameObject ButtonPen;
    public GameObject ButtonEraser;
    private bool isPen = true;
    private bool TwoDDrawingOn, stillDrawing = false;
    private bool triggerExit, onStay;
    private GameObject whiteboard;
    private GameObject lineRendererParentWhiteBoard, lineRendererParentFree;
    private LineRenderer lineRenderer;
    private Vector3 lastPositionCount, lastPositionCount2;
    private Ray ray;
    private RaycastHit rayHit;

    [SerializeField] Transform DrawPointPosition; //position information of the controller that retrieves position

    //LineObject currently drawing; 
    private GameObject CurrentLineObject = null;
    public XRDirectInteractor xrDirectInteractor;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);

        if (GameObject.FindObjectOfType<RightHandControllerReference>() != null)
        {
            xrDirectInteractor = GameObject.FindObjectOfType<RightHandControllerReference>().GetComponent<XRDirectInteractor>();
        }

        lineRendererParentWhiteBoard = GameObject.Find("WhiteBoard_3D_DrawTool_Lines");
        lineRendererParentFree = GameObject.Find("Free_3D_DrawTool_Lines");

        lastPositionCount = lastPositionCount2 = Vector3.zero;
    }


    private void Update()
    {
        if (onStay)
        {
            Physics.Raycast(this.transform.position, this.transform.forward * 0.1f, out rayHit);
        }
    }

    private Transform Pointer
    {
        get
        {
            return DrawPointPosition;
        }
    }

    public void StartDrawingErasing()
    {
        if (xrDirectInteractor && xrDirectInteractor.selectTarget)
        {
            var pointer = Pointer;

            if (pointer == null)
            {
                return;
            }

            if (isPen)
            {
                if (CurrentLineObject == null)
                {
                    //generate LineObject 
                    CurrentLineObject = Instantiate(LineObject, Vector3.zero, Quaternion.identity);
                }

                if (TwoDDrawingOn) // this defines if the lines are paired to the board of free 3d parent object
                {
                    CurrentLineObject.transform.parent = lineRendererParentWhiteBoard.transform;
                }
                else
                {
                    CurrentLineObject.transform.parent = lineRendererParentFree.transform;
                }

                //get the LineRenderer component from the game object 
                lineRenderer = CurrentLineObject.GetComponent<LineRenderer>();

                //get the size of the Positions from LineRenderer 
                int NextPositionIndex = lineRenderer.positionCount;

                //increase the size of the Positions of LineRenderer 
                lineRenderer.positionCount = NextPositionIndex + 1;

                if (TwoDDrawingOn)
                {
                    //add the position information of the current controller to Positions of LineRenderer 
                    if (rayHit.transform.gameObject.tag == "Whiteboard" && rayHit.transform.gameObject != null)
                    {
                        var whiteboardvector = rayHit.point; //new Vector3(whiteboard.transform.position.x, pointer.position.y, pointer.position.z);
                        lineRenderer.SetPosition(NextPositionIndex, whiteboardvector);
                    }
                }
                else
                {
                    //add the position information of the current controller to Positions of LineRenderer 
                    lineRenderer.SetPosition(NextPositionIndex, pointer.position);
                }

                if (stillDrawing)
                {
                    if (lineRenderer.positionCount >= 1)
                        lineRenderer.SetPosition(0, lastPositionCount);
                    if (lineRenderer.positionCount >= 2)
                        lineRenderer.SetPosition(1, lastPositionCount2);

                    Debug.Log("####### lastpositioncount" + lastPositionCount);
                    Debug.Log("####### lastpositioncount 2" + lastPositionCount2);
                }

                if (lineRenderer.positionCount >= 12)
                {
                    lastPositionCount = lineRenderer.GetPosition(10);
                    lastPositionCount2 = lineRenderer.GetPosition(11);

                    StopDrawing();
                }
            }

            else
            {
                var EraserScript = Eraser.GetComponent<DrawToolEraser>();
                EraserScript.startEraser();
            }
        }
        else
            StopDrawing();
    }

    public void StopDrawing()
    {
        if (CurrentLineObject != null)
        {
            stillDrawing = true;

            //Create a new mesh collider for the linerender object
            MeshCollider meshCollider = CurrentLineObject.AddComponent<MeshCollider>();
            Mesh mesh = new Mesh();
            lineRenderer.BakeMesh(mesh, true);
            meshCollider.sharedMesh = mesh;

            var lastLineRenderer = lineRenderer;

            //null if there is a line that is currently being drawn so that draw the next line. 
            CurrentLineObject = null;
        }
        else
        {
            stillDrawing = false;
        }

        if (!isPen) // stop eraser action 
        {
            var EraserScript = Eraser.GetComponent<DrawToolEraser>();
            EraserScript.stopEraser();
        }
    }

    public void activateCanvas() // Activate UI Settings via via UI call
    {
        if (xrDirectInteractor && xrDirectInteractor.selectTarget)
        {
            Canvas.transform.localScale = new Vector3(0.25f, 0.2f, 0.01f); // alternative Canvas.SetActive(true); // can lead to slight lag
        }
    }

    public void deactivateCanvas()
    {
        if (xrDirectInteractor && xrDirectInteractor.selectTarget)
        {
            Canvas.transform.localScale = new Vector3(0f, 0f, 0f); // alternative Canvas.SetActive(false); // can lead to slight lag
        }
    }

    public void switchTool() // Switch between pen and eraser via UI call
    {
        if (xrDirectInteractor && xrDirectInteractor.selectTarget)
        {
            if (isPen == true)
            {
                isPen = false;
                Pen.SetActive(false);
                Eraser.SetActive(true);
                ButtonPen.SetActive(true);
                ButtonEraser.SetActive(false);
            }
            else
            {
                isPen = true;
                Eraser.SetActive(false);
                Pen.SetActive(true);
                ButtonEraser.SetActive(true);
                ButtonPen.SetActive(false);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Whiteboard")
        {
            StopDrawing();
            whiteboard = other.gameObject;
            onStay = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Whiteboard")
        {
            TwoDDrawingOn = true;
            StartDrawingErasing();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Whiteboard")
        {
            onStay = false;
            TwoDDrawingOn = false;
            StopDrawing();
            StopDrawing();
        }
    }
}