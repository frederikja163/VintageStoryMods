using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace VintageStoryMods;

public sealed class ImprovedWayPointsModSystem : ModSystem
{
    public override void StartServerSide(ICoreServerAPI api)
    {
        base.StartServerSide(api);
        api.Network.GetChannel("worldmap").SetMessageHandler<MapLayerUpdate>(MessageReceived);
        api.Network.GetChannel("worldmap").SetMessageHandler<OnViewChangedPacket>(MessageReceived);
        api.Network.GetChannel("worldmap").SetMessageHandler<OnMapToggle>(MessageReceived);
    }

    private void MessageReceived(IServerPlayer player, MapLayerUpdate update)
    {
        
    }

    private void MessageReceived(IServerPlayer player, OnViewChangedPacket update)
    {
        
    }

    private void MessageReceived(IServerPlayer player, OnMapToggle update)
    {
        
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        base.StartClientSide(api);
        api
    }
}