using Godot;
using System.Collections.Generic;

public class LevelGenerator 
{
    private Room[,] _worldGrid;
    private int gridSizeX = 5;
    private int gridSizeY = 5;
    private List<Vector2> _usedPositions;
    private int _numberOfRooms = 8;
    private RandomNumberGenerator rng;
    private RoomManager _roomManager;
    private Room _startingRoom;
    private Room _bossRoom;

    public Room[,] GenerateLevel(int gridSizeX, int gridSizeY, RoomManager roomManager)
    {
        this.gridSizeX = gridSizeX;
        this.gridSizeY = gridSizeY;
        _worldGrid = new Room[gridSizeX * 2, gridSizeY * 2];
        _usedPositions = new List<Vector2>();
        rng = new RandomNumberGenerator();
        _roomManager = roomManager;

        _worldGrid[gridSizeX, gridSizeY] = new Room(0, 0, 1);
        _usedPositions.Add(new Vector2(0, 0));

        CreateRoomMap(_worldGrid, _usedPositions);
        SetRoomDoors(_worldGrid);
        SetRoomType(_worldGrid);

        return _worldGrid;
    }
    private void CreateRoomMap(Room[,] worldGrid, List<Vector2> usedPositions)
    {
        Vector2 checkPosition = new Vector2();
        for(int i = 0; i < _numberOfRooms - 1; i++){
            checkPosition = NewFreePosition(usedPositions);
            rng.Randomize();
            float randomChance = rng.Randf();
            if (NumberOfNeighbours(usedPositions, checkPosition) > 1){
                int iterations = 0;
                do{
                  checkPosition = NewFreePosition(usedPositions);
                  iterations++;
                  }while(NumberOfNeighbours(usedPositions, checkPosition) > 1 && iterations < 1000);
                if (iterations >= 1000)
                  GD.Print("Could not create with fewer neighbors than : " + NumberOfNeighbours(usedPositions, checkPosition));
            }

            int x = Mathf.RoundToInt(checkPosition.x);
            int y = Mathf.RoundToInt(checkPosition.y);

            worldGrid[x + gridSizeX, y + gridSizeY] = new Room(x, y, 0);
            usedPositions.Add(new Vector2(x, y));
        }
    }
    private void SetRoomDoors(Room[,] worldGrid)
    {
        int numberOfDoors = 0;
        for (int x = 0; x < worldGrid.GetLength(0); x++){
            for (int y = 0; y < worldGrid.GetLength(1); y++){
                if (worldGrid[x, y] == null){
                    continue;
                }
                if (x + 1 < gridSizeX * 2 && worldGrid[x+1, y] != null){
                    worldGrid[x+1, y].leftDoor = true;
                    numberOfDoors++;
                }
                if(x - 1 >= 0 && worldGrid[x-1, y] != null){
                    worldGrid[x-1, y].rightDoor = true;
                    numberOfDoors++;
                }
                if(y + 1 < gridSizeY * 2 && worldGrid[x, y+1] != null){
                    worldGrid[x, y+1].upDoor = true;
                    numberOfDoors++;
                }
                if(y - 1 >= 0 && worldGrid[x, y-1] != null){
                    worldGrid[x, y-1].downDoor = true;
                    numberOfDoors++;
                }
                //initialize door array
                worldGrid[x, y].doors = new Area2D[numberOfDoors];
            }
        }
    }

    private int NumberOfNeighbours(List<Vector2> usedPos, Vector2 checkPosition)
    {
        int count = 0;
        if (usedPos.Contains(checkPosition + Vector2.Right)){
            count++;
        }
        if (usedPos.Contains(checkPosition + Vector2.Left)){
            count++;
        }
        if (usedPos.Contains(checkPosition + Vector2.Up)){
            count++;
        }
        if (usedPos.Contains(checkPosition + Vector2.Down)){
            count++;
        }
        return count;
    }

    private Vector2 NewFreePosition(List<Vector2> usedPositions)
    {
        int breakLimit = 1000;
        int iterations = 0;
        Vector2 checkPos = new Vector2();
        int x = 0;
        int y = 0;
        do{
            rng.Randomize();
            int randIndex = rng.RandiRange(0, usedPositions.Count - 1);
            x = Mathf.RoundToInt(usedPositions[randIndex].x);
            y = Mathf.RoundToInt(usedPositions[randIndex].y);

            rng.Randomize();
            bool vertical = rng.Randf() > .5f;
            rng.Randomize();
            bool positive = rng.Randf() > .5f;

            if (vertical){
                if(positive){
                    y += 1;
                }
                else{
                    y -= 1;
                }
            }
            else{
                if(positive){
                    x += 1;
                }
                else{
                    x -= 1;
                }
            }
            checkPos = new Vector2(x, y);
            iterations++;
            if(iterations >= breakLimit){
                GD.Print("Error: Could not find new free position");
                break;
            }
        }while(usedPositions.Contains(checkPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        return checkPos;
    }

    private void SetRoomType(Room[,] rooms){
        Room checkRoom = rooms[gridSizeX, gridSizeY];
        float currentMaxDistance = 0;
        foreach(Room room in rooms){
            if(room != null){
                float checkDistance = (new Vector2(room.globalX, room.globalY) - new Vector2(checkRoom.globalX, checkRoom.globalY)).LengthSquared();
                if(currentMaxDistance < checkDistance){
                    currentMaxDistance = checkDistance;
                    _startingRoom = room;
                }
            }
        }
        _startingRoom.type = 1;
        currentMaxDistance = 0;
        foreach(Room room in rooms){
            if(room != null){
                float distance = (new Vector2(_startingRoom.globalX, _startingRoom.globalY) - new Vector2(room.globalX, room.globalY)).LengthSquared();
                if (distance > currentMaxDistance){
                    currentMaxDistance = distance;
                    _bossRoom = room;
                }
            }
        }
        _bossRoom.type = 2;
    }
    public Room GetStartingRoom(){
        return this._startingRoom;
    }
}
