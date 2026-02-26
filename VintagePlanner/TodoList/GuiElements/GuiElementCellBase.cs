using System.Collections.Generic;
using System.Linq;
using Cairo;
using Vintagestory.API.Client;

namespace VintageStoryMods.TodoList.GuiElements;

internal abstract class GuiElementCellBase : GuiElementContainer
{
    public GuiElementCellBase(ICoreClientAPI capi, ElementBounds bounds) : base(capi, bounds)
    {
        unscaledCellSpacing = 0;
        UnscaledCellHorPadding = 0;
        UnscaledCellVerPadding = 0;
        Add(new GuiElementCellBackground(capi, bounds.ForkContainingChild()));
    }
}