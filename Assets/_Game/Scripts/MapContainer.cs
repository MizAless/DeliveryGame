using System.Collections.Generic;
using UnityEngine;

public class MapContainer
{
    private readonly List<ChunkData> _chunks = new List<ChunkData>();  
    
    public void Add(ChunkData chunk)
    {
        _chunks.Add(chunk);
    }
    
    public void Clear()
    {
        _chunks.Clear();
    }
    
    public Vector3 ConvertToWorldPosition(Vector2 chunkPosition)
    {
        return new Vector3(chunkPosition.x, 0, chunkPosition.y) * ChunkView.ChunkOffset;
    }
    
    public Vector2Int ConvertToChunkPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / ChunkView.ChunkOffset);
        int y = Mathf.RoundToInt(worldPosition.z / ChunkView.ChunkOffset);
        return new Vector2Int(x, y);
    }
    
    public IReadOnlyList<ChunkData> Chunks => _chunks;
}