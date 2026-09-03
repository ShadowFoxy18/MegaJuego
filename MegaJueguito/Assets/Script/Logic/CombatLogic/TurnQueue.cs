using System.Collections.Generic;
using UnityEngine;

public class TurnQueue
{
    public const float BASE_ACTION_GAUGE = 10000f;

    Dictionary<string, ActionValueData> actionValues = new Dictionary<string, ActionValueData>();
    Dictionary<Team, List<string>> teamParticipants = new Dictionary<Team, List<string>>();

    void GenerateTeam(Team team)
    {
        if (!teamParticipants.ContainsKey(team))
        {
            teamParticipants.Add(team, new List<string>());
        }
    }

    string GenerateUniqueNickname(string baseName)
    {
        int counter = 1;
        string uniqueName = baseName;
        while (actionValues.ContainsKey(uniqueName))
        {
            uniqueName = $"{baseName}_{counter}";
            counter++;
        }
        return uniqueName;
    }

    bool CheckNicknameExists(string nickname)
    {
        return actionValues.ContainsKey(nickname);
    }


    Team CheckTeamWipedOut()
    {
        bool protagonistsAlive = teamParticipants.ContainsKey(Team.Protagonist) && teamParticipants[Team.Protagonist].Count > 0;
        bool antagonistsAlive = teamParticipants.ContainsKey(Team.Antagonist) && teamParticipants[Team.Antagonist].Count > 0;

        if (!antagonistsAlive) return Team.Antagonist;
        if (!protagonistsAlive) return Team.Protagonist;
        return Team.None;
    }

    void ResetAV(string nickname)
    {
        if (!actionValues.ContainsKey(nickname)) return;

        ActionValueData data = actionValues[nickname];
        data.currentAV = data.baseAV;
        actionValues[nickname] = data;
    }

    public string AddParticipant(Team team, float speed, string baseName)
    {
        string nickname = GenerateUniqueNickname(baseName);

        actionValues.Add(nickname, new ActionValueData(speed));

        if (!teamParticipants.ContainsKey(team))
            GenerateTeam(team);
        teamParticipants[team].Add(nickname);

        return nickname;
    }

    public Team RemoveParticipant(string nickname)
    {
        if (!CheckNicknameExists(nickname)) return Team.None;

        actionValues.Remove(nickname);
        foreach (Team team in teamParticipants.Keys)
        {
            teamParticipants[team].Remove(nickname);
        }

        return CheckTeamWipedOut();
    }

    string GetNextActor()
    {
        string nickname = null;
        float lowestAV = float.MaxValue;

        foreach (var kvp in actionValues)
        {
            if (kvp.Value.currentAV < lowestAV)
            {
                lowestAV = kvp.Value.currentAV;
                nickname = kvp.Key;
            }
        }

        return nickname;
    }

    public void ModifyAV(string nickname, float elapsedTime, float percent)
    {
        if (!actionValues.ContainsKey(nickname)) return;

        ActionValueData data = actionValues[nickname];

        float percentDelta = data.baseAV * percent;
        float newAV = data.currentAV - percentDelta;
        newAV -= elapsedTime;

        data.currentAV = Mathf.Max(0, newAV);
        actionValues[nickname] = data;
    }

    public void AdvanceTimeByAmount(float elapsed)
    {
        List<string> keys = new List<string>(actionValues.Keys);
        foreach (string nickname in keys)
        {
            ModifyAV(nickname, elapsed, 0f);
        }
    }

    void OnCharacterTurn(string nickname)
    {
        ResetAV(nickname);
    }

    public string ProcessNextTurn()
    {
        string nextActor = GetNextActor();

        if (nextActor == null)
        {
            Debug.Log("No participants left to act.");
            return null;
        }

        float elapsed = actionValues[nextActor].currentAV;
        AdvanceTimeByAmount(elapsed);
        OnCharacterTurn(nextActor);

        return nextActor;
    }
}