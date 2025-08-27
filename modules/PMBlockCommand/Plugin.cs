using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Capabilities;
using CounterStrikeSharp.API.Core.Translations;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using PrimeManager.API;

namespace PMBlockCommand;

public class PluginConfig : BasePluginConfig
{
    public List<string> BlockCommands { get; set; } = [];
}

public class Plugin : BasePlugin, IPluginConfig<PluginConfig>
{
    public override string ModuleName => "[PM] BlockCommand";
    public override string ModuleVersion => "1.0.1";
    public override string ModuleAuthor => "xstage";

    public static PluginCapability<IPrimeManager> PmApi { get; } = new("PrimeManager");
    public PluginConfig Config { get; set; } = new();

    private IPrimeManager _api = null!;
    private string? _immunityFlag;

    public override void Load(bool hotReload)
    {
        AddCommandListener(null, Command_Handler);
    }

    public override void OnAllPluginsLoaded(bool hotReload)
    {
        ArgumentNullException.ThrowIfNull(_api = PmApi.Get()!, nameof(_api));

        _immunityFlag = _api.GetModuleSetting<string>("immunity_flag");
    }

    private HookResult Command_Handler(CCSPlayerController? player, CommandInfo commandInfo)
    {
        if (player == null || Config.BlockCommands.Count == 0) return HookResult.Continue;

        string baseCommand = commandInfo.GetArg(0);

        if (_api.HasPrime(player)) return HookResult.Continue;
        if (_immunityFlag != null && AdminManager.PlayerHasPermissions(player, _immunityFlag)) return HookResult.Continue;

        foreach (var blockCmd in Config.BlockCommands)
        {
            if (baseCommand.Equals(blockCmd, StringComparison.CurrentCultureIgnoreCase) || baseCommand.StartsWith(blockCmd, StringComparison.CurrentCultureIgnoreCase))
            {
                _api.AlertToChat(player, Localizer.ForPlayer(player, "Chat.Alert"));

                if (commandInfo.CallingContext == CommandCallingContext.Console)
                {
                    commandInfo.ReplyToCommand(Localizer.ForPlayer(player, "Chat.Alert"));
                }

                return HookResult.Stop;
            }
        }

        return HookResult.Continue;
    }

    public void OnConfigParsed(PluginConfig config)
    {
        Config = config;
    }
}
