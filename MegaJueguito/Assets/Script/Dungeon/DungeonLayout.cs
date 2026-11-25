using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DungeonRoomPlacement
{
    public Vector2Int gridPosition;
    public int distanceFromStart;
    public int distanceFromBoss;
    public string roomKey;
    public DungeonRoomRole role;
    public DungeonDoor doors;
    public GameObject instance;
}

public class DungeonLayout
{
    public int[,] DistanceGrid { get; private set; }
    public IReadOnlyList<DungeonRoomPlacement> Rooms { get; private set; }
    public Vector2Int StartPosition { get; private set; }
    public Vector2Int BossPosition { get; private set; }

    public DungeonLayout(int[,] distanceGrid, List<DungeonRoomPlacement> rooms, Vector2Int start, Vector2Int boss)
    {
        DistanceGrid = distanceGrid;
        Rooms = rooms.AsReadOnly();
        StartPosition = start;
        BossPosition = boss;
    }
}
