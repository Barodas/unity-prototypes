using UnityEngine;
using System.Collections;

public static class TerrainUtils
{
    public static WorldPos GetBlockPos(Vector3 pos)
    {
        WorldPos blockPos = new WorldPos(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

        return blockPos;
    }

    public static WorldPos GetBlockPos(RaycastHit2D hit, bool adjacent = false)
    {
        Vector3 pos = new Vector3(
            MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
            MoveWithinBlock(hit.point.y, hit.normal.y, adjacent)
            );

        return GetBlockPos(pos);
    }

    public static Block GetBlock(RaycastHit2D hit, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return null;

        WorldPos pos = GetBlockPos(hit, adjacent);

        Block block = chunk.world.GetBlock(pos.x, pos.y, 0);

        return block;
    }

    public static bool SetBlock(RaycastHit2D hit, Block block, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return false;

        WorldPos pos = GetBlockPos(hit, adjacent);

        if(PlayerController.Instance.OccupiedBlocks().Contains(pos))
        {
            return false;
        }

        chunk.world.SetBlock(pos.x, pos.y, 0, block);
        return true;
    }

    static float MoveWithinBlock(float pos, float norm, bool adjacent = false)
    {
        if (pos - (int)pos == 0.5f || pos - (int)pos == -0.5f)
        {
            if (adjacent)
            {
                pos += (norm / 2);
            }
            else
            {
                pos -= (norm / 2);
            }
        }

        return pos;
    }
}