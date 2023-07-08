using UnityEngine;
[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public int Health = 10;
    public int Damage = 1;
    public float AttackDistance = 2f;
    public float AttackCooldownSeconds = 0.5f;
    public int VisionDistance = 5;
}