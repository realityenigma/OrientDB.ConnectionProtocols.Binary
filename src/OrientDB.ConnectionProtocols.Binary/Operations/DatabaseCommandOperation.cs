using OrientDB.ConnectionProtocols.Binary.Command;
using OrientDB.ConnectionProtocols.Binary.Contracts;
using OrientDB.ConnectionProtocols.Binary.Core;
using OrientDB.ConnectionProtocols.Binary.Extensions;
using OrientDB.Core;
using OrientDB.Core.Abstractions;
using OrientDB.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OrientDB.ConnectionProtocols.Binary.Operations
{
    internal class DatabaseCommandOperation<T> : IOrientDBOperation<CommandResult<T>> where T : OrientDBEntity
    {
        private readonly string _query;
        private readonly string _fetchPlan;
        private readonly ICommandPayloadConstructorFactory _payloadFactory;
        private readonly ConnectionMetaData _metaData;
        private readonly IOrientDBRecordSerializer<byte[]> _serializer;

        public DatabaseCommandOperation(ICommandPayloadConstructorFactory payloadFacctory, ConnectionMetaData metaData, IOrientDBRecordSerializer<byte[]> serializer, string query, string fetchPlan = "*:0")
        {
            _query = query;
            _fetchPlan = fetchPlan;
            _payloadFactory = payloadFacctory;
            _metaData = metaData;
            _serializer = serializer;
        }

        public Request CreateRequest()
        {
            return _payloadFactory.CreatePayload(_query, _fetchPlan, _metaData).CreatePayloadRequest();
        }

        public CommandResult<T> Execute(BinaryReader reader)
        {
            PayloadStatus payloadStatus = (PayloadStatus)reader.ReadByte();

            List<T> documents = new List<T>();

            int contentLength;

            switch (payloadStatus)
            {
                case PayloadStatus.NullResult: // 'n'
                                               // nothing to do
                    break;
                case PayloadStatus.SingleRecord: // 'r'
                    T document = ParseDocument(reader);
                    break;
                //case PayloadStatus.SerializedResult: // 'a'
                //    contentLength = reader.ReadInt32EndianAware();
                //    byte[] serializedBytes = reader.ReadBytes(contentLength);
                //    string serialized = System.Text.Encoding.UTF8.GetString(serializedBytes, 0, serializedBytes.Length);
                //    responseDocument.SetField("Content", serialized);
                //    break;
                case PayloadStatus.RecordCollection: // 'l'                   

                    int recordsCount = reader.ReadInt32EndianAware();

                    for (int i = 0; i < recordsCount; i++)
                    {
                        documents.Add(ParseDocument(reader));
                    }
                    break;
                //case PayloadStatus.SimpleResult: //'w'
                //    ODocument sDocument = ParseDocument(reader);
                //    var isNestedCollection = sDocument["result"] is List<List<object>>;
                //    responseDocument.SetField("Content",
                //        isNestedCollection ? sDocument["result"] : sDocument["result"].ToString());
                //    break;
                default:
                    break;
            }
            return new CommandResult<T>(Enumerable.Empty<T>());           
        }

        private T ParseDocument(BinaryReader reader) 
        {
            T document;

            short classId = reader.ReadInt16EndianAware();

            if (classId == -2) // NULL
            {
                document = null;
            }
            else if (classId == -3) // record id
            {
                ORID orid = new ORID();
                orid.ClusterId = reader.ReadInt16EndianAware();
                orid.ClusterPosition = reader.ReadInt64EndianAware();

                document = Activator.CreateInstance<T>();
                document.ORID = orid;
                document.OClassId = classId;
            }
            else
            {
                ORecordType type = (ORecordType)reader.ReadByte();

                ORID orid = new ORID();
                orid.ClusterId = reader.ReadInt16EndianAware();
                orid.ClusterPosition = reader.ReadInt64EndianAware();
                int version = reader.ReadInt32EndianAware();
                int recordLength = reader.ReadInt32EndianAware();
                byte[] rawRecord = reader.ReadBytes(recordLength);                

                document = _serializer.Deserialize<T>(rawRecord);
                document.ORID = orid;
                document.OVersion = version;
                document.OClassId = classId;
            }

            return document;
        }
    }
}
