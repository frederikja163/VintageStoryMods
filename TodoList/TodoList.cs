using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace VintageStoryMods;

public sealed class TodoList : ModSystem
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
        todoDialog = new TodoDialog(api);
        api.Gui.RegisterDialog(todoDialog);
        api.Input.RegisterHotKey("opentodo", "Open Todo", GlKeys.O, HotkeyType.GUIOrOtherControls);
        api.Input.SetHotKeyHandler("opentodo", _ =>
        {
            todoDialog.Toggle();
            return true;
        });
        #if DEBUG
        api.ChatCommands.Create("refresh-gui").RequiresPlayer().RequiresPrivilege(Privilege.chat).HandleWith(_ =>
        {
            todoDialog.Reload();
            return TextCommandResult.Success();
        });
        #endif //DEBUG
    }
}