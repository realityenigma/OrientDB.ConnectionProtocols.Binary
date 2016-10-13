using OrientDB.ConnectionProtocols.Binary.Core;

namespace OrientDB.ConnectionProtocols.Binary.Command
{
    internal interface ICommandPayload
    {
        Request CreatePayloadRequest();
    }
}
