using UnityEngine;

public class MapInterestPoints
{
    MapContainer _mapContainer;

    public MapInterestPoints(MapContainer mapContainer)
    {
        _mapContainer = mapContainer;
    }

    public Vector3 GetRandomNPCSpawnPoint()
    {
        var randomIndex = Random.Range(0, _mapContainer.Chunks.Count);

        return _mapContainer.Chunks[randomIndex].GetComponent<SpawnPointsContainer>().DeliveryLootPoint.position;
    }
    
    public Vector3 GetRandomNPCSpawnPoint()
    {
        var randomIndex = Random.Range(0, _mapContainer.Chunks.Count);

        return _mapContainer.Chunks[randomIndex].GetComponent<SpawnPointsContainer>().NpcPoint.position;
    }
}