public interface IInteractable
{
    public enum Type { Enemy, Chest, Collectable, Exit }
    public Type InteractableType();
    public Item Interact();
    public bool IsActive();
    public IDamagable Damagable();
}