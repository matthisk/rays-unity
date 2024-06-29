using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTracedSphere : MonoBehaviour
{
    public RayTracingMaterial material;

    [SerializeField, HideInInspector] int materialObjectID;
    [SerializeField, HideInInspector] bool materialInitFlag;
}
