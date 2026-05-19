using System;
using Random = UnityEngine.Random;

public class DeliverySystem : ITickable
{
    private MapInterestPoints _mapInterestPoints;

    private DeliveryTaskBuilder _deliveryTaskBuilder;

    private enum State
    {
        OnCooldown,
        GeneratingDelivery,
        HasDelivery,
    } 
    
    private State _state = State.GeneratingDelivery;
        
    private Updater _updater;

    private DateTime  _cooldownExpirationTime;

    public DeliverySystem(MapInterestPoints mapInterestPoints, DeliveryTaskBuilder deliveryTaskBuilder)
    {
        _mapInterestPoints = mapInterestPoints;
        _deliveryTaskBuilder = deliveryTaskBuilder;

        _updater = ServiceLocator.Get<Updater>();
    }

    public void Tick()
    {
        if (GlobalEvents.TryGet<DeliveryTaskCompletedEvent>(out var _) || 
            GlobalEvents.TryGet<DeliveryTaskCanceledEvent>(out var _))
            SetupCooldown();

        if (_state == State.OnCooldown && _cooldownExpirationTime < DateTime.Now)
        {
            _state = State.GeneratingDelivery;
        }
        
        TrySetupDelivery();
    }

    private void SetupCooldown()
    {
        _state = State.OnCooldown;
        
        var cooldown = Random.Range(1f, 3f);
        _cooldownExpirationTime = DateTime.Now + TimeSpan.FromSeconds(cooldown); 
    }
    
    private void TrySetupDelivery()
    {
        if (_state != State.GeneratingDelivery)
            return;
        
        _state = State.HasDelivery;

        DeliveryObjectModel deliveryObject = new DeliveryObjectModel()
        {
            SpawnPosition = _mapInterestPoints.GetRandomDeliveryLootPoint()
        };

        DeliveryRecipientModel deliveryRecipient = new DeliveryRecipientModel()
        {
            SpawnPosition = _mapInterestPoints.GetRandomRecipientSpawnPoint()
        };

        DeliveryTask deliveryTask = new DeliveryTask()
        {
            DeliveryObjectModel = deliveryObject,
            RecipientModel = deliveryRecipient,
            ExpiredDuration = Random.Range(2f, 5f),
        };
        
        StartTask(deliveryTask);
    }
    
    private void StartTask(DeliveryTask deliveryTask)
    {
        _updater.Register(deliveryTask);
        deliveryTask.Offer();
        deliveryTask.Completed += OnCompletedDeliveryTask;
        deliveryTask.Cancelled += OnDeliveryTaskCancelled;
        
        UIManager.Instance.InstantiateOffer(deliveryTask, () =>
        {
            deliveryTask.Accept();
            UIManager.Instance.InstantiateActiveTask(deliveryTask);
            _deliveryTaskBuilder.Build(deliveryTask);
        });
    }

    private void OnDeliveryTaskCancelled(DeliveryTask deliveryTask)
    {
        deliveryTask.Cancelled -= OnDeliveryTaskCancelled;
        _updater.Unregister(deliveryTask);
    }

    private void OnCompletedDeliveryTask(DeliveryTask task)
    {
        task.Completed -= OnCompletedDeliveryTask;
        _updater.Unregister(task);
    }
}