using UnityEngine;


[CreateAssetMenu(menuName = "Characters/NPCData")]
public class NPCData : ScriptableObject
{
    public string npcName;
    public int level = 1;

    public RaceData raceData;
    public ClassData classData;

    public Alignment alignment;
    public WeaponData startingWeapon;
}