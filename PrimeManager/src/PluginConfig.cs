using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;

namespace PrimeManager;

public class PluginConfig : BasePluginConfig
{
    public Dictionary<string, object> ModuleSettings { get; set; } = new()
    {
        ["tag_only_nonprime"] = true,
        ["immunity_flag"] = "@css/generic",
        ["flag_alert"] = "@css/ban"
    };

    [JsonPropertyName("ConfigVersion")]
    public override int Version { get; set; } = 2;
}