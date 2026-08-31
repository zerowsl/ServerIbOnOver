using MediatR;
using nue.protocol.exvs;

namespace ServerOver.Handlers.Game;

public record LoadRoomQuery(Request Request) : IRequest<Response>;

public class LoadRoomQueryHandler : IRequestHandler<LoadRoomQuery, Response>
{
    public Task<Response> Handle(LoadRoomQuery request, CancellationToken cancellationToken)
    {
        var loadRoom = new Response.LoadRoom();
        
        if (loadRoom != null)
        {
            loadRoom.PilotId = 1;
            loadRoom.IsNewCard = false;
            loadRoom.Status = 1;
            loadRoom.AcidError = AcidError.AcidSuccess;
            loadRoom.Rooms.Add(new()
            {
                TagId = 1, 
                RegistrationCode = "ggfj1", 
                TagType = 1, 
                MatchingType = 0, 
                StartDate = (ulong)DateTimeOffset.Parse($"{DateTime.Now:yyyy-MM-dd 00:00:00}").ToUnixTimeSeconds(), 
                EndDate = (ulong)DateTimeOffset.Parse($"{DateTime.Now:yyyy-MM-dd 12:00:00}").ToUnixTimeSeconds(), 
                CurrentNum = 1, 
                MemberLimit = 100, 
                WinRateMin = 0, 
                WinRateMax = 100, 
                ClassIdMin = 1, 
                ClassIdMax = 4, 
                GradeIdMin = 1, 
                GradeIdMax = 11, 
                RuleType = 0, 
                FesRuleType = 0, 
                Policy = 1, 
            });
            
            loadRoom.Rooms.Add(new()
            {
                TagId = 2, 
                RegistrationCode = "ff486", 
                TagType = 2, 
                MatchingType = 1, 
                StartDate = (ulong)DateTimeOffset.Parse($"{DateTime.Now:yyyy-MM-dd 00:00:00}").ToUnixTimeSeconds(), 
                EndDate = (ulong)DateTimeOffset.Parse($"{DateTime.Now:yyyy-MM-dd 23:59:00}").ToUnixTimeSeconds(), 
                CurrentNum = 0, 
                MemberLimit = 128, 
                WinRateMin = 1, 
                WinRateMax = 60, 
                ClassIdMin = 3, 
                ClassIdMax = 4, 
                GradeIdMin = 1, 
                GradeIdMax = 11, 
                RuleType = 0, 
                FesRuleType = 0, 
                Policy = 5, 
            });
            loadRoom.Rooms.Add(new()
            {
                TagId = 3, 
                RegistrationCode = "ff486", 
                TagType = 2, 
                MatchingType = 2, 
                StartDate = (ulong)DateTimeOffset.Parse($"{DateTime.Now:yyyy-MM-dd 00:00:00}").ToUnixTimeSeconds(), 
                EndDate = (ulong)DateTimeOffset.Parse($"{DateTime.Now:yyyy-MM-dd 23:59:00}").ToUnixTimeSeconds(), 
                CurrentNum = 0, 
                MemberLimit = 486001, 
                WinRateMin = 0, 
                WinRateMax = 100, 
                ClassIdMin = 4, 
                ClassIdMax = 4, 
                GradeIdMin = 1, 
                GradeIdMax = 11, 
                RuleType = 1, 
                FesRuleType = 11, 
                Policy = 2, 
            });
            loadRoom.Rooms.Add(new()
            {
                TagId = 4, 
                RegistrationCode = "ff486", 
                TagType = 2, 
                MatchingType = 2, 
                StartDate = (ulong)DateTimeOffset.Parse($"{DateTime.Now:yyyy-MM-dd 00:00:00}").ToUnixTimeSeconds(), 
                EndDate = (ulong)DateTimeOffset.Parse($"{DateTime.Now:yyyy-MM-dd 23:59:00}").ToUnixTimeSeconds(), 
                CurrentNum = 0, 
                MemberLimit = 16, 
                WinRateMin = 0, 
                WinRateMax = 100, 
                ClassIdMin = 1, 
                ClassIdMax = 4, 
                GradeIdMin = 1, 
                GradeIdMax = 11, 
                RuleType = 0, 
                FesRuleType = 1, 
                Policy = 3, 
            });
            loadRoom.Rooms.Add(new()
            {
                TagId = 5, 
                RegistrationCode = "ff486", 
                TagType = 2, 
                MatchingType = 1, 
                StartDate = (ulong)DateTimeOffset.Parse($"{DateTime.Now:yyyy-MM-dd 00:00:00}").ToUnixTimeSeconds(), 
                EndDate = (ulong)DateTimeOffset.Parse($"{DateTime.Now:yyyy-MM-dd 23:59:00}").ToUnixTimeSeconds(), 
                CurrentNum = 0, 
                MemberLimit = 4, 
                WinRateMin = 0, 
                WinRateMax = 100, 
                ClassIdMin = 2, 
                ClassIdMax = 4, 
                GradeIdMin = 1, 
                GradeIdMax = 11, 
                RuleType = 0, 
                FesRuleType = 2, 
                Policy = 4, 
            });
            loadRoom.Rooms.Add(new()
            {
                TagId = 6, 
                RegistrationCode = "ff486", 
                TagType = 2, 
                MatchingType = 1, 
                StartDate = (ulong)DateTimeOffset.Parse($"{DateTime.Now:yyyy-MM-dd 00:00:00}").ToUnixTimeSeconds(), 
                EndDate = (ulong)DateTimeOffset.Parse($"{DateTime.Now:yyyy-MM-dd 23:59:00}").ToUnixTimeSeconds(), 
                CurrentNum = 0, 
                MemberLimit = 8, 
                WinRateMin = 0, 
                WinRateMax = 100, 
                ClassIdMin = 3, 
                ClassIdMax = 4, 
                GradeIdMin = 10, 
                GradeIdMax = 11, 
                RuleType = 0, 
                FesRuleType = 2, 
                Policy = 7, 
            });
        }
        
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            load_room = loadRoom
        });
    }
}