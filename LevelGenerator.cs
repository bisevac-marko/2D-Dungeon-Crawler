using Godot;
using System;
using System.Collections.Generic;

public class LevelGenerator : Node2D
{
    private Room[,] _worldGrid;
    private int gridSizeX = 3;
    private int gridSizeY = 3;
    private List<Vector2> _usedPositions;
    private int numberOfRooms = 5;
    private RandomNumberGenerator rng;

    public override void _Ready(){
        _worldGrid = new Room[gridSizeX * 2, gridSizeY * 2];
        _usedPositions = new List<Vector2>();
        rng = new RandomNumberGenerator();

        //Add starting room;
        _worldGrid[gridSizeX, gridSizeY] = new Room(0, 0);
        _usedPositions.Add(new Vector2(0, 0));

        CreateRooms(_worldGrid, _usedPositions);

        var temp = GD.Load<PackedScene>("res://TestRoom.tscn");
        
        ////Debugging
        foreach(Room room in _worldGrid){
            if (room != null){
                Node2D roomPrefab = (Node2D)temp.Instance();
                roomPrefab.GlobalPosition = new Vector2(room.x * 60, room.y * 60);
                AddChild(roomPrefab);
            }
        }
    }

    private void CreateRooms(Room[,] worldGrid, List<Vector2> usedPositions)
    {
        Vector2 checkPosition = new Vector2();
        float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
        for(int i = 0; i < numberOfRooms - 1; i++){
            float randomPerc = ((float) i) / (((float)numberOfRooms - 1));
			randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
            checkPosition = NewFreePosition(usedPositions);
            rng.Randomize();
            if (NumberOfNeighbours(usedPositions, checkPosition) > 1 && rng.Randf() > randomPerc){
                int iterations = 0;
                do{
					checkPosition = NewFreePosition(usedPositions);
					iterations++;
				}while(NumberOfNeighbours(usedPositions, checkPosition) > 1 && iterations < 100);
				if (iterations >= 100)
					GD.Print("Could not create with fewer neighbors than : " + NumberOfNeighbours(usedPositions, checkPosition));
            }

            int x = Mathf.RoundToInt(checkPosition.x);
            int y = Mathf.RoundToInt(checkPosition.y);

            worldGrid[x + gridSizeX, y + gridSizeY] = new Room(x, y);
            usedPositions.Add(new Vector2(x, y));
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
        int breakLimit = 1000000;
        int index = 0;
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
            index++;
            if(index >= breakLimit){
                GD.Print("Could not find new free position");
                break;
            }
        }while(NumberOfNeighbours(usedPositions, checkPos) > 1 || usedPositions.Contains(checkPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);

        return checkPos;
    }
}
