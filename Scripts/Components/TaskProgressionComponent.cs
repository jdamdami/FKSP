using Godot;
using System;

public partial class TaskProgressionComponent : VBoxContainer
{
    [Export] private Label _taskNameLabel;
    [Export] private ProgressBar _progressBar;

    public void SetTaskProgressionValue(string taskName, int subTasksDone, int maxSubTaskToDo)
    {
        SetVisible(true);
        _taskNameLabel.Text = taskName;
        float percentage = ((float)subTasksDone / (float)maxSubTaskToDo) * 100;
        _progressBar.Value = percentage;
        GD.Print(percentage);
    }

    public async void EngageTaskProgressionBarFadeOut()
    {
        Tween tween = CreateTween();
        tween.TweenProperty(this, "modulate:a", 0.0f, 1.0f);
        await ToSignal(tween, Tween.SignalName.Finished);
        Visible = false;
        Modulate = new Color(Modulate.R, Modulate.G, Modulate.B, 1.0f);
    }
}
