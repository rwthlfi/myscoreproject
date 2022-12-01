using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace penAndPaper
{
    public enum hierarchy
    {
        childIdx, 
        totalCoor,
        lineW,
        lineC,
        lineCoorXZ
    }

    public class Paper2 : MonoBehaviour
    {
        public Transform mainT;
        [Header("Container")]
        public Transform lineContainer;

        [Header("Drawing Variable")]
        public LineRenderer lr;
        public int currIdx = 0; // this is the index position XYZ of the line renderer
        public Transform pointHelper;
        public SimpleObjectPool linePool;

        [Header("Drawing Visual")]
        public float lineWidth = 0.001f; // max 0.025f
        public Color lineColor = Color.black;

        [Header("Cache Variable")]
        public Pen2 currPen;
        char seperator = '|';

        [Header("just debugging")]
        
        public bool hit;
        public string lineValTest = "";
        
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("scaling paper" + this.transform.localScale);
        }

        // Update is called once per frame
        void Update()
        {
            /*
            if (hit)
            {
                hit = false;

                //to get everything
                //print(getAllLineInfo());
                AddAllLineRend(lineValTest);
            }
            */
        }


        /// <summary>
        /// Draw the 2D.
        /// </summary>
        public void Draw2D()
        {
            //if there is no line renderer assign them
            lr = lineContainer.GetChild(lineContainer.childCount-1).GetComponent<LineRenderer>();

            currIdx++;
            lr.positionCount = currIdx + 1;
            lr.SetPosition(currIdx, getPointHelperLocPos());

            //set color and  set the width
            SetLineWidth(lr, lineWidth);
            SetLineColor(lr, lineColor);

        }



        /// <summary>
        /// add new line renderer
        /// </summary>
        public void AddNewLineRend()
        {
            //there will be no line renderer first
            //afterwards if there is curr Pen
            GameObject newLine = linePool.GetObject();

            newLine.transform.SetParent(lineContainer, false);
            newLine.transform.localPosition = Vector3.zero;
            newLine.transform.localEulerAngles = Vector3.zero;
            newLine.transform.localScale = Vector3.one;
        }



        /// <summary>
        /// add a new line renderer
        /// </summary>
        /// <param name="_str">the string that contains all the data points</param>
        /// <param name="_continuePos">to add init line renderer at which position</param>
        private void AddNewLineRend(string _str, int _continuePos)
        {
            //split the string
            string[] info = _str.Split(seperator);

            //the ZERO is position index of the child
            //check if there is no line render of that child Position, add it
            int childIdx = ConverterFunction.StringToInt(info[(int)hierarchy.childIdx]);
            if (lineContainer.childCount <= childIdx)
                AddNewLineRend();

            LineRenderer l = lineContainer.GetChild(childIdx).GetComponent<LineRenderer>();

            //the FIRST is the total position count
            int count = ConverterFunction.StringToInt(info[(int)hierarchy.totalCoor]);
            l.positionCount = count;

            //the SECOND is the line width
            float lw = ConverterFunction.StringToFloat(info[(int)hierarchy.lineW]);
            SetLineWidth(l, lw);

            //the THIRD is the color
            Color c = ConverterFunction.hexToColor(info[(int)hierarchy.lineC]);
            SetLineColor(l, c);


            float x = 0f, y = 0f;
            //the rest just coordinate in X and Z
            for(int i = _continuePos; i < count; i++)
            {
                x = ConverterFunction.StringToFloat(info[(int) hierarchy.lineCoorXZ + 2*i]);
                y = ConverterFunction.StringToFloat(info[(int) hierarchy.lineCoorXZ + 2*i +1 ]);
                SetCoordinate(l, i, x, y);
            }
        }








        /// <summary>
        /// to initialize all the line Renderer
        /// </summary>
        /// <param name="_allStr"></param>
        public void AddAllLineRend(string _allStr)
        {
            string[] sArray = _allStr.Split('~');

            //choose where to start initializing
            // -> therefore reducing unnecessary init of the Line Renderer
            int start = lineContainer.childCount -1 ;
            if (start < sArray.Length)
            {
                for (int i = start; i < sArray.Length; i++)
                {
                    //if nothing there just ignore them
                    if (sArray[i] == "")
                        continue;

                    AddNewLineRend(sArray[i], 0);
                    /*
                    //also compare where to start with the line renderer
                    if (sArray[i].Split(seperator).Length <= 1)
                    {
                    }
                    */
                }
            }

            else
            {

                print("b");
                //otherwise start from that index
                foreach (string s in sArray)
                {
                    if (s == "")
                        continue;
                    AddNewLineRend(s, 0);
                }
            }
            
        }


        #region ---- setting up the point Helper

        //to set the line color
        public void SetLineColor(Color _color)
        {
            //print("color " + _color);
            lineColor = _color;
        }

        //to set the line color
        public void SetLineColor(LineRenderer lr, Color _color)
        {
            lr.startColor = _color;
            lr.endColor = _color;
        }

        //to set the line width
        public void SetLineWidth(float _width)
        {
            //print("width " + _width);
            lineWidth = _width;
        }

        //to set the line width
        public void SetLineWidth(LineRenderer lr, float _width)
        {
            lr.startWidth = _width;
            lr.endWidth = _width;
        }

        //set the coordinate of a line renderer
        public void SetCoordinate(LineRenderer lr, int _pos, float _x, float _z)
        {
            lr.SetPosition(_pos, new Vector3(_x, 0, _z));
        }


        //get where the point helper is.
        public Vector3 getPointHelperLocPos()
        {
            return pointHelper.localPosition;
        }

        //set the position of the point helper.
        public void SetPointHelperPos(Vector3 _pos)
        {
            pointHelper.position = _pos;
        }

        //detect if there is a pen
        public bool hasPen()
        {
            if (currPen != null)
                return true;
            else
                return false;
        }
        public void ResetPaperConf()
        {
            currIdx = -1;
            AddNewLineRend();
            currPen = null;
        }


        public int getTotalLine()
        {
            return lineContainer.childCount;
        }

        /// <summary>
        /// to get all the line info including its children
        /// </summary>
        /// <returns></returns>
        public string getAllLineInfo()
        {
            string str = "";
            int i = 0;
            foreach (LineRenderer l in lineContainer.GetComponentsInChildren<LineRenderer>())
            {
                str += getLineInfo(i);
                i++;
            }

            return str;
        }


        //get the line info at certain index.
        public string getLineInfo(int _idx)
        {
            string str = "";
            LineRenderer l = lineContainer.GetChild(_idx).GetComponent<LineRenderer>();

            //read the total position
            int count = l.positionCount;

            str += _idx + seperator.ToString();
            str += count + seperator.ToString();
            
            str += l.startWidth.ToString("f2")+ seperator.ToString();
            str += ConverterFunction.colorToHex(l.startColor) + seperator.ToString();
            

            /*
            str += lineWidth + seperator.ToString();
            str += ConverterFunction.colorToHex(lineColor) + seperator.ToString();
            */
            
            //read all the position coordinate.
            for (int i = 0; i < count; i++)
            {
                str += l.GetPosition(i).x.ToString("f3") + seperator.ToString()
                     + l.GetPosition(i).z.ToString("f3") + seperator.ToString();
            }

            str += "~";

            return str;
        }
        #endregion


        public void ui_changeSize_Hor(Slider _s)
        {
            mainT.localScale = new Vector3(_s.value, mainT.localScale.y, mainT.localScale.z);
        }

        public void ui_changeSize_Ver(Slider _s)
        {
            mainT.localScale = new Vector3(mainT.localScale.x, mainT.localScale.y, _s.value);
        }
    }


}
