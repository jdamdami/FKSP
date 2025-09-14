using Godot;
using System;

public partial class PhotoEditingComponent : Node
{
    [Export] private TextureRect _pictureTextureRect;

    public void SetCurrentPictureForEdit(string path)
    {
        // Load image from disk
        var image = new Image();
        var err = image.Load(path);
        if (err != Error.Ok)
        {
            GD.PrintErr($"Failed to load image: {path}, error: {err}");
            return;
        }

        // Convert it to a texture
        var texture = ImageTexture.CreateFromImage(image);

        // Assign to the TextureRect
        _pictureTextureRect.Texture = texture;

        // Keep aspect ratio
        _pictureTextureRect.StretchMode = TextureRect.StretchModeEnum.KeepAspect;

        /*
        _pictureTextureRect.Expand = false;
        _pictureTextureRect.CustomMinimumSize = texture.GetSize();*/
    }
    
}
