using Godot;
using System;

public class Player : Character
{
    private Vector2 _direction = Vector2.Zero;
    private Vector2 _speed = new Vector2(200, 200);

    public override void _Ready(){

    }

    public override void _PhysicsProcess(float delta){
        _direction = GetDirection();
        Velocity = CalculateVelocity(Velocity, _direction, _speed);
        MoveAndSlide(Velocity);
    }

    private Vector2 CalculateVelocity(Vector2 velocity, Vector2 direction, Vector2 speed){
        Vector2 vel = direction * speed;
        return vel;

    }

    private Vector2 GetDirection(){
        Vector2 direction = new Vector2(Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
                                            Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up"));
        return direction;                                          
    }
}
