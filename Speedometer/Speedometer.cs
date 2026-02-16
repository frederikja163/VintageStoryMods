using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;

namespace VintageStoryMods;

public sealed class Speedometer : ModSystem
{
    public override void StartClientSide(ICoreClientAPI api)
    {
        GuiDialog? gui = api.Gui.LoadedGuis.FirstOrDefault(g => g is HudElementCoordinates);
        if (gui is not null)
        {
            api.Gui.LoadedGuis.Remove(gui);
            api.Gui.OpenedGuis.Remove(gui);
            gui.Dispose();
            gui = new HudElementSpeedometer(api);
            api.Gui.RegisterDialog(gui);
        }
    }
}