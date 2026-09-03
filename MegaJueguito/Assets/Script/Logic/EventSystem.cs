using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    GameObject battleZonePrefab;

    GameObject rosterProtagonist; // Cambiar a un script que lo gestione.
    Dictionary<string, ITurnParticipant> protagonist = new Dictionary<string, ITurnParticipant>();

    void StartCombat(List<Dictionary<string, ITurnParticipant>> currentAntagonist, int waves)
    {
        CombatDirector director = Instantiate(battleZonePrefab).GetComponent<CombatDirector>();
        director.SetProtagonist(protagonist);
        director.InitializeCombat(currentAntagonist);
    }
}
