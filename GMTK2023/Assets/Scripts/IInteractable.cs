using System.Collections.Generic;

public interface IInteractable
{
    public enum Type { Enemy, Chest, Exit, Collectable, }
    public Type InteractableType();
    public List<Item> Interact();
    public bool IsActive();
    public IDamagable Damagable();
}