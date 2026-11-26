using UnityEngine;
using UnityEngine.InputSystem;

public class MovementPlayer : MovementBehavior
{
    [SerializeField] private GameObject movementTarget;
    [SerializeField, Min(0f)] private float moveSpeed = 5f;
    [SerializeField] private InputActionAsset inputActions;

    private InputAction moveAction;
    private Vector2 moveInput;
    private Rigidbody2D movementBody;

    private void Awake()
    {
        if (inputActions == null)
        {
            Debug.LogError("Asigna Player.inputactions en el Inspector.", this);
            enabled = false;
            return;
        }

        if (movementTarget != null)
        {
            movementBody = movementTarget.GetComponent<Rigidbody2D>();
            if (movementBody == null)
                Debug.LogError("El objeto elegido necesita un Rigidbody2D.", movementTarget);

            if (movementTarget.GetComponentInChildren<Collider2D>() == null)
                Debug.LogWarning("Añade un Collider2D al Player o a uno de sus hijos.", movementTarget);
        }
        else
        {
            Debug.LogWarning("Arrastra el Empty que quieres mover al campo Movement Target.", this);
        }

        moveAction = inputActions.FindAction("DungeonMovement/Movement", throwIfNotFound: false);
        if (moveAction == null)
        {
            Debug.LogError("No se encontró DungeonMovement/Movement en Player.inputactions.", this);
            enabled = false;
        }

    }

    private void OnEnable()
    {
        moveAction?.Enable();
    }

    private void OnDisable()
    {
        moveAction?.Disable();
    }

    private void Update()
    {
        if (moveAction != null)
            moveInput = Vector2.ClampMagnitude(moveAction.ReadValue<Vector2>(), 1f);
    }

    private void FixedUpdate()
    {
        if (movementBody == null)
            return;

        MoveInDirection(movementBody, moveInput, moveSpeed);
    }
}
