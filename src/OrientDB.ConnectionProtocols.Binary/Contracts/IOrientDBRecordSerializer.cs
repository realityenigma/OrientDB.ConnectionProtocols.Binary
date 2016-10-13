using System.Collections.Generic;

namespace OrientDB.ConnectionProtocols.Binary.Contracts
{
    public interface IOrientDBRecordSerializer
    {
        IEnumerable<T> Deserialize<T>(object data);

        void Serialize(object data);
    }
}