
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;

[ExecuteAlways]
public class ShadowOnlyRenderer : MonoBehaviour
{
    public Light targetLight;
    public Mesh mesh;
    public Material material;
    CommandBuffer commands;

    void OnEnable()
    {
        if (targetLight != null) {
            commands = new CommandBuffer();
            commands.name = "Shadow Only";
            targetLight.AddCommandBuffer(LightEvent.BeforeShadowMapPass, commands);
        }
    }

    void OnDisable()
    {
        if (commands != null)
            targetLight.RemoveCommandBuffer(LightEvent.BeforeShadowMapPass, commands);
    }

    void OnRenderObject()
    {
        if (mesh == null || material == null || commands == null)
            return;

        commands.Clear();
        commands.DrawMesh(mesh, transform.localToWorldMatrix, material);
    }
}
