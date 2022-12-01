using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Vuplex.WebView;


namespace AvatarCreationHandTracking
{
    //Do We need this ?
    public class GestureWebsiteRecognizer : MonoBehaviour
    {
        public GestureUIPointer gestureUiPointer;
        public RectTransform canvasRect;
        //public CanvasWebViewPrefab canvasWebview;
        public bool isPressed = false;

        //private Variable
        Vector2 canvasOffsetMax;
        float pixelX, pixelY;

        [Header("Debug variable")]
        public bool debug;


        private void Start()
        {
        }

        /*
        private void Update()
        {
            //enable this in the editor to simulate click using keycode.
            if (debug && canvasWebview)
            {
                if (Input.GetKey(KeyCode.J))
                {
                    var normalizedPoint = ConvertPointerToScreenPoint(gestureUiPointer.hitPointMarker.transform.position)
                                          / canvasWebview.WebView.SizeInPixels * canvasWebview.InitialResolution;
                    canvasWebview.WebView.Click(normalizedPoint);

                }
            }
        }

        public void EventPress()
        {
            if (canvasWebview)
            {
                canvasWebview.DragMode = DragMode.Disabled;
                isPressed = true;

            }
        }

        public void EventRelease()
        {
            if (isPressed)
            {
                //rearm the pressing again
                isPressed = false;
                var normalizedPoint = ConvertPointerToScreenPoint(gestureUiPointer.hitPointMarker.transform.position)
                                      / canvasWebview.WebView.SizeInPixels * canvasWebview.InitialResolution;
                canvasWebview.WebView.Click(normalizedPoint);
            }
        }

        float minDiff = 0.035f;
        float diff;
        bool press = false;
        public void EventPointing()
        {
            if (!canvasWebview)
                return;

            diff = (gestureUiPointer.lineRenderer.GetPosition(0) - gestureUiPointer.lineRenderer.GetPosition(1)).magnitude;
            //print("currentRay " + (gestureUiPointer.lineRenderer.GetPosition(0) - gestureUiPointer.lineRenderer.GetPosition(1)).magnitude);

            if (diff <= minDiff)
            {
                //then trigger press event
                canvasWebview.DragMode = DragMode.Disabled;
                //press = true;
                var normalizedPoint = ConvertPointerToScreenPoint(gestureUiPointer.hitPointMarker.transform.position)
                                      / canvasWebview.WebView.SizeInPixels * canvasWebview.InitialResolution;
                canvasWebview.WebView.Click(normalizedPoint);
            }

            else if (press && diff >= minDiff)
            {
                canvasWebview.DragMode = DragMode.DragToScroll;
                press = false;


            }

        }


        private void OnDisable()
        {
            if (canvasWebview)
            {
                canvasWebview.DragMode = DragMode.DragToScroll;
                isPressed = false;
                //canvasWebview = null;
            }
        }


        /// <summary>
        /// Converts the given world position to a normalized screen point.
        /// </summary>
        /// <returns>
        /// A point where the x and y components are normalized
        /// values between 0 and 1.
        /// </returns>
        public Vector2 ConvertPointerToScreenPoint(Vector3 worldPosition)
        {
            var localPosition = canvasRect.transform.InverseTransformPoint(worldPosition);

            //upper left corner start at 0,0
            canvasOffsetMax = canvasRect.offsetMax; // will return the half of the canvas

            pixelX = localPosition.x + canvasOffsetMax.x;
            pixelY = -1 * localPosition.y + canvasOffsetMax.y;
            //print("X: " + pixelX + " Y: " + pixelY);

            return new Vector2(pixelX, pixelY);
        }

        */
    }
}