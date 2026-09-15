using System.Net;

namespace StunService.Models;

public enum StunMessageType : ushort
{
    BindingRequest = 0x0001,
    BindingResponse = 0x0101,
    BindingErrorResponse = 0x0111,
    SharedSecretRequest = 0x0002,
    SharedSecretResponse = 0x0102,
    SharedSecretErrorResponse = 0x0112
}

public enum StunAttributeType : ushort
{
    MappedAddress = 0x0001,
    ResponseAddress = 0x0002,
    ChangeRequest = 0x0003,
    SourceAddress = 0x0004,
    ChangedAddress = 0x0005,
    Username = 0x0006,
    Password = 0x0007,
    MessageIntegrity = 0x0008,
    ErrorCode = 0x0009,
    UnknownAttributes = 0x000A,
    ReflectedFrom = 0x000B,
    XorMappedAddress = 0x0020,
    XorOnly = 0x0021,
    Software = 0x8022,
    AlternateServer = 0x8023,
    Fingerprint = 0x8028
}

public class StunMessage
{
    public StunMessageType Type { get; set; }
    public byte[] TransactionId { get; set; } = new byte[12];
    public List<StunAttribute> Attributes { get; set; } = new();
}

public class StunAttribute
{
    public StunAttributeType Type { get; set; }
    public byte[] Value { get; set; } = Array.Empty<byte>();
}

public class StunAddress
{
    public IPAddress Address { get; set; } = IPAddress.Any;
    public int Port { get; set; }
    public byte Family { get; set; } = 0x01; // IPv4
}