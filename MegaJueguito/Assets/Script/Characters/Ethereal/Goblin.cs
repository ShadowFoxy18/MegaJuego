using UnityEngine;

public class Goblin : Ethereal
{
    string _name;

    public Goblin(string name, int level, RaceData raceData, ClassData classData, int etherealPower) : base(name, level, raceData, classData, etherealPower)
    {
        _name = name;
    }
}
