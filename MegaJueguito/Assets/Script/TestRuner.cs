using UnityEngine;

public class TestRunner : MonoBehaviour
{
    public RaceData humanRaceData;
    public ClassData warriorClassData;

    void Start()
    {
        GameObject juanGO = new GameObject("Juan");
        CharacterView juanView = juanGO.AddComponent<CharacterView>();
        Human juan = new Human("Juan", 1, humanRaceData, warriorClassData);
        juanView.SetCharacter(juan);

        GameObject anaGO = new GameObject("Ana");
        CharacterView anaView = anaGO.AddComponent<CharacterView>();
        Human ana = new Human("Ana", 1, humanRaceData, warriorClassData);
        anaView.SetCharacter(ana);

        GameObject[] party = new GameObject[] { juanGO, anaGO };
        PartyManager.Instance.ChangeParty(party);

        PartyManager.Instance.DebugPrintParty();

        Debug.Log("ChangeParty ejecutado sin errores.");
    }
}

