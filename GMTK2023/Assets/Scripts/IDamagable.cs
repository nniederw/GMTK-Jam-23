public interface IDamagable : IInteractable
{
    public void GetAttacked(int damage);
    public int GetHealth();
}