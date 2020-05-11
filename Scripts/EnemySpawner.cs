using Godot;
using System.Collections.Generic;

public class EnemySpawner : Node2D{

    private List<KinematicBody2D> _enemys;
    private PackedScene _enemyScene;
    private RandomNumberGenerator rng;

    public override void _Ready(){
        rng = new RandomNumberGenerator();
        _enemys = new List<KinematicBody2D>();
        _enemyScene = GD.Load<PackedScene>("res://Prefabs/EnemyCharger.tscn");
    }

    public void SpawnEnemys(Room[,] rooms){
        foreach(Room room in rooms){
            if(room == null) continue;
            if(room.type == 0){
                rng.Randomize();
                int numberOfEnemys = rng.RandiRange(3, 7);
                for(int i = 0; i < numberOfEnemys; i++){
                    rng.Randomize();
                    KinematicBody2D enemy = (KinematicBody2D) _enemyScene.Instance();
                    _enemys.Add(enemy);
                    AddChild(enemy);
                    rng.Randomize();
                    enemy.GlobalPosition = new Vector2(rng.RandfRange(room.globalX, room.globalX + rng.RandfRange(-100, 100)), rng.RandfRange(room.globalY, room.globalY + rng.RandfRange(-50, 50)));
                }
            }
        }
    }
}
