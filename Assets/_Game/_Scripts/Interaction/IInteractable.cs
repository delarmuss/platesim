public enum InteractType
{
    Use,
    Hold
}

public interface IInteractable
{
    InteractType InteractType { get; }
    void Interact();
}
