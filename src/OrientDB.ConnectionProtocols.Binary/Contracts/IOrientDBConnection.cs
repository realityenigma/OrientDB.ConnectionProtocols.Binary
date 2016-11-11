using OrientDB.ConnectionProtocols.Binary.Core;

namespace OrientDB.ConnectionProtocols.Binary.Contracts
{
    internal interface IOrientDBConnection
    {
        IOrientDBCommand CreateCommand();
    }
}