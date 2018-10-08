using System;
using PipServices.Components.Build;
using PipServices.Commons.Refer;
using PipServices.Memcached.Cache;
using PipServices.Memcached.Lock;

namespace PipServices.Memcached.Build
{
    /// <summary>
    /// Creates Redis components by their descriptors.
    /// </summary>
    /// See <a href="https://rawgit.com/pip-services-dotnet/pip-services-memcached-dotnet/master/doc/api/class_pip_services_1_1_memcached_1_1_cache_1_1_memcached_cache.html">MemcachedCache</a>, 
    /// <a href="https://rawgit.com/pip-services-dotnet/pip-services-memcached-dotnet/master/doc/api/class_pip_services_1_1_memcached_1_1_lock_1_1_memcached_lock.html">MemcachedLock</a>
    public class DefaultMemcachedFactory: Factory
    {
        public static readonly Descriptor Descriptor = new Descriptor("pip-services", "factory", "memcached", "default", "1.0");
        public static readonly Descriptor MemcachedCacheDescriptor = new Descriptor("pip-services", "cache", "memcached", "*", "1.0");
        public static readonly Descriptor MemcachedLockDescriptor = new Descriptor("pip-services", "lock", "memcached", "*", "1.0");

        /// <summary>
        /// Create a new instance of the factory.
        /// </summary>
        public DefaultMemcachedFactory()
        {
            RegisterAsType(DefaultMemcachedFactory.MemcachedCacheDescriptor, typeof(MemcachedCache));
            RegisterAsType(DefaultMemcachedFactory.MemcachedLockDescriptor, typeof(MemcachedLock));
        }
    }
}
