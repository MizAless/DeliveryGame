using System.Linq;
using UnityEngine;

public class MapBounds
{
    private MapContainer _mapContainer;

    public MapBounds(MapContainer mapContainer)
    {
        _mapContainer = mapContainer;
    }
    
    public bool IsAllowToMove(Vector3 nextPosition)
    {
        var chunkPos = _mapContainer.ConvertToChunkPosition(nextPosition);

        var chunk = _mapContainer.Chunks
            .FirstOrDefault(c => c.Position == chunkPos);

        if (chunk == null)
            return false;

        Vector3 chunkCenter =
            _mapContainer.ConvertToWorldPosition(chunk.Position);

        float halfSize = ChunkView.ChunkOffset * 0.5f;

        if (nextPosition.x < chunkCenter.x - halfSize ||
            nextPosition.x > chunkCenter.x + halfSize)
            return false;

        if (nextPosition.z < chunkCenter.z - halfSize ||
            nextPosition.z > chunkCenter.z + halfSize)
            return false;

        return true;
    }
}