using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPrefab : MonoBehaviour
{
    [SerializeField] private Vector2Int roomSize = new Vector2Int(0,0);

    [SerializeField] private GameObject roomPrefab;

    public Vector2Int GetRoomPrefabSize(){
        return roomSize;
    }

    public GameObject GetRoomPrefab(){
        return roomPrefab;
    }
}
