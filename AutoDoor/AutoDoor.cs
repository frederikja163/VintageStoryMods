using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Vintagestory.API.Server;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace VintageStoryMods;

public sealed class AutoDoor : ModSystem
{
    private readonly HashSet<BlockPos> _openedDoors = new();
    private Regex _regex = new Regex("");
    private int _radius = 0;

    public override bool ShouldLoad(EnumAppSide forSide)
    {
        return forSide == EnumAppSide.Server;
    }

    public override void Start(ICoreAPI api)
    {
        Config config = null;
        try
        {
            config = api.LoadModConfig<Config>("AutoDoor.json");
        }
        catch (Exception e)
        {
            Mod.Logger.Error("Could not load config! Loading default settings instead.");
            Mod.Logger.Error(e);
        }

        if (config is null)
        {
            config = new Config();
        }

        _regex = new Regex(config.Regex, RegexOptions.Compiled);
        _radius = config.Radius;
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        api.World.RegisterGameTickListener(_ =>
        {
            foreach (IPlayer player in api.World.AllOnlinePlayers.Where(p => p.Entity.Controls.TriesToMove))
            {
                OpenDoors(api, player);
            }
            CloseDoors(api);
        }, 50);
    }

    private void CloseDoors(ICoreAPI api)
    {
        _openedDoors.RemoveWhere(p =>
        {
            float closeRadius = _radius + 1;
            Entity[] entities = api.World.GetIntersectingEntities(p, [new Cuboidf(-closeRadius, -closeRadius, -closeRadius, closeRadius, closeRadius + 1, closeRadius)],
                e => e.Class == nameof(EntityPlayer) && e.Pos.AsBlockPos.ManhattenDistance(p) <= _radius + 1);
            if (entities.Any())
            {
                return false;
            }

            Block b = api.World.BlockAccessor.GetBlock(p);
            Caller caller = new Caller()
            {
                Type = EnumCallerType.Console
            };
            BlockSelection selection = new BlockSelection(p, BlockFacing.DOWN, b);
            TreeAttribute activation = new();
            activation.SetBool("opened", false);
            b.Activate(api.World, caller, selection, activation);
            return true;
        });
    }

    private void OpenDoors(ICoreAPI api, IPlayer player)
    {
        BlockPos pos = player.Entity.Pos.AsBlockPos;
        BlockPos startPos = pos.AddCopy(-_radius, -_radius, -_radius);
        BlockPos endPos = pos.AddCopy(_radius, _radius + 1, _radius);
        api.World.BlockAccessor.SearchBlocks(startPos, endPos, (b, p) =>
        {
            if (player.Entity.Pos.AsBlockPos.ManhattenDistance(p) > _radius + 1)
            {
                return true;
            }
            
            // Try open doors within range.
            if (!_openedDoors.Contains(p) && _regex.IsMatch(b.GetPlacedBlockName(api.World, p)))
            {
                Caller caller = new Caller()
                {
                    Type = EnumCallerType.Player, Entity = player.Entity,
                };
                BlockSelection selection = new BlockSelection(p, BlockFacing.DOWN, b);
                TreeAttribute activation = new();
                activation.SetBool("opened", true);
                b.Activate(api.World, caller, selection, activation);
                _openedDoors.Add(p.Copy());
            }
            return true;
        });
    }

    private static bool OpenState(Block b, BlockPos p, bool state)
    {
        return b.GetBEBehavior<BEBehaviorDoor>(p) is { } door && door.Opened == state || b.GetBEBehavior<BEBehaviorTrapDoor>(p) is {} trapDoor && trapDoor.Opened == state;
    }
}