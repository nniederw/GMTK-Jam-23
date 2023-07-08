using System;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;
[RequireComponent(typeof(Movement))]
public class EnemyObject : MonoBehaviour, IDamagable
{
    public Enemy EnemyType;
    private Movement Movement;
    private int Health;
    private bool Active = true;
    private HeroAI HeroAI;
    private Vector2 HeroLastPosition;
    private float AttackCooldown = 0f;
    private void OnValidate()
    {
        if (EnemyType != null)
        {
            SetSprite();
        }
    }
    private void Start()
    {
        Movement = GetComponent<Movement>();
        Health = EnemyType.Health;
        Active = true;
        SetSprite();
    }
    private void SetSprite()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = EnemyType.Sprite;
        }
    }
    private void FixedUpdate()
    {
        if (Health < 0)
        {
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
    public List<Item> Interact() => new List<Item>();
    public IInteractable.Type InteractableType() => IInteractable.Type.Enemy;
    public bool IsActive() => Active;
    private void OnDestroy()
    {
        Active = false;
    }
}