using UnityEngine;

public class CharacterView : MonoBehaviour
{
    public Character Character { get; private set; }

    public void SetCharacter(Character character)
    {
        Character = character;
    }
}