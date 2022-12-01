using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace AvatarCreationHandTracking
{
    public class GestureUIPointer : MonoBehaviour
    {
        [Header("References")]
        public GameObject hitPointMarker;
        public LineRenderer lineRenderer;
        public EventSystem eventSystem;


        [Header("Ray settings")]
        public float raycastLength = 8.0f;
        public bool autoShowTarget = true;
        public LayerMask UILayer;


        [Header("Events")]
        public UnityEvent StartSelect;
        public UnityEvent StopSelect;
        public UnityEvent StartPoint;
        public UnityEvent StopPoint;

        /*
        [Header("Raycasting canvas Variable")]
        [Header("Raycasting canvas Variable")
        public RectTransform currentCanvas;
        Vector2 canvasOffsetMax;
        float pixelX, pixelY;
        */


        // Internal variables
        private bool hover = false;
        public GestureInputModule inputModule;


        static Camera cam;

        int pointerIndex;

        private void InitInputModule()
        {
            if (cam == null)
            {
                cam = new GameObject("Camera Canvas Pointer").AddComponent<Camera>();
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.stereoTargetEye = StereoTargetEyeMask.None;
                cam.orthographic = true;
                cam.orthographicSize = 0.001f;
                cam.cullingMask = 0;
                cam.nearClipPlane = 0.01f;
                cam.depth = 0f;
                cam.allowHDR = false;
                cam.enabled = false;
                cam.fieldOfView = 0.00001f;
                cam.stereoTargetEye = StereoTargetEyeMask.None;
                cam.gameObject.AddComponent<CanvasCameraPointerReference>();

                SetCameraToEveryCanvases();
            }

            if (inputModule.Instance != null)
                pointerIndex = inputModule.Instance.AddPointer(this);
        }

        void OnDisable()
        {
            //inputModule.Instance?.RemovePointer(this);
            RemoveCameraToEveryCanvases();
        }

        private void SetCameraToEveryCanvases()
        {
            foreach (var canvas in FindObjectsOfType<Canvas>(true))
            {
                canvas.worldCamera = cam;
            }
        }

        private void RemoveCameraToEveryCanvases()
        {
            foreach (var canvas in FindObjectsOfType<Canvas>(true))
            {
                canvas.worldCamera = null;
            }
        }

        public void SetIndex(int index)
        {
            pointerIndex = index;
        }

        internal void Preprocess()
        {
            cam.transform.position = transform.position;
            cam.transform.forward = transform.forward;
        }

        public void Press()
        {
            // Handle the UI events
            inputModule.ProcessPress(pointerIndex);

            // Show the ray when they attemp to press
            if (!autoShowTarget && hover) ShowRay(true);

            // Fire the Unity event
            StartSelect?.Invoke();
        }

        public void Release()
        {
            // Handle the UI events
            inputModule.ProcessRelease(pointerIndex);

            // Fire the Unity event
            StopSelect?.Invoke();
        }



        bool isInit = false;
        private void Start()
        {
            if (lineRenderer == null)
                gameObject.CanGetComponent(out lineRenderer);

            if (inputModule == null)
            {
                if (gameObject.CanGetComponent<GestureInputModule>(out var inputMod))
                {
                    inputModule = inputMod;
                }
                else if (!(inputModule = FindObjectOfType<GestureInputModule>()))
                {
                    EventSystem system;
                    if (!(system = FindObjectOfType<EventSystem>()))
                    {
                        system.name = "UI Input Event System";
                        system = new GameObject().AddComponent<EventSystem>();
                    }
                    inputModule = system.gameObject.AddComponent<GestureInputModule>();
                }
            }

            InitInputModule();
            isInit = true;
        }

        private void Update()
        {
            if (isInit)
            {
                UpdateLine();
                //canvasExist();
            }
        }

        //For updating line pointer
        private void UpdateLine()
        {
            PointerEventData data = inputModule.GetData(pointerIndex);
            float targetLength = data.pointerCurrentRaycast.distance == 0 ? raycastLength : data.pointerCurrentRaycast.distance;

            if (data.pointerCurrentRaycast.distance != 0 && !hover)
            {
                // Fire the Unity event
                StartPoint?.Invoke();

                // Show the ray if autoShowTarget is on when they enter the canvas
                if (autoShowTarget) ShowRay(true);

                hover = true;
            }
            else if (data.pointerCurrentRaycast.distance == 0 && hover)
            {
                // Fire the Unity event
                StopPoint?.Invoke();

                // Hide the ray when they leave the canvas
                ShowRay(false);

                hover = false;
            }

            RaycastHit hit = CreateRaycast(targetLength);

            Vector3 endPosition = transform.position + (transform.forward * targetLength);

            if (hit.collider) endPosition = hit.point;

            //Handle the hitmarker
            hitPointMarker.transform.position = endPosition;

            //Handle the line renderer
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, endPosition);
        }


        //creating raycasting
        private RaycastHit CreateRaycast(float dist)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, transform.forward);
            Physics.Raycast(ray, out hit, dist, UILayer);

            return hit;
        }

        public void ShowRay(bool show)
        {
            hitPointMarker.SetActive(show);
            lineRenderer.enabled = show;
        }



        /// <summary>
        /// Check if the pointer is entering a canvas. Afterwards assign them once. and de-assign them after pointer leave canvas
        /// </summary>
        /// <returns></returns>

        /*
        private RectTransform canvasExist()
        {
            //When entering ui...
            //it will detect UI child object (such as Button, image and stuff)
            PointerEventData data = inputModule.GetData(pointerIndex);
            if (!data.enterEventCamera) // if it doesnt detect any canvas, forget it.
            {
                //remove the canvas. and free up the memory.
                currentCanvas = null;
                return null;
            }
            else
            {
                if (!currentCanvas) // just to check if canvas already assign.. otherwise assign it.
                    currentCanvas = data.pointerEnter.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            }

            return currentCanvas;
        }
        */


        /// <summary>
        /// Converts the given world position to a normalized screen point.
        /// </summary>
        /// <returns>
        /// A point where the x and y components are normalized
        /// values between 0 and 1.
        /// </returns>
        /*
        public Vector2 ConvertToScreenPoint(Vector3 worldPosition)
        {
            if (!currentCanvas)
                return Vector2.zero;

            var localPosition = currentCanvas.transform.InverseTransformPoint(worldPosition);

            //upper left corner start at 0,0
            canvasOffsetMax = currentCanvas.offsetMax; // will return the half of the canvas

            pixelX = localPosition.x + canvasOffsetMax.x;
            pixelY = -1 * localPosition.y + canvasOffsetMax.y;
            //print("X: " + pixelX + " Y: " + pixelY);

            return new Vector2(pixelX, pixelY);
        }
        */
    }
}
