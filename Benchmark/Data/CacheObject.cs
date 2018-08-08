using System;
using System.Runtime.Serialization;

namespace Benchmark.Data
{
    [Serializable][DataContract]
    public class DummyCacheObject
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Resource { get; set; }
    }
}
