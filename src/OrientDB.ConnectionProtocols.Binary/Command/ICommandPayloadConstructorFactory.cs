using OrientDB.ConnectionProtocols.Binary.Core;

namespace OrientDB.ConnectionProtocols.Binary.Command
{
    internal interface ICommandPayloadConstructorFactory
    {
        ICommandPayload CreatePayload(string query, string fetchPlan, ConnectionMetaData metaData);
    }
}
