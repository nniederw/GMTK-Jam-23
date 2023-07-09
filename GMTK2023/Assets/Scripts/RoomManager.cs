using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public static List<PlacedObject> PlacedObjects = new List<PlacedObject>();
    public static string RoomSceneName = "Room";
    //public static string PrefabName;
    public static void ClearedRoom()
    {
        Debug.Log("Implement this function");
    }
    public static void AddPlacable(PlacedObject placedObject)
    {
        PlacedObjects.Add(placedObject);
    }
    public static void RemovePlacable(PlacedObject placedObject)
    {
        PlacedObjects.Remove(placedObject);
    }
    private static List<(Vector2 pos, GameObject prefab, List<Item> content)> GetRoomData()
    {
        var res = new List<(Vector2 pos, GameObject prefab, List<Item> content)>();
        PlacedObjects.ForEach(i => res.Add((i.transform.position.ToV2(), i.PlacableData.Prefab, i.Content)));
        return res;
    }
    public static void BuildNewRoom()
    {
        var roomData = GetRoomData();
        SceneManager.LoadScene(RoomSceneName);
        GameObject prefab = Resources.Load<GameObject>("Chest");
        GameObject.Instantiate(prefab);
        foreach (var data in roomData)
        {
            Debug.Log("Trying to Instantiate");
            var obj = GameObject.Instantiate(data.prefab, data.pos, Quaternion.identity);
            var Chest = obj.GetComponent<Chest>();
            if (Chest != null)
            {
                Chest.Content = data.content;
            }
        }
    }
}