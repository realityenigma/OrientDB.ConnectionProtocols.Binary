﻿using OrientDB.ConnectionProtocols.Binary.Operations;
using OrientDB.Core;
using System;
using System.Collections.Generic;

namespace OrientDB.ConnectionProtocols.Binary.Core
{
    internal class Request
    {
        internal OperationMode OperationMode { get; }

        internal List<RequestDataItem> DataItems { get; set; }

        internal int SessionId { get; private set; }

        internal Request(OperationMode mode)
        {
            this.OperationMode = mode;
            DataItems = new List<RequestDataItem>();
            SessionId = -1;  
        }

        internal void AddDataItem(byte b)
        {
            DataItems.Add(new RequestDataItem() { Type = "byte", Data = BinarySerializer.ToArray(b) });
        }
        internal void AddDataItem(byte[] b)
        {
            DataItems.Add(new RequestDataItem() { Type = "bytes", Data = b });
        }

        internal void AddDataItem(short s)
        {
            DataItems.Add(new RequestDataItem() { Type = "short", Data = BinarySerializer.ToArray(s) });
        }
        internal void AddDataItem(int i)
        {
            DataItems.Add(new RequestDataItem() { Type = "int", Data = BinarySerializer.ToArray(i) });
        }

        internal void AddDataItem(long l)
        {
            DataItems.Add(new RequestDataItem() { Type = "long", Data = BinarySerializer.ToArray(l) });
        }
        internal void AddDataItem(string s)
        {
            DataItems.Add(new RequestDataItem() { Type = "string", Data = BinarySerializer.ToArray(s) });
        }
        internal void AddDataItem(ORID _orid)
        {
            DataItems.Add(new RequestDataItem() { Type = "short", Data = BinarySerializer.ToArray(_orid.ClusterId) });
            DataItems.Add(new RequestDataItem() { Type = "long", Data = BinarySerializer.ToArray(_orid.ClusterPosition) });
        }

        internal void SetSessionId(int SessionId)
        {
            this.SessionId = SessionId;
        }
    }
}
