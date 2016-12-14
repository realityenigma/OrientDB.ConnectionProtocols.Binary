﻿using System;
using System.Net.Sockets;

namespace OrientDB.ConnectionProtocols.Binary.Core
{
    class OrientDBNetworkConnection : IDisposable
    {
        private TcpClient _socket;
        private NetworkStream _stream;
        private int _sessionId;
        private byte[] _token;
        private int useCount = 4;

        public byte[] Token { get { return _token; } internal set { _token = value; } }
        public int SessionId { get { return _sessionId; } internal set { _sessionId = value; } }

        public OrientDBNetworkConnection(TcpClient client, NetworkStream stream)
        {
            _socket = client;
            _stream = stream;
        }

        public void Dispose()
        {
            if(_stream != null && _socket != null)
            {
                _stream.Dispose();
#if NETCOREAPP1_0
                _socket.Dispose();
#else
                _socket.Close();
#endif
                _stream = null;
                _socket = null;
            }
        }

        public NetworkStream GetStream()
        {
            return _stream;
        }

        public bool IsActive()
        {
            return _socket != null && _socket.Connected;
        }

        public void UpdateUse()
        {
            useCount--;
        }

        public bool IsUsedUp()
        {
            return useCount <= 0;
        }
    }
}
