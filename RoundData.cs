using System;
using Godot;

namespace TheDeepFeast;

public partial class RoundData : Node
{
    [Export] public BeastRoute Route;
    [Export] public Node2D Obstacles;
}