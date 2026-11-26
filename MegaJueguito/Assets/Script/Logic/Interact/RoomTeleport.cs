using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField, Min(0f)] private float fadeDuration = 0.4f;
    [SerializeField, Min(0f)] private float blackHoldDuration = 0.15f;
    [SerializeField] private CanvasGroup fadeCanvas;

    private static CanvasGroup sharedFadeCanvas;
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

        CanvasGroup screen = GetFadeCanvas();
        screen.gameObject.SetActive(true);
        screen.blocksRaycasts = true;

        MovementBehavior[] movementScripts = actor.GetComponentsInChildren<MovementBehavior>(true);
        bool[] wasEnabled = new bool[movementScripts.Length];
        for (int i = 0; i < movementScripts.Length; i++)
        {
            wasEnabled[i] = movementScripts[i].enabled;
            movementScripts[i].enabled = false;
        }

        yield return Fade(screen, 1f);

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

        yield return Fade(screen, 0f);
        screen.blocksRaycasts = false;

        for (int i = 0; i < movementScripts.Length; i++)
        {
            if (movementScripts[i] != null)
                movementScripts[i].enabled = wasEnabled[i];
        }

        isTeleporting = false;
        transitionInProgress = false;
    }

    private IEnumerator Fade(CanvasGroup screen, float targetAlpha)
    {
        float startAlpha = screen.alpha;
        if (fadeDuration <= 0f)
        {
            screen.alpha = targetAlpha;
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            screen.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            yield return null;
        }

        screen.alpha = targetAlpha;
    }

    private CanvasGroup GetFadeCanvas()
    {
        if (fadeCanvas != null)
            return fadeCanvas;

        if (sharedFadeCanvas != null)
            return sharedFadeCanvas;

        GameObject canvasObject = new GameObject("Room Transition Fade Canvas");
        DontDestroyOnLoad(canvasObject);

        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = short.MaxValue;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        GameObject panel = new GameObject("Black Fade");
        panel.transform.SetParent(canvasObject.transform, false);
        RectTransform rect = panel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image = panel.AddComponent<Image>();
        image.color = Color.black;
        image.raycastTarget = false;

        sharedFadeCanvas = panel.AddComponent<CanvasGroup>();
        sharedFadeCanvas.alpha = 0f;
        sharedFadeCanvas.blocksRaycasts = false;
        sharedFadeCanvas.interactable = false;
        return sharedFadeCanvas;
    }
}
