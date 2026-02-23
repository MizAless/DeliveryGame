using System.Collections.Generic;
using UnityEngine;

public class MapGenerator
{
    List<ChunkData> chunks = new List<ChunkData>();
    
    public void Generate()
    {
        chunks.Clear();
        
        
    }
    
}

public class ChunkData
{
    public Vector2 Position;
    public Chunk.Sides ActiveSides;
}