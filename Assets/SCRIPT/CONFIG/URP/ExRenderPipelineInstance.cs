using UnityEngine;
using UnityEngine.Rendering;

public class ExRenderPipelineInstance : RenderPipeline
{
    public ExRenderPipelineInstance()
    {
    }

    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        // This is where you can write custom rendering code. Customize this method to customize your SRP.
    }
}