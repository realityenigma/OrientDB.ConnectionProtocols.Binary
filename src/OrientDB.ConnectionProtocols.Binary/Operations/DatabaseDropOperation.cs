using OrientDB.ConnectionProtocols.Binary.Contracts;
using System;
using System.IO;
using OrientDB.ConnectionProtocols.Binary.Core;
using OrientDB.ConnectionProtocols.Binary.Constants;

namespace OrientDB.ConnectionProtocols.Binary.Operations
{
    internal class DatabaseDropOperation : IOrientDBOperation<VoidOperationResult>
    {
        private readonly string _database;
        private readonly ConnectionMetaData _metaData;
        private readonly ConnectionOptions _options;
        private readonly StorageType _storageType;

        public DatabaseDropOperation(string database, StorageType storageType, ConnectionMetaData metaData, ConnectionOptions options)
        {
            _database = database;
            _metaData = metaData;
            _options = options;
            _storageType = storageType;
        }

        public Request CreateRequest(int sessionId, byte[] token)
        {
            Request request = new Request(OperationMode.Synchronous, sessionId);

            request.AddDataItem((byte)OperationType.DB_DROP);
            request.AddDataItem(request.SessionId);

            if (DriverConstants.ProtocolVersion > 26 && _metaData.UseTokenBasedSession)
            {
                request.AddDataItem(token);
            }

            request.AddDataItem(_database);
            if (DriverConstants.ProtocolVersion >= 16)
                request.AddDataItem(_storageType.ToString().ToLower());

            return request;
        }

        public VoidOperationResult Execute(BinaryReader reader)
        {
            return new VoidOperationResult();
        }
    }
}