using System;
using System.Threading.Tasks;
using PipServices3.Commons.Config;
using PipServices3.Commons.Convert;
using Xunit;

namespace PipServices3.Memcached.Cache
{
    [Collection("Sequential")]
    public class MemcachedCacheTest : IDisposable
    {
        private readonly bool _enabled;
        private readonly MemcachedCache _cache;
        private readonly CacheFixture _fixture;

        public MemcachedCacheTest()
        {
            var MEMCACHED_ENABLED = Environment.GetEnvironmentVariable("MEMCACHED_ENABLED") ?? "true";
            var MEMCACHED_SERVICE_HOST = Environment.GetEnvironmentVariable("MEMCACHED_SERVICE_HOST") ?? "localhost";
            var MEMCACHED_SERVICE_PORT = Environment.GetEnvironmentVariable("MEMCACHED_SERVICE_PORT") ?? "11211";

            _enabled = BooleanConverter.ToBoolean(MEMCACHED_ENABLED);
            if (_enabled)
            {
                _cache = new MemcachedCache();
                _cache.Configure(ConfigParams.FromTuples(
                    "connection.host", MEMCACHED_SERVICE_HOST,
                    "connection.port", MEMCACHED_SERVICE_PORT
                ));

                _fixture = new CacheFixture(_cache);

                _cache.OpenAsync(null).Wait();
            }
        }

        public void Dispose()
        {
            if (_cache != null)
            {
                _cache.CloseAsync(null).Wait();
            }
        }

        [Fact]
        public void TestStoreAsync()
        {
            _fixture.TestStoreAsync();
        }

        [Fact]
        public void TestStoreAndRetrieveAsync()
        {
            _fixture.TestStoreAndRetrieveAsync();
        }

        // Todo: Fix this test
        //[Fact]
        //public void TestStoreAndRetrieveExpiredAsync()
        //{
        //    _fixture.TestStoreAndRetrieveExpiredAsync();
        //}

        [Fact]
        public void TestStoreAndRetrieveRemovedAsync()
        {
            _fixture.TestStoreAndRetrieveRemovedAsync();
        }
    }
}
