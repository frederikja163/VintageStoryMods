using Cairo;
using Vintagestory.API.Client;

namespace VintageStoryMods.Todo.GuiElements;

internal sealed class GuiElementHeaderCell : GuiElementCellBase
{
    private readonly GuiElementStaticText _text;
    
    public GuiElementHeaderCell(ICoreClientAPI capi, ElementBounds bounds, string name, CairoFont font) : base(capi, bounds)
    {
        _text = new GuiElementStaticText(capi, name, EnumTextOrientation.Center, bounds.ForkContainingChild().WithAlignment(EnumDialogArea.CenterTop), font);
    }

    public override void ComposeElements(Context ctx, ImageSurface surface)
    {
        base.ComposeElements(ctx, surface);
        
        _text.ComposeTextElements(ctx, surface);
    }
}