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
    /// See <a href="https://rawgit.com/pip-services3-dotnet/pip-services3-memcached-dotnet/master/doc/api/class_pip_services_1_1_memcached_1_1_cache_1_1_memcached_cache.html">MemcachedCache</a>, 
    /// <a href="https://rawgit.com/pip-services3-dotnet/pip-services3-memcached-dotnet/master/doc/api/class_pip_services_1_1_memcached_1_1_lock_1_1_memcached_lock.html">MemcachedLock</a>
    public class DefaultMemcachedFactory: Factory
    {
        public static readonly Descriptor Descriptor = new Descriptor("pip-services3", "factory", "memcached", "default", "1.0");
        public static readonly Descriptor MemcachedCacheDescriptor = new Descriptor("pip-services3", "cache", "memcached", "*", "1.0");
        public static readonly Descriptor MemcachedLockDescriptor = new Descriptor("pip-services3", "lock", "memcached", "*", "1.0");

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
