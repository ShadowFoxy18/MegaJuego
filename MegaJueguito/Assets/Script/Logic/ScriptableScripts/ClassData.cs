using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewClass", menuName = "CharacterData/ClassData", order = 1)]
public class ClassData : ScriptableObject
{
    public string className = "Unknown";

    [Min(1)] public List<ElementType> afinityType = new List<ElementType> { ElementType.Fisico };
    public ResistenceElement[] resistenceElements;

    [System.Serializable]
    public class ResistenceElement
    {
        public ElementType[] elementsType;
        [Range(0.3f, 2.5f)] public float resistanceValue = 1.0f;
    }

    [Range(1f, 2.5f)] public float baseAttackMultiplier = 1f;
    [Range(1.1f, 1.8f)] public float baseDefenseMultiplier = 1.1f;
    [Range(1f, 5f)] public float baseHealthMultiplier = 1f;
    [Range(1f, 3f)] public float baseSpeedMultiplier = 1f;

    [Range(0.0f, 0.4f)] public float baseMitigation = 0.0f;

    [Range(10, 350)] public int baseMaestry = 10;
    [Range(1f, 2.5f)] public float baseAnomalyMultiplier = 1f;
}
