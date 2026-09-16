public static class CharacterFactory
{
    public static Character Create(NPCData data)
    {
        switch (data.raceType)
        {
            case RaceType.Human:
                return new Human(data.npcName, data.level, data.raceData, data.classData);

            case RaceType.Goblin:
                return new Goblin(data.npcName, data.level, data.raceData, data.classData, data.raceData.etherealPower);

            default:
                throw new System.Exception($"RaceType {data.raceType} no soportado en CharacterFactory.");
        }
    }
}