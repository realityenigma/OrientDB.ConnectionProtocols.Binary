﻿using Operations;
using OrientDB.ConnectionProtocols.Binary.Operations;
using OrientDB.Core.Abstractions;
using System;

namespace OrientDB.ConnectionProtocols.Binary.Core
{
    public class OrientDBBinaryServerConnection : IDisposable
    {
        private readonly ConnectionOptions _options;
        private readonly IOrientDBRecordSerializer<byte[]> _serializer;
        private OrientDBBinaryConnectionStream _connectionStream;

        public OrientDBBinaryServerConnection(ConnectionOptions options, IOrientDBRecordSerializer<byte[]> serializer)
        {
            _options = options;
            _serializer = serializer;
        }

        public void Open()
        {
            _connectionStream = new OrientDBBinaryConnectionStream(_options);
            foreach(var stream in _connectionStream.StreamPool)
            {
                var _openResult = _connectionStream.Send(new ServerOpenOperation(_options, _connectionStream.ConnectionMetaData));
                stream.SessionId = _openResult.SessionId;
                stream.Token = _openResult.Token;
            }
        }

        public OrientDBBinaryConnection CreateDatabase(string database, DatabaseType type, StorageType storageType)
        {
            return _connectionStream.Send(new DatabaseCreateOperation(database, type, storageType, _connectionStream.ConnectionMetaData, _options));
        }

        public void DropDatabase(string database, StorageType storageType)
        {
            _connectionStream.Send(new DatabaseDropOperation(database, storageType, _connectionStream.ConnectionMetaData, _options));
        }

        public bool DatabaseExists(string database, StorageType storageType)
        {
            return _connectionStream.Send(new DatabaseExistsOperation(database, storageType, _connectionStream.ConnectionMetaData, _options)).Exists;
        }

        public void Dispose()
        {
            _connectionStream.Close();
        }
    }
}
