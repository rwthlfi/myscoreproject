using UnityEngine;

// LineRenderer: Changes the color of the linerenderer which will be used after the color set
// TrailRendere: Uses same color as LineRenderer as preview effect
// Pen Material: Changes the color of the tip of the pen as indicator which color was selected

public class LineRendererChangeColor : MonoBehaviour
{
    public LineRenderer LineRenderer;
    public TrailRenderer TrailRenderer;
    public GameObject DrawToolTip;

    public void ColorChangeWhite()
    {

        float alpha = 1.0f;

        Gradient gradient = new Gradient();
        gradient.SetKeys
            (
            new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        LineRenderer.colorGradient = gradient;
        TrailRenderer.colorGradient = gradient;

        // set pen tip material color
        var DrawToolTipRenderer = DrawToolTip.GetComponent<Renderer>();
        DrawToolTipRenderer.material.SetColor("_Color", Color.white);
    }

    public void ColorChangeGray()
    {
        float alpha = 1.0f;

        Gradient gradient = new Gradient();
        gradient.SetKeys
            (
            new GradientColorKey[] { new GradientColorKey(Color.gray, 0.0f), new GradientColorKey(Color.gray, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        LineRenderer.colorGradient = gradient;
        TrailRenderer.colorGradient = gradient;

        // set pen tip material color
        var DrawToolTipRenderer = DrawToolTip.GetComponent<Renderer>();
        DrawToolTipRenderer.material.SetColor("_Color", Color.gray);
    }

    public void ColorChangeBlack()
    {
        float alpha = 1.0f;

        Gradient gradient = new Gradient();
        gradient.SetKeys
            (
            new GradientColorKey[] { new GradientColorKey(Color.black, 0.0f), new GradientColorKey(Color.black, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        LineRenderer.colorGradient = gradient;
        TrailRenderer.colorGradient = gradient;

        // set pen tip material color
        var DrawToolTipRenderer = DrawToolTip.GetComponent<Renderer>();
        DrawToolTipRenderer.material.SetColor("_Color", Color.black);
    }

    public void ColorChangeRed()
    {
        float alpha = 1.0f;

        Gradient gradient = new Gradient();
        gradient.SetKeys
            (
            new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        LineRenderer.colorGradient = gradient;
        TrailRenderer.colorGradient = gradient;

        // set pen tip material color
        var DrawToolTipRenderer = DrawToolTip.GetComponent<Renderer>();
        DrawToolTipRenderer.material.SetColor("_Color", Color.red);
    }

    public void ColorChangeGreen()
    {
        float alpha = 1.0f;

        Gradient gradient = new Gradient();
        gradient.SetKeys
            (
            new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.green, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        LineRenderer.colorGradient = gradient;
        TrailRenderer.colorGradient = gradient;

        // set pen tip material color
        var DrawToolTipRenderer = DrawToolTip.GetComponent<Renderer>();
        DrawToolTipRenderer.material.SetColor("_Color", Color.green);
    }

    public void ColorChangeBlue()
    {
        float alpha = 1.0f;

        Gradient gradient = new Gradient();
        gradient.SetKeys
            (
            new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.blue, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        LineRenderer.colorGradient = gradient;
        TrailRenderer.colorGradient = gradient;

        // set pen tip material color
        var DrawToolTipRenderer = DrawToolTip.GetComponent<Renderer>();
        DrawToolTipRenderer.material.SetColor("_Color", Color.blue);
    }

    public void ColorChangeMagenta()
    {
        float alpha = 1.0f;

        Gradient gradient = new Gradient();
        gradient.SetKeys
            (
            new GradientColorKey[] { new GradientColorKey(Color.magenta, 0.0f), new GradientColorKey(Color.magenta, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        LineRenderer.colorGradient = gradient;
        TrailRenderer.colorGradient = gradient;

        // set pen tip material color
        var DrawToolTipRenderer = DrawToolTip.GetComponent<Renderer>();
        DrawToolTipRenderer.material.SetColor("_Color", Color.magenta);
    }

    public void ColorChangeYellow()
    {
        float alpha = 1.0f;

        Gradient gradient = new Gradient();
        gradient.SetKeys
            (
            new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.yellow, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        LineRenderer.colorGradient = gradient;
        TrailRenderer.colorGradient = gradient;

        // set pen tip material color
        var DrawToolTipRenderer = DrawToolTip.GetComponent<Renderer>();
        DrawToolTipRenderer.material.SetColor("_Color", Color.yellow);
    }

    public void ColorChangeCyan()
    {
        float alpha = 1.0f;

        Gradient gradient = new Gradient();
        gradient.SetKeys
            (
            new GradientColorKey[] { new GradientColorKey(Color.cyan, 0.0f), new GradientColorKey(Color.cyan, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        LineRenderer.colorGradient = gradient;
        TrailRenderer.colorGradient = gradient;

        // set pen tip material color
        var DrawToolTipRenderer = DrawToolTip.GetComponent<Renderer>();
        DrawToolTipRenderer.material.SetColor("_Color", Color.cyan);
    }
}