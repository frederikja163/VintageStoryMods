using System.Linq;
using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace VintageStoryMods;

public sealed class SpeedometerModSystem : ModSystem
{
    private static Harmony? _harmony;
    
    public override void StartPre(ICoreAPI api)
    {
        base.StartPre(api);
        if (_harmony is null)
        {
            _harmony = new Harmony("speedometer");
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}