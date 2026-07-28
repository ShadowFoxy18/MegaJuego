using UnityEngine;

public class Goblin : NonEthereal
{
    string _name;
    public Goblin(string name, int level, RaceData raceData, ClassData classData) : base(name, level, raceData, classData)
    {
        _name = name;
    }
}
