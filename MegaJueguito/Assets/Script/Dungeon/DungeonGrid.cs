using System;
using System.Collections.Generic;
using UnityEngine;

public static class DungeonRoomKeys
{
    public const string Start = "I";
    public const string Basic = "C";
}

/// <summary>Una celda ocupada: key del prefab, usos de esa key y número global de sala.</summary>
public struct DungeonCell
{
    public string PrefabKey { get; private set; }
    public int PrefabUseNumber { get; private set; }
    public int RoomNumber { get; private set; }
    public bool IsEmpty { get { return RoomNumber == 0; } }

    public DungeonCell(string prefabKey, int prefabUseNumber, int roomNumber)
    {
        PrefabKey = prefabKey;
        PrefabUseNumber = prefabUseNumber;
        RoomNumber = roomNumber;
    }

    public static DungeonCell Empty
    {
        get { return new DungeonCell("", 0, 0); }
    }

    public override string ToString()
    {
        return IsEmpty ? "00" : PrefabKey + PrefabUseNumber + "," + RoomNumber;
    }
}

/// <summary>
/// Guarda el contenido de cada celda. Dos salas son vecinas si sus celdas
/// están una junto a otra en horizontal o vertical.
/// </summary>
public class DungeonGrid
{
    private readonly DungeonCell[,] cells;

    public int Width { get; private set; }
    public int Height { get; private set; }
    public int RoomCount { get; private set; }

    public DungeonGrid(int width, int height)
    {
        if (width <= 0) throw new ArgumentOutOfRangeException("width", "La cuadrícula debe tener al menos una columna.");
        if (height <= 0) throw new ArgumentOutOfRangeException("height", "La cuadrícula debe tener al menos una fila.");

        Width = width;
        Height = height;
        cells = new DungeonCell[width, height];
    }

    public bool IsInside(Vector2Int position)
    {
        return position.x >= 0 && position.y >= 0 && position.x < Width && position.y < Height;
    }

    public bool HasRoom(Vector2Int position)
    {
        return IsInside(position) && !cells[position.x, position.y].IsEmpty;
    }

    public DungeonCell GetCell(Vector2Int position)
    {
        if (!IsInside(position))
            throw new ArgumentOutOfRangeException("position", "La posición está fuera de la cuadrícula.");

        return cells[position.x, position.y];
    }

    /// <summary>
    /// Añade el inicio I o una sala básica C. El contador tras la key indica
    /// cuántas veces se ha usado ese prefab; RoomNumber es el número global.
    /// </summary>
    public bool TryAddRoom(Vector2Int position, string prefabKey)
    {
        if (!IsInside(position) || HasRoom(position)) return false;
        if (RoomCount == 0 && prefabKey != DungeonRoomKeys.Start) return false;
        if (RoomCount > 0 && prefabKey != DungeonRoomKeys.Basic) return false;

        int prefabUseNumber = CountUses(prefabKey) + 1;
        int roomNumber = RoomCount + 1;
        cells[position.x, position.y] = new DungeonCell(prefabKey, prefabUseNumber, roomNumber);
        RoomCount++;
        return true;
    }

    /// <summary>
    /// Devuelve celdas vacías que tocan alguna sala. Son las candidatas para
    /// añadir una nueva sala conectada al mapa existente.
    /// </summary>
    public List<Vector2Int> GetEmptyNeighborsOfRooms()
    {
        List<Vector2Int> candidates = new List<Vector2Int>();
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (cells[x, y].IsEmpty) continue;

                Vector2Int roomPosition = new Vector2Int(x, y);
                foreach (Vector2Int direction in directions)
                {
                    Vector2Int neighbor = roomPosition + direction;
                    if (IsInside(neighbor) && !HasRoom(neighbor) && !candidates.Contains(neighbor))
                        candidates.Add(neighbor);
                }
            }
        }

        return candidates;
    }

    /// <summary>
    /// Calcula la distancia por conexiones desde una sala. Estos valores son
    /// independientes de la key, del contador de uso y del número de sala.
    /// </summary>
    public int[,] GetDistancesFrom(Vector2Int origin)
    {
        if (!HasRoom(origin))
            throw new ArgumentException("El origen debe ser una celda que contenga una sala.", "origin");

        int[,] distances = new int[Width, Height];
        Queue<Vector2Int> pending = new Queue<Vector2Int>();
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

        distances[origin.x, origin.y] = 1;
        pending.Enqueue(origin);

        while (pending.Count > 0)
        {
            Vector2Int current = pending.Dequeue();
            foreach (Vector2Int direction in directions)
            {
                Vector2Int neighbor = current + direction;
                if (HasRoom(neighbor) && distances[neighbor.x, neighbor.y] == 0)
                {
                    distances[neighbor.x, neighbor.y] = distances[current.x, current.y] + 1;
                    pending.Enqueue(neighbor);
                }
            }
        }

        return distances;
    }

    private int CountUses(string prefabKey)
    {
        int uses = 0;
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                if (!cells[x, y].IsEmpty && cells[x, y].PrefabKey == prefabKey)
                    uses++;

        return uses;
    }
}
