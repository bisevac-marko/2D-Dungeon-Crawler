using Godot;

public class Character : KinematicBody2D
{
    public Vector2 Velocity { get; set; }
    public State CurrentState { get; set; }
}
