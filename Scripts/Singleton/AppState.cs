using Godot;
using System;

public partial class AppState : Node
{
    public static AppState Instance { get; private set; }

    private string _currentProjectPath;
    public override void _Ready()
    {
        Instance = this;
        
        GD.Print("AppState Loaded");
    }

    public void ChangeScene(PackedScene newScene)
    {
        GetTree().ChangeSceneToPacked(newScene);
    }

    public void SetCurrentProjectPath(string path)
    {
        _currentProjectPath = path;
        
        GD.Print("App State : SetCurrentProjectPath: " + _currentProjectPath);
    }
}
