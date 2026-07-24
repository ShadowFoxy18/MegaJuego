using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Character
{
    protected string _name;
    protected RaceData _raceData;
    protected ClassData _classData;
    protected int _characterLevel = 1;

    protected List<ElementType> _raceAfinityType;
    protected List<ElementType> _classAfinityType;
    protected RaceData.ResistenceElement[] _raceResistenceElements;
    protected ClassData.ResistenceElement[] _classResistenceElements;
    protected Dictionary<ElementType, float> _resistances;

    protected float _attack;
    protected float _defense;
    protected int _health;
    protected float _speed;
    protected float _mitigation;
    protected float _anomaly;
    protected float _maestry;

    protected Character(string name, int characterLevel, RaceData raceData, ClassData classData)
    {
        _name = name;
        _raceData = raceData;
        _classData = classData;
        _characterLevel = characterLevel;

        _raceAfinityType = raceData.afinityType;
        _classAfinityType = classData.afinityType;

        _raceResistenceElements = raceData.resistenceElements;
        _classResistenceElements = classData.resistenceElements;

        _attack = raceData.baseAttack * classData.baseAttackMultiplier;
        _defense = raceData.baseDefense * classData.baseDefenseMultiplier;
        _health = Mathf.RoundToInt(raceData.baseHealth * classData.baseHealthMultiplier);
        _speed = raceData.baseSpeed * classData.baseSpeedMultiplier;

        _mitigation = raceData.baseMitigation + classData.baseMitigation;
        _anomaly = raceData.baseAnomaly * classData.baseAnomalyMultiplier;

        _maestry = classData.baseMaestry;

        GenerateResistances();
    }

    void GenerateResistances()
    {
        Dictionary<ElementType, float> newResistences = new Dictionary<ElementType, float>();

        foreach (ElementType element in Enum.GetValues(typeof(ElementType)))
        {
            float resistence = 1.0f;

            if (_raceResistenceElements  != null)
            {
                foreach (RaceData.ResistenceElement raceResistence in _raceResistenceElements)
                {
                    ElementType[] elementsType = raceResistence.elementsType;
                    if (elementsType.Contains(element))
                    {
                        resistence *= raceResistence.resistanceValue;
                        break;
                    }
                }
            }
            if (_classResistenceElements != null)
            {
                foreach (ClassData.ResistenceElement classResistence in _classResistenceElements)
                {
                    ElementType[] elementsType = classResistence.elementsType;
                    if (elementsType.Contains(element))
                    {
                        resistence *= classResistence.resistanceValue;
                        break;
                    }
                }
            }


            if (!newResistences.ContainsKey(element))
            {
                newResistences[element] = resistence;
            }
        }
        _resistances = newResistences;
    }
        

}
