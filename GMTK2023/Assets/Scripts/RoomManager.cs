using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public static List<PlacedObject> PlacedObjects = new List<PlacedObject>();
    public static string RoomSceneName = "Room";
    public static string RoomBuilderSceneName = "RoomBuilder";
    public static string QuitSceneName = "BoredomQuit";
    public static string HeroDiedName = "HeroDied";
    public GameObject HeroPrefab;
    public static GameObject StaticHeroPrefab;
    public static bool Subscribed = false;
    public void Start()
    {
        if (HeroPrefab != null)
        {
            StaticHeroPrefab = HeroPrefab;
        }
        if (!Subscribed)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            Subscribed = true;
        }
    }
    public static void ClearedRoom()
    {
        SceneManager.LoadScene(RoomBuilderSceneName);
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
        RoomData = GetRoomData();
        SceneManager.LoadScene(RoomSceneName);

    }
    private static List<(Vector2 pos, GameObject prefab, List<Item> content)> RoomData;
    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == RoomSceneName)
        {
            foreach (var data in RoomData)
            {
                var obj = GameObject.Instantiate(data.prefab, data.pos, Quaternion.identity);
                var Chest = obj.GetComponent<Chest>();
                if (Chest != null)
                {
                    Chest.Content = data.content;
                }
            }
        }
    }
    public static void BoredomQuit()
    {
        SceneManager.LoadScene(QuitSceneName);
    }

    public static void HeroDied()
    {
        SceneManager.LoadScene(HeroDiedName);
    }
}