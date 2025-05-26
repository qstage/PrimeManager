using CounterStrikeSharp.API.Core;

namespace PrimeManager.API;

public delegate void PersonaDataRecived(CCSPlayerController player, bool hasPrime);

public interface IPrimeManager
{
    event PersonaDataRecived? PersonaDataRecivedEvent;
    
    bool HasPrime(CCSPlayerController player);
    void AlertToChat(CCSPlayerController player, string message, params object[] args);
}
