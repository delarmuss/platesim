using UnityEngine;

public class GunInteractable : MonoBehaviour, IInteractable
{
    public InteractType InteractType => InteractType.Hold;

    public void Interact()
    {
        Debug.Log("Silah alındı");
    }
}
