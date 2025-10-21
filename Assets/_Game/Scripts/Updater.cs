using System.Collections.Generic;
using UnityEngine;

public class Updater : MonoBehaviour
{
    private HashSet<ITickable> _tickables = new HashSet<ITickable>();
    private HashSet<ILateTickable> _lateTickables = new HashSet<ILateTickable>();
    
    public void Register(ITickable tickable)
    {
        _tickables.Add(tickable);
    }
    public void Register(ILateTickable lateTickable)
    {
        _lateTickables.Add(lateTickable);
    }

    private void Update()
    {
        foreach (var tickable in _tickables)
        {
            tickable.Tick();
        }
    }
    
    private void LateUpdate()
    {
        foreach (var lateTickable in _lateTickables)
        {
            lateTickable.LateTick();
        }
    }
}