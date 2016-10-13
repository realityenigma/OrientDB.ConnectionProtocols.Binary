namespace OrientDB.ConnectionProtocols.Binary.Contracts
{
    internal interface IOrientDBConnection
    {        
        IOrientDBCommand CreateCommand();
    }
}