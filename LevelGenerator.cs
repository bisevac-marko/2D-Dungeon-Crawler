using Godot;
using System;
using System.Collections.Generic;

public class LevelGenerator : Node2D
{
    private Room[,] _worldGrid;
    private int gridSizeX = 5;
    private int gridSizeY = 5;
    private List<Vector2> _usedPositions;
    private int _numberOfRooms = 5;
    private RandomNumberGenerator rng;

    public override void _Ready()
    {
        _worldGrid = new Room[gridSizeX * 2, gridSizeY * 2];
        _usedPositions = new List<Vector2>();
        rng = new RandomNumberGenerator();

        //Add starting room;
        _worldGrid[gridSizeX, gridSizeY] = new Room(0, 0);
        _usedPositions.Add(new Vector2(0, 0));

        CreateRooms(_worldGrid, _usedPositions);
        SetRoomDoors(_worldGrid);
        ////Debugging code
        VisualizeRooms();
    }

    private void VisualizeRooms()
    {
        var temp = GD.Load<PackedScene>("res://TestRoom.tscn");
        var label = GD.Load<PackedScene>("res://Label.tscn");
        foreach (Room room in _worldGrid)
        {
            if (room != null)
            {
                Node2D roomPrefab = (Node2D)temp.Instance();
                roomPrefab.GlobalPosition = new Vector2(room.x * 80, room.y * 80);
                AddChild(roomPrefab);
                Label labl = (Label)label.Instance();
                labl.SetGlobalPosition(new Vector2(room.x * 80 - 20, room.y * 80 - 12));
                labl.Text = room.x.ToString() + " " + room.y.ToString();
                AddChild(labl);
                if (room.rightDoor)
                {
                    Sprite door = (Sprite)GD.Load<PackedScene>("res://doorSprite.tscn").Instance();
                    door.GlobalPosition = new Vector2(roomPrefab.GlobalPosition.x + 30, roomPrefab.GlobalPosition.y);
                    AddChild(door);
                }
                if (room.leftDoor)
                {
                    Sprite door = (Sprite)GD.Load<PackedScene>("res://doorSprite.tscn").Instance();
                    door.GlobalPosition = new Vector2(roomPrefab.GlobalPosition.x - 30, roomPrefab.GlobalPosition.y);
                    AddChild(door);
                }
                if (room.downDoor)
                {
                    Sprite door = (Sprite)GD.Load<PackedScene>("res://doorSprite.tscn").Instance();
                    door.GlobalPosition = new Vector2(roomPrefab.GlobalPosition.x, roomPrefab.GlobalPosition.y + 30);
                    AddChild(door);
                }
                if (room.upDoor)
                {
                    Sprite door = (Sprite)GD.Load<PackedScene>("res://doorSprite.tscn").Instance();
                    door.GlobalPosition = new Vector2(roomPrefab.GlobalPosition.x, roomPrefab.GlobalPosition.y - 30);
                    AddChild(door);
                }
            }
        }
    }

    private void SetRoomDoors(Room[,] worldGrid)
    {
        for (int x = 0; x < worldGrid.GetLength(0); x++){
            for (int y = 0; y < worldGrid.GetLength(1); y++){
                if (worldGrid[x, y] == null){
                    continue;
                }
                if (x + 1 < gridSizeX * 2 && worldGrid[x+1, y] != null){
                    worldGrid[x+1, y].leftDoor = true;
                }
                if(x - 1 >= 0 && worldGrid[x-1, y] != null){
                    worldGrid[x-1, y].rightDoor = true;
                }
                if(y + 1 < gridSizeY * 2 && worldGrid[x, y+1] != null){
                    worldGrid[x, y+1].upDoor = true;
                }
                if(y - 1 >= 0 && worldGrid[x, y-1] != null){
                    worldGrid[x, y-1].downDoor = true;
                }
            }
        }
    }

    private void CreateRooms(Room[,] worldGrid, List<Vector2> usedPositions)
    {
        Vector2 checkPosition = new Vector2();
        for(int i = 0; i < _numberOfRooms - 1; i++){
            checkPosition = NewFreePosition(usedPositions);
            rng.Randomize();
            float randomChance = rng.Randf();
            if (NumberOfNeighbours(usedPositions, checkPosition) > 1 && randomChance > .2f){
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
        int breakLimit = 10000;
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
}
