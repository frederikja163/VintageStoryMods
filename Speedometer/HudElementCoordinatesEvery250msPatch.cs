using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

namespace VintageStoryMods;

[HarmonyPatch(typeof(HudElementCoordinates))]
[HarmonyPatch("Every250ms")]
public static class HudElementCoordinatesEvery250msPatch
{
    private static Vec3d _pos = Vec3d.Zero;
    
    public static void Postfix(ref HudElementCoordinates __instance, float dt)
    {
        ICoreClientAPI capi = __instance.SingleComposer.Api;

        Vec3d pos = capi.World.Player.Entity.Pos.XYZ;
        double speed = (pos - _pos).Length() / dt;
        GuiElementDynamicText? element = __instance.SingleComposer.GetDynamicText("text");
        string text = element.GetText();
        text += $"\n{speed:F1} B/s";
        element.SetNewText(text);
        _pos = pos.Clone();
    }
}