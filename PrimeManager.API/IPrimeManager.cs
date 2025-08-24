using CounterStrikeSharp.API.Core;

namespace PrimeManager.API;

public delegate void PersonaDataRecived(CCSPlayerController player, bool hasPrime);

public interface IPrimeManager
{
    /// <summary>
    /// Occurs when persona data is received.
    /// </summary>
    event PersonaDataRecived? PersonaDataRecivedEvent;

    /// <summary>
    /// Retrieves a module setting by key and attempts to cast it to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to cast the setting value to.</typeparam>
    /// <param name="key">The key of the setting to retrieve.</param>
    /// <returns>The setting value cast to type <typeparamref name="T"/>, or <c>null</c> if not found.</returns>
    T? GetModuleSetting<T>(string key);

    /// <summary>
    /// Determines whether the specified player has prime status.
    /// </summary>
    /// <param name="player">The player to check for prime status.</param>
    /// <returns><c>true</c> if the player has prime; otherwise, <c>false</c>.</returns>
    bool HasPrime(CCSPlayerController player);

    /// <summary>
    /// Sends an alert message to the specified player's chat.
    /// </summary>
    /// <param name="player">The player to send the chat alert to.</param>
    /// <param name="message">The message format string.</param>
    /// <param name="args">Optional arguments to format the message.</param>
    void AlertToChat(CCSPlayerController player, string message, params object[] args);
}
