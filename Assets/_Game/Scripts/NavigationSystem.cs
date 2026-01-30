public class NavigationSystem : ITickable
{
    private NavigationArrow _navigationArrow;

    public NavigationSystem(NavigationArrow navigationArrow)
    {
        _navigationArrow = navigationArrow;
    }

    public void Tick()
    {
        if (GlobalEvents.TryGet<DeliveryManGoingToDeliveryObjectEvent>(out var deliveryManGoingToDeliveryObjectEvent))
            _navigationArrow.SetTarget(deliveryManGoingToDeliveryObjectEvent.DeliveryObject.transform);
        
        if (GlobalEvents.TryGet<DeliveryManGoingToDeliveryRecipientEvent>(out var deliveryManGoingToDeliveryRecipientEvent))
            _navigationArrow.SetTarget(deliveryManGoingToDeliveryRecipientEvent.DeliveryRecipient.transform);
    }
}
