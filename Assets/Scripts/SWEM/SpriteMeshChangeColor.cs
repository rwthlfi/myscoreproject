using UnityEngine;

public class SpriteMeshChangeColor : MonoBehaviour
{
    public SpriteRenderer[] spriteRenderer;
    public new GameObject[] gameObject;
    public Color colorWhite;
    public Color colorBrown;


    public void ChangeToGreen()
    {
        if (spriteRenderer != null)
            foreach (SpriteRenderer spriteRenderer in spriteRenderer)
            {
                spriteRenderer.color = colorWhite;
            }

        if (gameObject != null)
            foreach (GameObject gameObject in gameObject)
            {
                var Renderer = gameObject.GetComponent<Renderer>();
                Renderer.material.SetColor("_Color", colorWhite);
            }
    }

    public void ChangeToBrown()
    {
        if (spriteRenderer != null)
            foreach (SpriteRenderer spriteRenderer in spriteRenderer)
            {
                spriteRenderer.color = colorBrown;
            }

        if (gameObject != null)
            foreach (GameObject gameObject in gameObject)
            {
                var Renderer = gameObject.GetComponent<Renderer>();
                Renderer.material.SetColor("_Color", colorBrown);
            }
    }
}
