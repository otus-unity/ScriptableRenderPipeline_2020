
using UnityEngine;
using UnityEngine.Rendering;

public class MyRenderPipeline : RenderPipeline
{
    CommandBuffer clearBuffer = new CommandBuffer();
    static ShaderTagId ForwardBase = new ShaderTagId("ForwardBase");

    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (var camera in cameras) {
            context.SetupCameraProperties(camera);

            // Очищаем буфер экрана

            clearBuffer.Clear();

            CameraClearFlags clearFlags = camera.clearFlags;
            clearBuffer.ClearRenderTarget(
                clearDepth: (clearFlags & CameraClearFlags.Depth) != 0,
                clearColor: (clearFlags & CameraClearFlags.Color) != 0,
                camera.backgroundColor);

            context.ExecuteCommandBuffer(clearBuffer);

            // Рисуем объекты редактора

          #if UNITY_EDITOR
            if (camera.cameraType == CameraType.SceneView)
                ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
          #endif

            // Отсекаем невидимые объекты

            ScriptableCullingParameters cullingParameters;
            if (!camera.TryGetCullingParameters(out cullingParameters))
                continue;

            CullingResults cullingResults = context.Cull(ref cullingParameters);

            // Рисуем непрозрачные объекты

            var opaqueSortingSettings = new SortingSettings(camera);
            opaqueSortingSettings.criteria = SortingCriteria.CommonOpaque;

            var opaqueDrawingSettings = new DrawingSettings(ForwardBase, opaqueSortingSettings);
            var opaqueFilteringSettings = new FilteringSettings(RenderQueueRange.opaque);
            context.DrawRenderers(cullingResults, ref opaqueDrawingSettings, ref opaqueFilteringSettings);

            // Рисуем небо

            if (camera.clearFlags == CameraClearFlags.Skybox && RenderSettings.skybox != null)
                context.DrawSkybox(camera);

            // Рисуем прозрачные объекты

            var transparentSortingSettings = new SortingSettings(camera);
            transparentSortingSettings.criteria = SortingCriteria.CommonTransparent;

            var transparentDrawingSettings = new DrawingSettings(ForwardBase, transparentSortingSettings);
            var transparentFilteringSettings = new FilteringSettings(RenderQueueRange.transparent);
            context.DrawRenderers(cullingResults, ref transparentDrawingSettings, ref transparentFilteringSettings);

            // Отправляем на отрисовку

            context.Submit();
        }
    }
}
