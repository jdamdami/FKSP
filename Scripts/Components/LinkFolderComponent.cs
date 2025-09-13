using Godot;
using System;

public partial class LinkFolderComponent : Node
{
    [Export] private FileDialog _fileDialog;
    [Export] private Button _linkNewFolderButton;
    [Export] private HBoxContainer _thunmnailsBoxContainer;
    


public override void _Ready()
    {
        _fileDialog.FilesSelected += OnNewFilesSelected;
        _linkNewFolderButton.Pressed += OnLinkNewFolderButtonPressed;
    }

    private void OnLinkNewFolderButtonPressed()
    {
        _fileDialog.PopupCentered(new Vector2I(800, 600));
    }


    private void OnNewFilesSelected(string[] paths)
    {
        GD.Print("OnNewFilesSelected");

        foreach (string path in paths)
        {
            GD.Print("Importing asset : " + path);
            
            
        }
        
        
    }
}
