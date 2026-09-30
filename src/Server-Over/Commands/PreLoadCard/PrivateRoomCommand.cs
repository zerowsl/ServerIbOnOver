using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Models.Config;
using ServerOver.Persistence;
using ServerOver.Utils;

namespace ServerOver.Commands.PreLoadCard;

public class PrivateRoomCommand(ServerDbContext context, CardServerConfig config) 
    : IPreLoadCardCommand
{
    public void Fill(CardProfile cardProfile, Response.PreLoadCard preLoadCard)
    {
        if (config.CustomConfigs?.Room?.Enable != true)
        {
            FillByDb(cardProfile, preLoadCard);
            return;
        }

        preLoadCard.MatchingTag = new MatchingTag()
        {
            TagName = config.CustomConfigs.Room.Code,
            TagType = config.CustomConfigs.Room.TagType,
            MatchingType = config.CustomConfigs.Room.MatchingType,
            MatchingAttribute = config.CustomConfigs.Room.MatchingAttribute,
            EndDate = (ulong)DateTimeOffset.Parse("2099-12-31").ToUnixTimeSeconds(),
            RuleType = config.CustomConfigs.Room.RuleType,
            SelectableMsIds = config.CustomConfigs.Room.SelectableMsIds,
            WorldId = config.CustomConfigs.Room.WorldId, //
            RevengeFlag = config.CustomConfigs.Room.RevengeFlag,
            Timer = config.CustomConfigs.Room.Timer,
            FesRuleType = config.CustomConfigs.Room.FesRuleType
        };
        var str = $"{preLoadCard.MatchingTag.TagName}:t{preLoadCard.MatchingTag.TagType}:m{preLoadCard.MatchingTag.MatchingType}";
        preLoadCard.MatchingTag.Id = preLoadCard.MatchingTag.WorldId = MyUtils.ToUintByCrc32(str);
        if (preLoadCard.MatchingTag.Id <= 100) 
        {
            // 为了加快匹配速度与准确,避免跟游戏自带的店外店内一样的值,
            // 房间id和worldId小于100,直接刷卡失败...
            throw new Exception("The roomid of gen is less than 100, please change the room code !!");
        }
    }

    private void FillByDb(CardProfile cardProfile, Response.PreLoadCard preLoadCard)
    {
        var privateMatchRoomSetting = context.PrivateMatchRoomSettingDbSet
            .First(x => x.CardProfile == cardProfile);

        if (!privateMatchRoomSetting.EnablePrivateMatch)
        {
            return;
        }
        
        var participatedPrivateRoomId = privateMatchRoomSetting.ParticipatedPrivateRoomId;
        if (participatedPrivateRoomId == 0)
        {
            return;
        }

        var privateRoom = context.PrivateMatchRoomDbSet
            .FirstOrDefault(x => x.Id == participatedPrivateRoomId);

        if (privateRoom is null)
        {
            return;
        }
        
        preLoadCard.MatchingTag = new MatchingTag()
        {
            Id = (uint) participatedPrivateRoomId,
            TagName = privateRoom.TagName, // code
            TagType = privateRoom.TagType, // 1=公开 2=私房
            MatchingType = privateRoom.MatchingType, // 1 = SOLO, 2 = TEAM
            MatchingAttribute = privateRoom.MatchingAttribute, //??
            //TagMatchingNum = 1,
            EndDate = (ulong)DateTimeOffset.Parse("2099-12-31").ToUnixTimeSeconds(),
            RuleType = privateRoom.RuleType, // 0 = Without MS Restriction, 1 = With MS Restriction
            SelectableMsIds = ArrayUtil.FromString(privateRoom.SelectableMsIds),
            WorldId = privateRoom.MatchingType == 1 ? 1013u : 1014u,
            RevengeFlag = privateRoom.RevengeFlag
        };
    }
}