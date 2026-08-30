using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;

namespace ServerOver.Mapper.Usage;

[Mapper]
public static partial class Response2Mapper
{
    public static partial Response2 ToResponse2(this Response response);
}