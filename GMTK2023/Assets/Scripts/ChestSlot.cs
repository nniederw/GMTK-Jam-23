using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider), typeof(SpriteRenderer))]
public class ChestSlot : MonoBehaviour
{
    [SerializeField] private Item Item;
    private SpriteRenderer Renderer;
    [SerializeField] private Sprite Default;
    PlacedObject PlacedObjectParent;
    private void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Renderer.sprite = Default;
        PlacedObjectParent = GetComponentInParent<PlacedObject>();
    }
    public void SetItem(Item item)
    {
        Item = item;
        Renderer.sprite = item.Sprite;
    }
    public void AddItem(Item item)
    {
        SetItem(item);
        PlacedObjectParent.Content.Add(item);
    }
    public void RemoveItem(Item item)
    {
        Item = null;
        Renderer.sprite = Default;
        PlacedObjectParent.Content.Remove(item);
    }
    private void OnMouseDown()
    {
        RemoveItem(Item);
    }
}