public class EventsSystem : ITickable
{
    public void Tick()
    {
        GlobalEvents.ClearLastFrameEvents();
    }
}