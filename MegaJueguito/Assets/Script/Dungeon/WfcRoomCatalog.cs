using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum WfcDoorSide
{
    None = 0,
    North = 1,
    East = 2,
    South = 4,
    West = 8,
    All = North | East | South | West
}

[Serializable]
public class WfcRoomPiece
{
    [SerializeField] private string pieceName;
    [SerializeField] private GameObject prefab;
    [SerializeField] private WfcDoorSide openSides;
    [SerializeField] private bool canBeStart;
    [SerializeField, Min(0.001f)] private float weight = 1f;

    public string PieceName { get { return pieceName; } }
    public GameObject Prefab { get { return prefab; } }
    public WfcDoorSide OpenSides { get { return openSides; } }
    public bool CanBeStart { get { return canBeStart; } }
    public float Weight { get { return Mathf.Max(0.001f, weight); } }
}

/// <summary>Una orientación concreta de una pieza que WFC puede seleccionar.</summary>
public class WfcRoomOption
{
    public WfcRoomPiece Source { get; private set; }
    public WfcDoorSide OpenSides { get; private set; }
    public float Weight { get; private set; }
    public bool CanBeStart { get { return Source != null && Source.CanBeStart; } }
    public bool IsEmpty { get { return Source == null; } }

    public WfcRoomOption(WfcRoomPiece source, WfcDoorSide openSides, float weight)
    {
        Source = source;
        OpenSides = openSides;
        Weight = weight;
    }

    public static WfcRoomOption Empty
    {
        get { return new WfcRoomOption(null, WfcDoorSide.None, 1f); }
    }
}

/// <summary>
/// Catálogo inicial para el modelo tiled de WFC. Cada pieza conserva la
/// orientación artística original de su prefab.
/// </summary>
public class WfcRoomCatalog : MonoBehaviour
{
    [SerializeField] private List<WfcRoomPiece> roomPieces = new List<WfcRoomPiece>();

    public IReadOnlyList<WfcRoomPiece> RoomPieces { get { return roomPieces.AsReadOnly(); } }

    public List<WfcRoomOption> BuildOptions()
    {
        List<WfcRoomOption> options = new List<WfcRoomOption>();

        foreach (WfcRoomPiece piece in roomPieces)
        {
            if (piece == null || piece.Prefab == null) continue;

            options.Add(new WfcRoomOption(piece, piece.OpenSides, piece.Weight));
        }

        return options;
    }

    public List<string> ValidateCatalog()
    {
        List<string> errors = new List<string>();
        bool foundStartPiece = false;

        if (roomPieces.Count == 0)
            errors.Add("Añade al menos una pieza de sala al catálogo.");

        foreach (WfcRoomPiece piece in roomPieces)
        {
            if (piece == null)
            {
                errors.Add("Hay una entrada vacía en el catálogo.");
                continue;
            }

            if (piece.Prefab == null)
                errors.Add("Asigna un prefab a la pieza '" + piece.PieceName + "'.");

            if (piece.CanBeStart && piece.Prefab != null)
                foundStartPiece = true;
        }

        if (!foundStartPiece)
            errors.Add("Al menos una pieza debe permitir ser sala de inicio.");

        return errors;
    }

    [ContextMenu("Validate and log WFC options")]
    private void ValidateAndLogVariants()
    {
        List<string> errors = ValidateCatalog();
        if (errors.Count > 0)
        {
            foreach (string error in errors)
                Debug.LogError("WFC catalog: " + error, this);
            return;
        }

        List<WfcRoomOption> options = BuildOptions();
        Debug.Log("WFC catalog valid. Found " + options.Count + " fixed-orientation pieces:", this);
        foreach (WfcRoomOption option in options)
        {
            Debug.Log(
                option.Source.PieceName +
                " | doors: " + option.OpenSides +
                " | weight: " + option.Weight +
                " | start: " + option.CanBeStart,
                this);
        }
    }
}
