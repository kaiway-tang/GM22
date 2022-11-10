using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Bridge : MonoBehaviour
{
    private Mesh mesh;
    private MeshCollider collider;
    
    [SerializeField] private GameObject startPoint;
    [SerializeField] private GameObject endPoint;
    [SerializeField] private float width = 1f;

    private float curveMagnitude;

    private List<Vector3> vertices= new();
    private List<int> triangles = new();
    
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        collider = GetComponent<MeshCollider>();
    }

    private void Update()
    {
        mesh.Clear();
        vertices.Clear();
        triangles.Clear();

        var startPosition = startPoint.transform.position;
        var endPosition = endPoint.transform.position;
        var startForward = startPoint.transform.forward;
        var endForward = endPoint.transform.forward;
        
        curveMagnitude = Vector3.Distance(startPosition, endPosition) / 2;

        Bezier curve = new Bezier(startPosition, startPosition + startForward * curveMagnitude,
            endPosition + endPoint.transform.forward * curveMagnitude, endPosition, 20);

        Vector3 startPerpendicular = new Vector3(-startForward.z, 0, startForward.x).normalized * width;
        vertices.Add(startPosition + startPerpendicular);
        vertices.Add(startPosition - startPerpendicular);
        
        for (int i = 0; i < curve.points.Length - 1; i++)
        {
            Vector3 midpoint = (curve.points[i] + curve.points[i + 1]) / 2;
            Vector3 slope = curve.points[i + 1] - curve.points[i];
            Vector3 perp = new Vector3(-slope.z, 0, slope.x).normalized * width;
            
            vertices.Add(midpoint + perp);
            vertices.Add(midpoint - perp);
        }
        
        Vector3 endPerpendicular = new Vector3(-endForward.z, 0, endForward.x).normalized * width;
        vertices.Add(endPosition - endPerpendicular);
        vertices.Add(endPosition + endPerpendicular);

        for (int i = 0; i < vertices.Count - 2; i+=2)
        {
            triangles.Add(i+2);
            triangles.Add(i+1);
            triangles.Add(i);
            triangles.Add(i+3);
            triangles.Add(i+1);
            triangles.Add(i+2);
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        
        mesh.RecalculateNormals();

        collider.sharedMesh = mesh;
    }
}


