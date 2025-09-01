using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class Chunk : MonoBehaviour
{
    private MeshFilter _filter;
    private PolygonCollider2D _col;

    public Block[,,] blocks = new Block[chunkSize, chunkSize, chunkDepth]; // X, Y, Foreground/Background
    public World world;
    public WorldPos pos;
    public static int chunkSize = 16; // X, Y
    public static int chunkDepth = 2; // Foreground/Background
    public bool update = true;

    private void Start()
    {
        _filter = gameObject.GetComponent<MeshFilter>();
        _col = gameObject.GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        if (update)
        {
            update = false;
            UpdateChunk();
        }
    }

    public Block GetBlock(int x, int y, int z)
    {
        if (InRange(x) && InRange(y) && InRange(z))
            return blocks[x, y, z];
        return world.GetBlock(pos.x + x, pos.y + y, z);
    }

    public void SetBlock(int x, int y, int z, Block block)
    {
        if (InRange(x) && InRange(y) && InRange(z))
        {
            blocks[x, y, z] = block;
        }
        else
        {
            world.SetBlock(pos.x + x, pos.y + y, z, block);
        }
    }

    public void SetBlocksUnmodified()
    {
        foreach (Block block in blocks)
        {
            block.changed = false;
        }
    }

    public static bool InRange(int index)
    {
        if (index < 0 || index >= chunkSize)
            return false;

        return true;
    }

    private void UpdateChunk()
    {
        MeshData meshData = new MeshData();
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkDepth; z++)
                {
                    meshData = blocks[x, y, z].Blockdata(this, x, y, z, meshData);
                }
            }
        }
        RenderMesh(meshData);
    }

    private void RenderMesh(MeshData meshData)
    {
        _filter.mesh.Clear();
        _filter.mesh.vertices = meshData.vertices.ToArray();
        _filter.mesh.triangles = meshData.triangles.ToArray();
        _filter.mesh.uv = meshData.uv.ToArray();
        _filter.mesh.RecalculateNormals();

        Build2dCollider(meshData);
    }

    private void Build2dCollider(MeshData meshData)
    {
        if(meshData.tris2d.Count <= 0)
        {
            return;
        }

        // Get triangles and vertices from mesh
        int[] triangles = meshData.tris2d.ToArray();
        Vector2[] vertices = meshData.verts2d.ToArray();

        // Get just the outer edges from the mesh's triangles (ignore or remove any shared edges)
        Dictionary<string, KeyValuePair<int, int>> edges = new Dictionary<string, KeyValuePair<int, int>>();
        for (int i = 0; i < triangles.Length; i += 3)
        {
            for (int e = 0; e < 3; e++)
            {
                int vert1 = triangles[i + e];
                int vert2 = triangles[i + e + 1 > i + 2 ? i : i + e + 1];
                string edge = Mathf.Min(vert1, vert2) + ":" + Mathf.Max(vert1, vert2);
                if (edges.ContainsKey(edge))
                {
                    edges.Remove(edge);
                }
                else
                {
                    edges.Add(edge, new KeyValuePair<int, int>(vert1, vert2));
                }
            }
        }

        // Create edge lookup (Key is first vertex, Value is second vertex, of each edge)
        Dictionary<int, int> lookup = new Dictionary<int, int>();
        foreach (KeyValuePair<int, int> edge in edges.Values)
        {
            if (lookup.ContainsKey(edge.Key) == false)
            {
                lookup.Add(edge.Key, edge.Value);
            }
        }

        _col.pathCount = 0;

        // Loop through edge vertices in order
        int startVert = 0;
        int nextVert = startVert;
        int highestVert = startVert;
        List<Vector2> colliderPath = new List<Vector2>();
        while (true)
        {
            if(nextVert >= vertices.Length)
            {
                Debug.Log("aaaahhhhh");
            }

            // Add vertex to collider path
            colliderPath.Add(vertices[nextVert]);

            // Get next vertex
            nextVert = lookup[nextVert];

            // Store highest vertex (to know what shape to move to next)
            if (nextVert > highestVert)
            {
                highestVert = nextVert;
            }

            // Shape complete
            if (nextVert == startVert)
            {

                // Add path to polygon collider
                _col.pathCount++;
                _col.SetPath(_col.pathCount - 1, colliderPath.ToArray());
                colliderPath.Clear();

                // Go to next shape if one exists
                if (lookup.ContainsKey(highestVert + 1))
                {

                    // Set starting and next vertices
                    startVert = highestVert + 1;
                    nextVert = startVert;

                    // Continue to next loop
                    continue;
                }

                // No more verts
                break;
            }
        }
    }
}