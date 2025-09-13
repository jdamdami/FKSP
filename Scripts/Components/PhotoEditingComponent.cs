using Godot;
using System;

public partial class PhotoEditingComponent : Node
{
    [Export] private TextureRect _pictureTextureRect;

    public void SetCurrentPictureForEdit(Texture2D submittedTexture)
    {
        _pictureTextureRect.Texture = submittedTexture;
    }
    
}
