Auto opens and closes doors, gates and trapdoors within a 2 block radius of the player.

Get the mod [here](https://mods.vintagestory.at/edit/mod/?assetid=42012).

# Configuration

To configure the mod locate the VintagestoryData folder, this depends on your operating system. But you can find it by going to mods in the game and clicking "Open Mods Folder". After that go to the parent directory, and enter the Config folder. Here you will see a file called AutoDoor.json, this is the config file that you can edit in a program of your choice.

## Config description

WhitelistOpen: Regex for which blocks should open.
BlacklistOpen: Regex for which blocks should not open (takes priority over WhitelistOpen)

WhitelistClose: Regex for which blocks should close.
BlacklistClose: Regex for which blocks should not close (takes priority over WhitelistClose)

Radius: The radius at which the mod closes/opens doors to the player (manhattan distance)

AutoOpen: If true the mod will automatically open doors matching the whitelist and blacklist options.
AutoOpen: If true the mod will automatically close doors matching the whitelist and blacklist options.
LeaveOpen: If true the mod will not close doors that were open when you walked into their opening radius.

I have noticed people have two playstyles, either with auto open/close or only with auto close.

### Auto open/close (Default)
```json
{
  "WhitelistOpen": "([Dd]oor)|([Gg]ate)",
  "BlacklistOpen": "([Rr]ust)",
  "WhitelistClose": "([Dd]oor)|([Gg]ate)",
  "BlacklistClose": "([Rr]ust)",
  "Radius": 2,
  "AutoOpen": true,
  "AutoClose": true,
  "LeaveOpen": true
}
```

### Auto close only
```json
{
  "WhitelistOpen": "([Dd]oor)|([Gg]ate)",
  "BlacklistOpen": "([Rr]ust)",
  "WhitelistClose": "([Dd]oor)|([Gg]ate)",
  "BlacklistClose": "([Rr]ust)",
  "Radius": 1,
  "AutoOpen": false,
  "AutoClose": true,
  "LeaveOpen": false
}
```