using UnityEngine;

public abstract class Ethereal : Character
{
    private int _etherealPower;

    public Ethereal(string name, int level, RaceData raceData, ClassData classData, int etherealPower) : base(name, level, raceData, classData)
    {
        _etherealPower = etherealPower;
    }
}
