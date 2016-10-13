﻿using System.Collections.Generic;

namespace OrientDB.ConnectionProtocols.Binary.Contracts
{
    public interface IOrientDBCommand
    {
        IEnumerable<T> Execute<T>(string query);
    }
}