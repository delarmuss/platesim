using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    public InteractType InteractType => InteractType.Use;   // E tuşu

    private bool isOpen;

    public void Interact()
    {
        isOpen = !isOpen;
        Debug.Log(isOpen ? "Kapı açıldı" : "Kapı kapandı");

        // Animasyon veya pozisyon değişikliği buraya
    }
}
