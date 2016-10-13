using OrientDB.ConnectionProtocols.Binary.Command;
using OrientDB.ConnectionProtocols.Binary.Contracts;
using OrientDB.ConnectionProtocols.Binary.Operations;
using System.Collections.Generic;

namespace OrientDB.ConnectionProtocols.Binary.Core
{
    public class OrientDBCommand : IOrientDBCommand
    {
        private readonly OrientDBBinaryConnectionStream _stream;
        private readonly IOrientDBRecordSerializer _serializer;
        private readonly ICommandPayloadConstructorFactory _payloadFactory;

        internal OrientDBCommand(OrientDBBinaryConnectionStream stream, IOrientDBRecordSerializer serializer, ICommandPayloadConstructorFactory payloadFactory)
        {
            _stream = stream;
            _serializer = serializer;
            _payloadFactory = payloadFactory;
        }

        public IEnumerable<T> Execute<T>(string query)
        {
            return _stream.Send(new DatabaseCommandOperation<T>(_payloadFactory, _stream.ConnectionMetaData, query)).Results;
        }
    }
}
