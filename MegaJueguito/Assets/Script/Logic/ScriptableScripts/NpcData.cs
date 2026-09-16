using UnityEngine;

public enum RaceType
{
    Human,
    Goblin
    // se agrega una entrada acá por cada raza nueva que crees
}

[CreateAssetMenu(menuName = "Characters/NPCData")]
public class NPCData : ScriptableObject
{
    public string npcName;
    public int level = 1;

    public RaceType raceType;
    public RaceData raceData;
    public ClassData classData;

    public Alignment alignment;
    public WeaponData startingWeapon;
}