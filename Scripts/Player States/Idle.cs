using Godot;

public class Idle : State{
    public Idle(Character character) : base(character){
    }

    public override void Update(){
        Player player = (Player)character;
        if (IsPressingMovementKeys()){
            player.ChangeState(new Move(character));
        }
    }

    private bool IsPressingMovementKeys(){
        return (Input.GetActionStrength("move_left") - Input.GetActionStrength("move_right")) != 0
            || (Input.GetActionStrength("move_up") - Input.GetActionStrength("move_down")) != 0;
    }

}
