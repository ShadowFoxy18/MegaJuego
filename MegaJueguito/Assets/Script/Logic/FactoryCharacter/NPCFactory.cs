public static class EnemyFactory
{
    public static CombatUnit CreateCombatUnit(NPCData data, Team team)
    {
        Character character = CharacterFactory.Create(data);

        if (data.startingWeapon != null)
        {
            character.EquipWeapon(data.startingWeapon.itemId);
        }

        EnemyBrain brain = EnemyBrainFactory.Create(data.alignment);

        return new CombatUnit
        {
            Participant = character,
            Agent = brain,
            Team = team
        };
    }
}