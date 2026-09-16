using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    [SerializeField] private GameObject battleZonePrefab;

    public void StartCombat(List<Dictionary<string, CombatUnit>> enemyWaves)
    {
        Dictionary<string, CombatUnit> protagonists = PartyManager.Instance.GetCombatUnits();

        CombatDirector director = Instantiate(battleZonePrefab).GetComponent<CombatDirector>();
        director.SetProtagonist(protagonists);
        director.InitializeCombat(enemyWaves);
    }
}