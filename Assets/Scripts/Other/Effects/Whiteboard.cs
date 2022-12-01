using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.IO;
using System.IO.Compression;
using System.Threading;
using Threading;

public class Whiteboard : MonoBehaviour
{
    private int textureSize = 640;
    private int penSize = 5;

    //private Texture2D currentTexture;

    private Texture2D texture;
    private Color[] color;
    private byte[] tempPixArry;
    public byte[] convertedPixArry;


    private bool touching, touchingLast;
    private float posX, posY;
    private float lastX, lastY;

    private Renderer rend;

    //update variable
    int x, y;
    int lerpX, lerpY;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        this.texture = new Texture2D(textureSize, textureSize);

        rend.material.mainTexture = this.texture;

    }

    // Update is called once per frame
    void Update()
    {
        x = (int)(posX * textureSize - (penSize / 2));
        y = (int)(posY * textureSize - (penSize / 2));
        
        
        if (touchingLast)
        {
            //Debug.Log("X: " + posX + " Y: " + posY);
            //To Check if it is out of coordinates or not
            if (posX <= 0.01 || posX >= 0.99 
                || posY <= 0.01 || posY >= 0.99)
                return; // if yes..-> do nothing.
            
            texture.SetPixels(x, y, penSize, penSize, color);

            //set the lerping so, there will be no empty space in between.
            for (float t = 0.01f; t < 1f; t += 0.01f)
            {
                lerpX = (int)Mathf.Lerp(lastX, (float)x, t);
                lerpY = (int)Mathf.Lerp(lastY, (float)y, t);
                texture.SetPixels(lerpX, lerpY, penSize, penSize, color);
            }

            texture.Apply();
        }
        
        this.lastX = (float)x;
        this.lastY = (float)y;

        this.touchingLast = this.touching;

    }
    
    /// <summary>
    /// to get the texture of this whiteboard as byte[]
    /// </summary>
    /// <returns></returns>
    public byte[] getTextureByte()
    {
        
        return this.texture.EncodeToJPG(50);

        /*
        Texture2D tex = this.texture;
        //tex.Compress(true);
        //has to be compress... max is 65536 bytes
        //byte[] data = tex.GetRawTextureData();
        byte[] data = tex.EncodeToJPG(50);
        CompressByte(data);
        
        return tempPixArry;
        */
    }

    /// <summary>
    /// receive and apply the byte[] as texture of this whiteboard.
    /// </summary>
    public IEnumerator receiveTextureBytess(byte[] _pixels)
    {
        if (_pixels == null || _pixels.Length <= 0)
            yield return 0;

        //Debug.Log("pix " + _pixels.Length);
        texture.LoadImage(_pixels);
        rend.material.mainTexture = texture;
        //Dispose the texture 2D therefore we wont get memory leaks
        texture.hideFlags = HideFlags.HideAndDontSave;

    }






    /// <summary>
    /// receive and apply the byte[] as texture of this whiteboard.
    /// </summary>
    public void receiveTextureByte(byte[] _pixels)
    {
        if (_pixels == null || _pixels.Length <= 0)
          return;

        
        Texture2D a = new Texture2D(textureSize, textureSize);
        a.LoadImage(_pixels);
        rend.material.mainTexture = a;

        //Dispose the texture 2D therefore we wont get memory leaks
        a.hideFlags = HideFlags.HideAndDontSave;
        


        /*
        DeCompressByte(_pixels);

        //dont do anything if there is no pixels
        if (convertedPixArry == null || convertedPixArry.Length <=0)
            return;

        Texture2D a = new Texture2D(textureSize, textureSize);
        a.LoadImage(convertedPixArry);
        //a.LoadRawTextureData(convertedPixArry);
        //a.Apply();
        rend.material.mainTexture = a;
        */
    }




    public void ToggleTouch(bool _touching)
    {
        this.touching = _touching;
    }

    public void SetTouchPosition(float x, float y)
    {
        this.posX = x;
        this.posY = y;
    }

    public void SetPenSize(Slider _penSizeSlider)
    {
        penSize = ((int)_penSizeSlider.value);
    }

    public void SetColor(Color color)
    {
        this.color = Enumerable.Repeat<Color>(color, penSize * penSize).ToArray<Color>();
    }

    public void SetEraserMode()
    {
        //255,255,255
    }


    public void ClearTexture()
    {
        Renderer renderer = GetComponent<Renderer>();
        this.texture = new Texture2D(textureSize, textureSize);

        renderer.material.mainTexture = this.texture;
    }



    //threading function to compress image in the background
    private void CompressByte(byte[] _data)
    {
        Async.Run(() =>
        {
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(output, System.IO.Compression.CompressionLevel.Fastest))
            {
                dstream.Write(_data, 0, _data.Length);
            }
            return output.ToArray();

        }).ContinueInMainThread((result) =>
        {
            //store it in a temp container. For later usage.
            tempPixArry = result;
        });
    }


    private void DeCompressByte(byte[] _data)
    {
        if (_data == null) // in case there is no data.
            return;

        Async.Run(() =>
        {
            //decompress the data
            MemoryStream input = new MemoryStream(_data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();

        }).ContinueInMainThread((result) =>
        {
            //store in an array for later.
            convertedPixArry = result;
        });
    }

}


