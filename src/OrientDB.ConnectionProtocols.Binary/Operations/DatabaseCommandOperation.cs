using OrientDB.ConnectionProtocols.Binary.Command;
using OrientDB.ConnectionProtocols.Binary.Contracts;
using OrientDB.ConnectionProtocols.Binary.Core;
using System;
using System.IO;
using System.Linq;

namespace OrientDB.ConnectionProtocols.Binary.Operations
{
    internal class DatabaseCommandOperation<T> : IOrientDBOperation<CommandResult<T>>
    {
        private readonly string _query;
        private readonly string _fetchPlan;
        private readonly ICommandPayloadConstructorFactory _payloadFactory;
        private readonly ConnectionMetaData _metaData;

        public DatabaseCommandOperation(ICommandPayloadConstructorFactory payloadFacctory, ConnectionMetaData metaData, string query, string fetchPlan = "*:0")
        {
            _query = query;
            _fetchPlan = fetchPlan;
            _payloadFactory = payloadFacctory;
            _metaData = metaData;
        }

        public Request CreateRequest()
        {
            return _payloadFactory.CreatePayload(_query, _fetchPlan, _metaData).CreatePayloadRequest();
        }

        public CommandResult<T> Execute(BinaryReader reader)
        {
            // Need to fuse Serializer logic here in order to return results to caller.
            return new CommandResult<T>(Enumerable.Empty<T>());           
        }

        private T ParseDocument(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
