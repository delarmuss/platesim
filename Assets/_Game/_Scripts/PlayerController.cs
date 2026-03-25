using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float gravity = -19.62f;
    [SerializeField] private float groundCheckDistance = 0.3f;

    [Header("Look Settings")]
    [SerializeField] private float mouseSensitivity = 0.15f;  // Time.deltaTime yok, eski değere döndü
    [SerializeField] private float verticalClamp = 85f;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Camera cam;

    [Header("Ground Layer")]
    [SerializeField] private LayerMask groundMask = ~0;

    private CharacterController cc;
    private Vector3 velocity;
    private float verticalRotation;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();

        if (cam == null) cam = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void LateUpdate()
    {
        HandleLook();
        HandleCamera();
    }

    private void HandleLook()
    {
        Vector2 lookInput = InputManager.Instance.LookInput;

        // Mouse delta zaten "bu frame'de kaç piksel hareket etti" bilgisini taşır.
        // Time.deltaTime ile çarpmak FPS farkında dengesizliğe yol açar.
        float yaw = lookInput.x * mouseSensitivity;
        transform.Rotate(Vector3.up, yaw);

        verticalRotation -= lookInput.y * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalClamp, verticalClamp);
        cameraHolder.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void HandleCamera()
    {
        cam.transform.SetPositionAndRotation(cameraHolder.position, cameraHolder.rotation);
    }

    private void HandleMovement()
    {
        Vector2 moveInput = InputManager.Instance.MoveInput;
        bool isGrounded = IsGrounded();

        if (isGrounded)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
            cc.Move(velocity * Time.deltaTime);
        }

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        float speed = InputManager.Instance.SprintInput ? runSpeed : walkSpeed;
        cc.Move(speed * Time.deltaTime * move);
    }

    private bool IsGrounded()
    {
        Vector3 bottom = transform.position + cc.center - Vector3.up * (cc.height * 0.5f - cc.radius);

        return Physics.SphereCast(
            bottom,
            cc.radius * 0.9f,
            Vector3.down,
            out _,
            groundCheckDistance,
            groundMask,
            QueryTriggerInteraction.Ignore
        );
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (cc == null) cc = GetComponent<CharacterController>();
        Vector3 bottom = transform.position + cc.center - Vector3.up * (cc.height * 0.5f - cc.radius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(bottom + Vector3.down * groundCheckDistance, cc.radius * 0.9f);
    }
#endif
}
