using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Capabilities;
using CounterStrikeSharp.API.Core.Translations;
using CounterStrikeSharp.API.Modules.Admin;
using PrimeManager.API;

namespace PMAlert;

public class Plugin : BasePlugin
{
    public override string ModuleName => "[PM] Alert";
    public override string ModuleVersion => "1.0.2";
    public override string ModuleAuthor => "xstage";

    public static PluginCapability<IPrimeManager> PmApi { get; } = new("PrimeManager");
    private IPrimeManager _api = null!;

    public override void OnAllPluginsLoaded(bool hotReload)
    {
        ArgumentNullException.ThrowIfNull(_api = PmApi.Get()!, nameof(_api));
        _api.PersonaDataRecivedEvent += OnPersonaDataRecived;
    }

    private void OnPersonaDataRecived(CCSPlayerController player, bool hasPrime)
    {
        var flag = _api.GetModuleSetting<string>("flag_alert");

        foreach (var target in Utilities.GetPlayers())
        {
            if (target.IsBot) continue;
            if (flag != null && !AdminManager.PlayerHasPermissions(target, flag)) continue;

            _api.AlertToChat(target,
                Localizer.ForPlayer(target, "Alert.General", player.PlayerName,
                hasPrime ? Localizer.ForPlayer(target, "Alert.Prime") : Localizer.ForPlayer(target, "Alert.NonPrime"))
            );
        }
    }

    public override void Unload(bool hotReload)
    {
        _api.PersonaDataRecivedEvent -= OnPersonaDataRecived;
    }
}