using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Dissolve : MonoBehaviour
{
    public float value = 1.0f;
    Renderer myRenderer;

    void OnEnable()
    {
        myRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        myRenderer.sharedMaterial.SetFloat("Vector1_8E401645", value);
    }
}
