using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, IService> _services =  new Dictionary<Type, IService>();

    public static void Init()
    {
        _services.Clear();
    }
    
    public static void Register<TType>(IService service)
    {
        _services.Add(typeof(TType), service);
    }

    public static TType Get<TType>()
    {
        return (TType)_services[typeof(TType)];
    }
}