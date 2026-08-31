using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ServerOver.Mapper.Card.Titles;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.MobileSuit;
using ServerOver.Models.Config;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;
using WebUIOver.Shared.Exception;
using WebUIOVer.Shared.Dto.Common;

namespace ServerOver.Handlers.UI.MobileSuit;

public record UpdateAllFavouriteMsCommand(UpdateAllFavouriteMsRequest Request) : IRequest<BasicResponse>;

public class UpdateAllFavouriteMsCommandHandler(ServerDbContext context, IOptions<CardServerConfig> options) 
    : IRequestHandler<UpdateAllFavouriteMsCommand, BasicResponse>
{
    private readonly CardServerConfig _config = options.Value;

    public Task<BasicResponse> Handle(UpdateAllFavouriteMsCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;

        if (updateRequest.FavouriteMsList.Count > _config.CustomConfigs.MaxFavouriteMs)
        {
            throw new InvalidRequestDataException($"Favourite MS List should be having maximum length of {_config.CustomConfigs.MaxFavouriteMs}");
        }
        
        var cardProfile = context.CardProfiles
                .First(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);
        
        if (cardProfile == null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }

        var favoriteMsGroups = updateRequest.FavouriteMsList
            .Select(ToFavouriteMsGroup(cardProfile))
            .ToList();

        context.FavouriteMobileSuitDbSet
            .Include(x => x.MobileSuitDefaultTitle)
            .Include(x => x.MobileSuitTriadTitle)
            .Include(x => x.MobileSuitClassMatchTitle)
            .Where(x => x.CardProfile == cardProfile)
            .ToList()
            .ForEach(previousData => context.Remove(previousData));
        
        favoriteMsGroups.ForEach(favoriteMsGroup =>
        {
            context.Add(favoriteMsGroup);
        });
        
        context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }

    Func<FavouriteMs, FavouriteMobileSuit> ToFavouriteMsGroup(CardProfile cardProfile)
    {
        return favouriteMs =>
        {
            AddMsUsage(cardProfile, favouriteMs);
            AddNaviData(cardProfile, favouriteMs);

            return new FavouriteMobileSuit()
            {
                CardProfile = cardProfile,
                MstMobileSuitId = favouriteMs.MsId,
                OpenSkillpoint = true,
                GaugeDesignId = favouriteMs.GaugeDesignId,
                BgmSettings = string.Join(",", favouriteMs.BgmList.ToArray()),
                BgmPlayMethod = (uint) favouriteMs.BgmPlayingMethod,
                BattleNavId = favouriteMs.BattleNaviId,
                BurstType = (uint) favouriteMs.BurstType,
                MobileSuitDefaultTitle = favouriteMs.DefaultTitle.ToMobileSuitDefaultTitle(),
                MobileSuitTriadTitle = favouriteMs.TriadTitle.ToMobileSuitTriadTitle(),
                MobileSuitClassMatchTitle = favouriteMs.ClassMatchTitle.ToMobileSuitClassMatchTitle(),
            };
        };
    }
    
    void AddMsUsage(CardProfile cardProfile, FavouriteMs favouriteMs)
    {
        var targetMsUsage = context.MobileSuitUsageDbSet
            .FirstOrDefault(msSkill => msSkill.MstMobileSuitId == favouriteMs.MsId && msSkill.CardProfile == cardProfile);

        if (favouriteMs.MsId is not 0 && targetMsUsage is null)
        {
            context.Add(new MobileSuitUsage
            {
                CardProfile = cardProfile,
                MstMobileSuitId = favouriteMs.MsId,
                CostumeId = 0,
                MsUsedNum = 0,
            });

            context.SaveChanges();
        }
    }
    
    void AddNaviData(CardProfile cardProfile, FavouriteMs favouriteMs)
    {
        var targetNavi = context.NaviDbSet
            .FirstOrDefault(navi => navi.GuestNavId == favouriteMs.BattleNaviId && navi.CardProfile == cardProfile);

        if (favouriteMs.BattleNaviId is not 0 && targetNavi is null)
        {
            context.Add(new Models.Cards.Navi.Navi()
            {
                CardProfile = cardProfile,
                GuestNavId = favouriteMs.BattleNaviId,
                GuestNavSettingFlag = false,
                BattleNavSettingFlag = false,
            });

            context.SaveChanges();
        }
    }
}