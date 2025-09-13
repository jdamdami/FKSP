using Godot;
using System;
using System.Threading.Tasks;

public partial class LaunchScreen : Control
{   
    [ExportCategory("Background Images")]
    [Export] private Godot.Collections.Array<LaunchPicData> _photos = new();
    [Export] private TextureRect _textureRectA;
    [Export] private TextureRect _textureRectB;
    
    [ExportCategory("Pictures Data")]
    [Export] private RichTextLabel _photographerName;
    [Export] private RichTextLabel _photographyTitle;
    
    [ExportCategory("Project Selection Screen")]
    [Export] private PackedScene _projectSelectionScreen;

    private Random _rand = new Random();
    private Godot.Collections.Array<LaunchPicData> _backgroundImagesPool = new();
    private bool _useTextureA = true;
    private bool _firstImage = true;

    public override void _Ready()
    {
        CreateLaunchWindow();

        _ = SetNewBackgroundImageSafe();
        _ = ShowProjectScreenSafe();
    }

    private void CreateLaunchWindow()
    {
        DisplayServer.WindowSetSize(new Vector2I(1280, 720));

        DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        
        DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.ResizeDisabled, true);
        
        DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, true);
        
        _textureRectA.StretchMode = TextureRect.StretchModeEnum.KeepAspectCovered;
        _textureRectA.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
        _textureRectA.Modulate = new Color(1, 1, 1, 1);

        _textureRectB.StretchMode = TextureRect.StretchModeEnum.KeepAspectCovered;
        _textureRectB.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
        _textureRectB.Modulate = new Color(1, 1, 1, 0);

        _backgroundImagesPool = new Godot.Collections.Array<LaunchPicData>(_photos);
    }

    private async Task SetNewBackgroundImageSafe()
    {
        try
        {
            await SetNewBackgroundImage();
        }
        catch (Exception ex)
        {
            GD.PrintErr("Error in SetNewBackgroundImage: ", ex);
        }
    }

    private async Task ShowProjectScreenSafe()
    {
        try
        {
            await ShowProjectScreen();
        }
        catch (Exception ex)
        {
            GD.PrintErr("Error in ShowProjectScreen: ", ex);
        }
    }

    private async Task SetNewBackgroundImage()
    {
        if (_backgroundImagesPool.Count < 1)
        {
            _backgroundImagesPool = new Godot.Collections.Array<LaunchPicData>(_photos);
        }

        if (_backgroundImagesPool.Count == 0) return;

        int index = _rand.Next(_backgroundImagesPool.Count);
        LaunchPicData chosen = _backgroundImagesPool[index];
        _backgroundImagesPool.RemoveAt(index);

        if (chosen.Image == null) return;

        if (_firstImage)
        {
            _textureRectA.Texture = chosen.Image;
            _photographerName.Text = chosen.Photographer;
            _photographyTitle.Text = chosen.Title;
            _firstImage = false;

            await ToSignal(GetTree().CreateTimer(2.0), SceneTreeTimer.SignalName.Timeout);

            await SetNewBackgroundImage();
            return;
        }

        TextureRect fadeOutRect;
        TextureRect fadeInRect;

        if (_useTextureA)
        {
            fadeOutRect = _textureRectA;
            fadeInRect = _textureRectB;
        }
        else
        {
            fadeOutRect = _textureRectB;
            fadeInRect = _textureRectA;
        }

        fadeInRect.Texture = chosen.Image;
        fadeInRect.Modulate = new Color(1, 1, 1, 0);

        _photographerName.Text = chosen.Photographer;
        _photographyTitle.Text = chosen.Title;

        var tween = CreateTween();
        tween.Parallel().TweenProperty(fadeOutRect, "modulate:a", 0.0f, 0.5f);
        tween.Parallel().TweenProperty(fadeInRect, "modulate:a", 1.0f, 0.5f);

        await ToSignal(tween, Tween.SignalName.Finished);

        _useTextureA = !_useTextureA;

        await ToSignal(GetTree().CreateTimer(2.0), SceneTreeTimer.SignalName.Timeout);

        await SetNewBackgroundImage();
    }

    private async Task ShowProjectScreen()
    {
        await ToSignal(GetTree().CreateTimer(10.0), SceneTreeTimer.SignalName.Timeout);

        if (_projectSelectionScreen == null) return;

        GetTree().ChangeSceneToPacked(_projectSelectionScreen);
    }
}
