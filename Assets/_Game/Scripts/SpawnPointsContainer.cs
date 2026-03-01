using UnityEngine;

public class SpawnPointsContainer : MonoBehaviour
{
    [SerializeField] private Transform _npcPoint;
    [SerializeField] private Transform _deliveryLootPoint;
    
    public Transform NpcPoint => _npcPoint;
    public Transform DeliveryLootPoint => _deliveryLootPoint;
}