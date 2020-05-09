using Godot;
using System;

public abstract class State {
    public State(Character character){
        this.character = character;
    }

    public Character character;
    public abstract void Update();

    public virtual void OnStateEnter();
    public virtual void OnStateExit();
}
