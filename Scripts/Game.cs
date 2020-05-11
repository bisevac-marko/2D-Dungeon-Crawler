using Godot;
using System.Collections.Generic;

public class Game : Node2D
{
    private int gridSizeX = 5; //half extends
    private int gridSizeY = 5; //half extends
    private LevelGenerator _levelGenerator;
    private EnemySpawner _enemySpawner;
    private RoomManager _roomManager;
    private Room[,] _worldMap;
    private KinematicBody2D _player;
    private Camera2D _mainCam;
    private Dictionary<Room, List<Area2D>> _doorMap;


    public override void _Ready()
    {
        _roomManager = (RoomManager)GetNode("Room Manager");
        _levelGenerator = new LevelGenerator();
        _enemySpawner = GetNode<EnemySpawner>("Enemy Spawner");
        _mainCam = GetNode<Camera2D>("Camera2D");
        _player = (KinematicBody2D)GD.Load<PackedScene>("res://Prefabs/Player.tscn").Instance();
        //Generate Level
        _worldMap = _levelGenerator.GenerateLevel(gridSizeX, gridSizeY, _roomManager);
        //Instantiate rooms
        _roomManager.InstantiateRooms(_worldMap);
        //Connect door signals
        ConnectDoorSignals(_doorMap);
        //Open starting room doors
        OpenRoomDoors(_doorMap[_currentRoom]);
        _enemySpawner.SpawnEnemys(_worldMap);
        //Spawn player
        AddChild(_player);
        _player.GlobalPosition = new Vector2(_levelGenerator.GetStartingRoom().globalX, _levelGenerator.GetStartingRoom().globalY);
        _mainCam.GlobalPosition = new Vector2(_levelGenerator.GetStartingRoom().globalX, _levelGenerator.GetStartingRoom().globalY);
    }

}
