using System.Linq;
using UnityEngine;

public class MapBounds
{
    private MapContainer _mapContainer;

    public MapBounds(MapContainer mapContainer)
    {
        _mapContainer = mapContainer;
    }
    
    public bool IsPositionValid(Vector3 position)
    {
        var chunkPos = _mapContainer.ConvertToChunkPosition(position);
        var chunk = _mapContainer.Chunks.FirstOrDefault(c => c.Position == chunkPos);
        
        if (chunk == null)
            return false;

        Vector3 chunkCenter = _mapContainer.ConvertToWorldPosition(chunk.Position);
        float halfSize = ChunkView.ChunkOffset * 0.5f;

        return position.x >= chunkCenter.x - halfSize &&
               position.x <= chunkCenter.x + halfSize &&
               position.z >= chunkCenter.z - halfSize &&
               position.z <= chunkCenter.z + halfSize;
    }
    
    // Получаем разрешённое движение с учётом границ
    public Vector3 GetValidatedMovement(Vector3 currentPosition, Vector3 desiredMovement)
    {
        Vector3 targetPosition = currentPosition + desiredMovement;
        
        if (IsPositionValid(targetPosition))
            return desiredMovement;
        
        // Пробуем движение только по X
        Vector3 xMovement = new Vector3(desiredMovement.x, 0, 0);
        Vector3 xTarget = currentPosition + xMovement;
        
        // Пробуем движение только по Z
        Vector3 zMovement = new Vector3(0, 0, desiredMovement.z);
        Vector3 zTarget = currentPosition + zMovement;
        
        bool canMoveX = IsPositionValid(xTarget);
        bool canMoveZ = IsPositionValid(zTarget);
        
        if (canMoveX && canMoveZ)
        {
            // Можно двигаться в обе стороны - комбинируем
            return new Vector3(desiredMovement.x, 0, desiredMovement.z);
        }
        else if (canMoveX)
        {
            // Двигаемся только по X
            return new Vector3(desiredMovement.x, 0, 0);
        }
        else if (canMoveZ)
        {
            // Двигаемся только по Z
            return new Vector3(0, 0, desiredMovement.z);
        }
        
        // Нельзя двигаться никуда
        return Vector3.zero;
    }
}