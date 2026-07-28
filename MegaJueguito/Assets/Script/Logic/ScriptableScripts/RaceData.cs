using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRace", menuName = "CharacterData/RaceData", order = 1)]
public class RaceData : ScriptableObject
{ 
    public string raceName = "Unknown";

    [Min (1)] public List<ElementType> afinityType = new List<ElementType> { ElementType.Fisico };
    public ResistenceElement[] resistenceElements;

    [System.Serializable]
    public class ResistenceElement
    {
        public ElementType[] elementsType;
        [Range(0.3f, 2.5f)] public float resistanceValue = 1.0f;
    }

    [Range (5, 250)] public int baseAttack = 5;
    [Range (20, 150)] public int baseDefense = 20;
    [Range (10, 500)] public int baseHealth = 10;
    [Range (60, 280)] public int baseSpeed = 60;
    [Range (100, 600)] public int baseStamina = 120;

    [Range (0.0f, 0.4f)] public float baseMitigation = 0.0f;
    [Range (1.0f, 2.0f)] public float baseAnomaly = 1.0f;
}
