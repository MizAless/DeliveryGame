using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryTask : ITickable
{
    public DeliveryRecipientModel RecipientModel;
    public DeliveryObjectModel DeliveryObjectModel;
    public ITaskReward TaskReward;
    public float ExpiredDuration;
    
    public event Action<DeliveryTask> Cancelled;
    public event Action<DeliveryTask> Completed;

    private DateTime _expirationTime;

    private State _state;

    private enum State
    {
        Created,
        WaitingForAccept,
        InProgress,
        Completed,
        Cancelled,
    }    
    
    public void Tick()
    {
        if (_state != State.WaitingForAccept)
            return;

        if (_expirationTime < DateTime.Now)
            Cancel();
    }

    public void Offer()
    {
        _expirationTime = DateTime.Now + TimeSpan.FromSeconds(ExpiredDuration); 
        _state = State.WaitingForAccept;
    }
    
    public void Accept()
    {
        _state = State.InProgress;
    }

    public void Complete()
    {
        _state = State.Completed;
        Completed?.Invoke(this);
        
        GlobalEvents.Send(new DeliveryTaskCompletedEvent()
        {
            DeliveryTask = this
        });
    }
    
    private void Cancel()
    {
        _state = State.Cancelled;
        Cancelled?.Invoke(this);
        
        GlobalEvents.Send(new DeliveryTaskCanceledEvent()
        {
            DeliveryTask = this
        });
    }

    public DeliveryTask Copy()
    {
        return new DeliveryTask()
        {
            RecipientModel = RecipientModel,
            DeliveryObjectModel = DeliveryObjectModel,
            TaskReward = TaskReward,
        };
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