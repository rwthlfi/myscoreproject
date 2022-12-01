using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.InputSystem.UI;


namespace AvatarCreationHandTracking
{
    public class GestureInputModule : BaseInputModule
    {
        private List<GestureUIPointer> pointers = new List<GestureUIPointer>();
        private PointerEventData[] eventDatas;

        GestureInputModule _instance;
        private bool _isDestroyed;

        [Header("Other input module")]
        public XRUIInputModule thisXrUi;
        public InputSystemUIInputModule thisInputSystemUI;

        public bool debugHand = false;
        public GestureInputModule Instance
        {
            get
            {
                if (_isDestroyed)
                    return null;

                if (_instance == null)
                {
                    if (!(_instance = FindObjectOfType<GestureInputModule>()))
                    {
                        _instance = new GameObject().AddComponent<GestureInputModule>();
                    }
                }

                return _instance;
            }
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnDestroy()
        {
            _isDestroyed = true;
        }


        //re-checking every X period 
        //in order to make sure that the camera is being assigned to the canvas
        private float nextActionTime = 1f;
        private float period = 2f;
        private void Update()
        {
            if (Time.time > nextActionTime)
            {
                nextActionTime += period;

                if (debugHand)
                {
                    EnableOtherInput(false);
                    return;
                }



                if (GlobalSettings.handTrackingActive())
                {
                    EnableOtherInput(false);
                }
                else
                    EnableOtherInput(true);
            }


        }

        //To enable/disable the input module...
        //cause Unfortunately the controller and the hand tracking requires diffent input module
        private void EnableOtherInput(bool _value)
        {
            if (thisXrUi)
                GetComponent<XRUIInputModule>().enabled = _value;
            if (thisInputSystemUI)
                GetComponent<InputSystemUIInputModule>().enabled = _value;
        }

        //adding pointer to the event data.
        public int AddPointer(GestureUIPointer pointer)
        {
            if (!pointers.Contains(pointer))
            {
                pointers.Add(pointer);
                eventDatas = new PointerEventData[pointers.Count];

                for (int i = 0; i < eventDatas.Length; i++)
                {
                    eventDatas[i] = new PointerEventData(eventSystem);
                    eventDatas[i].delta = Vector2.zero;
                    eventDatas[i].position = new Vector2(Screen.width / 2, Screen.height / 2);
                }
            }

            return pointers.IndexOf(pointer);
        }

        public void RemovePointer(GestureUIPointer pointer)
        {
            if (pointers.Contains(pointer))
                pointers.Remove(pointer);
            foreach (var point in pointers)
            {
                point.SetIndex(pointers.IndexOf(point));
            }
            eventDatas = new PointerEventData[pointers.Count];
            for (int i = 0; i < eventDatas.Length; i++)
            {
                eventDatas[i] = new PointerEventData(eventSystem);
                eventDatas[i].delta = Vector2.zero;
                eventDatas[i].position = new Vector2(Screen.width / 2, Screen.height / 2);
            }
        }

        public override void Process()
        {
#pragma warning disable
            for (int index = 0; index < pointers.Count; index++)
            {
                try
                {
                    if (pointers[index] != null && pointers[index].enabled)
                    {
                        pointers[index].Preprocess();
                        // Hooks in to Unity's event system to handle hovering
                        
                        eventSystem.RaycastAll(eventDatas[index], m_RaycastResultCache);
                        eventDatas[index].pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);

                        HandlePointerExitAndEnter(eventDatas[index], eventDatas[index].pointerCurrentRaycast.gameObject);

                        ExecuteEvents.Execute(eventDatas[index].pointerDrag, eventDatas[index], ExecuteEvents.dragHandler);
                        
                    }

                }
                catch { }
            }
#pragma warning restore
        }

        public void ProcessPress(int index)
        {
            pointers[index].Preprocess();
            // Hooks in to Unity's event system to process a release
            eventDatas[index].pointerPressRaycast = eventDatas[index].pointerCurrentRaycast;

            eventDatas[index].pointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(eventDatas[index].pointerPressRaycast.gameObject);
            eventDatas[index].pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(eventDatas[index].pointerPressRaycast.gameObject);

            ExecuteEvents.Execute(eventDatas[index].pointerPress, eventDatas[index], ExecuteEvents.pointerDownHandler);
            ExecuteEvents.Execute(eventDatas[index].pointerDrag, eventDatas[index], ExecuteEvents.beginDragHandler);
        }

        public void ProcessRelease(int index)
        {
            pointers[index].Preprocess();
            // Hooks in to Unity's event system to process a press
            GameObject pointerRelease = ExecuteEvents.GetEventHandler<IPointerClickHandler>(eventDatas[index].pointerCurrentRaycast.gameObject);

            if (eventDatas[index].pointerPress == pointerRelease)
                ExecuteEvents.Execute(eventDatas[index].pointerPress, eventDatas[index], ExecuteEvents.pointerClickHandler);

            ExecuteEvents.Execute(eventDatas[index].pointerPress, eventDatas[index], ExecuteEvents.pointerUpHandler);
            ExecuteEvents.Execute(eventDatas[index].pointerDrag, eventDatas[index], ExecuteEvents.endDragHandler);


            eventDatas[index].pointerPress = null;
            eventDatas[index].pointerDrag = null;

            eventDatas[index].pointerCurrentRaycast.Clear();
        }


        public PointerEventData GetData(int index) { return eventDatas[index]; }
    }
}
