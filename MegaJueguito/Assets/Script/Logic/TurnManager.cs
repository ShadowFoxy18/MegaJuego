using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public const float BASE_ACTION_GAUGE = 10000f;

    private Dictionary<string, ITurnParticipant> participants = new Dictionary<string, ITurnParticipant>();
    private Dictionary<string, ActionValueData> actionValues = new Dictionary<string, ActionValueData>();
    private Dictionary<Team, List<string>> teamParticipants = new Dictionary<Team, List<string>>();

    string GenerateUniqueNickname(string baseName)
    {
        int counter = 1;
        string uniqueName = baseName;
        while (participants.ContainsKey(uniqueName))
        {
            uniqueName = $"{baseName}_{counter}";
            counter++;
        }
        return uniqueName;
    }

    void AddParticipant(Team team, string nickname)
    {

    }

    void GetNextActor()
    {

    }

    void AdvanceTime(float elapsedTime)
    {

    }

    void OnCharacterActed()
    {

    }

}
