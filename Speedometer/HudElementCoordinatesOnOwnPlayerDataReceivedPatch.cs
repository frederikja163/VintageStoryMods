using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

namespace VintageStoryMods;

[HarmonyPatch(typeof(HudElementCoordinates))]
[HarmonyPatch("OnOwnPlayerDataReceived")]
public static class HudElementCoordinatesOnOwnPlayerDataReceivedPatch
{
    private static Vec3d _pos = Vec3d.Zero;
    
    public static void Postfix(ref HudElementCoordinates __instance)
    {
        GuiElementDynamicText? element = __instance.SingleComposer.GetDynamicText("text");
        element.Bounds.fixedHeight += GuiStyle.SubNormalFontSize;
        element.Bounds.ParentBounds.fixedHeight += GuiStyle.SubNormalFontSize;
        __instance.SingleComposer.ReCompose();
    }
}