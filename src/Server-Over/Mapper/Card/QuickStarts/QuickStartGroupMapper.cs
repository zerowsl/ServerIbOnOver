using Riok.Mapperly.Abstractions;
using ServerOver.Models.Cards.Profile;
using WebUIOver.Shared.Dto.Common;
using Response2 = nue.protocol.exvs.Response2;

namespace ServerOver.Mapper.QuickStarts;

[Mapper]
public static partial class QuickStartGroupMapper
{
    public static partial Response2.LoadCard.MobileUserGroup.QuickStartGroup ToResponse2QuickStartGroup(this QuickStartInfoProfile m);

    public static partial QuickStartProfile ToQuickStartProfile(this QuickStartInfoProfile m);
}