using Microsoft.AspNetCore.Mvc;

namespace ServerOver.Controllers;

public abstract class BaseController<T> : ControllerBase where T : BaseController<T>
{
    private ILogger<T>? logger;

    protected ILogger<T> Logger => (logger ??= HttpContext.RequestServices.GetService<ILogger<T>>()) ?? throw new InvalidOperationException();

    protected string GetBaseAddressWithScheme()
    {
        var s = Request.Scheme?.ToLower();
        s = s?.StartsWith("http") == true ? (s.TrimEnd('s') + (Request.IsHttps ? "s" : "")) : s;
        return (s + "://" + Request.Host.ToString()).TrimEnd('/');
    }
}