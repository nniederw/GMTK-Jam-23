using System;
using UnityEngine;
[RequireComponent(typeof(Movement))]
public class EnemyObject : MonoBehaviour, IDamagable
{
    public Enemy EnemyType;
    private Movement Movement;
    private int Health;
    private bool IsDead = false;
    private HeroAI HeroAI;
    private Vector2 HeroLastPosition;
    private float AttackCooldown = 0f;
    private void Start()
    {
        Movement = GetComponent<Movement>();
        Health = EnemyType.Health;
        IsDead = false;
    }
    private void FixedUpdate()
    {
        if (Health < 0)
        {
            IsDead = true;
            Destroy(gameObject);
            return;
        }
        if (LookForHero())
        {
            TryAttackHero();
            if (!HeroAttackable())
            {
                Movement.WalkTo(HeroLastPosition);
            }
        }
        else
        {
            Movement.WalkTo(HeroLastPosition);
        }
        AttackCooldown -= Time.fixedDeltaTime;
    }
    private void TryAttackHero()
    {
        if (HeroAttackable())
        {
            Attack();
        }
    }
    private bool HeroAttackable() => transform.DistanceTo(HeroAI.transform.position) < EnemyType.AttackDistance;
    private void Attack()
    {
        if (!HeroAttackable()) throw new Exception("The Hero isn't attackable");
        if (AttackCooldown > 0f) return;
        HeroAI.GetAttacked(EnemyType.Damage);
        AttackCooldown = EnemyType.AttackCooldownSeconds;
    }
    private bool LookForHero()
    {
        HeroAI hero = null;
        var cols = Physics2D.OverlapCircleAll(transform.position, EnemyType.VisionDistance);
        foreach (var col in cols)
        {
            hero = col.GetComponent<HeroAI>();
            if (hero == null) continue;
            break;
        }
        if (hero != null)
        {
            HeroAI = hero;
            HeroLastPosition = hero.transform.position;
            return true;
        }
        return false;
    }
    public IDamagable Damagable() => this;
    public void GetAttacked(int damage) => Health -= damage;
    public int GetHealth() => Health;
    public Item Interact() => throw new System.Exception("Enemies aren't interactable yet");
    public IInteractable.Type InteractableType() => IInteractable.Type.Enemy;
    public bool IsActive() => !IsDead;
}