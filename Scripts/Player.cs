using Godot;

public class Player : Character
{
    public Vector2 direction = Vector2.Zero;
    public Vector2 speed = new Vector2(50, 50);
    public Vector2 rollSpeed = new Vector2(200, 200);
    public float rollDistance = 40;

    public override void _Ready(){
        CurrentState = new Idle(this);
    }

    public override void _PhysicsProcess(float delta){
        CurrentState.Update();
    }

    public void ChangeState(State state){
        if (CurrentState != null){
            CurrentState.OnStateExit();
        }

        CurrentState = state;

        if(CurrentState != null){
            CurrentState.OnStateEnter();
        }
    }
}
