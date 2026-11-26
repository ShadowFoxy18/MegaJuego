using System.Collections;
using UnityEngine;

/// <summary>
/// Teleports an actor to a room marker while the screen is covered in black.
/// Call Teleport(GameObject) from the interaction event.
/// </summary>
public class RoomTeleport : MonoBehaviour
{
    [Header("Destino")]
    [SerializeField] private Transform destination;
    [SerializeField] private InteractionSensor2D destinationInteractionSensor;

    [Header("Fundido")]
    [SerializeField, Min(0f)] private float blackHoldDuration = 0.15f;

    private static bool transitionInProgress;
    private bool isTeleporting;

    /// <summary>Connect this to the interaction event that runs when the player presses Interact.</summary>
    public void Teleport(GameObject actor)
    {
        if (isTeleporting || transitionInProgress)
            return;

        if (actor == null)
        {
            Debug.LogWarning("RoomTeleport necesita recibir el GameObject del actor.", this);
            return;
        }

        if (destination == null)
        {
            Debug.LogWarning("Asigna el punto de destino en el Inspector.", this);
            return;
        }

        if (GlobalTpObjs.Instance == null || !GlobalTpObjs.Instance.IsReady)
        {
            Debug.LogWarning("No se encontró un GlobalTpObjs activo con un Canvas asignado.", this);
            return;
        }

        StartCoroutine(TeleportRoutine(actor));
    }

    /// <summary>Convenience method for events without a GameObject argument.</summary>
    public void TeleportPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("No se encontró ningún GameObject con el tag Player.", this);
            return;
        }

        Teleport(player);
    }

    private IEnumerator TeleportRoutine(GameObject actor)
    {
        isTeleporting = true;
        transitionInProgress = true;

        MovementBehavior[] movementScripts = actor.GetComponentsInChildren<MovementBehavior>(true);
        bool[] wasEnabled = new bool[movementScripts.Length];
        for (int i = 0; i < movementScripts.Length; i++)
        {
            wasEnabled[i] = movementScripts[i].enabled;
            movementScripts[i].enabled = false;
        }

        yield return GlobalTpObjs.Instance.OscurecerYEsperar();

        // Evita que el sensor de llegada ofrezca inmediatamente el teletransporte de vuelta.
        if (destinationInteractionSensor != null)
            destinationInteractionSensor.BlockActorUntilExit(actor);

        Rigidbody2D body = actor.GetComponent<Rigidbody2D>();
        Vector3 destinationPosition = destination.position;
        if (body != null)
        {
            body.position = destinationPosition;
            body.linearVelocity = Vector2.zero;
            body.angularVelocity = 0f;
        }
        else
        {
            actor.transform.position = destinationPosition;
        }

        actor.transform.rotation = destination.rotation;
        Physics2D.SyncTransforms();

        if (blackHoldDuration > 0f)
            yield return new WaitForSecondsRealtime(blackHoldDuration);

        yield return GlobalTpObjs.Instance.AclararYEsperar();

        for (int i = 0; i < movementScripts.Length; i++)
        {
            if (movementScripts[i] != null)
                movementScripts[i].enabled = wasEnabled[i];
        }

        isTeleporting = false;
        transitionInProgress = false;
    }

}
