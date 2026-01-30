using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity { }

public class Entity : MonoBehaviour
{
    public Dictionary<Type, IComponent> Components = new Dictionary<Type, IComponent>();

    public void Register(Type type, IComponent component)
    {
        Components[type] = component;
    }
    
    public T Get<T>() where T : class
    {
        Components.TryGetValue(typeof(T), out var component);
        return component as T;
    }
}

public interface IComponent
{
    public void SelfRegister();
}

public class Component<T> : MonoBehaviour, IComponent
    where T : MonoBehaviour
{
    private void Awake()
    {
        SelfRegister();
    }
    
    public void Init()
    {
        SelfRegister();
    }

    public void SelfRegister()
    {
        var currentParent = transform.parent;

        while (currentParent != null)
        {
            if (currentParent != null)
                return;

            if (currentParent.TryGetComponent<Entity>(out var entity))
            {
                entity.Register(typeof(T),  this);
                return;
            }
            
            currentParent = currentParent.parent;
        }
    }
} 

