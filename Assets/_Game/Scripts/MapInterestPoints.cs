using System.Collections.Generic;
using UnityEngine;

public class MapInterestPoints
{
    private MapContainer _mapContainer;
    
    private List<ChunkData> _availableChunkPool = new List<ChunkData>();

    public MapInterestPoints(MapContainer mapContainer)
    {
        _mapContainer = mapContainer;
        ResetPool();
    }

    public Vector3 GetRandomDeliveryLootPoint()
    {
        var chunk = GetRandomChunk();
        _availableChunkPool.Remove(chunk);
        return chunk.SpawnPointsContainer.GetRandomDeliveryLootPoint();
    }

    public Vector3 GetRandomNpcSpawnPoint()
    {
        var point = GetRandomChunk().SpawnPointsContainer.GetRandomNPCSpawnPoint();
        ResetPool();
        return point;
    }

    private ChunkData GetRandomChunk()
    {
        return _availableChunkPool[Random.Range(0, _availableChunkPool.Count)];
    }
    
    private void ResetPool()
    {
        _availableChunkPool = new List<ChunkData>();
        _availableChunkPool.AddRange(_mapContainer.Chunks);
    }
}