using Godot;

public class Game : Node2D{

    private int _gridSizeX = 10; //use only even number
    private int _gridSizeY = 10;
    private LevelGenerator _levelGenerator;
    private EnemySpawner _enemySpawner;
    private RoomManager _roomManager;
    private Room[,] _worldMap;
    private KinematicBody2D _player;
    private Camera2D _mainCam;

    public override void _Ready(){
        _roomManager = (RoomManager)GetNode("Room Manager");
        _levelGenerator = new LevelGenerator();
        _enemySpawner = GetNode<EnemySpawner>("Enemy Spawner");
        _mainCam = GetNode<Camera2D>("Camera2D");
        _player = (KinematicBody2D)GD.Load<PackedScene>("res://Prefabs/Player.tscn").Instance();
        //Generate Level
        _worldMap = _levelGenerator.GenerateLevel(_gridSizeX, _gridSizeY, _roomManager);
        //Instantiate rooms in scene
        _roomManager.InstantiateRooms(_worldMap, _levelGenerator.GetStartingRoom(), _mainCam, _gridSizeX, _gridSizeY);
        //Open starting room doors
        _enemySpawner.SpawnEnemys(_worldMap);
        //Spawn player
        AddChild(_player);
        _player.GlobalPosition = new Vector2(_levelGenerator.GetStartingRoom().GlobalX, _levelGenerator.GetStartingRoom().GlobalY);
        _mainCam.GlobalPosition = new Vector2(_levelGenerator.GetStartingRoom().GlobalX, _levelGenerator.GetStartingRoom().GlobalY);
    }

}
