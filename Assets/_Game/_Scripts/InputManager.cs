using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private PlayerControls inputs;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        inputs = new PlayerControls();
    }

    private void OnEnable()
    {
        inputs.Player.Enable();
    }

    private void OnDisable()
    {
        inputs.Player.Disable();
    }

    public Vector2 MoveInput => inputs.Player.Move.ReadValue<Vector2>();
    public Vector2 LookInput => inputs.Player.Look.ReadValue<Vector2>();
    public bool SprintInput => inputs.Player.Sprint.IsPressed();
    public bool InteractInput => inputs.Player.Interact1.IsPressed();
    public bool HoldInput => inputs.Player.Interact2.IsPressed();
}
