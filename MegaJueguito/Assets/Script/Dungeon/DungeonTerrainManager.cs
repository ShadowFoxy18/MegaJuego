using UnityEngine;

/// <summary>Implementación base para que el administrador de terrenos reciba una mazmorra ya generada.</summary>
public abstract class DungeonTerrainManager : MonoBehaviour
{
    public abstract void OnDungeonGenerated(DungeonLayout layout);
}
