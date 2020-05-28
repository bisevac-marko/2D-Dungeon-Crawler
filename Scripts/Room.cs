using Godot;

public class Room{
    public int X;
    public int Y;
    public int Type;
    public bool LeftDoor, RightDoor, UpDoor, DownDoor;
    public float GlobalX;
    public float GlobalY;
    public int RoomSizeX = 200;
    public int RoomSizeY = 120;
    public const float ROOM_OFFSET = 1.2f;
    public Area2D[] Doors;

    public Room(int x, int y, int type){
        this.X = x;
        this.Y = y;
        this.Type = type;
        LeftDoor = false;
        RightDoor = false;
        UpDoor = false;
        DownDoor = false;
        GlobalX = RoomSizeX * x;
        GlobalY = RoomSizeY * y;
    }

}
