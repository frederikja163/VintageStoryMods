using Cairo;
using Vintagestory.API.Client;

namespace VintageStoryMods.TodoList.GuiElements;

internal sealed class GuiElementCellBackground : GuiElement
{
    public static float OutlineThickness = 2;
    public GuiElementCellBackground(ICoreClientAPI capi, ElementBounds bounds) : base(capi, bounds)
    {
    }
    
    public override void ComposeElements(Context ctx, ImageSurface surface)
    {
        base.ComposeElements(ctx, surface);
        Bounds.CalcWorldBounds();
        double thickness = scaled(OutlineThickness);
        Rectangle(ctx, Bounds.absFixedX, Bounds.absFixedY, Bounds.OuterWidth, Bounds.OuterHeight);
        ctx.SetSourceRGBA(GuiStyle.ColorSchematic);
        ctx.Fill();
        
        Rectangle(ctx, Bounds.absFixedX + thickness, Bounds.absFixedY + thickness, Bounds.OuterWidth - thickness * 2, Bounds.OuterHeight - thickness * 2);
        ctx.SetSourceRGBA(GuiStyle.DialogStrongBgColor);
        ctx.Fill();
    }
}