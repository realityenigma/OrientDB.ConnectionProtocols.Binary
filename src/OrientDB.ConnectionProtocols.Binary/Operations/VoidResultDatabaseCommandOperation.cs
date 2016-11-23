using Operations;
using OrientDB.ConnectionProtocols.Binary.Contracts;
using System;
using OrientDB.ConnectionProtocols.Binary.Core;
using System.IO;
using OrientDB.ConnectionProtocols.Binary.Command;
using OrientDB.Core.Abstractions;
using OrientDB.ConnectionProtocols.Binary.Extensions;
using System.Reflection;
using System.Net.Sockets;

namespace OrientDB.ConnectionProtocols.Binary.Operations
{
    internal class VoidResultDatabaseCommandOperation : IOrientDBOperation<VoidResult>
    {
        private readonly string _fetchPlan;
        private ConnectionMetaData _connectionMetaData;
        private string _query;
        private ICommandPayloadConstructorFactory _payloadFactory;
        private IOrientDBRecordSerializer<byte[]> _serializer;

        public VoidResultDatabaseCommandOperation(ICommandPayloadConstructorFactory _payloadFactory, ConnectionMetaData connectionMetaData, IOrientDBRecordSerializer<byte[]> _serializer, string query, string fetchPlan = "*:0")
        {
            this._payloadFactory = _payloadFactory;
            this._connectionMetaData = connectionMetaData;
            this._serializer = _serializer;
            this._query = query;
            _fetchPlan = fetchPlan;
        }

        public Request CreateRequest(int sessionId, byte[] token)
        {
            return _payloadFactory.CreatePayload(_query, _fetchPlan, _connectionMetaData).CreatePayloadRequest(sessionId, token);
        }

        public VoidResult Execute(BinaryReader reader)
        {
            while (!EndOfStream(reader))
                reader.ReadByte();

            return new VoidResult();
        }

        protected bool EndOfStream(BinaryReader reader)
        {
            return !(bool)reader.BaseStream.GetType().GetProperty("DataAvailable").GetValue(reader.BaseStream);
        }
    }
}
