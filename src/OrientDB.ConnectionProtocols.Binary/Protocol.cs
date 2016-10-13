using OrientDB.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using OrientDB.ConnectionProtocols.Binary.Core;
using OrientDB.ConnectionProtocols.Binary.Contracts;

namespace OrientDB.ConnectionProtocols.Binary
{
    public class Protocol : IOrientDBConnectionProtocol<byte[]>
    {
        private readonly IOrientDBConnection _connection;

        public Protocol(string hostName, string userName, string password, string databaseName, DatabaseType type, int port = 2480) : this(new ConnectionOptions
        {
            Database = databaseName,
            Type = type,
            HostName = hostName,
            Password = password,
            Port = port,
            UserName = userName
        })
        { }

        public Protocol(ConnectionOptions options)
        {
            
        }

        public IOrientDBCommandResult ExecuteCommand(string sql, IOrientDBRecordSerializer<byte[]> serializer)
        {
            // The Core interface will be changing.
            throw new NotImplementedException();
        }

        public IEnumerable<TResultType> ExecuteQuery<TResultType>(string sql, IOrientDBRecordSerializer<byte[]> serializer)
        {
            byte[] data = new byte[] { };

            IOrientDBEntity resultType = (IOrientDBEntity)Activator.CreateInstance<TResultType>();
            resultType.Hydrate(new Dictionary<string, object>());

            return serializer.Deserialize<IOrientDBEntity>(data).Select(n => (TResultType)n).ToList();
        }
    }
}
