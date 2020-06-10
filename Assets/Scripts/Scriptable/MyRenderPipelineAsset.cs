
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName="MyRenderPipeline")]
public class MyRenderPipelineAsset : RenderPipelineAsset
{
    protected override RenderPipeline CreatePipeline()
    {
        return new MyRenderPipeline();
    }
}

