using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Chest : MonoBehaviour, IInteractable
{
    public List<Item> Content = new List<Item>();
    public bool Looted = false;
    public bool Active = true;
    public IDamagable Damagable()
    => throw new System.Exception("A chest isn't Damagable");
    public List<Item> Interact()
    {
        Active = false;
        if (!Looted)
        {
            Looted = true;
            return Content;
        }
        return new List<Item>();
    }
    public IInteractable.Type InteractableType() => IInteractable.Type.Chest;
    public bool IsActive() => Active;
    private void OnDestroy()
    {
        Active = false;
    }
    private void OnValidate()
    {
        if (Content.Where(i => i == null).Any())
        {
            Debug.Log($"Please change the null entry in the Content field of the chest");
        }
    }
}