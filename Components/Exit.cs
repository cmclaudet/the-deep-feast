using Godot;
using System;
using System.Threading.Tasks;

public partial class Exit : ColorRect
{
    [Export] private Area2D area2D;
    [Export] private Vector2 unlockedPosition;
    private bool isUnlocked;
    private bool canEndDay;
    
    public override void _Ready()
    {
        GameManagerScript.Instance.SetExit(this);
        area2D.BodyEntered += OnBodyEntered;
        area2D.BodyExited += OnBodyExited;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        
        if (Input.IsActionJustPressed("ui_accept") && canEndDay)
        {
            GD.Print("End the day!");
            GameStateScript.Instance.IncrementRound();
            _ = Reload();
        }
    }

    private async Task Reload()
    {
        GD.Print("Reloading");
        var fader = GameManagerScript.Instance.FadeOut;

        await fader.DoFadeOut();
        
        GetTree().ReloadCurrentScene();
        await fader.DoFadeIn();
    }

    private void OnBodyExited(Node2D body)
    {
        if (body is Player && isUnlocked)
        {
            var prompt = GameManagerScript.Instance.Prompt;
            prompt.ToggleDisplay(false);
            canEndDay = false;
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Player && isUnlocked)
        {
            var prompt = GameManagerScript.Instance.Prompt;
            prompt.SetText("END DAY");
            prompt.SetOver(area2D, -20);
            prompt.ToggleDisplay(true);
            canEndDay = true;
        }
    }

    public void SetUnlocked()
    {
        isUnlocked = true;
        Position = unlockedPosition;
    }
}
