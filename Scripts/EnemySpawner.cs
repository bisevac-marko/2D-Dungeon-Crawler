using Godot;
using System.Collections.Generic;

public class EnemySpawner : Node2D{

    private List<KinematicBody2D> _enemys;
    private PackedScene _enemyScene;
    private RandomNumberGenerator _rng;

    public override void _Ready(){
         _rng = new RandomNumberGenerator();
        _enemys = new List<KinematicBody2D>();
        _enemyScene = GD.Load<PackedScene>("res://Prefabs/EnemyCharger.tscn");
   }

    public void SpawnEnemys(Room[,] rooms){
        foreach(Room room in rooms){
            if(room == null) continue;
            if(room.Type == 0){
                _rng.Randomize();
                int numberOfEnemys = _rng.RandiRange(3, 7);
                for(int i = 0; i < numberOfEnemys; i++){
                    _rng.Randomize();
                    KinematicBody2D enemy = (KinematicBody2D) _enemyScene.Instance();
                    _enemys.Add(enemy);
                    AddChild(enemy);
                    _rng.Randomize();
                    enemy.GlobalPosition = new Vector2(_rng.RandfRange(room.GlobalX, room.GlobalX + _rng.RandfRange(-100, 100)), _rng.RandfRange(room.GlobalY, room.GlobalY + _rng.RandfRange(-50, 50)));
                }
            }
        }
    }
}
