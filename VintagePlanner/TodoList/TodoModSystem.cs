using System.Linq;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace VintageStoryMods.TodoList;

public sealed class TodoModSystem : ModSystem
{
    private TodoDialog? todoDialog;
    
    public override bool ShouldLoad(ICoreAPI api)
    {
        return true;
    }


    public override void StartPre(ICoreAPI api)
    {
        base.StartPre(api);
    }

    public override void Start(ICoreAPI api)
    {
        if (api is ICoreClientAPI capi)
        {
            StartClient(capi);
        }
    }


    public override void StartClientSide(ICoreClientAPI api)
    {
        base.StartClientSide(api);
    }

    private void StartClient(ICoreClientAPI api)
    {
        api.Input.RegisterHotKey("opentodo", "Open Todo", GlKeys.O, HotkeyType.GUIOrOtherControls);
        #if DEBUG
        api.ChatCommands.Create("r").RequiresPlayer().RequiresPrivilege(Privilege.chat).HandleWith(_ =>
        {
            GuiDialog dialog = api.Gui.LoadedGuis.FirstOrDefault(g => g is TodoDialog);
            if (dialog is not null)
            {
                api.Gui.TriggerDialogClosed(dialog);
                api.Gui.LoadedGuis.Remove(dialog);
                api.Gui.OpenedGuis.Remove(dialog);
            }
            todoDialog = new TodoDialog(api);
            api.Gui.RegisterDialog(todoDialog);
            todoDialog.TryOpen(true);
            return TextCommandResult.Success();
        });
        #endif //DEBUG
    }
}