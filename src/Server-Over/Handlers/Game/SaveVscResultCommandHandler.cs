using MediatR;
using Microsoft.Extensions.Options;
using nue.protocol.exvs;
using ServerOver.Commands.SaveBattle;
using ServerOver.Commands.SaveBattle.Common;
using ServerOver.Commands.SaveBattle.Triad;
using ServerOver.Commands.SaveBattle.Triad.Ranking;
using ServerOver.Context.Battle;
using ServerOver.Mapper.Context;
using ServerOver.Models.Config;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Enum;

namespace ServerOver.Handlers.Game;

public record SaveVscResultCommand(Request Request) : IRequest<Response>;

public class SaveVscResultCommandHandler(ILogger<SaveVscResultCommandHandler> _logger, 
    ServerDbContext _context, 
    IOptions<CardServerConfig> options)
    : IRequestHandler<SaveVscResultCommand, Response>
{
    private readonly CardServerConfig _config = options.Value;
    
    public Task<Response> Handle(SaveVscResultCommand request, CancellationToken cancellationToken)
    {
        var sessionId = request.Request.save_vsc_result.SessionId;
        var cardId = request.Request.save_vsc_result.PilotId;

        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.SessionId == sessionId && x.Id == cardId);

        if (cardProfile == null)
        {
            return Task.FromResult(new Response
            {
                Type = request.Request.Type,
                RequestId = request.Request.RequestId,
                Error = Error.Success,
                save_vsc_result = new Response.SaveVscResult()
            });
        }

        var currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
        var currentYear = (uint) currentTime.Year;
        var currentMonth = (uint) currentTime.Month;
        
        var playResultGroupIb = request.Request.save_vsc_result.Result;
        var playResultGroup = IbToOb(playResultGroupIb);
        var battleResultContext = playResultGroup.ToBattleResultContext();
        SetContext(battleResultContext, playResultGroupIb);
        battleResultContext.CommonDomain.BattleMode = BattleModeConstant.Triad;
        
        var saveBattleDataCommands = new List<ISaveBattleDataCommand>()
        {
            // Common Commands
            new SaveGpCommand(_context), // ob
            new SavePlayerLevelCommand(_context, _logger),
            new SaveNaviCommand(_context), // 导航员
            new SaveMobileSuitMasteryCommand(_context), // ms使用数+1
            //new SaveTeamCommand(_context),
            // Triad Mode Exclusive Commands
            new SaveTriadPartnerCommand(_context), // cpu ms使用数+1
            new SaveTriadMiscInfoCommand(_context),
            //new SaveTriadCourseScoreCommand(_context), // ob 1关3scene 最高分,清关,过关次数
            //new AddReleaseTriadCourseCommand(_context), // 点亮关卡?
            // Triad Ranking Commands
            //new SaveTriadRankTargetCommand(_context, currentYear, currentMonth),
            //new SaveTriadRankWantedCommand(_context, currentYear, currentMonth),
            //new SaveTriadRankHighScoreCommand(_context, _config, currentYear, currentMonth),
            //new SaveTriadRankClearTimeCommand(_context, _config, currentYear, currentMonth),
            //
            // ib
            new RemoveIbVsRouteBattleDataCommand(_context, playResultGroupIb.PhaseId),
            new SaveQuickStartInfoCommand(_context, playResultGroupIb.QuickStartInfo),
            new SavePlayerBadgeCommand(_context, _logger),
        };
        
        saveBattleDataCommands.ForEach(command => command?.Save(cardProfile, battleResultContext));
        
        _context.SaveChanges();
        
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vsc_result = new Response.SaveVscResult()
        });
    }
    
    static Request.SaveVscResult.PlayResultGroup IbToOb(Request.SaveVscResult.IbPlayResultGroup ib)
    {
        var ob = new Request.SaveVscResult.PlayResultGroup();
        ob.PlayedAt = ib.PlayedAt; // 1
        ob.WinFlag = ib.WinFlag; // 6
        ob.LevelId = ib.LevelId; // 13
        ob.PlayerLevelId = ib.PlayerLevelId; // 14
		
		// ob Triad max exp is 300, ib is seem to be 70 ...
        ob.LevelExp = ib.LevelExp; // 3*x
        ob.Gp = ib.ObtainedBadgePoint > 0 ? (uint)ib.ObtainedBadgePoint : 0;
        
        ob.GuestNavId = ib.GuestNavId;
        ob.GuestNavFamiliarity = ib.GuestNavFamiliarity;
        ob.BattleNavId = ob.GuestNavId;
        ob.BattleNavFamiliarity = ob.GuestNavFamiliarity;
        
        ob.MstMobileSuitId = ib.MstMobileSuitId; // 10
        ob.SkillPointMobileSuitId = ib.MstMobileSuitId;
        if (ib.QuickStartInfo.MstMobileSuitId != 0) ob.BurstType = ib.QuickStartInfo.BurstType;
        
        ob.Partner = new(); // 21
        ob.Partner.MobileSetFlag = true; // ib.Partner.MobileSetFlag // IsTriadPartner
        //ob.Partner.MobileSetFlag = ib.Partner.PilotId == 0;
        ob.Partner.CpuFlag = ib.Partner.CpuFlag;
        ob.Partner.MstMobileSuitId = ib.Partner.MstMobileSuitId;
        //ob.Partner.DownNum = 
        
        ob.SceneScore = ib.SceneScore; // 12
        //ob.CourseId = ib.StageId; // ib与ob的关卡不一样
		ob.CourseId = 0;
        ob.SceneId = ib.PhaseId;
        ob.SceneSeq = ib.PhaseId;
        ob.SceneType = ib.PhaseType;
        //ob.TotalWantedDefeatNum = 
        //ob.ReleasedRibbonIds = // CpuRibbons
        //ob.ReleasedCourseIds = 
        
        return ob;
    }

    static void SetContext(BattleResultContext battleResultContext, Request.SaveVscResult.IbPlayResultGroup ib)
    {
        battleResultContext.PlayerBadgeDomain ??= new();
        battleResultContext.PlayerBadgeDomain.ExpIncrement = ib.ObtainedBadgePoint;
        battleResultContext.PlayerBadgeDomain.IsWin = ib.WinFlag;
        battleResultContext.PlayerBadgeDomain.PlayerLevelId = ib.PlayerLevelId;
    }
}
