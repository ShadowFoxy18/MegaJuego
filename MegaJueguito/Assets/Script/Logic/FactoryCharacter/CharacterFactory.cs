public static class CharacterFactory
{
    public static Character Create(NPCData data)
    {
        return new Goblin(data.npcName, data.level, data.raceData, data.classData, data.raceData.etherealPower);
    }
}