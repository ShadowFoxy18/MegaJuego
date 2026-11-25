using UnityEngine;

public enum DungeonRoomRole
{
    Normal,
    Start,
    Boss,
    Safe,
    Merchant
}

[System.Flags]
public enum DungeonDoor
{
    None = 0,
    North = 1,
    East = 2,
    South = 4,
    West = 8,
    All = North | East | South | West
}

[CreateAssetMenu(menuName = "Dungeon/Room Definition")]
public class DungeonRoomDefinition : ScriptableObject
{
    [Tooltip("Key estable que usa la tabla para identificar esta sala.")]
    public string roomKey;
    public DungeonRoomRole role = DungeonRoomRole.Normal;
    [Min(1)] public int weight = 1;
    public GameObject roomPrefab;
    [Tooltip("Puertas que existen en el prefab. Se usa para validar conexiones.")]
    public DungeonDoor doors = DungeonDoor.All;
}
