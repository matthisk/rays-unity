using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello world!");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        Camera cam = Camera.main;
        Transform camT = cam.transform;

        float planeHeight = cam.nearClipPlane * Mathf.Sin(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * 2;
        float planeWidth = planeHeight * cam.aspect;

        Vector3 bottomLeftLocal = new Vector3(-planeWidth / 2, -planeHeight / 2, cam.nearClipPlane);

        var debugPointsX = 10;
        var debugPointsY = 10;

        for (int x = 0; x < debugPointsX; x++)
        {
            for (int y = 0; y < debugPointsY; y++)
            {
                float tx = x / (debugPointsX - 1f);
                float ty = y / (debugPointsY - 1f);

                Vector3 pointLocal = bottomLeftLocal + new Vector3(planeWidth * tx, planeHeight * ty);
                Vector3 point = camT.position + camT.right * pointLocal.x + camT.up * pointLocal.y + camT.forward * pointLocal.z;
                Vector3 dir = point - camT.position;

                Gizmos.DrawSphere(point, 0.05f);
                Gizmos.DrawRay(camT.position, dir);
            }
        }
    }
}
