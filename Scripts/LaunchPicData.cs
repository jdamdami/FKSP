using Godot;

[GlobalClass]
public partial class LaunchPicData : Resource
{
    [Export] public string Title = ""; 
    [Export] public string Photographer = ""; 
    [Export] public Texture2D Image;
}