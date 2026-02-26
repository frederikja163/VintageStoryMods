using Cairo;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace VintageStoryMods.TodoList.GuiElements;

internal sealed class GuiElementTextCell : GuiElementCellBase
{
    private readonly GuiElementTextInput _text;
    
    public GuiElementTextCell(ICoreClientAPI capi, ElementBounds bounds, string text) : base(capi, bounds)
    {
        _text = new GuiElementTextInput(capi, bounds.ForkContainingChild(), OnTextChanged, CairoFont.WhiteSmallishText());
        // var _text = new GuiElementStaticText(capi, text, EnumTextOrientation.Center, bounds.ForkContainingChild(), CairoFont.WhiteMediumText());
        Add(_text);
        Tabbable = true;
    }

    public override void ComposeElements(Context ctx, ImageSurface surface)
    {
        // _text.SetValue(_text.Text + "1");
        base.ComposeElements(ctx, surface);
    }

    public void OnTextChanged(string str)
    {
    }
}