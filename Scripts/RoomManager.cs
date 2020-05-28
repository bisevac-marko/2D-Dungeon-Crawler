using Godot;

public class RoomManager : Node2D{

    private Room[,] _worldMap;
    private Room _currentRoom;
    private Camera2D _mainCamera;
    private int _gridSizeX;
    private int _gridSizeY;

    public void InstantiateRooms(Room[,] worldMap, Room startingRoom, Camera2D mainCam, int gridSizeX, int gridSizeY){
        _worldMap = worldMap;
        var roomPrefab = GD.Load<PackedScene>("res://Prefabs/Room.tscn");
        var doorPrefab = GD.Load<PackedScene>("res://Prefabs/Door.tscn");
        Node2D roomScene;
        _currentRoom = startingRoom;
        _mainCamera = mainCam;
        this._gridSizeX = gridSizeX;
        this._gridSizeY = gridSizeY;

        foreach (Room room in worldMap)
        {
            if (room != null)
            {
                Area2D[] doors = room.Doors;
                roomScene = (Node2D)roomPrefab.Instance();
                roomScene.GlobalPosition = new Vector2(room.GlobalX, room.GlobalY);
                AddChild(roomScene);
                int index = 0;

                if (room.RightDoor)
                {
                    Area2D door = (Area2D)doorPrefab.Instance();
                    door.GlobalPosition = new Vector2(room.GlobalX + (room.RoomSizeX / 2), room.GlobalY);
                    AddChild(door);
                    door.Monitoring = false;
                    doors[index] = door;
                    door.Connect("body_entered", this, nameof(OnDoorEntered));
                    index++;
                }
                if (room.LeftDoor)
                {
                    Area2D door = (Area2D)doorPrefab.Instance();
                    door.GlobalPosition = new Vector2(room.GlobalX - (room.RoomSizeX / 2), room.GlobalY);
                    AddChild(door);
                    door.Monitoring = false;
                    doors[index] = door;
                    door.Connect("body_entered", this, nameof(OnDoorEntered));
                    index++;
                }
                if (room.DownDoor)
                {
                    Area2D door = (Area2D)doorPrefab.Instance();
                    door.GlobalPosition = new Vector2(room.GlobalX, room.GlobalY + (room.RoomSizeY / 2) - 20);
                    AddChild(door);
                    door.Monitoring = false;
                    doors[index] = door;
                    door.Connect("body_entered", this, nameof(OnDoorEntered));
                    index++;
                }
                if (room.UpDoor)
                {
                    Area2D door = (Area2D)doorPrefab.Instance();
                    door.GlobalPosition = new Vector2(room.GlobalX, room.GlobalY - (room.RoomSizeY / 2));
                    AddChild(door);
                    door.Monitoring = false;
                    doors[index] = door;
                    door.Connect("body_entered", this, nameof(OnDoorEntered));
                }
            }
        }
        //Open starting room doors
        OpenRoomDoors(startingRoom.Doors);
    }
    public void ConnectDoorSignals(){
        foreach(Room room in _worldMap){
            foreach(Area2D door in room.Doors){
                door.Connect("body_entered", this, nameof(OnDoorEntered));
            }
        }
    }
    private void OnDoorEntered(PhysicsBody2D player){
        Vector2 dir = player.GlobalPosition - new Vector2(_currentRoom.GlobalX, _currentRoom.GlobalY);
        float checkTreshold = 25f;
        GD.Print(dir);

        if(dir.x > checkTreshold){
            //Right door
            TransitionToRoom("RIGHT", player);
        }
        else if (dir.x < -checkTreshold){
            //Left door
            TransitionToRoom("LEFT", player);
        }
        else if(dir.y < -checkTreshold){
            //Down door
            TransitionToRoom("UP", player);
        }
        else if(dir.y > checkTreshold){
            //Up door
            TransitionToRoom("DOWN", player);
        }
    }

    private void TransitionToRoom(string direction, PhysicsBody2D player){
        Room nextRoom = null;
        int x = _currentRoom.X + (_gridSizeX / 2);
        int y = _currentRoom.Y + (_gridSizeY / 2);
        switch(direction){
            case "RIGHT":
                nextRoom = _worldMap[x + 1, y];
                player.GlobalPosition = new Vector2(nextRoom.GlobalX, nextRoom.GlobalY);
                _mainCamera.GlobalPosition = new Vector2(nextRoom.GlobalX, nextRoom.GlobalY);
                break;
            case "LEFT":
                nextRoom = _worldMap[x - 1, y];
                player.GlobalPosition = new Vector2(nextRoom.GlobalX, nextRoom.GlobalY);
                _mainCamera.GlobalPosition = new Vector2(nextRoom.GlobalX, nextRoom.GlobalY);
                break;
            case "DOWN":
                nextRoom = _worldMap[x, y + 1];
                player.GlobalPosition = new Vector2(nextRoom.GlobalX, nextRoom.GlobalY);
                _mainCamera.GlobalPosition = new Vector2(nextRoom.GlobalX, nextRoom.GlobalY);
                break;
            case "UP":
                nextRoom = _worldMap[x, y - 1];
                player.GlobalPosition = new Vector2(nextRoom.GlobalX, nextRoom.GlobalY);
                _mainCamera.GlobalPosition = new Vector2(nextRoom.GlobalX, nextRoom.GlobalY);
                break;
        }
        OpenRoomDoors(nextRoom.Doors);
        _currentRoom = nextRoom;
    }
    public void OpenRoomDoors(Area2D[] doors){
        foreach(Area2D door in doors){
            door.Monitoring = true;
        }
    }
}
