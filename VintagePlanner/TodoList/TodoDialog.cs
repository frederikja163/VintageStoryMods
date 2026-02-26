using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.GameContent;
using VintageStoryMods.Extensions;
using VintageStoryMods.TodoList.GuiElements;
using VintageStoryMods.TodoList.Model;
using VintageStoryMods.TodoList.Model.Columns;
using VintageStoryMods.TodoList.Model.EventArgs;

namespace VintageStoryMods.TodoList;

internal sealed class TodoDialog : GuiDialog
{
    private bool _recomposeScheduled;
    private Todo Todo { get; set; }
    private readonly GuiElementContainer _header;
    private readonly GuiElementContainer _body;
    private readonly Dictionary<Guid, GuiElementContainer> _columns = new();
    private const int TableWidth = 500;
    private const int TableHeight = 500;
    
    public TodoDialog(ICoreClientAPI capi) : base(capi)
    {
        Todo = new Todo();
        Todo.AddColumn(new TextColumn("Col1g"));
        Todo.AddColumn(new TextColumn("Col2"));
        Todo.AddRow();
        Todo.OnColumnAdded += (_, args) => AddColumn(args.Column);
        Todo.OnRowAdded += (_, _) =>
        {
            foreach (ColumnBase column in Todo.GetColumns())
            {
                AddCell(column);
            }
        };
        try
        {
            ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(EnumDialogArea.CenterMiddle);

            ElementBounds bgBounds = ElementBounds.FixedPos(EnumDialogArea.CenterTop, 0, GuiStyle.TitleBarHeight)
                .WithSizing(ElementSizing.FitToChildren)
                .WithFixedPadding(GuiStyle.ElementToDialogPadding);

            ElementBounds headerBounds = ElementBounds.Fixed(0, 0, TableWidth, GuiStyle.NormalFontSize + GuiStyle.HalfPadding * 2);
            ElementBounds tableBounds = headerBounds.BelowCopy().WithSizing(ElementSizing.Fixed).WithFixedSize(TableWidth, TableHeight);
            ElementBounds verticalScrollBar = tableBounds.RightCopy().WithFixedWidth(20);
            ElementBounds addRowBounds = tableBounds.BelowCopy(0, GuiStyle.HalfPadding);
            CairoFont.ButtonText().AutoBoxSize("Add Row", addRowBounds);
            ElementBounds addColBounds = addRowBounds.RightCopy(GuiStyle.HalfPadding);
            CairoFont.ButtonText().AutoBoxSize("Add Col", addColBounds);

            _header = new GuiElementContainer(capi, ElementBounds.Fill.WithSizing(ElementSizing.FitToChildren));
            _body = new GuiElementContainer(capi, ElementBounds.Fill.WithSizing(ElementSizing.FitToChildren));

            SingleComposer = capi.Gui.CreateCompo("Todo", dialogBounds)
                .AddDialogTitleBar("Todo", () => TryClose())
                .AddShadedDialogBG(ElementBounds.Fill)
                .BeginChildElements(bgBounds)
                .BeginClip(headerBounds)
                .AddInteractiveElement(_header)
                .EndClip()
                .BeginClip(tableBounds)
                .AddInteractiveElement(_body)
                .EndClip()
                .AddVerticalScrollbar(f => { _body.Bounds.fixedY = 5 - f; _body.Bounds.CalcWorldBounds(); }, verticalScrollBar, "verticalscrollbar")
                .AddButton("Add Row", () => { Todo.AddRow(); return true; }, addRowBounds)
                .AddButton("Add Col", () => { Todo.AddColumn(new TextColumn("Text")); return true; }, addColBounds)
                .EndChildElements();
            
            foreach (ColumnBase column in Todo.GetColumns())
            {
                AddColumn(column);
            }
            
            _body.Tabbable = true;
            NeedsRecompose();
            
            capi.ShowChatMessage("Recomposed!");
        }
        catch (Exception e)
        {
            capi.ShowChatMessage("Error!");
            capi.Logger.Error(e);
        }
    }

    private void AddColumn(ColumnBase column)
    {
        ElementBounds headerBounds = _header.Elements.LastOrDefault()?.Bounds?.RightCopy() ??
                                     ElementBounds.Fixed(0, 0, 0, GuiStyle.NormalFontSize + GuiStyle.HalfPadding * 2);
        _header.Add(new GuiElementHeaderCell(capi, headerBounds.WithFixedWidth(100), column.Name));
        
        foreach (object value in column.GetValues())
        {
            AddCell(column);
        }
        NeedsRecompose();
    }

    private void AddCell(ColumnBase column)
    {
        if (!_columns.TryGetValue(column.Id, out GuiElementContainer? columnEle))
        {
            ElementBounds columnBounds = _body.Elements.LastOrDefault()?.Bounds?.RightCopy() ??
                                         ElementBounds.Fixed(0, 0, 100, 1).WithSizing(ElementSizing.Fixed, ElementSizing.Fixed);
            columnEle = new GuiElementContainer(capi, columnBounds.WithFixedWidth(100));
            _body.Add(columnEle);
            _columns.Add(column.Id, columnEle);
            columnEle.Tabbable = true;
        }

        ElementBounds rowBounds = columnEle.Elements.LastOrDefault()?.Bounds?.BelowCopy() ??
                                  ElementBounds.Fill.WithSizing(ElementSizing.Percentual, ElementSizing.Fixed).WithFixedHeight(GuiStyle.SmallishFontSize + GuiStyle.HalfPadding * 2);
        columnEle.Add(new GuiElementTextCell(capi, rowBounds, "test"));
        // columnEle.Add(new GuiElementTextCell(capi, rowBounds, "test"));
        NeedsRecompose();
    }

    private void NeedsRecompose()
    {
        if (_recomposeScheduled)
            return;
        _recomposeScheduled = true;
        
        capi.World.RegisterCallback(_ =>
        {
            _recomposeScheduled = false;
            SingleComposer.ReCompose();
            capi.Gui.RequestFocus(this);
            SingleComposer.GetScrollbar("verticalscrollbar").SetHeights(TableHeight, (float)_body.Bounds.fixedHeight);
        }, 0);
    }

    public override string ToggleKeyCombinationCode { get; } = "opentodo";
}