using System.Collections.Generic;
using UnityEngine;

public class DeliveryTask
{
    public DeliveryRecipientModel RecipientModel;
    public DeliveryObjectModel DeliveryObjectModel;

    public ITaskReward TaskReward;
    
    public void Start()
    {
        
    }
    
    private void Complete()
    {
        
    }
    
}

public class DeliveryRecipientModel
{
    public Vector3 SpawnPosition; 
}

public class DeliveryObjectModel
{
    public Vector3 SpawnPosition; 
}

public interface ITaskReward
{
    List<(int, ResourceType)> Values { get; }
}

public enum ResourceType
{
    Coin,
    Battery,
    Energy,
}