using System;
using System.Text;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Prefabs disponibles")]
    [Tooltip("Prefab asociado a la key I (sala inicial).")]
    [SerializeField] private GameObject startRoomPrefab;
    [Tooltip("Prefab asociado a la key C (sala básica).")]
    [SerializeField] private GameObject basicRoomPrefab;

    [Header("Tamaño de la cuadrícula")]
    [Min(1)] [SerializeField] private int gridSize = 15;

    [Header("Cantidad total de salas")]
    [Min(1)] [SerializeField] private int minRooms = 8;
    [Min(1)] [SerializeField] private int maxRooms = 12;

    [Header("Ejecución")]
    [SerializeField] private bool generateOnStart;
    [SerializeField] private bool logGridOnGenerate = true;

    public DungeonGrid CurrentGrid { get; private set; }
    public Vector2Int StartPosition { get; private set; }

    public GameObject GetPrefabForKey(string prefabKey)
    {
        if (prefabKey == DungeonRoomKeys.Start) return startRoomPrefab;
        if (prefabKey == DungeonRoomKeys.Basic) return basicRoomPrefab;
        return null;
    }

    private void Start()
    {
        if (generateOnStart)
            Generate();
    }

    /// <summary>Genera un mapa conectado con una cantidad aleatoria de salas entre minRooms y maxRooms.</summary>
    public DungeonGrid Generate(int? seed = null)
    {
        if (minRooms > maxRooms)
            throw new InvalidOperationException("El mínimo de salas no puede ser mayor que el máximo.");

        if (maxRooms > gridSize * gridSize)
            throw new InvalidOperationException("El máximo de salas supera las celdas disponibles en la cuadrícula.");

        if (seed.HasValue)
            UnityEngine.Random.InitState(seed.Value);

        DungeonGrid grid = new DungeonGrid(gridSize, gridSize);
        StartPosition = new Vector2Int(gridSize / 2, gridSize / 2);
        if (!grid.TryAddRoom(StartPosition, DungeonRoomKeys.Start))
            throw new InvalidOperationException("No se pudo colocar la sala inicial.");

        // El segundo límite de Random.Range es exclusivo; sumamos 1 para permitir maxRooms.
        int desiredRoomCount = UnityEngine.Random.Range(minRooms, maxRooms + 1);

        while (grid.RoomCount < desiredRoomCount)
        {
            var candidates = grid.GetEmptyNeighborsOfRooms();
            if (candidates.Count == 0)
                throw new InvalidOperationException("No quedan posiciones libres para completar la cantidad de salas solicitada.");

            Vector2Int chosenPosition = candidates[UnityEngine.Random.Range(0, candidates.Count)];
            // En este paso, todas las salas no iniciales usan la key C (básica).
            if (!grid.TryAddRoom(chosenPosition, DungeonRoomKeys.Basic))
                throw new InvalidOperationException("No se pudo colocar una sala en una celda candidata.");
        }

        CurrentGrid = grid;

        if (logGridOnGenerate)
        {
            Debug.Log("Mazmorra generada: " + grid.RoomCount + " salas. 00 = vacío; key+uso,sala = contenido de celda.");
            Debug.Log(FormatGrid(grid));
        }

        return grid;
    }

    private string FormatGrid(DungeonGrid grid)
    {
        StringBuilder result = new StringBuilder();
        for (int y = gridSize - 1; y >= 0; y--)
        {
            for (int x = 0; x < gridSize; x++)
            {
                result.Append(grid.GetCell(new Vector2Int(x, y)).ToString()).Append(' ');
            }

            result.AppendLine();
        }

        return result.ToString();
    }
}
