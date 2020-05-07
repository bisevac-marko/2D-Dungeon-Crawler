using Godot;
using System.Collections.Generic;

public class Game : Node2D
{
    private int gridSizeX = 5; //half extends
    private int gridSizeY = 5; //half extends
    private LevelGenerator _levelGenerator;
    private Room[,] _worldGrid;
    private KinematicBody2D _player;
    private Room _currentRoom;
    private Camera2D _mainCam;
    private Dictionary<Room, List<Area2D>> _doorMap;

    public override void _Ready()
    {
        _levelGenerator = GetNode<LevelGenerator>("Level Generator");
        _mainCam = GetNode<Camera2D>("Camera2D");
        _player = (KinematicBody2D)GD.Load<PackedScene>("res://Prefabs/Player.tscn").Instance();
        _worldGrid = _levelGenerator.GenerateLevel(gridSizeX, gridSizeY);
        _currentRoom = _worldGrid[gridSizeX, gridSizeY]; //Starting room
        _doorMap = _levelGenerator.GetDoorMap();
        OpenRoomDoors(_doorMap[_currentRoom]);
        ConnectDoorSignals(_doorMap);
        AddChild(_player);
        _player.GlobalPosition = new Vector2(_levelGenerator.GetStartingRoom().globalX, _levelGenerator.GetStartingRoom().globalY);
    }

    private void OpenRoomDoors(List<Area2D> roomDoors)
    {
        foreach (Area2D door in roomDoors)
        {
            door.Monitoring = true;
        }
    }

    private void ConnectDoorSignals(Dictionary<Room, List<Area2D>> doorMap){
        foreach(List<Area2D> areas in doorMap.Values){
            foreach(Area2D area in areas){
                area.Connect("body_entered", this, nameof(OnDoorEntered));
            }
        }
    }

    public void OnDoorEntered(PhysicsBody2D body2D){
        int x = _currentRoom.x + gridSizeX;
        int y = _currentRoom.y + gridSizeY;
        float playerX = _player.GlobalPosition.x;
        float playerY = _player.GlobalPosition.y;

        float xDir = _currentRoom.globalX - playerX;
        float yDir = _currentRoom.globalY - playerY;
        float treshold = 20f; 

        if(xDir > treshold){
            //LEFT
            TransitionToRoom(x - 1, y);
        }
        else if(xDir < -treshold){
            //RIGHT
            TransitionToRoom(x + 1, y);
        }
        else if(yDir > treshold){
            //UP
            TransitionToRoom(x, y - 1);
        }
        else if(yDir < -treshold){
            //DOWN
            TransitionToRoom(x, y + 1);
        }
    }

    private void TransitionToRoom(int x, int y)
    {
        _mainCam.GlobalPosition = new Vector2(_worldGrid[x, y].globalX, _worldGrid[x, y].globalY);
        _player.GlobalPosition = new Vector2(_worldGrid[x, y].globalX, _worldGrid[x, y].globalY);
        _currentRoom = _worldGrid[x, y];
        OpenRoomDoors(_doorMap[_currentRoom]);
    }
}
