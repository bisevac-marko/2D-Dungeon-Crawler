using Godot;
using System;

public class LevelDrawer : Node
{
    private const int ROOM_SIZE_X = 200;
    private const int ROOM_SIZE_Y = 120;
    public void DrawLevel(Room[,] worldGrid){
        var temp = GD.Load<PackedScene>("res://Prefabs/Room.tscn");
        var doorPrefab = GD.Load<PackedScene>("res://Prefabs/Door.tscn");
        Node2D roomPrefab;

        foreach (Room room in worldGrid)
        {
            if (room != null)
            {
                roomPrefab = (Node2D)temp.Instance();
                roomPrefab.GlobalPosition = new Vector2(room.globalX, room.globalY);
                AddChild(roomPrefab);

                if (room.rightDoor)
                {
                    Sprite door = (Sprite)doorPrefab.Instance();
                    door.GlobalPosition = new Vector2(roomPrefab.GlobalPosition.x + (ROOM_SIZE_X / 2), roomPrefab.GlobalPosition.y);
                    AddChild(door);
                }
                if (room.leftDoor)
                {
                    Sprite door = (Sprite)doorPrefab.Instance();
                    door.GlobalPosition = new Vector2(roomPrefab.GlobalPosition.x - (ROOM_SIZE_X / 2), roomPrefab.GlobalPosition.y);
                    AddChild(door);
                }
                if (room.downDoor)
                {
                    Sprite door = (Sprite)doorPrefab.Instance();
                    door.GlobalPosition = new Vector2(roomPrefab.GlobalPosition.x, roomPrefab.GlobalPosition.y + (ROOM_SIZE_Y / 2));
                    AddChild(door);
                }
                if (room.upDoor)
                {
                    Sprite door = (Sprite)doorPrefab.Instance();
                    door.GlobalPosition = new Vector2(roomPrefab.GlobalPosition.x, roomPrefab.GlobalPosition.y - (ROOM_SIZE_Y / 2));
                    AddChild(door);
                }
            }
        }
    }
}
