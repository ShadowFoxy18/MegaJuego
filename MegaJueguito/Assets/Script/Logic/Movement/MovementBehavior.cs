using UnityEngine;
using UnityEngine.Rendering;

public class MovementBehavior : MonoBehaviour
{

    public class MovementAnimation
    {
        Animation animationOrientation;
    }

    protected bool MoveNorth(GameObject objectMove, float speed)
    {
        objectMove.transform.position += new Vector3(0, 1, 0) * speed;
        return true;
    }

    protected bool MoveSouth(GameObject objectMove, float speed)
    {
        objectMove.transform.position += new Vector3(0, -1, 0) * speed;
        return true;
    }

    protected bool MoveEast(GameObject objectMove, float speed)
    {
        objectMove.transform.position += new Vector3(1, 0, 0) * speed;
        return true;
    }

    protected bool MoveWest(GameObject objectMove, float speed)
    {
        objectMove.transform.position += new Vector3(-1, 0, 0) * speed;
        return true;
    }
}
