using Godot;
using System;

public class Move : State
{
    public Move(Character character) : base(character)
    {
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }

}
