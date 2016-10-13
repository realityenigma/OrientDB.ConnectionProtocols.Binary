﻿using OrientDB.ConnectionProtocols.Binary.Core;
using System.Collections.Generic;
using System.Linq;

namespace OrientDB.ConnectionProtocols.Binary.Operations
{
    internal class OpenDatabaseResult
    {
        public int SessionId { get; }
        public byte[] Token { get; }
        public int ClusterCount { get; }
        public ICollection<Cluster> Clusters { get; }
        public byte[] ClusterConfig { get; }
        public string OrientRelease { get; }

        public OpenDatabaseResult(int sessionId, byte[] token, int clusterCount, IEnumerable<Cluster> clusters, byte[] clusterConfig, string release)
        {
            SessionId = sessionId;
            Token = token;
            ClusterCount = clusterCount;
            Clusters = clusters.ToList();
            ClusterConfig = clusterConfig;
            OrientRelease = release;
        }
    }
}