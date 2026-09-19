namespace Common;

public readonly struct Releaser(Action? action = null, Func<Task>? func = null)
{
    public void Dispose()
    {
        action?.Invoke();
        if (func != null) func().GetAwaiter().GetResult();
    }

    public async ValueTask DisposeAsync()
    {
        action?.Invoke();
        if (func != null) await func();
    }
}