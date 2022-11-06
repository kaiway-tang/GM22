using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    private Mesh mesh;
    
    [SerializeField] private GameObject startPoint;
    [SerializeField] private GameObject endPoint;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float width = 1f;

    private float curveMagnitude;

    private List<Vector3> vertices= new();
    private List<int> triangles = new();
    
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Update()
    {
        mesh.Clear();
        vertices.Clear();
        triangles.Clear();

        curveMagnitude = Vector3.Distance(startPoint.transform.position, endPoint.transform.position) / 2;

        Bezier curve = new Bezier(startPoint.transform.position, startPoint.transform.position + direction * curveMagnitude,
            endPoint.transform.position - direction * curveMagnitude, endPoint.transform.position, 20);

        Vector3 perpendicular = new Vector3(-direction.z, 0, direction.x).normalized * width;
        vertices.Add(startPoint.transform.position + perpendicular);
        vertices.Add(startPoint.transform.position - perpendicular);
        for (int i = 0; i < curve.points.Length - 1; i++)
        {
            Vector3 midpoint = (curve.points[i] + curve.points[i + 1]) / 2;
            Vector3 slope = curve.points[i + 1] - curve.points[i];
            Vector3 perp = new Vector3(-slope.z, 0, slope.x).normalized * width;
            
            vertices.Add(midpoint + perp);
            vertices.Add(midpoint - perp);
        }
        vertices.Add(endPoint.transform.position + perpendicular);
        vertices.Add(endPoint.transform.position - perpendicular);

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
    }
}


