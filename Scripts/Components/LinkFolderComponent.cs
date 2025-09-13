using Godot;
using System;

public partial class LinkFolderComponent : Node
{
    [Export] private FileDialog _fileDialog;
    [Export] private Button _linkNewFolderButton;
    [Export] private HBoxContainer _thunmnailsBoxContainer;
    
    [Export] private PackedScene _photoThumbnailScene;
    


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

        foreach (string path in paths)
       {
           GD.Print("Importing asset : " + path);

           Image img = Image.LoadFromFile(path); 
           
           if (img == null || img.IsEmpty())
           {
               GD.PrintErr($"Failed to load image: {path}");
               
               continue;
           }

           Texture2D texture = ImageTexture.CreateFromImage(img);
           
           if (texture == null)
           {
               GD.PrintErr($"Failed to create texture from image: {path}");
               
               continue;
           }

           var thumbnail = _photoThumbnailScene.Instantiate<PhotoThumbnail>();
           
           if (thumbnail == null)
           {
               GD.PrintErr($"Failed to create thumbnail: {path}");

               continue;
           }
           
           
           thumbnail.SetPhotoThumbnail(texture,path);
           
           _thunmnailsBoxContainer.AddChild(thumbnail);
       }
        
        
    }
}
