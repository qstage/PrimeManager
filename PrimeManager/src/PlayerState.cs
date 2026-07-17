using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;

namespace PrimeManager;

internal class PlayerState
{
    public bool HasPrime => _primeStatus;
    public int PlayerLevel => _playerLevel;

    private readonly CCSPlayerController _controller;
    private Timer? _receivingDataTimer;
    private bool _primeStatus;
    private int _playerLevel;

    public PlayerState(CCSPlayerController player)
    {
        _controller = player;
    }

    internal void OnClientPutInServer(Action<CCSPlayerController, CEconPersonaDataPublic> PersonaDataRecived)
    {
        var personaDataPublic = Plugin.GetPersonaDataPublic(_controller);

        if (personaDataPublic.HasValue)
        {
            CEconPersonaDataPublic econPersonaData = personaDataPublic.Value;

            _primeStatus = econPersonaData.ElevatedState;
            _playerLevel = econPersonaData.PlayerLevel;
            
            PersonaDataRecived(_controller, econPersonaData);

            return;
        }

        _receivingDataTimer = new Timer(1f, () =>
        {
            if (!_controller.IsValid || _controller.Connected != PlayerConnectedState.Connected)
            {
                _receivingDataTimer?.Kill();
                _receivingDataTimer = null;

                return;
            }

            OnClientPutInServer(PersonaDataRecived);
        });
    }
}