using System.Collections.Generic;
using UnityEngine;

public class Updater : MonoBehaviour
{
    private HashSet<ITickable> _tickables = new HashSet<ITickable>();

    public void Register(ITickable tickable)
    {
        _tickables.Add(tickable);
    }

    private void Update()
    {
        foreach (var tickable in _tickables)
        {
            tickable.Tick();
        }
    }
}