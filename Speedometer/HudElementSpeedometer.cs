using System;
using System.Collections.Generic;
using System.Globalization;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

namespace VintageStoryMods;

internal sealed class HudElementSpeedometer : HudElement
{
  private Vec3d _lastFramePos = Vec3d.Zero;
  
  public HudElementSpeedometer(ICoreClientAPI capi) : base(capi)
  {
  }

  public override string ToggleKeyCombinationCode => "coordinateshud";

  public override void OnOwnPlayerDataReceived()
  {
    ElementBounds bounds1 = ElementBounds.Fixed(EnumDialogArea.None, 0.0, 0.0, 244, 72.0);
    ElementBounds bounds2 = bounds1.ForkBoundingParent(5.0, 5.0, 5.0, 5.0);
    SingleComposer = capi.Gui
      .CreateCompo("coordinateshud",
        ElementStdBounds.AutosizedMainDialog.WithAlignment(EnumDialogArea.RightTop)
          .WithFixedAlignmentOffset(-GuiStyle.DialogToScreenPadding, GuiStyle.DialogToScreenPadding))
      .AddGameOverlay(bounds2).AddDynamicText("",
        CairoFont.WhiteSmallishText().WithOrientation(EnumTextOrientation.Center), bounds1, "text").Compose();
    
    if (!ClientSettings.ShowCoordinateHud)
      return;
    TryOpen();
  }

  public override void OnBlockTexturesLoaded()
  {
    base.OnBlockTexturesLoaded();
    if (!capi.World.Config.GetBool("allowCoordinateHud", true))
    {
      (capi.World as ClientMain).EnqueueMainThreadTask((Action) (() =>
      {
        (capi.World as ClientMain).UnregisterDialog((GuiDialog) this);
        capi.Input.SetHotKeyHandler("coordinateshud", (ActionConsumable<KeyCombination>) null);
        Dispose();
      }), "unreg");
    }
    else
    {
      capi.Event.RegisterGameTickListener(Every250ms, 250);
      ClientSettings.Inst.AddWatcher("showCoordinateHud", (OnSettingsChanged<bool>) (on =>
      {
        if (on)
          TryOpen();
        else
          TryClose();
      }));
    }
  }

  private void Every250ms(float dt)
  {
    if (!IsOpened())
      return;
    
    BlockPos asBlockPos = capi.World.Player.Entity.Pos.AsBlockPos;
    asBlockPos.Sub(capi.World.DefaultSpawnPosition.AsBlockPos);
    string heading = Lang.Get("facing-" + BlockFacing.HorizontalFromYaw(capi.World.Player.Entity.Pos.Yaw));
    string coordChange = string.Empty;
    if (ClientSettings.ExtendedDebugInfo)
    {
      switch (heading)
      {
        case "North":
          coordChange = " / Z-";
          break;
        case "East":
          coordChange = " / X+";
          break;
        case "South":
          coordChange = " / Z+";
          break;
        case "West":
          coordChange = " / X-";
          break;
        default:
          coordChange = string.Empty;
          break;
      }
    }
    double speed = (capi.World.Player.Entity.Pos.XYZ - _lastFramePos).Length() / dt;
    string text = $"{heading}{coordChange}\nX: {asBlockPos.X.ToString()}, Y: {asBlockPos.Y.ToString()}, Z:{asBlockPos.Z.ToString()}\nSpeed: {speed:F1}";
    SingleComposer.GetDynamicText("text").SetNewText(text);
    List<ElementBounds> dialogBoundsInArea = capi.Gui.GetDialogBoundsInArea(EnumDialogArea.RightTop);
    SingleComposer.Bounds.absOffsetY = GuiStyle.DialogToScreenPadding;
    for (int index = 0; index < dialogBoundsInArea.Count; ++index)
    {
      if (dialogBoundsInArea[index] != SingleComposer.Bounds)
      {
        ElementBounds elementBounds = dialogBoundsInArea[index];
        SingleComposer.Bounds.absOffsetY = GuiStyle.DialogToScreenPadding + elementBounds.absY + elementBounds.OuterHeight;
        break;
      }
    }

    _lastFramePos = capi.World.Player.Entity.Pos.XYZ.Clone();
  }

  public override void OnGuiOpened()
  {
    base.OnGuiOpened();
    ClientSettings.ShowCoordinateHud = true;
  }

  public override void OnGuiClosed()
  {
    base.OnGuiClosed();
    ClientSettings.ShowCoordinateHud = false;
  }
}