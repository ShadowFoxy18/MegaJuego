using UnityEngine;

public abstract class Character
{
    protected string _name;
    protected RaceData _raceData;
    protected ClassData _classData;

    protected float _attack;
    protected float _defense;
    protected int _health;
    protected float _speed;
    protected float _mitigation;
    protected float _anomaly;
    protected float _maestry;

    protected Character(string name, RaceData raceData, ClassData classData)
    {
        _name = name;
        _raceData = raceData;
        _classData = classData;

        _attack = raceData.baseAttack * classData.baseAttackMultiplier;
        _defense = raceData.baseDefense * classData.baseDefenseMultiplier;
        _health = Mathf.RoundToInt(raceData.baseHealth * classData.baseHealthMultiplier);
        _speed = raceData.baseSpeed * classData.baseSpeedMultiplier;

        _mitigation = raceData.baseMitigation + classData.baseMitigation;
        _anomaly = raceData.baseAnomaly * classData.baseAnomalyMultiplier;

        _maestry = classData.baseMaestry;
    }
}
