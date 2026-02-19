using Cairo;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace VintageStoryMods.Todo.GuiElements;

internal sealed class GuiElementTextCell : GuiElementCellBase
{
    private readonly GuiElementTextInput _text;
    
    public GuiElementTextCell(ICoreClientAPI capi, ElementBounds bounds, string text, CairoFont font) : base(capi, bounds)
    {
        _text = new GuiElementTextInput(capi, bounds.ForkContainingChild().WithAlignment(EnumDialogArea.CenterTop), OnTextChanged, font);
        _text.Text = text;
    }

    public override void ComposeElements(Context ctx, ImageSurface surface)
    {
        base.ComposeElements(ctx, surface);
        
        _text.ComposeTextElements(ctx, surface);
    }

    public void OnTextChanged(string str)
    {
        
    }

    public override void BeforeCalcBounds()
    {
        base.BeforeCalcBounds();
        _text.BeforeCalcBounds();
    }

    public override void OnFocusGained()
    {
        base.OnFocusGained();
        _text.OnFocusGained();
    }

    public override void OnFocusLost()
    {
        base.OnFocusLost();
        _text.OnFocusLost();
    }

    public override void OnKeyDown(ICoreClientAPI api, KeyEvent args)
    {
        base.OnKeyDown(api, args);
        _text.OnKeyDown(api, args);
    }

    public override void OnKeyPress(ICoreClientAPI api, KeyEvent args)
    {
        base.OnKeyPress(api, args);
        _text.OnKeyPress(api, args);
    }

    public override void OnKeyUp(ICoreClientAPI api, KeyEvent args)
    {
        base.OnKeyUp(api, args);
        _text.OnKeyUp(api, args);
    }

    public override void OnMouseDown(ICoreClientAPI api, MouseEvent mouse)
    {
        base.OnMouseDown(api, mouse);
        _text.OnMouseDown(api, mouse);
    }

    public override void OnMouseDownOnElement(ICoreClientAPI api, MouseEvent args)
    {
        base.OnMouseDownOnElement(api, args);
        _text.OnMouseDown(api, args);
    }

    public override bool OnMouseEnterSlot(ICoreClientAPI api, ItemSlot slot)
    {
        return base.OnMouseEnterSlot(api, slot) || _text.OnMouseEnterSlot(api, slot);
    }

    public override bool OnMouseLeaveSlot(ICoreClientAPI api, ItemSlot slot)
    {
        return base.OnMouseLeaveSlot(api, slot) || _text.OnMouseLeaveSlot(api, slot);
    }
}