# PrimeManager
Allows you to get information about the availability of Prime-status.

## Modules:
- [PM] Tag - Adds a ClanTag to the player.
- [PM] BlockCommand - Blocks commands specified in the config for players without a Prime-status.
- [PM] Alert - Notifies in the chat when the player is connected about the availability of the Prime-status.
- [PM] Kick - Kicks players without Prime-status.

## Configuration
```json
{
  "ModuleSettings": {
    "tag_only_nonprime": true, // true - tag only for non-prime players / otherwise - false
    "flag_alert": "@css/ban" // send alert for admins with `@css/ban` flag / remove this setting to send for everyone
  },
  "ConfigVersion": 1
}
```

## Requirments
- [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp/)
