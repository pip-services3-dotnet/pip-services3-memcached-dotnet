using System;
using PipServices.Components.Build;
using PipServices.Commons.Refer;
using PipServices.Memcached.Cache;
using PipServices.Memcached.Lock;

namespace PipServices.Memcached.Build
{
    public class DefaultMemcachedFactory: Factory
    {
        public static readonly Descriptor Descriptor = new Descriptor("pip-services", "factory", "memcached", "default", "1.0");
        public static readonly Descriptor MemcachedCacheDescriptor = new Descriptor("pip-services", "cache", "memcached", "*", "1.0");
        public static readonly Descriptor MemcachedLockDescriptor = new Descriptor("pip-services", "lock", "memcached", "*", "1.0");

        public DefaultMemcachedFactory()
        {
            RegisterAsType(DefaultMemcachedFactory.MemcachedCacheDescriptor, typeof(MemcachedCache));
            RegisterAsType(DefaultMemcachedFactory.MemcachedLockDescriptor, typeof(MemcachedLock));
        }
    }
}
