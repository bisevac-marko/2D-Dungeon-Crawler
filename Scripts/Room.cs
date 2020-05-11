using Godot;

public class Room{
    public int x;
    public int y;
    public int type;
    public bool leftDoor, rightDoor, upDoor, downDoor;
    public float globalX;
    public float globalY;
    public int roomSizeX = 200;
    public int roomSizeY = 120;
    public const float ROOM_OFFSET = 1.2f;
    public Area2D[] doors;

    public Room(int x, int y, int type){
        this.x = x;
        this.y = y;
        leftDoor = false;
        rightDoor = false;
        upDoor = false;
        downDoor = false;
        this.type = type;
        globalX = roomSizeX * x * ROOM_OFFSET;
        globalY = roomSizeY * y * ROOM_OFFSET;
    }

}
