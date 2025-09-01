using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Save
{
    public Dictionary<WorldPos, Blocks> blocks = new Dictionary<WorldPos, Blocks>();

    public Save(Chunk chunk)
    {
        for (int x = 0; x < Chunk.chunkSize; x++)
        {
            for (int y = 0; y < Chunk.chunkSize; y++)
            {
                if (!chunk.blocks[x, y, 0].changed && !chunk.blocks[x, y, 1].changed)
                    continue;

                WorldPos pos = new WorldPos(x, y);
                blocks.Add(pos, 
                    new Blocks
                    {
                        Foreground = chunk.blocks[x, y, 0],
                        Background = chunk.blocks[x, y, 1]
                    });
            }
        }
    }
}

[Serializable]
public struct Blocks
{
    public Block Foreground;
    public Block Background;
}
