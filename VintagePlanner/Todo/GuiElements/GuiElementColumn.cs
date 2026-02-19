using System;
using System.Collections.Generic;
using System.Linq;
using Cairo;
using Vintagestory.API.Client;
using Vintagestory.GameContent;

namespace VintageStoryMods.Todo.GuiElements;

internal sealed class GuiElementColumn: GuiElementContainer
{
    public GuiElementColumn(ICoreClientAPI capi, ElementBounds bounds, IEnumerable<object> cells) : base(capi, bounds)
    {
        bounds = bounds.ForkContainingChild()
            .WithSizing(ElementSizing.Percentual, ElementSizing.Fixed)
            .WithPercentualWidth(1).WithFixedHeight(GuiStyle.DetailFontSize + GuiStyle.HalfPadding * 2);
        foreach (object cell in cells)
        {
            GuiElementCellBase cellEle = CreateCell(capi, ref bounds, cell);
            Add(cellEle);
        }
    }

    private GuiElementCellBase CreateCell(ICoreClientAPI capi, ref ElementBounds bounds, object obj)
    {
        GuiElementCellBase cellBase;
        switch (obj)
        {
            case string str:
                var g = new GuiElementTextInput(capi, bounds, OnTextChanged, CairoFont.WhiteDetailText());
                cellBase = new GuiElementCellBase(capi, bounds.ForkContainingChild());
                g.Text = str;
                Add(g);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(obj));
        }
        bounds = bounds.BelowCopy();
        return cellBase;
    }

    private void OnTextChanged(string obj)
    {
    }
}