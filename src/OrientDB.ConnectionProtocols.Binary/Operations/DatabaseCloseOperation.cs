using OrientDB.ConnectionProtocols.Binary.Constants;
using OrientDB.ConnectionProtocols.Binary.Contracts;
using OrientDB.ConnectionProtocols.Binary.Core;
using OrientDB.ConnectionProtocols.Binary.Extensions;
using System.IO;

namespace OrientDB.ConnectionProtocols.Binary.Operations
{
    internal class DatabaseCloseOperation : IOrientDBOperation<CloseDatabaseResult>
    {
        private readonly ConnectionMetaData _metaData;
        private readonly byte[] _connectionToken;

        public DatabaseCloseOperation(byte[] token, ConnectionMetaData metaData)
        {
            _connectionToken = token;
            _metaData = metaData;
        }

        internal byte[] ReadToken(BinaryReader reader)
        {
            var size = reader.ReadInt32EndianAware();
            var token = reader.ReadBytesRequired(size);            

            return token;
        }

        public Request CreateRequest()
        {
            Request request = new Request(OperationMode.Asynchronous);

            request.AddDataItem((byte)OperationType.DB_CLOSE);
            request.AddDataItem(request.SessionId);

            if (DriverConstants.ProtocolVersion > 26 && _metaData.UseTokenBasedSession)
            {
                request.AddDataItem(_connectionToken);
            }

            return request;
        }

        public CloseDatabaseResult Execute(BinaryReader reader)
        {
            return new CloseDatabaseResult(true);
        }
    }
}