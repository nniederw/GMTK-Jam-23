using UnityEngine;
[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public enum Type { HealthPotion, Weapon, Armor, KeyToNextRoom }
    public Type ItemType;
    public Sprite Sprite;
    public int Damage;
    public int DamageReduction;
    //int Health = 10;
}