using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatDirector : MonoBehaviour
{
    private TurnQueue turnQueue = new TurnQueue();
    private Dictionary<string, CombatUnit> _units = new Dictionary<string, CombatUnit>();

    private List<Dictionary<string, CombatUnit>> _enemyWaves = new List<Dictionary<string, CombatUnit>>();
    private int _currentWaveIndex = 0;

    public void SetProtagonist(Dictionary<string, CombatUnit> allyUnits)
    {
        RegisterTeam(Team.Protagonist, allyUnits);
    }

    public void InitializeCombat(List<Dictionary<string, CombatUnit>> currentEnemyWaves)
    {
        _enemyWaves = currentEnemyWaves;
        _currentWaveIndex = 0;

        if (_enemyWaves.Count > 0)
        {
            RegisterTeam(Team.Antagonist, _enemyWaves[0]);
        }
    }

    private bool TryAdvanceWave(int waveIndex)
    {
        bool endWave = waveIndex >= _enemyWaves.Count;
        if (!endWave)
        {
            RegisterTeam(Team.Antagonist, _enemyWaves[waveIndex]);
        }

        return endWave;
    }

    private void RegisterTeam(Team team, Dictionary<string, CombatUnit> units)
    {
        foreach (var kvp in units)
        {
            string nickname = turnQueue.AddParticipant(team, kvp.Value.Participant.Speed, kvp.Key);

            kvp.Value.Team = team;
            _units.Add(nickname, kvp.Value);

            if (kvp.Value.Agent == null)
            {
                Debug.LogWarning($"{kvp.Key} no tiene un ITurnAgent asignado — no podrá decidir acciones.");
            }
        }
    }

    public CombatUnit GetCombatUnit(string nickname)
    {
        return _units.ContainsKey(nickname) ? _units[nickname] : null;
    }

    private BattleContext BuildBattleContext(string selfNickname)
    {
        Team selfTeam = _units[selfNickname].Team;

        List<CombatUnit> allies = _units
            .Where(kvp => kvp.Value.Team == selfTeam)
            .Select(kvp => kvp.Value)
            .ToList();

        List<string> enemyNicknames = _units
            .Where(kvp => kvp.Value.Team != selfTeam)
            .Select(kvp => kvp.Key)
            .ToList();

        return new BattleContext
        {
            selfNickname = selfNickname,
            allies = allies,
            enemyNicknames = enemyNicknames,
            director = this
        };
    }

    public void ProcessTurn()
    {
        string nickname = turnQueue.ProcessNextTurn();
        if (nickname == null) return;

        CombatUnit unit = GetCombatUnit(nickname);
        if (unit == null || unit.Agent == null) return;

        BattleContext context = BuildBattleContext(nickname);
        TurnAction decision = unit.Agent.DecideAction(context);

        Debug.Log($"{nickname} decidió: {decision.type} -> {decision.targetNickname}");
    }

    public void RemoveUnit(string nickname)
    {
        Team wipedTeam = turnQueue.RemoveParticipant(nickname);
        _units.Remove(nickname);

        if (wipedTeam == Team.Antagonist)
        {
            _currentWaveIndex++;
            if (TryAdvanceWave(_currentWaveIndex))
            {
                Debug.Log("Victoria: no quedan más oleadas.");
                // TODO: disparar evento OnVictory
            }
        }
        else if (wipedTeam == Team.Protagonist)
        {
            Debug.Log("Derrota.");
            // TODO: disparar evento OnDefeat
        }
    }
}