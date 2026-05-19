using System.Collections.Generic;
using UnityEngine;

public class Updater : MonoBehaviour, IService
{
    private readonly List<ITickable> _tickables = new List<ITickable>();
    private readonly List<ILateTickable> _lateTickables = new List<ILateTickable>();
    
    public void Register(ITickable tickable)
    {
        _tickables.Add(tickable);
    }
    
    public void Register(ILateTickable lateTickable)
    {
        _lateTickables.Add(lateTickable);
    }
    
    public void Unregister(ITickable tickable)
    {
        _tickables.Remove(tickable);
    }
    
    public void Unregister(ILateTickable lateTickable)
    {
        _lateTickables.Remove(lateTickable);
    }

    private void Update()
    {
        for (int i = 0; i < _tickables.Count; i++)
        {
            _tickables[i].Tick();
        }
    }
    
    private void LateUpdate()
    {
        for (int i = 0; i < _lateTickables.Count; i++)
        {
            _lateTickables[i].LateTick();
        }
    }
}