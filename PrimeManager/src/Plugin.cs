using System.Runtime.InteropServices;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Capabilities;
using CounterStrikeSharp.API.Core.Translations;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using PrimeManager.API;

namespace PrimeManager;

[StructLayout(LayoutKind.Explicit)]
public struct CEconPersonaDataPublic
{
    [FieldOffset(0x28)]
    public int PlayerLevel;

    [FieldOffset(0x2C)]
    [MarshalAs(UnmanagedType.I1)]
    public bool ElevatedState;
}

public class Plugin : BasePlugin, IPrimeManager
{
    public override string ModuleName => "PrimeManager";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "xstage";

    public event PersonaDataRecived? PersonaDataRecivedEvent;
    public static PluginCapability<IPrimeManager> Api { get; } = new("PrimeManager");

    private readonly PlayerState[] _players = new PlayerState[65];
    private static readonly int _nearestFieldOffset = Schema.GetSchemaOffset("CCSPlayerController_InventoryServices", "m_unEquippedPlayerSprayIDs");

    public override void Load(bool hotReload)
    {
        if (_nearestFieldOffset == 0)
        {
            throw new Exception("Not found offset `m_unEquippedPlayerSprayIDs`");
        }

        Capabilities.RegisterPluginCapability(Api, () => this);
        RegisterListener<Listeners.OnClientPutInServer>(OnClientPutInServer);
    }

    private void OnClientPutInServer(int slot)
    {
        var player = Utilities.GetPlayerFromSlot(slot);
        if (player == null || player.IsBot) return;

        var playerState = new PlayerState(player);
        playerState.OnClientPutInServer(OnPersonaDataRecived);

        _players[player.Index] = playerState;
    }

    private void OnPersonaDataRecived(CCSPlayerController player, CEconPersonaDataPublic data)
    {
        PersonaDataRecivedEvent?.Invoke(player, data.ElevatedState);
    }

    internal static CEconPersonaDataPublic? GetPersonaDataPublic(CCSPlayerController player)
    {
        if (!player.IsValid) return null;

        var inventoryServices = player.InventoryServices;
        if (inventoryServices == null) return null;

        nint pEconPersonaData = Marshal.ReadIntPtr(inventoryServices.Handle, _nearestFieldOffset - 8);
        if (pEconPersonaData == nint.Zero) return null;

        var econPersonaData = Marshal.PtrToStructure<CEconPersonaDataPublic>(pEconPersonaData);

        return econPersonaData;
    }

    public bool HasPrime(CCSPlayerController player)
    {
        return _players[player.Index].HasPrime;
    }

    public void AlertToChat(CCSPlayerController player, string message, params object[] args)
    {
        string fmtString = string.Format("{0} {1}", Localizer.ForPlayer(player, "Plugin.Tag"), string.Format(message, args));
        player.PrintToChat(fmtString);
    }
}
