using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class PlacedObject : MonoBehaviour
{
    public PlacableData PlacableData;
    private bool FollowMouse = true;
    SpriteRenderer Renderer;
    public List<Item> Content;
    public GameObject ChestSlots;
    private GameObject ChestSlotsActive;
    private void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Renderer.sprite = PlacableData.Sprite;
        RoomManager.AddPlacable(this);
    }
    public void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Destroy(gameObject);
        }
    }
    public void OnMouseUp()
    {
        if (PlacableData.PlacableType != PlacableData.Type.Item)
        {
            var pos = transform.position.ToV2();
            if (-5f <= pos.x && pos.x <= 5 && -5f <= pos.y && pos.y <= 5)
            {
                FollowMouse = false;
                if (PlacableData.PlacableType == PlacableData.Type.Chest)
                {
                    ChestSlotsActive = Instantiate(ChestSlots, transform);
                    var slots = ChestSlotsActive.GetComponentsInChildren<ChestSlot>();
                    var count = Content.Count;
                    if (count > 0)
                    {
                        slots[0].SetItem(Content[0]);
                    }
                    if (count > 1)
                    {
                        slots[1].SetItem(Content[1]);
                    }
                    if (count > 2)
                    {
                        slots[2].SetItem(Content[2]);
                    }
                }
            }
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var cols = Physics.RaycastAll(ray);
            bool foundSlot = false;
            foreach (var col in cols)
            {
                var slot = col.collider.gameObject.GetComponent<ChestSlot>();
                if (slot != null)
                {
                    slot.AddItem(PlacableData.Item);
                    foundSlot = true;
                    break;
                }
            }
            Debug.Log(foundSlot);
        }
    }
    public void OnMouseDown()
    {
        FollowMouse = true;
        if (PlacableData.PlacableType == PlacableData.Type.Chest && ChestSlotsActive != null)
        {
            Destroy(ChestSlotsActive);
        }
    }
    private void Update()
    {
        if (FollowMouse)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition).ToV2();
        }
        if (Input.GetMouseButtonDown(1) && PlacableData.PlacableType == PlacableData.Type.Chest)
        {

        }
    }
    private void OnDestroy()
    {
        RoomManager.RemovePlacable(this);
    }
}