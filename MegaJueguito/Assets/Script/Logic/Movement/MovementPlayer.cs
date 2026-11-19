using UnityEngine;

public class MovementPlayer : MovementBehavior
{
    [SerializeField] GameObject player;
    [SerializeField, Min(1)] float playerSpeed = 10;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveNorth(player, playerSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            MoveSouth(player, playerSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            MoveWest(player, playerSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveEast(player, playerSpeed);
        }
    }
}
