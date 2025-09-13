using Godot;
using System;

public partial class MainScreen : Node
{
    public override void _Ready()
    {
        CallDeferred(nameof(SetWindowsSettings));
    }

    private void SetWindowsSettings()
    {
        DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
        DisplayServer.WindowSetMode(DisplayServer.WindowMode.Maximized);
        GD.Print("SetWindowsSettings() - borderless OFF, maximized ON");
    }
}