using UnityEngine;

public class DeliverySystem : ITickable
{
    private DeliveryMan _deliveryMan;
    private DeliveryObject _deliveryObject;
    private DeliveryRecipient _deliveryRecipient;

    private DeliveryObjectFactory _deliveryObjectFactory;
    private DeliveryRecipientFactory _deliveryRecipientFactory;

    private RandomPlacer _randomPlacer;
    
    private bool _deliveryIsExists;

    public DeliverySystem(DeliveryMan deliveryMan, DeliveryObjectFactory deliveryObjectFactory, DeliveryRecipientFactory deliveryRecipientFactory, RandomPlacer randomPlacer)
    {
        _deliveryMan = deliveryMan;
        _deliveryObjectFactory = deliveryObjectFactory;
        _deliveryRecipientFactory = deliveryRecipientFactory;
        _randomPlacer = randomPlacer;
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
        
        _deliveryObject = _deliveryObjectFactory.Create();
        _randomPlacer.Place(_deliveryObject.transform);
        _deliveryMan.SetTarget(_deliveryObject);
        _deliveryMan.GrabEnded += OnGrabEnded;

        _deliveryRecipient = _deliveryRecipientFactory.Create();
        _randomPlacer.Place(_deliveryRecipient.transform);
        _deliveryMan.ThrowEnded += OnThrowEnded;
    }

    private void OnGrabEnded(DeliveryObject obj)
    {
        _deliveryMan.GrabEnded -= OnGrabEnded;
        
        _deliveryMan.SetTarget(_deliveryRecipient);
    }

    private void OnThrowEnded(DeliveryObject obj)
    {
        _deliveryMan.ThrowEnded -= OnThrowEnded;
        
        Object.Destroy(_deliveryRecipient.gameObject);
        _deliveryRecipient = null;
        
        _deliveryIsExists = false;
    }
}