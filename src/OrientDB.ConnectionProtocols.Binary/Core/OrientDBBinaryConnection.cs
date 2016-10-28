using OrientDB.ConnectionProtocols.Binary.Command;
using OrientDB.ConnectionProtocols.Binary.Contracts;
using OrientDB.ConnectionProtocols.Binary.Operations;
using OrientDB.Core.Abstractions;
using System;

namespace OrientDB.ConnectionProtocols.Binary.Core
{
    public class OrientDBBinaryConnection : IOrientDBConnection, IDisposable
    {
        private readonly IOrientDBRecordSerializer<byte[]> _serialier;
        private readonly ConnectionOptions _connectionOptions;
        private OrientDBBinaryConnectionStream _connectionStream;
        private OpenDatabaseResult _openResult; // might not be how I model this here in the end.
        private ICommandPayloadConstructorFactory _payloadFactory;


        public OrientDBBinaryConnection(ConnectionOptions options, IOrientDBRecordSerializer<byte[]> serializer)
        {
            _connectionOptions = options;
            _serialier = serializer;
            _payloadFactory = new CommandPayloadConstructorFactory();
        }

        public OrientDBBinaryConnection(string hostname, string username, string password, IOrientDBRecordSerializer<byte[]> serializer, int port = 2424, int poolsize = 10)
        {
            _serialier = serializer;
            _connectionOptions = new ConnectionOptions
            {
                HostName = hostname,
                Password = password,
                PoolSize = poolsize,
                Port = port,
                UserName = username
            };
        }

        public void Open()
        {
            _connectionStream = new OrientDBBinaryConnectionStream(_connectionOptions);
            _openResult = _connectionStream.Send(new DatabaseOpenOperation(_connectionOptions, _connectionStream.ConnectionMetaData));
            _connectionStream.ConnectionMetaData.SessionId = _openResult.SessionId; // This is temporary.
        }

        public void Close()
        {
            _connectionStream.Send(new DatabaseCloseOperation(_openResult.Token, _connectionStream.ConnectionMetaData));
            _connectionStream.Close();
        }

        public IOrientDBCommand CreateCommand()
        {
            return new OrientDBCommand(_connectionStream, _serialier, _payloadFactory);
        }

        public bool CreateDatabase(string name, DatabaseType type)
        {
            throw new NotImplementedException();
        }

        public void UseDatabase(string database)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Close();
        }
    }
}
