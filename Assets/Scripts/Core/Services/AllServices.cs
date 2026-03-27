using System;
using System.Collections.Generic;

namespace Core.Services
{
    public static class AllServices
    {
        private static readonly Dictionary<Type, object> _services = new();

        public static void Register<TService>(TService instance) where TService : class
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            _services[typeof(TService)] = instance;
        }

        public static TService Get<TService>() where TService : class
        {
            if (_services.TryGetValue(typeof(TService), out var service))
            {
                return (TService)service;
            }

            throw new InvalidOperationException($"Service not registered: {typeof(TService).Name}");
        }

        public static bool TryGet<TService>(out TService service) where TService : class
        {
            if (_services.TryGetValue(typeof(TService), out var serviceInstance))
            {
                service = (TService)serviceInstance;
                return true;
            }

            service = null;
            return false;
        }

        public static void Reset()
        {
            _services.Clear();
        }
    }
}