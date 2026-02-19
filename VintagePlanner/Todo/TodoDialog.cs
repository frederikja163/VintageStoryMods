using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using VintageStoryMods.Todo.Columns;
using VintageStoryMods.Todo.GuiElements;

namespace VintageStoryMods.Todo;

internal sealed class TodoDialog : GuiDialog
{
    private List<ColumnBase> _columns = new();
    private readonly Dictionary<ColumnBase, float> _columnWidths = new();
    
    public TodoDialog(ICoreClientAPI capi) : base(capi)
    {
        Reload();
    }

    public override string ToggleKeyCombinationCode { get; } = "opentodo";

    public void ClearColumns()
    {
        _columns.Clear();
        _columnWidths.Clear();
    }

    public void ClearRows()
    {
        foreach (ColumnBase column in _columns)
        {
            column.Clear();
        }
    }
    
    public void AddColumn(ColumnBase column)
    {
        _columns.Add(column);
        _columnWidths.Add(column, 100);
    }

    public void AddRow()
    {
        foreach (ColumnBase column in _columns)
        {
            column.AddRow();
        }
    }

    public void SetValue(int column, int row, object value)
    {
        _columns[column].SetValue(row, value);
    }

    public void Reload()
    {
        ClearColumns();
        AddColumn(new TextColumn("hello"));
        AddColumn(new TextColumn("testg"));
        AddColumn(new TextColumn("hello"));
        AddRow();
        AddRow();
        AddRow();
        SetValue(1, 2, "Hello world");
        ReComposeGui();
    }

    private void ReComposeGui()
    {
        try
        {
            ElementBounds dialogEle = ElementStdBounds.AutosizedMainDialog.WithAlignment(EnumDialogArea.CenterMiddle);
            ElementBounds bgEle = ElementBounds.FixedSize(500, 500).WithFixedPadding(GuiStyle.ElementToDialogPadding);
            ElementBounds headerEle = bgEle.ForkContainingChild(0, GuiStyle.TitleBarHeight).WithFixedHeight(GuiStyle.NormalFontSize).WithAlignment(EnumDialogArea.LeftTop);
            ElementBounds tableEle = bgEle.ForkContainingChild(0, GuiStyle.TitleBarHeight + GuiStyle.NormalFontSize + GuiStyle.HalfPadding * 2);
            ElementBounds addRowBtnEle = tableEle.BelowCopy().WithFixedWidth(1).WithFixedHeight(GuiStyle.SmallFontSize);
            
            SingleComposer = capi.Gui.CreateCompo("Todo", dialogEle)
                    .AddShadedDialogBG(bgEle)
                    .AddDialogTitleBar("Todo", () => TryClose())
                    .BeginChildElements()
                    .AddContainer(headerEle, "header")
                    .AddContainer(tableEle, "table")
                    .AddSmallButton(" + ", OnAddRowButtonClicked, addRowBtnEle)
                    .EndChildElements()
                ;
            
            GuiElementContainer headerContainer = SingleComposer.GetContainer("header");
            CairoFont font = CairoFont.WhiteMediumText();
            ElementBounds headerBounds = headerEle.ForkContainingChild().WithFixedPadding(GuiStyle.HalfPadding);
            GuiElementContainer tableContainer = SingleComposer.GetContainer("table");
            ElementBounds tableBounds = tableEle.ForkContainingChild().WithFixedPadding(GuiStyle.HalfPadding);
            foreach (ColumnBase column in _columns)
            {
                float width = _columnWidths[column];
                headerBounds.WithFixedWidth(width);
                headerContainer.Add(new GuiElementHeaderCell(capi, headerBounds, column.Name, font));
                headerContainer.Add(new GuiElementTextInput(capi, ElementBounds.Fixed(0, 0, 100, 100), Text, CairoFont.TextInput()));
                headerBounds = headerBounds.RightCopy(-GuiElementCellBase.OutlineThickness);

                tableBounds.WithFixedWidth(width);
                tableContainer.Add(new GuiElementColumn(capi, tableBounds, column.GetValues()));
                tableBounds = tableBounds.RightCopy(-GuiElementCellBase.OutlineThickness);
            }
            
            SingleComposer.Compose();
            capi.ShowChatMessage("Reloaded!");
        }
        catch (Exception e)
        {
            capi.ShowChatMessage("Error!");
            capi.Logger.Error(e);
        }
    }

    private bool OnAddRowButtonClicked()
    {
        AddRow();
        ReComposeGui();
        return true;
    }

    private void Text(string txt)
    {
        
    }
}