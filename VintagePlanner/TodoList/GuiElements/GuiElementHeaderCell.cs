using Cairo;
using Vintagestory.API.Client;

namespace VintageStoryMods.TodoList.GuiElements;

internal sealed class GuiElementHeaderCell : GuiElementCellBase
{
    private readonly GuiElementStaticText _text;
    
    public GuiElementHeaderCell(ICoreClientAPI capi, ElementBounds bounds, string name) : base(capi, bounds)
    {
        _text = new GuiElementStaticText(capi, name, EnumTextOrientation.Center, bounds.ForkContainingChild(), CairoFont.WhiteMediumText());
        Add(_text);
    }
}