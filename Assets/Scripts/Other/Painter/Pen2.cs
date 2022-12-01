using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace penAndPaper
{
    public class Pen2 : MonoBehaviour
    {
        [Header("Enabler")]
        public bool isLocalUse;

        [Header("Raycasting")]
        public RaycastingExt rayExt;
        public Paper2 currPaper;

        [Header("Pen Variable")]
        public Color penColor;
        public float penWidth;
        public ColorPicker colPicker;


        //for cache-ing
        private Vector3 cachePos;


        // Start is called before the first frame update
        void Start()
        {
            cachePos = this.transform.position;
            penColor = colPicker.color;
        }

        private float nextActionTime = 0.0f;
        private float period = 0.05f;
        // Update is called once per frame
        void Update()
        {
            if (!isLocalUse)
                return;

            
            if (Time.time > nextActionTime)
            {
                nextActionTime = Time.time + period;
                //execute the code here

                if (currPaper != null && rayExt.isHit && rayExt.hitTarget != currPaper.gameObject)
                {
                    //reset the previous one first
                    currPaper.ResetPaperConf();
                    currPaper.SetPointHelperPos(rayExt.endDistance);

                    //assign a new subject and the reset it.
                    currPaper = rayExt.hitTarget.GetComponent<Paper2>();
                    currPaper.ResetPaperConf();
                    currPaper.SetPointHelperPos(rayExt.endDistance);
                    //print("a");
                    //return;
                }

                //if there is raycast and the point helper is far enough 
                if (rayExt.isHit && isPointHelperFarEnough())
                {
                    //update the cache position
                    cachePos = this.transform.position;

                    //set the paper variable first
                    if (!currPaper)
                    {
                        currPaper = rayExt.hitTarget.GetComponent<Paper2>();
                        //if still its not finding anything, search the children

                        //print("b " + rayExt.hitTarget.name);
                    }


                    //if the paper is set up., start drawing.
                    else
                    {
                        //if the current paper doesnt register any pen yet
                        //and is not this pen, then register this pen
                        if (!currPaper.hasPen() && !isThisPen())
                        {
                            currPaper.currPen = this;
                            currPaper.SetPointHelperPos(rayExt.endDistance);
                            //print("c");
                        }


                        else if(isThisPen())
                        {
                            //set the point helper (start drawing)
                            currPaper.SetPointHelperPos(rayExt.endDistance);
                            currPaper.SetLineColor(penColor);
                            currPaper.SetLineWidth(penWidth);
                            currPaper.Draw2D();
                            // print("e " + rayExt.hitTarget.name);

                        }
                    }

                    //if nothing being hit, make it as null therefore leaving some space for other pen.
                    if (!rayExt.isHit && currPaper.hasPen())
                    {
                        currPaper.ResetPaperConf();
                        currPaper.SetPointHelperPos(rayExt.endDistance);
                        currPaper = null;
                        //print("f");
                    }
                }

                //but if it doesnt hit anything and there is still paper., remove everything
                else if(!rayExt.isHit && currPaper)
                {
                    if (currPaper.hasPen())
                        currPaper.ResetPaperConf();
                    //print("g");

                    currPaper = null;
                }
            }
        }





        # region --- for helper variable

        private bool isPointHelperFarEnough()
        {
            if (Vector3.Distance(cachePos, this.transform.position) > 0.001f)
                return true;

            else
                return false;
        }

        private bool isThisPen()
        {
            if(currPaper.currPen == this)
                return true;
            else
            return false;
        }

        #endregion


        public void Ui_ChangeLineWidth(Slider _slider)
        {
            penWidth = _slider.value;
        }

    }
}