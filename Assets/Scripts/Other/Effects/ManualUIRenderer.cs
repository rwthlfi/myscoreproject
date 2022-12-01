using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class ManualUIRenderer : MonoBehaviour
{
    private static readonly int MainTexProperty = Shader.PropertyToID("_MainTex");
    private static readonly int TextureSampleAddProperty = Shader.PropertyToID("_TextureSampleAdd");
    private static readonly int ColorProperty = Shader.PropertyToID("_Color");

    private Camera targetCamera;
    private CommandBuffer commandBuffer;

    private void Awake()
    {
        this.targetCamera = this.GetComponent<Camera>();

        this.commandBuffer = new CommandBuffer();
        this.commandBuffer.name = "Manual UI rendering";
        //this.targetCamera.AddCommandBuffer(CameraEvent.AfterSkybox, commandBuffer);
        this.targetCamera.AddCommandBuffer(CameraEvent.AfterEverything, commandBuffer);
    }

    private void OnDisable()
    {
        this.commandBuffer.Clear();
    }

    private void Update()
    {
        this.commandBuffer.Clear();
        AddUiDrawingCommands(this.targetCamera, this.commandBuffer);
    }

    private static void AddUiDrawingCommands(Camera cam, CommandBuffer buffer)
    {
        // Root canvases ordered by screen space depth
        var rootCanvases = FindObjectsOfType<Canvas>()
            .Where(canvas => canvas.isRootCanvas && canvas.renderMode == RenderMode.WorldSpace)
            .OrderByDescending(canvas => cam.WorldToScreenPoint(canvas.transform.position).z);

        foreach (var canvas in rootCanvases)
        {
            // Graphics after culling sorted by depth
            var graphics = canvas.GetComponentsInChildren<Graphic>()
                .Where(graphic => TestCullingMask(graphic, cam.cullingMask))
                .OrderBy(graphic => graphic.depth);

            foreach (Graphic graphic in graphics)
            {
                AddGraphicDrawingCommands(graphic, buffer);
            }
        }
    }

    private static bool TestCullingMask(Graphic graphic, int cullingMask)
    {
        return ((1 << graphic.gameObject.layer) & cullingMask) != 0;
    }

    private static void AddGraphicDrawingCommands(Graphic graphic, CommandBuffer buffer)
    {
        // Probably not needed, but let's call it anyway
        graphic.Rebuild(CanvasUpdate.PreRender);

        // Determine effective alpha from CanvasGroups (probably not how Unity does this)
        float effectiveAlpha = graphic.GetComponentsInParent<CanvasGroup>()
            .Aggregate(1f, (alpha, group) => alpha * group.alpha);

        var material = new Material(graphic.materialForRendering); // material with IMaterialModifiers already applied
        material.SetTexture(MainTexProperty, graphic.mainTexture);
        material.SetColor(ColorProperty, graphic.color * new Color(1f, 1f, 1f, effectiveAlpha));
        if (graphic is Text)
        {
            // Not sure how/when Unity decides to set _TextureSampleAdd, but Text is
            // the only component I've encountered that needs it
            material.SetVector(TextureSampleAddProperty, new Vector3(1, 1, 1));
        }

        var mesh = new Mesh();
        // Call protected member Graphic.OnPopulateMesh through reflection to
        // populate the mesh. Probably really slow and skipping quite a few steps
        // in Unity's UI render pipeline, but it seems to work
        using (VertexHelper vh = new VertexHelper())
        {
            graphic.GetType().InvokeMember("OnPopulateMesh",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.InvokeMethod |
                System.Reflection.BindingFlags.NonPublic,
                null, graphic, new object[] { vh });

            vh.FillMesh(mesh);
        }

        buffer.DrawMesh(mesh, graphic.rectTransform.localToWorldMatrix, material);
    }
}
