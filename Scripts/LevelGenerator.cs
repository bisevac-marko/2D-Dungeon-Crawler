using Godot;
using System.Collections.Generic;

public class LevelGenerator{
    private Room[,] _worldGrid;
    private int gridSizeX;
    private int gridSizeY;
    private List<Vector2> _usedPositions;
    private int _numberOfRooms = 8;
    private RandomNumberGenerator _rng;
    private RoomManager _roomManager;
    private Room _startingRoom;
    private Room _bossRoom;

    public Room[,] GenerateLevel(int gridSizeX, int gridSizeY, RoomManager roomManager){
        this.gridSizeX = gridSizeX;
        this.gridSizeY = gridSizeY;
        _worldGrid = new Room[gridSizeX, gridSizeY];
        _usedPositions = new List<Vector2>();
        _rng = new RandomNumberGenerator();
        _roomManager = roomManager;
        //Center of the array
        _worldGrid[gridSizeX / 2, gridSizeY / 2] = new Room(0, 0, 0);
        _usedPositions.Add(new Vector2(0, 0));

        CreateRoomMap(_worldGrid, _usedPositions);
        SetRoomDoors(_worldGrid);
        SetRoomType(_worldGrid);

        return _worldGrid;
    }
    private void CreateRoomMap(Room[,] worldGrid, List<Vector2> usedPositions){
        Vector2 checkPosition = new Vector2();
        for(int i = 0; i < _numberOfRooms - 1; i++){
            checkPosition = NewFreePosition(usedPositions);
            _rng.Randomize();
            float randomChance = _rng.Randf();
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

            worldGrid[x + (gridSizeX / 2), y + (gridSizeY / 2)] = new Room(x, y, 0);
            usedPositions.Add(new Vector2(x, y));
        }
    }
    private void SetRoomDoors(Room[,] worldGrid){
        for (int x = 0; x < worldGrid.GetLength(0); x++){
            for (int y = 0; y < worldGrid.GetLength(1); y++){
                if (worldGrid[x, y] == null){
                    continue;
                }
                int numberOfDoors = 0;
                if (x + 1 < gridSizeX && worldGrid[x+1, y] != null){
                    worldGrid[x, y].RightDoor = true;
                    numberOfDoors++;
                }
                if(x - 1 >= 0 && worldGrid[x-1, y] != null){
                    worldGrid[x, y].LeftDoor = true;
                    numberOfDoors++;
                }
                if(y + 1 < gridSizeY && worldGrid[x, y+1] != null){
                    worldGrid[x, y].DownDoor = true;
                    numberOfDoors++;
                }
                if(y - 1 >= 0 && worldGrid[x, y-1] != null){
                    worldGrid[x, y].UpDoor = true;
                    numberOfDoors++;
                }
                //initialize door array
                worldGrid[x, y].Doors = new Area2D[numberOfDoors];
            }
        }
    }

    private int NumberOfNeighbours(List<Vector2> usedPos, Vector2 checkPosition){
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

    private Vector2 NewFreePosition(List<Vector2> usedPositions){
        int breakLimit = 1000;
        int iterations = 0;
        Vector2 checkPos = new Vector2();
        int x = 0;
        int y = 0;
        do{
            _rng.Randomize();
            int randIndex = _rng.RandiRange(0, usedPositions.Count - 1);
            x = Mathf.RoundToInt(usedPositions[randIndex].x);
            y = Mathf.RoundToInt(usedPositions[randIndex].y);

            _rng.Randomize();
            bool vertical = _rng.Randf() > .5f;
            _rng.Randomize();
            bool positive = _rng.Randf() > .5f;

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
        }while(usedPositions.Contains(checkPos) || x >= gridSizeX / 2|| x < 0 || y >= gridSizeY / 2|| y < 0);
        return checkPos;
    }

    private void SetRoomType(Room[,] rooms){
        Room checkRoom = rooms[(gridSizeX / 2), (gridSizeY / 2)];
        float currentMaxDistance = 0;
        foreach(Room room in rooms){
            if(room != null){
                float checkDistance = (new Vector2(room.GlobalX, room.GlobalY) - new Vector2(checkRoom.GlobalX, checkRoom.GlobalY)).LengthSquared();
                if(currentMaxDistance < checkDistance){
                    currentMaxDistance = checkDistance;
                    _startingRoom = room;
                }
            }
        }
        _startingRoom.Type = 1;
        currentMaxDistance = 0;
        foreach(Room room in rooms){
            if(room != null){
                float distance = (new Vector2(_startingRoom.GlobalX, _startingRoom.GlobalY) - new Vector2(room.GlobalX, room.GlobalY)).LengthSquared();
                if (distance > currentMaxDistance){
                    currentMaxDistance = distance;
                    _bossRoom = room;
                }
            }
        }
        _bossRoom.Type = 2;
    }
    public Room GetStartingRoom(){
        return this._startingRoom;
    }
}
