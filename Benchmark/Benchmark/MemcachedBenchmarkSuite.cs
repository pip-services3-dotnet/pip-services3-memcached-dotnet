using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Benchmark.Data;

using PipBenchmark;
using PipServices.Commons.Config;
using PipServices.Commons.Random;
using PipServices.Memcached.Cache;

namespace Benchmark.Benchmark
{
    public class MemcachedBenchmarkSuite : BenchmarkSuite, IDisposable
    {
        private List<DummyCacheObject> listCacheObjects = new List<DummyCacheObject>();
        private List<string> _ids = new List<string>();
        
        private string _correlationId = "Memcached.Benchmark";

        private MemcachedCache _cache = new MemcachedCache();

        private static readonly ConfigParams CacheConfig = ConfigParams.FromTuples(
            "connection.host", Environment.GetEnvironmentVariable("CACHE_HOST"),
            "connection.port", Environment.GetEnvironmentVariable("CACHE_PORT") 
        );

        public MemcachedBenchmarkSuite() : base("Memcached.Benchmark", "Measures performance of Memcached Benchmark")
        {
            InitBenchmark();

            //CreateBenchmark("Create", "Measures performance of Add", BenchmarkCreate);
            //CreateBenchmark("Delete", "Measures performance of Delete", BenchmarkDelete);
            CreateBenchmark("Get", "Measures performance of Get", BenchmarkGet);
        }

        private void InitBenchmark()
        {
            _cache.Configure(CacheConfig);
            _cache.OpenAsync(null).Wait();

            for (int i = 0; i < 300000; i++)
            {
                listCacheObjects.Add(RandomDummyCacheObject.GenerateCacheObject());    
                _ids.Add(listCacheObjects[i].Id);
                _cache.StoreAsync(_correlationId, listCacheObjects[i].Id, listCacheObjects[i], 600000).Wait();
            }
        }


        public void Dispose() { }

        public override async void SetUp()
        {
            await Task.Delay(0);
        }

        public void BenchmarkCreate()
        {
            var _object = RandomDummyCacheObject.GenerateCacheObject();

            var result = _cache.StoreAsync(_correlationId, _object.Id, _object, 600000).Result;
            _ids.Add(result.Id);
        }

        public void BenchmarkDelete()
        {
            var id = RandomArray.Pick(_ids.ToArray());

            if (!string.IsNullOrWhiteSpace(id))
            {
                _cache.RemoveAsync(_correlationId, id).Wait();
            }
        }
        public void BenchmarkGet()
        {
            var id = RandomArray.Pick(_ids.ToArray());

            if (!string.IsNullOrWhiteSpace(id))
            {
                var a = _cache.RetrieveAsync<DummyCacheObject>(_correlationId, id).Result;

                if (a == null)
                {
                    var obj = listCacheObjects.FirstOrDefault(t => t.Id == id);
                    _cache.StoreAsync(_correlationId, obj.Id, obj, 600000).Wait();
                }
            }
        }

    }
}
