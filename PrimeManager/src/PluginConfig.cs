using CounterStrikeSharp.API.Core;

namespace PrimeManager;

public class PluginConfig : BasePluginConfig
{
    public Dictionary<string, object> ModuleSettings { get; set; } = new()
    {
        ["tag_only_nonprime"] = true,
        ["flag_alert"] = "@css/ban"
    };
}