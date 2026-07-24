using UnityEngine;

public abstract class Ethereal : Character
{
    private int _etherealPower;

    public Ethereal(string name, RaceData raceData, ClassData classData, int etherealPower) : base(name, raceData, classData)
    {
        _etherealPower = etherealPower;
    }
}
