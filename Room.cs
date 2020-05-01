public class Room{
    public int x;
    public int y;
    public Type type;
    public bool leftDoor, rightDoor, upDoor, downDoor;

    public Room(int x, int y){
        this.x = x;
        this.y = y;
        leftDoor = false;
        rightDoor = false;
        upDoor = false;
        downDoor = false;
    }

    public enum Type{
        Normal,
        Starting,
        Boss
    }
}
