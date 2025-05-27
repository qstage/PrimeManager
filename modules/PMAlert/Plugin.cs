using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Capabilities;
using CounterStrikeSharp.API.Core.Translations;
using PrimeManager.API;

namespace PMAlert;

public class Plugin : BasePlugin
{
    public override string ModuleName => "[PM] Alert";
    public override string ModuleVersion => "1.0.1";
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
        foreach (var target in Utilities.GetPlayers())
        {
            if (target.IsBot) continue;

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