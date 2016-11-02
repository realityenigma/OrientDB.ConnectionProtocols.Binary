using OrientDB.ConnectionProtocols.Binary.Core;
using System.IO;

namespace OrientDB.ConnectionProtocols.Binary.Contracts
{
    internal interface IOrientDBOperation<T>
    {
        Request CreateRequest(int sessionId, byte[] token);
        T Execute(BinaryReader reader);
    }
}
