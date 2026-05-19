public class NavigationSystem : ITickable
{
    private NavigationArrow _navigationArrow;

    public NavigationSystem(NavigationArrow navigationArrow)
    {
        _navigationArrow = navigationArrow;
    }

    public void Tick()
    {
        if (GlobalEvents.TryGet<DeliveryManGoingToDeliveryPackageEvent>(out var deliveryManGoingToDeliveryPackageEvent))
            _navigationArrow.SetTarget(deliveryManGoingToDeliveryPackageEvent.DeliveryPackage.transform);
        
        if (GlobalEvents.TryGet<DeliveryManGoingToDeliveryRecipientEvent>(out var deliveryManGoingToDeliveryRecipientEvent))
            _navigationArrow.SetTarget(deliveryManGoingToDeliveryRecipientEvent.DeliveryRecipient.transform);
    }
}
