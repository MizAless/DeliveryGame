using UnityEngine;

public class DeliveryTaskBuilder
{
    private DeliveryObjectFactory _deliveryObjectFactory;
    private DeliveryRecipientFactory _deliveryRecipientFactory;
    private IHorizontalAngleOffset _horizontalAngleOffset;
    private DeliveryMan _deliveryMan;

    private DeliveryRecipient _currentDeliveryRecipient;
    
    private DeliveryTask _currentTask;

    public DeliveryTaskBuilder(DeliveryObjectFactory deliveryObjectFactory, DeliveryRecipientFactory deliveryRecipientFactory, IHorizontalAngleOffset horizontalAngleOffset, DeliveryMan deliveryMan)
    {
        _deliveryObjectFactory = deliveryObjectFactory;
        _deliveryRecipientFactory = deliveryRecipientFactory;
        _horizontalAngleOffset = horizontalAngleOffset;
        _deliveryMan = deliveryMan;
    }

    public void Build(DeliveryTask deliveryTask)
    {
        _currentTask = deliveryTask;
        
        var deliveryObject = _deliveryObjectFactory.Create();
        deliveryObject.transform.position = deliveryTask.DeliveryObjectModel.SpawnPosition;
        _deliveryMan.SetTarget(deliveryObject);
        _deliveryMan.GrabEnded += OnGrabEnded;

        _currentDeliveryRecipient = _deliveryRecipientFactory.Create();
        _currentDeliveryRecipient.Init(_horizontalAngleOffset);
        _currentDeliveryRecipient.transform.position = deliveryTask.RecipientModel.SpawnPosition;
        _deliveryMan.ThrowEnded += OnThrowEnded;
    }

    private void OnGrabEnded(DeliveryPackage obj)
    {
        _deliveryMan.GrabEnded -= OnGrabEnded;
        
        _deliveryMan.SetTarget(_currentDeliveryRecipient);
    }

    private void OnThrowEnded(DeliveryPackage obj)
    {
        _deliveryMan.ThrowEnded -= OnThrowEnded;
        
        Object.Destroy(_currentDeliveryRecipient.gameObject);
        _currentDeliveryRecipient = null;

        _currentTask.Complete();
        _currentTask = null;
    }
}