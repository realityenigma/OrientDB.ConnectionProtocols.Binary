using OrientDB.ConnectionProtocols.Binary.Core;

namespace OrientDB.ConnectionProtocols.Binary.Contracts
{
    internal interface IOrientDBConnection
    {
        bool CreateDatabase(string name, DatabaseType type);
        void UseDatabase(string database);
        IOrientDBCommand CreateCommand();
    }
}