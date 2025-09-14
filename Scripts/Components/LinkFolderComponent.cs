using Godot;
using System;

public partial class LinkFolderComponent : Node
{
    [Export] private FileDialog _fileDialog;
    [Export] private Button _linkNewFolderButton;
    [Export] private HBoxContainer _thumbnailsBoxContainer;
    [Export] private PackedScene _photoThumbnailScene;
    [Export] private TaskProgressionComponent _taskProgressionBox;
    [Export] private string _importingFilesProgressDescription;

    private int _currentImportedIndex = 0;
    private int _maxElementToImport = 1;
    private int _totalElementsImported = 0;

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
        
        _thumbnailsBoxContainer.GetChildren().Clear();

        _currentImportedIndex = 0;
        
        _pathsToImport.Clear();

        _pathsToImport = new Godot.Collections.Array<string>(paths);

        _maxElementToImport = paths.Length;
        
        _totalElementsImported = 0;

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

            const int maxSize = 256;
            int w = img.GetWidth();
            int h = img.GetHeight();
            float scale = (float)maxSize / Mathf.Max(w, h);
            int newW = Mathf.RoundToInt(w * scale);
            int newH = Mathf.RoundToInt(h * scale);
            img.Resize(newW, newH, Image.Interpolation.Lanczos);
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
            _thumbnailsBoxContainer.AddChild(thumbnail);
            _currentImportedIndex++;
            _totalElementsImported++;

            if (_taskProgressionBox != null)
            {
                string progressMessage = _importingFilesProgressDescription + img.ResourceName;
                
                _taskProgressionBox.SetTaskProgressionValue(progressMessage,_totalElementsImported,_maxElementToImport);
            }
            
            await ToSignal(GetTree().CreateTimer(0.1), SceneTreeTimer.SignalName.Timeout);
            ImportFiles();


        }
        else
        {
            if (_taskProgressionBox != null)
            {
                _taskProgressionBox.EngageTaskProgressionBarFadeOut();
            }
        }

    }
}
