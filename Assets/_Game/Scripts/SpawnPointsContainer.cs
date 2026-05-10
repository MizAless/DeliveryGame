using System.Collections.Generic;
using UnityEngine;

public class SpawnPointsContainer : MonoBehaviour
{
    [SerializeField] private List<Transform> _npcPoints;
    [SerializeField] private List<Transform> _deliveryLootPoints;
    
    public IReadOnlyList<Transform> NPCPoints => _npcPoints;
    public IReadOnlyList<Transform> DeliveryLootPoint => _deliveryLootPoints;
    
    public Vector3 GetRandomNPCSpawnPoint()
    {
        return _npcPoints[Random.Range(0, _npcPoints.Count)].position;
    }
    
    public Vector3 GetRandomDeliveryLootPoint()
    {
        return _deliveryLootPoints[Random.Range(0, _deliveryLootPoints.Count)].position;
    }
}