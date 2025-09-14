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
        DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.ResizeDisabled, false);
        DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
        DisplayServer.WindowSetMode(DisplayServer.WindowMode.Maximized);
        GD.Print("SetWindowsSettings() - borderless OFF, maximized ON");
        
        var state = (AppState)GetNode("/root/AppState");

        if (state == null)
        {
            GD.Print("AppState not found");
        }
        else
        {
            GD.Print("AppState FOUND");
        }
    }
}