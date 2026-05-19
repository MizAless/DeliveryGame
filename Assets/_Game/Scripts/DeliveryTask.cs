using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryTask : ITickable
{
    public DeliveryRecipientModel RecipientModel;
    public DeliveryPackageModel DeliveryPackageModel;
    public ITaskReward TaskReward;
    public float ExpiredDuration;
    
    public event Action<DeliveryTask> Cancelled;
    public event Action<DeliveryTask> Completed;
    public event Action<TaskState> StateChanged;

    private DateTime _expirationTime;

    private TaskState _state;

    public TaskState State
    {
        get => _state;
        set
        {
            _state = value;
            StateChanged?.Invoke(State);
        }
    }

    public enum TaskState
    {
        WaitingForAccept,
        InProgress,
        GrabPackage,
        GivePackage,
        Completed,
        Cancelled,
    }
    
    public void Tick()
    {
        if (State != TaskState.WaitingForAccept)
            return;

        if (_expirationTime < DateTime.Now)
            Cancel();
    }

    public void Offer()
    {
        _expirationTime = DateTime.Now + TimeSpan.FromSeconds(ExpiredDuration); 
        State = TaskState.WaitingForAccept;
    }
    
    public void Accept()
    {
        State = TaskState.InProgress;
        State = TaskState.GrabPackage;
    }
    
    public void SetGiveState()
    {
        State = TaskState.GivePackage;
    }

    public void Complete()
    {
        State = TaskState.Completed;
        Completed?.Invoke(this);
        
        GlobalEvents.Send(new DeliveryTaskCompletedEvent()
        {
            DeliveryTask = this
        });
    }
    
    private void Cancel()
    {
        State = TaskState.Cancelled;
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
            DeliveryPackageModel = DeliveryPackageModel,
            TaskReward = TaskReward,
        };
    }
}

public class DeliveryRecipientModel
{
    public Vector3 SpawnPosition; 
}

public class DeliveryPackageModel
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