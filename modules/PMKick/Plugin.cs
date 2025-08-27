using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Capabilities;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.ValveConstants.Protobuf;
using PrimeManager.API;

namespace PMKick;

public class Plugin : BasePlugin
{
    public override string ModuleName => "[PM] Kick";
    public override string ModuleVersion => "1.0.1";
    public override string ModuleAuthor => "xstage";

    public static PluginCapability<IPrimeManager> PmApi { get; } = new("PrimeManager");

    private string? _immunityFlag;

    public override void OnAllPluginsLoaded(bool hotReload)
    {
        IPrimeManager? api = PmApi.Get();

        if (api != null)
        {
            api.PersonaDataRecivedEvent += OnPersonaDataRecived;
            _immunityFlag = api.GetModuleSetting<string>("immunity_flag");
        }
    }

    private void OnPersonaDataRecived(CCSPlayerController player, bool hasPrime)
    {
        if (!player.IsValid || hasPrime) return;
        if (_immunityFlag != null && AdminManager.PlayerHasPermissions(player, _immunityFlag)) return;

        player.Disconnect(NetworkDisconnectionReason.NETWORK_DISCONNECT_CLIENT_CONSISTENCY_FAIL);
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
