using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Room Table")]
public class DungeonRoomTable : ScriptableObject
{
    [Tooltip("Añade aquí hasta cinco variantes por rol si ese es el tamaño de tu catálogo.")]
    public List<DungeonRoomDefinition> rooms = new List<DungeonRoomDefinition>();

    public DungeonRoomDefinition GetFirst(DungeonRoomRole role)
    {
        return rooms.Find(room => room != null && room.role == role && room.roomPrefab != null);
    }

    public DungeonRoomDefinition PickWeighted(DungeonRoomRole role)
    {
        int total = 0;
        foreach (DungeonRoomDefinition room in rooms)
            if (IsUsable(room, role)) total += Mathf.Max(1, room.weight);
        if (total == 0) return null;

        int roll = Random.Range(0, total);
        foreach (DungeonRoomDefinition room in rooms)
        {
            if (!IsUsable(room, role)) continue;
            roll -= Mathf.Max(1, room.weight);
            if (roll < 0) return room;
        }
        return null;
    }

    private static bool IsUsable(DungeonRoomDefinition room, DungeonRoomRole role)
    {
        return room != null && room.role == role && room.roomPrefab != null;
    }
}
