namespace ServerOver.Models.Config;

public sealed class RemoteCardServerConfigs
{
	public bool Enable { get; set; }

    public string Address { get; set; } = default!;

    //public string? Token { get; set; }
}