using Godot;

public class Roll : State
{
    private Vector2 direction;
    private Vector2 startingPosition;
    private Player player;

    public Roll(Character character, Vector2 direction) : base(character){
        this.direction = direction;
        player = (Player)character;
        startingPosition = player.GlobalPosition;
    }

    public override void Update(){
        player.Velocity = CalculateVelocity(player.Velocity, player.rollSpeed, direction);
        player.Velocity = player.MoveAndSlide(player.Velocity);
        float distanceCovered = (player.GlobalPosition - startingPosition).Length();
        if (distanceCovered >= player.rollDistance || player.Velocity == Vector2.Zero){
            player.ChangeState(new Idle(character));
        }
    }

    private Vector2 CalculateVelocity(Vector2 velocity, Vector2 speed, Vector2 dir){
        Vector2 output = speed * dir;
        return output;
    }
}
