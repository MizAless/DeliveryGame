using UnityEngine;

public class ChunkSceneEntryPoint : MonoBehaviour
{
    [SerializeField] private ChunkView _chunkPrefab;
    [SerializeField] private int _chunkCount;

    private void Start()
    {
        var mapContainer = new MapContainer();
        
        var mapGenerator = new MapGenerator(_chunkCount, _chunkPrefab, mapContainer);
        
        mapGenerator.Generate();
    }
}