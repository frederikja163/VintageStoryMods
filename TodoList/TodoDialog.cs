using Vintagestory.API.Client;
using Vintagestory.GameContent;

namespace VintageStoryMods;

internal sealed class TodoDialog : GuiDialog
{
    public TodoDialog(ICoreClientAPI capi) : base(capi)
    {
        Reload();
    }

    public override string ToggleKeyCombinationCode { get; } = "opentodo";

    public void Reload()
    {
        ElementBounds bounds = ElementBounds.FixedSize(500, 500).WithAlignment(EnumDialogArea.CenterMiddle);
        
        SingleComposer = capi.Gui.CreateCompo("Todo", bounds)
            .AddShadedDialogBG(ElementBounds.Fill)
            .AddDialogTitleBar("Todo123", () => TryClose())
            .Compose(true);
    }
}