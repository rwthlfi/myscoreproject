using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;


namespace penAndPaper
{
    public class PenNetwork : NetworkBehaviour
    {
        [Header("Pen Variable")]
        [SyncVar(hook = nameof(OnColorChange))]
        public string colorData;
        
        
        [SyncVar(hook = nameof(OnLineWidthChange))]
        public float lineWidthData;

        void OnColorChange(string _old, string _new)
        {
            pen.penColor = ConverterFunction.hexToColor(_new);
            //print("change pen color " + _new);
        }

        void OnLineWidthChange(float _old, float _new)
        {
            pen.penWidth = _new;
            //print("change pen width " + _new);
        }



        public Pen2 pen;

        //public RandomizePenTest r;
        // Start is called before the first frame update
        void Start()
        {


            if (isServer)
            {
                pen.isLocalUse = true;
                pen.enabled = true;
                pen.rayExt.enabled = true;
            }

            else
            {
                pen.isLocalUse = false;
                pen.enabled = false;
                pen.rayExt.enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (isServer)
            {
                /*
                pen.penColor = ConverterFunction.hexToColor(colorData);
                pen.penWidth = lineWidthData;
                print("Pen's Color " + pen.penColor + " width " + pen.penWidth);
                */
            }
        }


        #region - - -> pen Changes variable
        public void Ui_ChangeColor()
        {
            Cmd_changeColor(ConverterFunction.colorToHex(pen.colPicker.color));
            //print("Color " + ConverterFunction.colorToHex(pen.colPicker.color));
        }

        [Command(requiresAuthority =false)]
        void Cmd_changeColor(string _str) 
        {
            colorData = _str;
            pen.penColor = ConverterFunction.hexToColor(colorData);
            //print("color Changes " + _str);
        }


        public void Ui_ChangeLineWidth(Slider _s)
        {
            Cmd_changeLineWidth(_s.value);
            //print("linewidth " + _s);
        }

        [Command(requiresAuthority = false)]
        void Cmd_changeLineWidth(float _f)
        {
            lineWidthData = _f;
            pen.penWidth = lineWidthData;
            //print("line width Changes " + _f);
        }

        #endregion

    }

}