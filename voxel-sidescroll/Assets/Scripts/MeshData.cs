using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshData
{
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();

    public List<Vector2> verts2d = new List<Vector2>();
    public List<int> tris2d = new List<int>();

    public List<Vector2> uv = new List<Vector2>();
    public List<List<Vector2>> colVertices = new List<List<Vector2>>();

    public bool useRenderDataForCol;

    public MeshData()
    {

    }

    public void AddVertex(Vector3 vertex)
    {
        vertices.Add(vertex);
    }

    public void AddVertex2d(Vector2 vertex)
    {
        verts2d.Add(vertex);
    }

    public void AddColliderVertices(List<Vector2> vertexList)
    {
        colVertices.Add(vertexList);
    }

    public void AddTriangle(int tri)
    {
        triangles.Add(tri);
    }

    public void AddQuadTriangles()
    {
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);
    }

    public void AddQuadTriangles2d()
    {
        tris2d.Add(verts2d.Count - 4);
        tris2d.Add(verts2d.Count - 3);
        tris2d.Add(verts2d.Count - 2);
        tris2d.Add(verts2d.Count - 4);
        tris2d.Add(verts2d.Count - 2);
        tris2d.Add(verts2d.Count - 1);
    }
}