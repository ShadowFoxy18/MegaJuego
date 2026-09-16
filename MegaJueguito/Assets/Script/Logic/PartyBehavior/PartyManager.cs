using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private Dictionary<string, CharacterView> _partyMembers = new Dictionary<string, CharacterView>();

    public GameObject[] actualParty;

    private string GenerateUniqueNickname(string baseName)
    {
        int counter = 1;
        string uniqueName = baseName;
        while (_partyMembers.ContainsKey(uniqueName))
        {
            uniqueName = $"{baseName}_{counter}";
            counter++;
        }
        return uniqueName;
    }

    public void ChangeParty(GameObject[] newParty)
    {
        actualParty = newParty;
        _partyMembers.Clear();

        foreach (var member in newParty)
        {
            var characterView = member.GetComponent<CharacterView>();

            if (characterView != null && characterView.Character != null)
            {
                string nickname = GenerateUniqueNickname(characterView.Character.Name);
                _partyMembers.Add(nickname, characterView);
            }
            else
            {
                Debug.LogWarning($"GameObject {member.name} does not have a CharacterView with a valid Character.");
            }
        }
    }


    public Dictionary<string, CombatUnit> GetCombatUnits()
    {
        Dictionary<string, CombatUnit> units = new Dictionary<string, CombatUnit>();

        foreach (var kvp in _partyMembers)
        {
            CharacterView view = kvp.Value;
            ITurnAgent agent = view.GetComponent<ITurnAgent>();

            units.Add(kvp.Key, new CombatUnit
            {
                Participant = view.Character,
                Agent = agent,
                Team = Team.Protagonist
            });
        }

        return units;
    }

    public void DebugPrintParty()
    {
        foreach (var kvp in _partyMembers)
        {
            Debug.Log($"{kvp.Key} -> {kvp.Value.Character.Name}");
        }
    }
}