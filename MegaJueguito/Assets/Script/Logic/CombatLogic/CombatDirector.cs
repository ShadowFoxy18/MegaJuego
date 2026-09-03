using System.Collections.Generic;
using UnityEngine;

public class CombatDirector : MonoBehaviour
{
    private TurnQueue turnQueue = new TurnQueue();

    Dictionary<string, ITurnParticipant> allyDicc = new Dictionary<string, ITurnParticipant>();
    List<Dictionary<string, ITurnParticipant>> enemyDicc = new List<Dictionary<string, ITurnParticipant>>();

    int waves;

    public void SetProtagonist(Dictionary<string, ITurnParticipant> allyDicc)
    {
        this.allyDicc = allyDicc;
        foreach (var kvp in allyDicc)
        {
            string nickname = turnQueue.AddParticipant(Team.Protagonist, kvp.Value.Speed, kvp.Key);
        }
    }

    public void InitializeCombat(List<Dictionary<string, ITurnParticipant>> currentEnemyDicc)
    {
        this.waves = currentEnemyDicc.Count;
        foreach (var kvp in currentEnemyDicc[0])
        {
            string nickname = turnQueue.AddParticipant(Team.Antagonist, kvp.Value.Speed, kvp.Key);
        }
        enemyDicc = currentEnemyDicc;
    }

    void SetNewWave(int waveIndex)
    {
        foreach (var kvp in enemyDicc[waveIndex])
        {
            string nickname = turnQueue.AddParticipant(Team.Antagonist, kvp.Value.Speed, kvp.Key);
        }
    }
}
