using Godot;

public class Move : State
{
    public Move(Character character) : base(character){

    }

    public override void Update(){
        Player player = (Player)character;
        player.direction = GetDirection();
        player.Velocity = CalculateVelocity(player.Velocity, player.direction, player.speed);
        Vector2 playerPreviousPosition = player.GlobalPosition;
        player.Velocity = player.MoveAndSlide(player.Velocity);

        if(player.Velocity.LengthSquared() == 0){
            player.ChangeState(new Move(character));
        }
        else if(Input.IsActionJustPressed("roll")){
            Vector2 direction = (player.GlobalPosition - playerPreviousPosition).Normalized();
            player.ChangeState(new Roll(character, direction));
        }
    }

    private Vector2 CalculateVelocity(Vector2 velocity, Vector2 direction, Vector2 speed){
        return direction * speed;
    }

    public Vector2 GetDirection(){
        Vector2 direction = new Vector2(Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
                                        Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up"));
        return direction;
    }

}
