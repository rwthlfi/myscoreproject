using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Globalization;

//List of converter.
public static class ConverterFunction
{

    /// <summary>
    /// Convert an String to int
    /// </summary>
    public static int StringToInt(string _value)
    {
        if (_value.Length > 6)
            return int.MaxValue; // will throw an error message since the maximum 65.536

        if (_value == "")
            return 0;

        //try parsing if the it is a valid string  
        int n;
        bool isNumeric = int.TryParse(_value, out n);
        if (!isNumeric)
            return 0;

        int a = int.Parse(_value);
        return a;
    }

    /// <summary>
    /// convert int to String
    /// </summary>
    public static string IntToString(int _value)
    {
        string result;
        result = _value.ToString();
        return result;
    }

    static readonly NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = "," };

    /// <summary>
    /// convert String to Float
    /// </summary>
    public static float StringToFloat(string _value)
    {
        if (_value == "")
            return 0;

        float a = float.Parse(_value, nfi);
        return a;
    }

    //Convert the "supposed to be readed as Boolean" to a uniform boolean value
    public static bool StrToBool(string value)
    {
        switch (value.ToLower()) // convert the string value to lower letter.
        {
            case "true":
                return true;
            case "false":
                return false;
            //--------------//    
            case "TRUE":
                return true;
            case "FALSE":
                return false;
            //--------------//
            case "t":
                return true;
            case "f":
                return false;
            //--------------//
            case "1":
                return true;
            case "0":
                return false;
            default:
                Debug.Log("You can't cast that value to a bool!");
                return false;
        }
    }



    public static int BoolToInt(bool value)
    {
        if (value)
            return 1;
        else
            return 0;
    }


    public static bool IntToBool(int value)
    {
        if (value == 0)
            return false;
        else
            return true;
    }

    public static string BoolToString(bool value)
    {
        if (value)
            return "true";
        else
            return "false";
    }

    /// <summary>
    /// To determine if a string contains some strings
    /// </summary>
    /// <param name="haystack">the full string</param>
    /// <param name="needles">the string to detect</param>
    /// <returns></returns>
    public static bool ContainsAny(string haystack, string needles)
    {
        if (haystack.Contains(needles))
            return true;
        else
            return false;
    }

    /// <summary>
    /// convert a String-Boolean to int
    /// </summary>
    public static int StrBoolToInt(string value)
    {
        switch (value.ToLower()) // convert the string value to lower letter.
        {
            case "true":
                return 1;
            case "false":
                return 0;
            //--------------//    
            case "TRUE":
                return 1;
            case "FALSE":
                return 0;
            //--------------//
            case "t":
                return 1;
            case "f":
                return 0;
            //--------------//
            case "1":
                return 1;
            case "0":
                return 0;
            default:
                Debug.Log("You can't cast that value to a bool!");
                return 0;
        }
    }




    /// <summary>
    /// convert the First chararcter to an upper string
    /// </summary>
    /// <param name="_input"></param>
    /// <returns></returns>
    public static string FirstCharToUpper(string _input)
    {
        switch (_input)
        {
            case "":
                Debug.Log("should never happened");
                return "";
            default:
                return _input.First().ToString().ToUpper() + _input.Substring(1);
        }
    }

    /// <summary>
    /// convert all char in a string to lowercase
    /// </summary>
    /// <param name="_value"></param>
    /// <returns></returns>
    public static string stringToAllLowercase(string _value)
    {
        string s = _value.ToLower();
        return s;
    }

    /// <summary>
    /// convert all char in a string to uppercase
    /// </summary>
    /// <param name="_value"></param>
    /// <returns></returns>
    public static string stringToAllUppercase(string _value)
    {
        string s = _value.ToUpper();
        return s;
    }

    /// <summary>
    /// convert color: from color to HexCode
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string colorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    /// <summary>
    /// convert the HEXcode color to RGB( this will create a value between 0 - 255)
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    /*
    public static Color hexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }

        //to create the value from (0 - 1)(the precentage value
        //instead of using the "Color" use "Color32" in the return path
        return new Color(r, g, b, a);
    }
    */

    /// <summary>
    /// convert the HEXcode color to RGB( this will create a value between 0 - 255)
    /// </summary>
    /// <param name="_hex"></param>
    /// <returns></returns>
    public static Color hexToColor(string _hex)
    {
        Color a;
        string s = _hex;
        //if it doesnt contain the # sign, then assign the sign at the beginning
        if (!_hex.Contains("#"))
        {
            s = "";
            s = "#" + _hex;
        }

        ColorUtility.TryParseHtmlString(s, out a);
        return a;
    }


    /// <summary>
    /// Convert the RenderTex to Texture2D
    /// </summary>
    /// <param name="rTex"></param>
    /// <returns></returns>
    public static Texture2D RenderTexToTexture2D(RenderTexture rTex)
    {
        //Resources
        //https://stackoverflow.com/questions/44264468/convert-rendertexture-to-texture2d

        Texture2D tex = new Texture2D(1200, 720, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;

        //Usage
        /*
         public RenderTexture tex;
         Texture2D myTexture = toTexture2D(tex);
        */
    }

    public static byte[] CompressByte(byte[] data)
    {
        MemoryStream output = new MemoryStream();
        using (DeflateStream dstream = new DeflateStream(output, System.IO.Compression.CompressionLevel.Optimal))
        {
            dstream.Write(data, 0, data.Length);
        }
        return output.ToArray();
    }


    public static byte[] DecompressByte(byte[] data)
    {
        MemoryStream input = new MemoryStream(data);
        MemoryStream output = new MemoryStream();
        using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
        {
            dstream.CopyTo(output);
        }
        return output.ToArray();
    }




    public static byte[] Color32ArrayToByteArray(Color32[] colors)
    {
        if (colors == null || colors.Length == 0)
            return null;

        int lengthOfColor32 = Marshal.SizeOf(typeof(Color32));
        int length = lengthOfColor32 * colors.Length;
        byte[] bytes = new byte[length];

        GCHandle handle = default(GCHandle);
        try
        {
            handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
            IntPtr ptr = handle.AddrOfPinnedObject();
            Marshal.Copy(ptr, bytes, 0, length);
        }
        finally
        {
            if (handle != default(GCHandle))
                handle.Free();
        }

        return bytes;
    }

    /// <summary>
    /// To Generate random integer without any duplication
    /// </summary>
    /// <param name="_total">The total generate number</param>
    /// <param name="_maxRandom">The maximum integer allowed</param>
    /// <returns></returns>
    public static List<int> generateRandom_NoDuplicate(int _total, int _maxRandom)
    {
        if (_maxRandom <= _total)
        {
            Debug.LogError("Cannot Create random, because the requested total is higher than the available values. Returning a null list");
            return null;
        }

        List<int> list = new List<int>();
        for (int j = 0; j < _total; j++)
        {
            int Rand = UnityEngine.Random.Range(0, _maxRandom);

            while (list.Contains(Rand))
            {
                Rand = UnityEngine.Random.Range(0, _maxRandom);
            }

            list.Add(Rand);
            //print(list[j]);
        }
        return list;
    }

    /// <summary>
    /// to get a new number outside given List
    /// </summary>
    /// <param name="_list">the given List with the number you wanna avoid</param>
    /// <param name="_min">min random number</param>
    /// <param name="_max">max random number</param>
    /// <returns></returns>
    public static int getNewNumberOutsideGivenList(List<int> _list, int _min, int _max)
    {
        int trying = 0;

        int a = 0;
        a = UnityEngine.Random.Range(_min, _max + 1);
        while (_list.Contains(a))
        {
            a = UnityEngine.Random.Range(_min, _max + 1);

            //for safety.. so after x amount of try, if it still doesnt find any new number
            //return -1., therefore it wont hang the program.
            trying++;
            if (trying >= _max*2)
                return -1;
        }

        return a;
    }

    /// <summary>
    /// To Generate random integer without any duplication, with a given list to ignore
    /// </summary>
    /// <param name="_total">The total generate number</param>
    /// <param name="_maxRandom">The maximum integer allowed</param>
    /// <param name="_toExcludeList">the integer to ignore</param>
    /// <returns></returns>
    public static List<int> generateRandom_NoDuplicate(int _total, int _maxRandom, List<int> _toExcludeList)
    {
        if (_maxRandom <= _total + _toExcludeList.Count)
        {
            Debug.LogError("Cannot Create random, because the requested total is higher than the available values. Returning a null list");
            return null;
        }

        List<int> list = new List<int>();
        for (int j = 0; j < _total; j++)
        {
            int Rand = UnityEngine.Random.Range(0, _maxRandom);

            //if the current list contains the random number and to exclude contains it too
            while (list.Contains(Rand) || _toExcludeList.Contains(Rand))
            {
                Rand = UnityEngine.Random.Range(0, _maxRandom);
            }

            list.Add(Rand);
            //print(list[j]);
        }
        return list;
    }

    /// <summary>
    /// to extract the filename from a given url
    /// </summary>
    /// <param name="_url">the url </param>
    /// <returns></returns>
    public static string extractFileNamefromURL(string _url)
    {
        var uri = new Uri(_url);
        return Path.GetFileName(uri.LocalPath);
    }


    /// <summary>
    /// convert string to Date
    /// </summary>
    /// <param name="_str">e.g. 05/05/2005 or  </param>
    /// <returns></returns>
    public static DateTime StringToDate(string _str)
    {
        return Convert.ToDateTime(_str);
    }

    /// <summary>
    /// to convert a given string to Time 
    /// </summary>
    /// <param name="_str">example 16:23:01</param>
    /// <returns></returns>
    public static DateTime StringToTime(string _str)
    {
        return DateTime.ParseExact(_str, "HH:mm:ss", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// convert time to stirng
    /// </summary>
    /// <param name="_time"></param>
    /// <returns>will return something like 12:10:50</returns>
    public static string TimeToString(DateTime _time)
    {
        return _time.ToLongTimeString(); // will return "12:10:50"
    }

    /// <summary>
    /// convert a given double(in seconds) variable to Time
    /// </summary>
    /// <param name="_time">double variable in seconds</param>
    /// <returns>example: 12:01:01 0003</returns>
    public static TimeSpan doubleToTime(double _sec)
    {
        return TimeSpan.FromSeconds(_sec);
    }

    /// <summary>
    /// convert a given double(in seconds) variable to Time
    /// </summary>
    /// <param name="_time">double variable in seconds</param>
    /// <returns>example: 12:01:01</returns>
    /// https://docs.microsoft.com/en-us/dotnet/api/system.timespan.tostring?redirectedfrom=MSDN&view=net-6.0#System_TimeSpan_ToString
    public static string doubleToTimeString(double _sec)
    {
        return TimeSpan.FromSeconds(_sec).ToString(@"hh\:mm\:ss");
    }



    /// <summary>
    /// convert timespan to double
    /// </summary>
    /// <param name="_time"></param>
    /// <returns>the seconds</returns>
    public static double timeToDouble(TimeSpan _time)
    {
        return _time.TotalSeconds;
    }


    /// <summary>
    /// This function will convert 270 to -90
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }

    public static float UnwrapAngle(float angle)
    {
        if (angle >= 0)
            return angle;

        angle = -angle % 360;

        return 360 - angle;
    }


    /// <summary>
    /// To return the angled inspector
    /// </summary>
    /// <param name="_obj"></param>
    /// <returns></returns>
    public static Vector3 getAngleInspector(Transform _obj)
    {
        Vector3 angle = _obj.localEulerAngles;
        float x = angle.x;
        float y = angle.y;
        float z = angle.z;

        if (Vector3.Dot(_obj.up, Vector3.up) >= 0f)
        {
            if (angle.x >= 0f && angle.x <= 90f)
            {
                x = angle.x;
            }
            if (angle.x >= 270f && angle.x <= 360f)
            {
                x = angle.x - 360f;
            }
        }
        if (Vector3.Dot(_obj.up, Vector3.up) < 0f)
        {
            if (angle.x >= 0f && angle.x <= 90f)
            {
                x = 180 - angle.x;
            }
            if (angle.x >= 270f && angle.x <= 360f)
            {
                x = 180 - angle.x;
            }
        }

        if (angle.y > 180)
        {
            y = angle.y - 360f;
        }

        if (angle.z > 180)
        {
            z = angle.z - 360f;
        }
        //Debug.Log(angle + " :::: " + Mathf.Round(x) + " , " + Mathf.Round(y) + " , " + Mathf.Round(z));
        return new Vector3(Mathf.Round(x), Mathf.Round(y), Mathf.Round(z));
    }


    /// <summary>
    /// to shuffle the list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values"></param>
    public static void Shuffle<T>(this IList<T> values)
    {
        System.Random rand = new System.Random();
        for (int i = values.Count - 1; i > 0; i--)
        {
            int k = rand.Next(i + 1);
            T value = values[k];
            values[k] = values[i];
            values[i] = value;
        }
    }
    /// <summary>
    /// to split the given string using || as seperator
    /// </summary>
    /// <param name="_str">give the strings</param>
    /// <returns></returns>
    public static List<string> splitGivenString(string _str, string _charSeperator)
    {
        return _str.Split(new string[] { _charSeperator }, StringSplitOptions.RemoveEmptyEntries).ToList();
    }



}
 