//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[CreateAssetMenu]
//public class ElementSO : ScriptableObject
//{
//    [SerializeField] private int modelIndex;
//    [SerializeField] private int score;
//    public int ModelIndex { get => modelIndex; private set => modelIndex = value; }
//    public int Score { get => score; private set => score = value; }

//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/ElementSO")]
public class ElementSO : ScriptableObject
{
    [SerializeField] private int modelIndex;
    [SerializeField] private int score;
    [SerializeField] private GameObject pickupParticlePrefab; // thêm particle riêng cho element
    [Header("Spawn Settings")]
    [Range(0f, 1f)]
    public float spawnWeight = 1f;  // Trọng số xuất hiện (tần suất)

    public int ModelIndex { get => modelIndex; private set => modelIndex = value; }
    public int Score { get => score; private set => score = value; }
    public GameObject PickupParticlePrefab => pickupParticlePrefab; // cho phép đọc ra ngoài
}
