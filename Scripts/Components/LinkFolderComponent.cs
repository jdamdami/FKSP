using Godot;
using System;

public partial class LinkFolderComponent : Node
{
    [Export] private FileDialog _fileDialog;

    public override void _Ready()
    {
        _fileDialog.FilesSelected += OnNewFilesSelected;
    }

    private void OnNewFilesSelected(string[] paths)
    {
        
    }
}
