using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(Movement))]
public class HeroAI : MonoBehaviour
{
    private int Health = 20;
    private int Damage = 1;
    private int VisionDistance = 50;
    private HashSet<PointOfIntrest> PointOfIntrests = new HashSet<PointOfIntrest>();
    private Vector2 Walkdirection = Vector2.up;
    private float WalkSpeed = 0f;
    private float MaxWalkSpeed = 2f;
    private float Boredom = 0f;
    private float QuitBordom = 10f;
    private float AttackCooldown = 0f;
    private float AttackCooldownSeconds = 0.5f;
    private float AttackDistance = 1.5f;
    private Movement Movement;
    //private List<> PastEnemies;
    private void Start()
    {
        Health = 20;
        Damage = 1;
        Movement = GetComponent<Movement>();

    }
    private void Update()
    {

    }
    private void FixedUpdate()
    {
        LookForInteractables();
        CheckIneractables();
        CheckHealth();
        CheckBoredom();
        AttackCooldown -= Time.fixedDeltaTime;
    }
    private void CheckBoredom()
    {
        if (Boredom >= QuitBordom)
        {
            Debug.Log("Hero Quit due to boredom");
        }
    }
    public void GetAttacked(int damage) => Health -= damage;
    private bool Attackable(PointOfIntrest poi)
        => transform.DistanceTo(poi.Position) <= AttackDistance;
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
    private void CheckIneractables()
    {
        if (PointOfIntrests.Count == 0)
        {
            float x = Random.Range(0f, 1f);
            float y = Random.Range(0f, 1f);
            Vector2 change = new Vector2(x, y);
            Walkdirection += change.normalized * Time.fixedDeltaTime;
            WalkSpeed = MaxWalkSpeed;
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
        List<PointOfIntrest> collectable = new List<PointOfIntrest>();
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
            //else if (poi.Interactable.InteractableType() == IInteractable.Type.Collectable) { collectable.Add(poi); }
        }
        if (enemies.Count > 0)
        {
            var attackable = enemies.Where(i => Attackable(i));
            if (attackable.Count() > 0)
            {
                var min = attackable.ToList().Min(i => ((IDamagable)i.Interactable).GetHealth());
                Attack(attackable.First(i => ((IDamagable)i.Interactable).GetHealth() == min));
            }
            else
            {
                var min = enemies.Min(i => transform.DistanceTo(i.Position));
                Movement.WalkTo(enemies.First(i => transform.DistanceTo(i.Position) == min).Position);
            }
        }
        else
        {
            //todo chest
            //todo collectable
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