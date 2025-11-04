using Godot;
using System;
using System.Linq;

public partial class GameStateScript : Node
{
    public static GameStateScript Instance { get; private set; }
    
    public bool IsBrunoFed = false;
    public bool IsLilyFed = false;
    public bool IsMarthaFed = false;
    public bool IsBeastFed = false;
    public int Round = 1;

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }

    public void SetFishFed(string FishName)
    {
        switch (FishName)
        {
            case "Bruno":
                IsBrunoFed = true;
                break;
            case "Lily":
                IsLilyFed = true;
                break;
            case "Martha":
                IsMarthaFed = true;
                break;
            case "??":
                IsBeastFed = true;
                break;
        }
    }

    public void IncrementRound()
    {
        Round++;
        IsBrunoFed = false;
        IsLilyFed = false;
        IsMarthaFed = false;
        IsBeastFed = false;
    }

    public bool IsAllFishFed()
    {
        return IsBrunoFed && IsLilyFed && IsMarthaFed && IsBeastFed;
    }

    public bool IsAllButBeastFed()
    {
        return IsBrunoFed && IsLilyFed && IsMarthaFed && !IsBeastFed;
    }
}
