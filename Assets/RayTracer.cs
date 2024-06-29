using System;
using UnityEngine;

[ExecuteAlways, ImageEffectAllowedInSceneView]
public class RayTracer : MonoBehaviour
{
    Material rayTracingMaterial;

    [Header("View settings")]
    [SerializeField] bool useShaderInSceneView;

    [Header("References")]
    [SerializeField] Shader rayTracingShader;

    void OnRenderImage(RenderTexture src, RenderTexture target)
    {
        if (rayTracingMaterial == null)
        {
            rayTracingMaterial = new Material(rayTracingShader);
        }

        if (Camera.current.name != "SceneCamera" || useShaderInSceneView)
        {
            UpdateCameraParams(Camera.current);
            Graphics.Blit(null, target, rayTracingMaterial);
        }
        else
        {
            Graphics.Blit(src, target);
        }
    }

    void UpdateCameraParams(Camera cam)
    {
        float planeHeight = cam.nearClipPlane * MathF.Sin(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * 2;
        float planeWidth = planeHeight * cam.aspect;

        rayTracingMaterial.SetVector("ViewParams", new Vector3(planeWidth, planeHeight, cam.nearClipPlane));
        rayTracingMaterial.SetMatrix("CamLocalToWorldMatrix", cam.transform.localToWorldMatrix);
    }

    void CreateSpheres()
    {
        var spheres = FindObjectOfType<RayTracedSphere>();
    }
}
