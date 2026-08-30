using System.Net.Http.Json;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Throw;
using WebUIOver.Client.Context.CustomizeCard;
using WebUIOver.Client.Services;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;
using WebUIOver.Shared.Resources;

namespace WebUIOver.Client.Command.CustomizeCard.Save;

public class QuickStartProfileSaver(HttpClient httpClient, IResponseSnackService responseSnackService, 
    IStringLocalizer<Resource> localizer) 
    : ICustomizeCardContentSaver
{
    public async Task Save(CustomizeCardContext customizeCardContext, ProgressContext progressContext, ISnackbar snackbar, Action stateHasChanged)
    {
        progressContext.HideQuickStartProfileProgress = "visible";
        stateHasChanged.Invoke();
        
        var dto = new UpdateQuickStartProfileRequest()
        {
            AccessCode = customizeCardContext.AccessCode,
            ChipId = customizeCardContext.ChipId,
            QuickStartProfile = customizeCardContext.QuickStartProfile
        };
        
        var response = await httpClient.PostAsJsonAsync("/ui/QuickStart/save", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();
        
        responseSnackService.ShowBasicResponseSnack(snackbar, result, localizer["save_hint_quick_start_profile"]);

        progressContext.HideQuickStartProfileProgress = "invisible";
        stateHasChanged.Invoke();
    }
}