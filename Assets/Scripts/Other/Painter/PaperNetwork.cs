using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

namespace penAndPaper
{

    public class PaperNetwork : NetworkBehaviour
    {
        public Transform mTransform;
        public Paper2 paper;

        [Header("Paper Variable")]
        [SyncVar(hook = nameof(OnDrawn))]
        public string drawData;

        [SyncVar(hook = nameof(OnSizeHorChange))]
        public float scaleHor;

        [SyncVar(hook = nameof(OnSizeVerChange))]
        public float scaleVer;


        void OnDrawn(string _old, string _new)
        {
            paper.AddAllLineRend(_new);
        }

        void OnSizeHorChange(float _old, float _new)
        {
            mTransform.localScale = new Vector3(_new, mTransform.localScale.y, mTransform.localScale.z);
            //print("size hor Change " + _new);
        }

        void OnSizeVerChange(float _old, float _new)
        {
            mTransform.localScale = new Vector3(mTransform.localScale.x, mTransform.localScale.y, _new);
            //print("size ver Change " + _new);
        }




        private float nextActionTime = 0.0f;
        private float period = 0.1f;
        private void Update()
        {
            if (Time.time > nextActionTime)
            {
                nextActionTime = Time.time + period;
                //execute the code here

                if (isServer)
                {
                    ServerChangeDrawData(paper.getAllLineInfo());
                }

                /*
                print("paper2 " + paper.transform.localScale);
                print("line rend Container " + paper.lineContainer.transform.localScale);
                print("draw data " + drawData);

                foreach (LineRenderer l in paper.lineContainer.GetComponentsInChildren<LineRenderer>())
                {
                    print("lineChild scale " + l.transform.localScale);
                    print("lineWidth " + l.startWidth + " & " + l.endWidth);
                }
                */
            }
        }


        //change the url from client's input to the server
        [Server]
        public void ServerChangeDrawData(string _data)
        {
            drawData = _data;
            //print("_data " + _data.Length);
        }



        #region - - - -  scale of hor and Ver


        public void Ui_ChangeHorSize(Slider _s)
        {
            Cmd_ChangeHorSize(_s.value);

        }

        [Command(requiresAuthority = false)]
        public void Cmd_ChangeHorSize(float _f)
        {
            scaleHor = _f;
            mTransform.localScale = new Vector3(scaleHor, mTransform.localScale.y, mTransform.localScale.z);
        }

        public void Ui_ChangeVerSize(Slider _s)
        {
            Cmd_ChangeVerSize(_s.value);
        }

        [Command(requiresAuthority =false)]
        public void Cmd_ChangeVerSize(float _f)
        {
            scaleVer = _f;
            mTransform.localScale = new Vector3(mTransform.localScale.x, mTransform.localScale.y, scaleVer);
        }

        #endregion
    }

}
