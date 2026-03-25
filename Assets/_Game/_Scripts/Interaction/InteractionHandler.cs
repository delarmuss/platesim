using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactMask = ~0;
    [SerializeField] private Transform rayOrigin;   // cameraHolder veya cam transform

    private IInteractable currentTarget;

    private void Awake()
    {
        rayOrigin ??= Camera.main.transform;
    }

    private void Update()
    {
        DetectTarget();
        HandleInput();
    }

    private void DetectTarget()
    {
        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hit, interactRange, interactMask))
            currentTarget = hit.collider.GetComponent<IInteractable>();
        else
            currentTarget = null;
    }

    private void HandleInput()
    {
        if (currentTarget == null) return;

        switch (currentTarget.InteractType)
        {
            case InteractType.Use when InputManager.Instance.InteractInput:
                currentTarget.Interact();
                break;

            case InteractType.Hold when InputManager.Instance.HoldInput:
                currentTarget.Interact();
                break;
        }
    }

    // Hedef varsa UI'da göstermek için dışarıdan okunabilir
    public IInteractable CurrentTarget => currentTarget;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (rayOrigin == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(rayOrigin.position, rayOrigin.forward * interactRange);
    }
#endif
}
