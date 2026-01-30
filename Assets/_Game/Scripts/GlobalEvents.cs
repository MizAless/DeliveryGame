using System;
using System.Collections.Generic;
using System.Linq;

public static class GlobalEvents
{
    private static List<IEvent> _lastFrameEvents = new List<IEvent>();
    
    public static void Send(IEvent @event)
    {
        _lastFrameEvents.Add(@event);
    }
        
    public static bool TryGet<T>(out T outEvent) 
        where T : class
    {
        outEvent = _lastFrameEvents.FirstOrDefault(ev => ev.GetType() == typeof(T)) as T;

        return outEvent != null;
    }

    public static void ClearLastFrameEvents()
    {
        _lastFrameEvents.Clear();
    }
}

public interface IEvent { }

public class DeliveryManGoingToDeliveryObjectEvent : IEvent
{
    public DeliveryObject DeliveryObject { get; set; }
}

public class DeliveryManGoingToDeliveryRecipientEvent : IEvent
{
    public DeliveryRecipient DeliveryRecipient { get; set; }
}