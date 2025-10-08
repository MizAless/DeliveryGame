using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static Dictionary<Type, IService> _services;

    public static void Register<TType>(IService service)
    {
        _services.Add(typeof(TType), service);
    }

    public static TType Get<TType>()
    {
        return (TType)_services[typeof(TType)];
    }
}