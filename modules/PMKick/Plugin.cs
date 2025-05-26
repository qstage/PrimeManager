using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Capabilities;
using CounterStrikeSharp.API.ValveConstants.Protobuf;
using PrimeManager.API;

namespace PMKick;

public class Plugin : BasePlugin
{
    public override string ModuleName => "[PM] Kick";
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
        if (!player.IsValid || hasPrime) return;

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
