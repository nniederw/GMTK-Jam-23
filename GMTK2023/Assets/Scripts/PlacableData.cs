using UnityEngine;
[CreateAssetMenu(fileName = "PlacableData", menuName = "PlacableData")]
public class PlacableData : ScriptableObject
{
    public Sprite Sprite;
    public GameObject Prefab;
    public Item Item;
    public Type PlacableType;
    public enum Type { Chest, Enemy, Item }
}