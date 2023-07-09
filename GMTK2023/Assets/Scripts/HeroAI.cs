using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(Movement))]
public class HeroAI : MonoBehaviour
{
    [SerializeField] public int Health = 20;
    public int Damage => Weapon == null ? 1 : Weapon.Damage + 1;
    private int VisionDistance = 50;
    private HashSet<PointOfIntrest> PointOfIntrests = new HashSet<PointOfIntrest>();
    private float Boredom = 0f;
    private float QuitBordom = 10f;
    private float AttackCooldown = 0f;
    private float AttackCooldownSeconds = 0.5f;
    private float ReachDistance = 1.5f;
    private Movement Movement;
    private const int HealthPotionHealing = 10;
    public int HealthPotions = 0;
    private int EnemiesCountApprox = 0;
    private Item Weapon = null;
    private Item Armor = null;
    [SerializeField] private string CurrentGoal = "";
    //private List<> PastEnemies;
    private void Start()
    {
        Health = 20;
        Movement = GetComponent<Movement>();
    }
    private void FixedUpdate()
    {
        LookForInteractables();
        CheckPointOfInterests();
        CheckHealth();
        CheckBoredom();
        ConsiderHealthPotion();
        AttackCooldown -= Time.fixedDeltaTime;
    }
    private void ConsiderHealthPotion()
    {
        if (HealthPotions > 0 && EnemiesCountApprox > 0 && Health <= 6)
        {
            HealthPotions--;
            Health += HealthPotionHealing;
        }
    }
    private void CheckBoredom()
    {
        if (Boredom >= QuitBordom)
        {
            Debug.Log("Hero Quit due to boredom");
        }
    }
    public void GetAttacked(int damage)
    {
        if (Armor != null)
        {
            Health -= System.Math.Max(damage - Armor.DamageReduction, 0);
            return;
        }
        Health -= damage;
    }
    private bool Attackable(PointOfIntrest poi)
        => transform.DistanceTo(poi.Position) <= ReachDistance;
    private void Attack(PointOfIntrest poi)
    {
        if (!Attackable(poi)) throw new System.Exception("Hero tried to attack an poi out of reach");
        if (poi.Interactable.InteractableType() != IInteractable.Type.Enemy) throw new System.Exception("Hero tried to attack a non enemy");
        if (AttackCooldown > 0f) return;
        ((IDamagable)poi.Interactable).GetAttacked(Damage);
        AttackCooldown = AttackCooldownSeconds;
    }
    private void CheckHealth()
    {
        if (Health <= 0)
        {
            Debug.Log("Hero Died");
        }
    }
    private void CheckPointOfInterests()
    {
        if (PointOfIntrests.Count == 0)
        {
            Movement.WalkToRandom();
        }
        HashSet<PointOfIntrest> activePoi = new HashSet<PointOfIntrest>();
        foreach (var poi in PointOfIntrests)
        {
            if (poi.Interactable.IsActive())
            {
                activePoi.Add(poi);
            }
        }
        PointOfIntrests = activePoi;
        List<PointOfIntrest> enemies = new List<PointOfIntrest>();
        List<PointOfIntrest> chest = new List<PointOfIntrest>();
        PointOfIntrest? exit = null;
        //List<PointOfIntrest> collectable = new List<PointOfIntrest>();
        foreach (var poi in PointOfIntrests)
        {
            if (poi.Interactable.InteractableType() == IInteractable.Type.Enemy)
            {
                enemies.Add(poi);
            }
            else if (poi.Interactable.InteractableType() == IInteractable.Type.Chest)
            {
                chest.Add(poi);
            }
            else if (poi.Interactable.InteractableType() == IInteractable.Type.Exit)
            {
                if (exit != null) throw new System.Exception("Are there multiple exits? The Hero found 2");
                exit = poi;
            }
            //else if (poi.Interactable.InteractableType() == IInteractable.Type.Collectable) { collectable.Add(poi); }
        }
        EnemiesCountApprox = enemies.Count;
        if (enemies.Count > 0)
        {
            var attackable = enemies.Where(i => Attackable(i));
            if (attackable.Count() > 0)
            {
                var min = attackable.ToList().Min(i => ((IDamagable)i.Interactable).GetHealth());
                var minEn = attackable.First(i => ((IDamagable)i.Interactable).GetHealth() == min);
                Attack(minEn);
                CurrentGoal = $"Attacking enemy";
            }
            else
            {
                var min = enemies.Min(i => transform.DistanceTo(i.Position));
                var minEn = enemies.First(i => transform.DistanceTo(i.Position) == min);
                Movement.WalkTo(minEn.Position);
                CurrentGoal = $"Walking to enemy at {minEn.Position}";
            }
        }
        else if (chest.Count > 0)
        {

            var min = chest.Min(i => transform.DistanceTo(i.Position));
            var minChest = chest.First(i => transform.DistanceTo(i.Position) == min);
            if (min <= ReachDistance)
            {
                AddToInventory(minChest.Interactable.Interact());

                CurrentGoal = $"Opening Chest";
            }
            else
            {
                Movement.WalkTo(minChest.Position);
                CurrentGoal = $"Walk to Chest at {minChest.Position}";
            }
        }
        else
        {
            if (exit == null)
            {
                throw new System.Exception("Hero can't find exit");
            }
            if (transform.DistanceTo(exit.Value.Position) <= ReachDistance)
            {
                AddToInventory(exit.Value.Interactable.Interact());
                CurrentGoal = $"Opening Exit";
            }
            else
            {
                Movement.WalkTo(exit.Value.Position);
                CurrentGoal = $"Walking to Exit at {exit.Value.Position}";
            }
        }

    }
    private void AddToInventory(List<Item> items) => items.ForEach(AddToInventory);
    private void AddToInventory(Item item)
    {
        switch (item.ItemType)
        {
            case Item.Type.HealthPotion:
                HealthPotions++;
                break;
            case Item.Type.Weapon:
                if (Weapon == null || item.Damage > Weapon.Damage)
                {
                    Weapon = item;
                }
                break;
            case Item.Type.Armor:
                if (Armor == null || item.DamageReduction > Armor.DamageReduction)
                {
                    Armor = item;
                }
                break;
            case Item.Type.KeyToNextRoom:
                RoomManager.ClearedRoom();
                break;
        }
    }
    private void LookForInteractables()
    {
        var cols = Physics2D.OverlapCircleAll(transform.position, VisionDistance);
        foreach (var col in cols)
        {
            if (col.gameObject == this) continue;
            var i = col.gameObject.GetComponent<IInteractable>();
            if (i == null) continue;
            PointOfIntrests.Add(new PointOfIntrest(i, col.transform.position));
        }
    }
}
public struct PointOfIntrest
{
    public IInteractable Interactable;
    public Vector2 Position;
    public PointOfIntrest(IInteractable interactable, Vector2 position)
    {
        Interactable = interactable;
        Position = position;
    }
}