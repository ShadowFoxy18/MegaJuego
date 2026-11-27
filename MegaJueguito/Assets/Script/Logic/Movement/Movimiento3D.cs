using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Movimiento3D : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField, Min(0f)] private float moveSpeed = 4f;
    [SerializeField] private float gravity = -20f;

    [Header("Controles")]
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private Transform cameraTransform;

    private CharacterController characterController;
    private InputAction moveAction;
    private Vector2 moveInput;
    private float verticalSpeed;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (inputActions == null)
        {
            Debug.LogError("Asigna Player.inputactions en el campo Input Actions.", this);
            enabled = false;
            return;
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
        if (moveAction == null || characterController == null)
            return;

        moveInput = Vector2.ClampMagnitude(moveAction.ReadValue<Vector2>(), 1f);

        Vector3 forward = GetCameraForward();
        Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
        Vector3 movement = forward * moveInput.y + right * moveInput.x;

        if (movement.sqrMagnitude > 1f)
            movement.Normalize();

        if (characterController.isGrounded && verticalSpeed < 0f)
            verticalSpeed = -2f;

        verticalSpeed += gravity * Time.deltaTime;
        Vector3 velocity = movement * moveSpeed;
        velocity.y = verticalSpeed;

        characterController.Move(velocity * Time.deltaTime);
    }

    private Vector3 GetCameraForward()
    {
        if (cameraTransform == null)
            return Vector3.forward;

        Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up);

        // Una cámara cenital apunta al suelo; usamos su eje superior para orientar el movimiento.
        if (forward.sqrMagnitude < 0.001f)
            forward = Vector3.ProjectOnPlane(cameraTransform.up, Vector3.up);

        return forward.sqrMagnitude < 0.001f ? Vector3.forward : forward.normalized;
    }
}
