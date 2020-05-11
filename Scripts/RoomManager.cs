using Godot;

public class RoomManager : Node2D{

    private Room[,] _worldMap;

    public void InstantiateRooms(Room[,] worldMap){
        _worldMap = worldMap;
        var roomPrefab = GD.Load<PackedScene>("res://Prefabs/Room.tscn");
        var doorPrefab = GD.Load<PackedScene>("res://Prefabs/Door.tscn");
        Node2D roomScene;

        foreach (Room room in worldMap)
        {
            if (room != null)
            {
                Area2D[] doors = room.doors;
                roomScene = (Node2D)roomPrefab.Instance();
                roomScene.GlobalPosition = new Vector2(room.globalX, room.globalY);
                AddChild(roomScene);

                if (room.rightDoor)
                {
                    Area2D door = (Area2D)doorPrefab.Instance();
                    door.GlobalPosition = new Vector2(room.globalX + (room.roomSizeX / 2), room.globalY);
                    AddChild(door);
                    door.Monitoring = false;
                    doors[0] = door;
                }
                if (room.leftDoor)
                {
                    Area2D door = (Area2D)doorPrefab.Instance();
                    door.GlobalPosition = new Vector2(room.globalX - (room.roomSizeX / 2), room.globalY);
                    AddChild(door);
                    door.Monitoring = false;
                    doors[1] = door;
                }
                if (room.downDoor)
                {
                    Area2D door = (Area2D)doorPrefab.Instance();
                    door.GlobalPosition = new Vector2(room.globalX, room.globalY + (room.roomSizeY / 2) - 20);
                    AddChild(door);
                    door.Monitoring = false;
                    doors[2] = door;
                }
                if (room.upDoor)
                {
                    Area2D door = (Area2D)doorPrefab.Instance();
                    door.GlobalPosition = new Vector2(room.globalX, room.globalY - (room.roomSizeY / 2));
                    AddChild(door);
                    door.Monitoring = false;
                    doors[3] = door;
                }
            }
        }
    }
    public void ConnectDoorSignals(){
        foreach(Room room in _worldMap){
            foreach(Area2D door in room.doors){
                door.Connect("body_entered", this, nameof(OnDoorEntered));
            }
        }
    }
    private void OnDoorEntered(PhysicsBody2D body2D){
        
    }
}
