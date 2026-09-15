using StunService.Models;
using System.Buffers.Binary;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace StunService.Services;

public class StunProtocol
{
    public const int StunHeaderSize = 20;
    public const int MagicCookie = 0x2112A442;
    public const int AttributeHeaderSize = 4;
    public const int XorMappedAddressSize = 8; // IPv4
    public const int MappedAddressSize = 8; // IPv4
    
    // STUN 消息编码
    public byte[] Encode(StunMessage message)
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);

        // 消息类型 (2 bytes)
        writer.Write(BinaryPrimitives.ReverseEndianness((ushort)message.Type));
        
        // 消息长度 (2 bytes) - 稍后填充
        writer.Write((ushort)0);
        
        // Magic Cookie (4 bytes)
        writer.Write(BinaryPrimitives.ReverseEndianness(MagicCookie));
        
        // Transaction ID (12 bytes)
        writer.Write(message.TransactionId);

        // 编码属性
        foreach (var attr in message.Attributes)
        {
            WriteAttribute(writer, attr);
        }

        // 更新消息长度
        var length = (ushort)(ms.Length - StunHeaderSize);
        ms.Seek(2, SeekOrigin.Begin);
        writer.Write(BinaryPrimitives.ReverseEndianness(length));

        return ms.ToArray();
    }

    // STUN 消息解码
    public StunMessage Decode(byte[] data)
    {
        if (data.Length < StunHeaderSize)
            throw new ArgumentException("Invalid STUN message: too short");

        using var ms = new MemoryStream(data);
        using var reader = new BinaryReader(ms);

        var message = new StunMessage
        {
            Type = (StunMessageType)BinaryPrimitives.ReverseEndianness(reader.ReadUInt16())
        };

        var length = BinaryPrimitives.ReverseEndianness(reader.ReadUInt16());
        
        // 验证 Magic Cookie
        var magicCookie = BinaryPrimitives.ReverseEndianness(reader.ReadUInt32());
        if (magicCookie != MagicCookie)
            throw new ArgumentException("Invalid STUN message: wrong magic cookie");

        // 读取 Transaction ID
        message.TransactionId = reader.ReadBytes(12);

        // 读取属性
        var bytesRead = 0;
        while (bytesRead < length)
        {
            var attr = ReadAttribute(reader);
            message.Attributes.Add(attr);
            bytesRead += AttributeHeaderSize + attr.Value.Length;
            
            // 属性对齐到 4 字节边界
            var padding = (4 - (attr.Value.Length % 4)) % 4;
            if (padding > 0)
            {
                reader.ReadBytes(padding);
                bytesRead += padding;
            }
        }

        return message;
    }

    private void WriteAttribute(BinaryWriter writer, StunAttribute attr)
    {
        // 属性类型 (2 bytes)
        writer.Write(BinaryPrimitives.ReverseEndianness((ushort)attr.Type));
        
        // 属性长度 (2 bytes)
        writer.Write(BinaryPrimitives.ReverseEndianness((ushort)attr.Value.Length));
        
        // 属性值
        writer.Write(attr.Value);
        
        // 对齐到 4 字节边界
        var padding = (4 - (attr.Value.Length % 4)) % 4;
        if (padding > 0)
            writer.Write(new byte[padding]);
    }

    private StunAttribute ReadAttribute(BinaryReader reader)
    {
        var attr = new StunAttribute
        {
            Type = (StunAttributeType)BinaryPrimitives.ReverseEndianness(reader.ReadUInt16()),
            Value = reader.ReadBytes(BinaryPrimitives.ReverseEndianness(reader.ReadUInt16()))
        };
        return attr;
    }

    // 创建 XOR-MAPPED-ADDRESS 属性
    public StunAttribute CreateXorMappedAddress(IPEndPoint endpoint, byte[] transactionId)
    {
        var attr = new StunAttribute
        {
            Type = StunAttributeType.XorMappedAddress,
            Value = new byte[XorMappedAddressSize]
        };

        // Reserved (1 byte)
        attr.Value[0] = 0;
        
        // Family (1 byte) - 0x01 for IPv4
        attr.Value[1] = 0x01;

        // XOR Port (2 bytes)
        var port = (ushort)endpoint.Port;
        var xorPort = (ushort)(port ^ (MagicCookie >> 16));
        BinaryPrimitives.WriteUInt16BigEndian(attr.Value.AsSpan(2), xorPort);

        // XOR Address (4 bytes)
        var addressBytes = endpoint.Address.GetAddressBytes();
        var magicCookieBytes = BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(MagicCookie));
        
        for (int i = 0; i < 4; i++)
            attr.Value[4 + i] = (byte)(addressBytes[i] ^ magicCookieBytes[i]);

        return attr;
    }

    // 创建 MAPPED-ADDRESS 属性
    public StunAttribute CreateMappedAddress(IPEndPoint endpoint)
    {
        var attr = new StunAttribute
        {
            Type = StunAttributeType.MappedAddress,
            Value = new byte[MappedAddressSize]
        };

        // Reserved (1 byte)
        attr.Value[0] = 0;
        
        // Family (1 byte) - 0x01 for IPv4
        attr.Value[1] = 0x01;

        // Port (2 bytes)
        BinaryPrimitives.WriteUInt16BigEndian(attr.Value.AsSpan(2), (ushort)endpoint.Port);

        // Address (4 bytes)
        var addressBytes = endpoint.Address.GetAddressBytes();
        Array.Copy(addressBytes, 0, attr.Value, 4, 4);

        return attr;
    }

    // 创建 ERROR-CODE 属性
    public StunAttribute CreateErrorCode(int code, string reason)
    {
        var attr = new StunAttribute
        {
            Type = StunAttributeType.ErrorCode,
            Value = new byte[4 + Encoding.UTF8.GetByteCount(reason)]
        };

        // Reserved (2 bytes)
        attr.Value[0] = 0;
        attr.Value[1] = 0;
        
        // Error Class (3 bits) and Number (8 bits)
        var errorClass = (byte)(code / 100);
        var errorNumber = (byte)(code % 100);
        attr.Value[2] = (byte)(errorClass & 0x07);
        attr.Value[3] = errorNumber;

        // Reason Phrase
        var reasonBytes = Encoding.UTF8.GetBytes(reason);
        Array.Copy(reasonBytes, 0, attr.Value, 4, reasonBytes.Length);

        return attr;
    }

    // 创建 SOFTWARE 属性
    public StunAttribute CreateSoftwareAttribute(string software)
    {
        return new StunAttribute
        {
            Type = StunAttributeType.Software,
            Value = Encoding.UTF8.GetBytes(software)
        };
    }

    // 创建 FINGERPRINT 属性
    public StunAttribute CreateFingerprint(byte[] message)
    {
        var fingerprint = new StunAttribute
        {
            Type = StunAttributeType.Fingerprint,
            Value = new byte[4]
        };

        // CRC32 of the message up to (but excluding) the FINGERPRINT attribute
        var crc32 = ComputeCrc32(message.AsSpan(0, message.Length - 8));
        BinaryPrimitives.WriteUInt32BigEndian(fingerprint.Value.AsSpan(), crc32 ^ 0x5354554E);

        return fingerprint;
    }

    private uint ComputeCrc32(ReadOnlySpan<byte> data)
    {
        uint crc = 0xFFFFFFFF;
        for (int i = 0; i < data.Length; i++)
        {
            crc ^= data[i];
            for (int j = 0; j < 8; j++)
            {
                crc = (crc >> 1) ^ (0xEDB88320 & (uint)(-(int)(crc & 1)));
            }
        }
        return ~crc;
    }
}