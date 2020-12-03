using System;
using System.Threading.Tasks;

using Enyim.Caching;
using Enyim.Caching.Configuration;

using PipServices3.Commons.Config;
using PipServices3.Commons.Errors;
using PipServices3.Commons.Refer;
using PipServices3.Components.Auth;
using PipServices3.Components.Cache;
using PipServices3.Components.Connect;

namespace PipServices3.Memcached.Cache
{
    /// <summary>
    /// Distributed cache that stores values in Memcaches caching service.
    /// 
    /// The current implementation does not support authentication.
    /// 
    /// ### Configuration parameters ###
    /// 
    /// connection(s):
    /// - discovery_key:         (optional) a key to retrieve the connection from <a href="https://pip-services3-dotnet.github.io/pip-services3-components-dotnet/interface_pip_services_1_1_components_1_1_connect_1_1_i_discovery.html">IDiscovery</a>
    /// - host:                  host name or IP address
    /// - port:                  port number
    /// - uri:                   resource URI or connection string with all parameters in it
    /// 
    /// options:
    /// - max_size:              maximum number of values stored in this cache (default: 1000)        
    /// - max_key_size:          maximum key length (default: 250)
    /// - max_expiration:        maximum expiration duration in milliseconds (default: 2592000)
    /// - max_value:             maximum value length (default: 1048576)
    /// - pool_size:             pool size (default: 5)
    /// - reconnect:             reconnection timeout in milliseconds (default: 10 sec)
    /// - retries:               number of retries (default: 3)
    /// - timeout:               default caching timeout in milliseconds (default: 1 minute)
    /// - failures:              number of failures before stop retrying (default: 5)
    /// - retry:                 retry timeout in milliseconds (default: 30 sec)
    /// - idle:                  idle timeout before disconnect in milliseconds (default: 5 sec)
    /// 
    /// ### References ###
    /// 
    /// - *:discovery:*:*:1.0        (optional) <a href="https://pip-services3-dotnet.github.io/pip-services3-components-dotnet/interface_pip_services_1_1_components_1_1_connect_1_1_i_discovery.html">IDiscovery</a> services to resolve connection
    /// </summary>
    /// <example>
    /// <code>
    /// var cache = new MemcachedCache();
    /// cache.configure(ConfigParams.FromTuples(
    /// "host", "localhost",
    /// "port", 11211 ));
    /// cache.Open("123");
    /// 
    /// cache.Store("123", "key1", "ABC");
    /// </code>
    /// </example>
    public class MemcachedCache : AbstractCache
    {
        private ConnectionResolver _connectionResolver = new ConnectionResolver();
        private CredentialResolver _credentialResolver = new CredentialResolver();
        private MemcachedClient _client = null;

        /// <summary>
        /// Creates a new instance of this cache.
        /// </summary>
        public MemcachedCache()
        {
        }

        /// <summary>
        /// Configures component by passing configuration parameters.
        /// </summary>
        /// <param name="config">configuration parameters to be set.</param>
        public override void Configure(ConfigParams config)
        {
            base.Configure(config);

            _connectionResolver.Configure(config);
            _credentialResolver.Configure(config);
        }

        /// <summary>
        /// Sets references to dependent components.
        /// </summary>
        /// <param name="references">references to locate the component dependencies.</param>
        public override void SetReferences(IReferences references)
        {
            _connectionResolver.SetReferences(references);
            _credentialResolver.SetReferences(references);
        }

        /// <summary>
        /// Checks if the component is opened.
        /// </summary>
        /// <returns>true if the component has been opened and false otherwise.</returns>
        public override bool IsOpen()
        {
            return _client != null;
        }

        /// <summary>
        /// Opens the component.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        public override async Task OpenAsync(string correlationId)
        {
            var connections = await _connectionResolver.ResolveAllAsync(correlationId);
            if (connections.Count == 0)
                throw new ConfigException(correlationId, "NO_CONNECTION", "Connection is not configured");

            var options = new MemcachedClientConfiguration();

            foreach (var connection in connections)
            {
                var uri = connection.Uri;

                if (!string.IsNullOrEmpty(uri))
                {
                    options.AddServer(uri, 11211);
                }
                else 
                {
                    var host = connection.Host ?? "localhost";
                    var port = connection.Port != 0 ? connection.Port : 11211;

                    options.AddServer(host, port);
                }
            }

            _client = new MemcachedClient(null, options);
        }

        /// <summary>
        /// Closes component and frees used resources.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        public override async Task CloseAsync(string correlationId)
        {
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }

            await Task.Delay(0);
        }

        private void CheckOpened(string correlationId)
        {
            if (!IsOpen())
                throw new InvalidStateException(correlationId, "NOT_OPENED", "Connection is not opened");
        }

        /// <summary>
        /// Retrieves cached value from the cache using its key.
        /// If value is missing in the cache or expired it returns null.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <param name="key">a unique value key.</param>
        /// <returns>cached value.</returns>
        public override async Task<T> RetrieveAsync<T>(string correlationId, string key)
        {
            CheckOpened(correlationId);

            return await _client.GetAsync<T>(key);
        }

        /// <summary>
        /// Stores value in the cache with expiration time.
        /// </summary>
        /// <typeparam name="T">the class type</typeparam>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <param name="key">a unique value key.</param>
        /// <param name="value">a value to store.</param>
        /// <param name="timeout">expiration timeout in milliseconds.</param>
        /// <returns>stored value.</returns>
        public override async Task<T> StoreAsync<T>(string correlationId, string key, T value, long timeout)
        {
            CheckOpened(correlationId);

            timeout = timeout > 0 ? timeout : Timeout;

            var result = await _client.StoreAsync(Enyim.Caching.Memcached.StoreMode.Set, key, value, TimeSpan.FromMilliseconds(timeout));

            return result ? value : default(T);
        }

        /// <summary>
        /// Removes a value from the cache by its key.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <param name="key">a unique value key.</param>
        public override async Task RemoveAsync(string correlationId, string key)
        {
            CheckOpened(correlationId);

            await _client.RemoveAsync(key);
        }
    }
}
