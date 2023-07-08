using System.Collections.Generic;
using UnityEngine;
public class Exit : MonoBehaviour, IInteractable
{
    [SerializeField] private Item KeyToNextRoom;
    public IDamagable Damagable() => throw new System.Exception("The Exit isn't damagable");
    public List<Item> Interact() => new List<Item>() { KeyToNextRoom };
    public IInteractable.Type InteractableType() => IInteractable.Type.Exit;
    public bool IsActive() => true;
}