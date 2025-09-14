using Godot;
using System;

public partial class LinkFolderComponent : Node
{
    [Export] private FileDialog _fileDialog;
    [Export] private Button _linkNewFolderButton;
    [Export] private HBoxContainer _thunmnailsBoxContainer;
    
    [Export] private PackedScene _photoThumbnailScene;

    private int _currentImportedIndex = 0;

    private Godot.Collections.Array<string> _pathsToImport = new();
    


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

        if (_photoThumbnailScene == null) return;
        
        _thunmnailsBoxContainer.GetChildren().Clear();

        _currentImportedIndex = 0;
        
        _pathsToImport.Clear();

        _pathsToImport = new Godot.Collections.Array<string>(paths);
        
        ImportFiles();
    }

    private async void ImportFiles()
    {
        if (_pathsToImport != null && _currentImportedIndex >= 0 && _currentImportedIndex < _pathsToImport.Count)
        {
            string path = _pathsToImport[_currentImportedIndex];
            
            Image img = Image.LoadFromFile(path);
            
            if (img == null || img.IsEmpty())
            {
                GD.PrintErr($"Failed to load image: {path}");

                return;
            }

            Texture2D texture = ImageTexture.CreateFromImage(img);

            if (texture == null)
            {
                GD.PrintErr($"Failed to create texture from image: {path}");

                return;
            }

            var thumbnail = _photoThumbnailScene.Instantiate<PhotoThumbnail>();

            if (thumbnail == null)
            {
                GD.PrintErr($"Failed to create thumbnail: {path}");

                return;
            }

            thumbnail.SetPhotoThumbnail(texture,path);

            _thunmnailsBoxContainer.AddChild(thumbnail);

            _currentImportedIndex++;
            
            await ToSignal(GetTree().CreateTimer(0.1), SceneTreeTimer.SignalName.Timeout);
            
            ImportFiles();


        }

    }
}
