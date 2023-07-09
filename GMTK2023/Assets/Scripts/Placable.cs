using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Placable : MonoBehaviour
{
    public PlacedObject PlacedObjectPrefab;
    public PlacableData PlacableData;
    private SpriteRenderer Renderer;
    private void OnValidate()
    {
        if (PlacableData != null)
        {
            GetComponent<SpriteRenderer>().sprite = PlacableData.Sprite;
        }
    }
    private void Start()
    {
        if (PlacedObjectPrefab == null) throw new System.Exception($"{nameof(PlacedObjectPrefab)} shouldn't be null");
        if (PlacableData == null) throw new System.Exception($"{nameof(PlacedObjectPrefab)} shouldn't be null");
        Renderer = GetComponent<SpriteRenderer>();
        Renderer.sprite = PlacableData.Sprite;
    }
    private void OnMouseDown()
    {

        PlacedObject po = Instantiate(PlacedObjectPrefab, transform.position, transform.localRotation);
        po.PlacableData = PlacableData;
    }
    void Update()
    {

    }
}