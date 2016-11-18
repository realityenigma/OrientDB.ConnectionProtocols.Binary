using OrientDB.ConnectionProtocols.Binary.Command;
using OrientDB.ConnectionProtocols.Binary.Contracts;
using OrientDB.ConnectionProtocols.Binary.Operations;
using OrientDB.Core.Abstractions;
using OrientDB.Core.Models;
using System.Collections.Generic;
using System;
using Operations;

namespace OrientDB.ConnectionProtocols.Binary.Core
{
    public class OrientDBCommand : IOrientDBCommand
    {
        private readonly OrientDBBinaryConnectionStream _stream;
        private readonly IOrientDBRecordSerializer<byte[]> _serializer;
        private readonly ICommandPayloadConstructorFactory _payloadFactory;

        internal OrientDBCommand(OrientDBBinaryConnectionStream stream, IOrientDBRecordSerializer<byte[]> serializer, ICommandPayloadConstructorFactory payloadFactory)
        {
            _stream = stream;
            _serializer = serializer;
            _payloadFactory = payloadFactory;
        }

        public IEnumerable<T> Execute<T>(string query) where T : OrientDBEntity
        {
            return _stream.Send(new DatabaseCommandOperation<T>(_payloadFactory, _stream.ConnectionMetaData, _serializer, query)).Results;
        }

        public void Execute(string query)
        {
            _stream.Send(new VoidResultDatabaseCommandOperation(_payloadFactory, _stream.ConnectionMetaData, _serializer, query));
        }
    }
}
