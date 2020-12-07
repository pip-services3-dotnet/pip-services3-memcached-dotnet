using System;
using PipServices3.Components.Build;
using PipServices3.Commons.Refer;
using PipServices3.Memcached.Cache;
using PipServices3.Memcached.Lock;

namespace PipServices3.Memcached.Build
{
    /// <summary>
    /// Creates Redis components by their descriptors.
    /// </summary>
    /// See <a href="https://pip-services3-dotnet.github.io/pip-services3-memcached-dotnet/class_pip_services_1_1_memcached_1_1_cache_1_1_memcached_cache.html">MemcachedCache</a>, 
    /// <a href="https://pip-services3-dotnet.github.io/pip-services3-memcached-dotnet/class_pip_services_1_1_memcached_1_1_lock_1_1_memcached_lock.html">MemcachedLock</a>
    public class DefaultMemcachedFactory: Factory
    {
        public static readonly Descriptor Descriptor = new Descriptor("pip-services", "factory", "memcached", "default", "1.0");
        public static readonly Descriptor Descriptor3 = new Descriptor("pip-services3", "factory", "memcached", "default", "1.0");
        public static readonly Descriptor MemcachedCacheDescriptor = new Descriptor("pip-services", "cache", "memcached", "*", "1.0");
        public static readonly Descriptor MemcachedCache3Descriptor = new Descriptor("pip-services3", "cache", "memcached", "*", "1.0");
        public static readonly Descriptor MemcachedLockDescriptor = new Descriptor("pip-services", "lock", "memcached", "*", "1.0");
        public static readonly Descriptor MemcachedLock3Descriptor = new Descriptor("pip-services3", "lock", "memcached", "*", "1.0");

        /// <summary>
        /// Create a new instance of the factory.
        /// </summary>
        public DefaultMemcachedFactory()
        {
            RegisterAsType(MemcachedCacheDescriptor, typeof(MemcachedCache));
            RegisterAsType(MemcachedCache3Descriptor, typeof(MemcachedCache));
            RegisterAsType(MemcachedLockDescriptor, typeof(MemcachedLock));
            RegisterAsType(MemcachedLock3Descriptor, typeof(MemcachedLock));
        }
    }
}
