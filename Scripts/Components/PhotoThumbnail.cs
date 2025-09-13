using Godot;
using System;

public partial class PhotoThumbnail : Control
{
    [Export] private TextureRect _textureRect;
    [Export] private Button _thumbnailButton;


    private string _path;

    private PhotoEditingComponent _photoEditComponent;

    public override void _Ready()
    {
        CustomMinimumSize = new Vector2(128, 128);  // fixed square
        SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
        SizeFlagsVertical   = Control.SizeFlags.ShrinkCenter;

        if (_textureRect != null)
        {
            _textureRect.StretchMode = TextureRect.StretchModeEnum.KeepAspectCovered;
        }

        _thumbnailButton.Pressed += OnThumbnailButtonPressed;


    }

    private void OnThumbnailButtonPressed()
    {
        if (_photoEditComponent == null)
        {
            _photoEditComponent = GetTree().Root.FindChild("PhotoEditingComponent", recursive: true, owned: false) as PhotoEditingComponent;

            if (_photoEditComponent == null)
            {
                GD.Print("PEC is null");

                return;
            }
        }
        
        _photoEditComponent.SetCurrentPictureForEdit(_textureRect.Texture);
    }

    public void SetPhotoThumbnail(Texture2D submittedTexture,string path)
    { 
        _textureRect.Texture = submittedTexture;
        _path = path;
    }
}
