using System.Net.Http.Json;
using Throw;
using WebUIOver.Client.Context.CustomizeCard;
using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Command.CustomizeCard.Fill;

public class QuickStartProfileFiller(HttpClient httpClient) : ICustomizeCardContextFiller
{
    public async Task Fill(CustomizeCardContext customizeCardContext)
    {
        var p = await httpClient.GetFromJsonAsync<QuickStartProfile>($"/ui/QuickStart/get/{customizeCardContext.AccessCode}/{customizeCardContext.ChipId}");
        p.ThrowIfNull();

        customizeCardContext.QuickStartProfile = p;
    }
}