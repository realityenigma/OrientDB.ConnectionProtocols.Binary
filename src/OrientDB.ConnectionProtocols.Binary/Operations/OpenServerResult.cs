namespace OrientDB.ConnectionProtocols.Binary.Operations
{
    public class OpenServerResult
    {
        public int SessionId { get; set; }
        public byte[] Token { get; set; }
    }
}
