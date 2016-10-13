using System.Collections.Generic;

namespace OrientDB.ConnectionProtocols.Binary.Operations
{
    internal class CommandResult<T>
    {
        public IEnumerable<T> Results { get; }

        public CommandResult(IEnumerable<T> results)
        {
            Results = results;
        }
    }
}