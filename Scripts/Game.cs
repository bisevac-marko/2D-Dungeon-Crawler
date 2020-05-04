using Godot;
using System;

public class Game : Node2D
{
    private int gridSizeX = 5; //half extends
    private int gridSizeY = 5; //half extends
    private LevelGenerator _levelGenerator;
    private LevelDrawer _levelDrawer;
    private Room[,] _worldGrid;
    private KinematicBody2D _player;
    private float playerX;
    private float playerY;
    private Vector2 _currentRoom;
    private Camera2D _mainCam;
    public override void _Ready()
    {
        _levelGenerator = GetNode<LevelGenerator>("Level Generator");
        _levelDrawer = GetNode<LevelDrawer>("Level Drawer");
        _mainCam = GetNode<Camera2D>("Camera2D");
        _player = (KinematicBody2D)GD.Load<PackedScene>("res://Prefabs/Player.tscn").Instance();
        _currentRoom = new Vector2(gridSizeX, gridSizeY); //Starting room 

        _worldGrid = _levelGenerator.GenerateLevel(gridSizeX, gridSizeY);
        _levelDrawer.DrawLevel(_worldGrid);
        AddChild(_player);
        AddDoorTriggers();
    }

    private void AddDoorTriggers()
    {
        var doorScene = GD.Load<PackedScene>("res://Prefabs/DoorTrigger.tscn");
        foreach (Room room in _worldGrid)
        {
            if (room != null)
            {
                if (room.rightDoor)
                {
                    Area2D doorTrigger = (Area2D)doorScene.Instance();
                    doorTrigger.GlobalPosition = new Vector2(room.globalX + (room.roomSizeX / 2), room.globalY);
                    AddChild(doorTrigger);
                    doorTrigger.Connect("body_entered", this, nameof(OnDoorEntered));
                }
                if (room.leftDoor)
                {
                    Area2D doorTrigger = (Area2D)doorScene.Instance();
                    doorTrigger.GlobalPosition = new Vector2(room.globalX - (room.roomSizeX / 2), room.globalY);
                    AddChild(doorTrigger);
                    doorTrigger.Connect("body_entered", this, nameof(OnDoorEntered));
                }
                if (room.downDoor)
                {
                    Area2D doorTrigger = (Area2D)doorScene.Instance();
                    doorTrigger.GlobalPosition = new Vector2(room.globalX, room.globalY + (room.roomSizeY / 2));
                    AddChild(doorTrigger);
                    doorTrigger.Connect("body_entered", this, nameof(OnDoorEntered));
                }
                if (room.upDoor)
                {
                    Area2D doorTrigger = (Area2D)doorScene.Instance();
                    doorTrigger.GlobalPosition = new Vector2(room.globalX, room.globalY - (room.roomSizeY / 2));
                    AddChild(doorTrigger);
                    doorTrigger.Connect("body_entered", this, nameof(OnDoorEntered));
                }
            }
        }
    }

    public void OnDoorEntered(Node node){
        int x = (int)_currentRoom.x;
        int y = (int)_currentRoom.y;
        playerX = _player.GlobalPosition.x;
        playerY = _player.GlobalPosition.y;

        Vector2 currentRoomGlobalPosition = new Vector2(_worldGrid[x, y].globalX, _worldGrid[x, y].globalY);
        float xDir = currentRoomGlobalPosition.x - playerX;
        float yDir = currentRoomGlobalPosition.y - playerY;

        GD.Print(x + " " + y);
        if(xDir > 20){
            //LEFT
            TransitionToRoom(x - 1, y);
        }
        else if(xDir < -20){
            //RIGHT
            TransitionToRoom(x + 1, y);
        }
        else if(yDir > 20){
            //UP
            TransitionToRoom(x, y - 1);
        }
        else if(yDir < -20){
            //DOWN
            TransitionToRoom(x, y + 1);
        }
    }

    private void TransitionToRoom(int x, int y)
    {
        _mainCam.GlobalPosition = new Vector2(_worldGrid[x, y].globalX, _worldGrid[x, y].globalY);
        _player.GlobalPosition = new Vector2(_worldGrid[x, y].globalX, _worldGrid[x, y].globalY);
        _currentRoom = new Vector2(x, y);
    }
}
