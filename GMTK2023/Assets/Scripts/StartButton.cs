using UnityEngine;
public class StartButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        RoomManager.BuildNewRoom();
    }
}