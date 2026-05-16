using UnityEngine;

public class DeliverySystem : ITickable
{
    private DeliveryMan _deliveryMan;
    private DeliveryObject _deliveryObject;
    private DeliveryRecipient _deliveryRecipient;

    private DeliveryObjectFactory _deliveryObjectFactory;
    private DeliveryRecipientFactory _deliveryRecipientFactory;
    
    private IHorizontalAngleOffset _horizontalAngleOffset;

    private MapInterestPoints _mapInterestPoints;

    private DeliveryTaskBuilder _deliveryTaskBuilder;
    
    private bool _deliveryIsExists;

    public DeliverySystem(DeliveryMan deliveryMan, DeliveryObjectFactory deliveryObjectFactory, DeliveryRecipientFactory deliveryRecipientFactory, MapInterestPoints mapInterestPoints, IHorizontalAngleOffset horizontalAngleOffset, DeliveryTaskBuilder deliveryTaskBuilder)
    {
        _deliveryMan = deliveryMan;
        _deliveryObjectFactory = deliveryObjectFactory;
        _deliveryRecipientFactory = deliveryRecipientFactory;
        _mapInterestPoints = mapInterestPoints;
        _horizontalAngleOffset = horizontalAngleOffset;
        _deliveryTaskBuilder = deliveryTaskBuilder;
    }

    public void Tick()
    {
        TrySetupDelivery();
    }

    private void TrySetupDelivery()
    {
        if (_deliveryIsExists)
            return;

        _deliveryIsExists = true;

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
        };

        // TODO: После завершения квеста доставки нужно сообщить, что он был выполнен.
        // Возможно отправлять сигнал/ивент об этом
        
        UIManager.Instance.InstantiateOffer(deliveryTask, () =>
        {
            _deliveryTaskBuilder.Build(deliveryTask);
        });
    }
}