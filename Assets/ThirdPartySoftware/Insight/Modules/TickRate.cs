using UnityEngine;

namespace Insight
{
    class TickRate : MonoBehaviour 
    {
        public int tickRate = 30;

        void Start() 
        {
            Application.targetFrameRate = tickRate;
        }
    }
}
