using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Vintagestory.API.Server;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace VintageStoryMods;

public sealed class AutoDoor : ModSystem
{
    private Regex _whitelistOpenRegex = new Regex("");
    private Regex _blacklistOpenRegex = new Regex("");
    private Regex _whitelistCloseRegex = new Regex("");
    private Regex _blacklistCloseRegex = new Regex("");
    
    private readonly Dictionary<object, BlockPos> _trackingDoors = new();
    private readonly Dictionary<object, bool> _doorStates = new();
    private Config _config = new Config();

    public override bool ShouldLoad(EnumAppSide forSide)
    {
        return forSide == EnumAppSide.Server;
    }

    public override void Start(ICoreAPI api)
    {
        Config? config = null;
        try
        {
            config = api.LoadModConfig<Config>("AutoDoor.json");
        }
        catch (Exception e)
        {
            Mod.Logger.Error("Could not load config! Loading default settings instead.");
            Mod.Logger.Error(e);
        }
        
        _config = config ?? new Config();
        api.StoreModConfig(_config, "AutoDoor.json");
        
        _whitelistOpenRegex = new Regex(config.WhitelistOpen, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
        _blacklistOpenRegex = new Regex(config.BlacklistOpen, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
        _whitelistCloseRegex = new Regex(config.WhitelistClose, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
        _blacklistCloseRegex = new Regex(config.BlacklistClose, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        api.World.RegisterGameTickListener(_ =>
        {
            CloseDoors(api);
            foreach (IPlayer player in api.World.AllOnlinePlayers.Where(p => p.Entity.Controls.TriesToMove))
            {
                OpenDoors(api, player);
            }
            UpdateDoors(api);
        }, 50);
    }

    private void UpdateDoors(ICoreAPI api)
    {
        _trackingDoors.RemoveAll((key, p) =>
        {
            Block b = api.World.BlockAccessor.GetBlock(p);
            string name = b.GetPlacedBlockName(api.World, p);
            // Door was just added, we might need to open it.
            if (!_doorStates.ContainsKey(key))
            {
                _doorStates[key] = true;
                SetStateIf(api, b, p, true, _config.AutoOpen && MatchesRegexes(name, _whitelistOpenRegex, _blacklistOpenRegex));
            }
            // Door was just removed, we might need to close it.
            else if (!_doorStates[key])
            {
                _doorStates.Remove(key);
                SetStateIf(api, b, p, false, _config.AutoClose && MatchesRegexes(name, _whitelistCloseRegex, _blacklistCloseRegex));
                return true;
            }

            return false;
        });
    }

    private void SetStateIf(ICoreAPI api, Block b, BlockPos p, bool state, bool condition)
    {
        if (!condition)
            return;
        
        Caller caller = new Caller()
        {
            Type = EnumCallerType.Console
        };
        BlockSelection selection = new BlockSelection(p, BlockFacing.DOWN, b);
        TreeAttribute activation = new();
        activation.SetBool("opened", state);
        b.Activate(api.World, caller, selection, activation);
    }

    private void CloseDoors(ICoreAPI api)
    {
        foreach ((object key, BlockPos p) in _trackingDoors)
        {
            float closeRadius = _config.Radius + 1;
            Entity[] entities = api.World.GetIntersectingEntities(p, [new Cuboidf(-closeRadius, -closeRadius, -closeRadius, closeRadius, closeRadius + 1, closeRadius)],
                e => e.Class == nameof(EntityPlayer) && e.Pos.AsBlockPos.ManhattenDistance(p) <= _config.Radius + 1);
            if (entities.Any())
            {
                continue;
            }

            _doorStates[key] = false;
        }
    }

    private void OpenDoors(ICoreAPI api, IPlayer player)
    {
        BlockPos pos = player.Entity.Pos.AsBlockPos;
        BlockPos startPos = pos.AddCopy(-_config.Radius, -_config.Radius, -_config.Radius);
        BlockPos endPos = pos.AddCopy(_config.Radius, _config.Radius + 1, _config.Radius);
        api.World.BlockAccessor.SearchBlocks(startPos, endPos, (b, p) =>
        {
            if (player.Entity.Pos.AsBlockPos.ManhattenDistance(p) > _config.Radius + 1)
            {
                return true;
            }
            
            // Try open doors within range.
            string blockName = b.GetPlacedBlockName(api.World, p);
            if (!MatchesRegexes(blockName, _whitelistOpenRegex, _blacklistOpenRegex) ||
                !MatchesRegexes(blockName, _whitelistCloseRegex, _blacklistCloseRegex) ||
                b.Code.ToString().StartsWith("game:multiblock-monolithic"))
                return true;
            object key = GetUniqueKey(b, p);
            if (_trackingDoors.ContainsKey(key))
                return true;

            if (_config.LeaveOpen && GetStateOrDefault(b, p, true, false))
                return true;
            
            _trackingDoors[key] = p.Copy();
            return true;
        });
    }

    private static bool GetStateOrDefault(Block b, BlockPos p, bool isOpen, bool defaultValue)
    {
        if (b.GetBEBehavior<BEBehaviorDoor>(p) is { } door)
            return door.Opened == isOpen;

        if (b.GetBEBehavior<BEBehaviorTrapDoor>(p) is { } trapDoor && trapDoor.Opened != isOpen)
            return trapDoor.Opened == isOpen;

        if (b.Variant.TryGetValue("state", out string state))
        {
            if (state == "opened")
                return isOpen;
            if (state == "closed")
                return !isOpen;
        }

        return defaultValue;
    }

    private static object GetUniqueKey(Block b, BlockPos p)
    {
        if (b.GetBEBehavior<BEBehaviorDoor>(p) is { } door)
            return door;
        if (b.GetBEBehavior<BEBehaviorTrapDoor>(p) is { } trapdoor)
            return trapdoor;
        return p.Copy();
    }

    private static bool MatchesRegexes(string text, Regex whitelist, Regex blacklist)
    {
        return whitelist.IsMatch(text) && !blacklist.IsMatch(text);
    }
}