using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Capabilities;
using PrimeManager.API;

namespace PMTag;

public class Plugin : BasePlugin
{
    public override string ModuleName => "[PM] Tag";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "xstage";

    public static PluginCapability<IPrimeManager> PmApi { get; } = new("PrimeManager");

    public override void OnAllPluginsLoaded(bool hotReload)
    {
        IPrimeManager? api = PmApi.Get();

        if (api != null)
        {
            api.PersonaDataRecivedEvent += OnPersonaDataRecived;
        }
    }

    private void OnPersonaDataRecived(CCSPlayerController player, bool hasPrime)
    {
        if (!player.IsValid) return;

        player.Clan = hasPrime ? Localizer["Tag.Prime"] : Localizer["Tag.NonPrime"];
        Utilities.SetStateChanged(player, "CCSPlayerController", "m_szClan");

        new EventBeginNewMatch(false).FireEventToClient(player);
    }

    public override void Unload(bool hotReload)
    {
        IPrimeManager? api = PmApi.Get();

        if (api != null)
        {
            api.PersonaDataRecivedEvent -= OnPersonaDataRecived;
        }
    }
}