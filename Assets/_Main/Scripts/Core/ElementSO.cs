using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ElementSO : ScriptableObject
{
    [SerializeField] private int modelIndex;
    [SerializeField] private int score;

    public int ModelIndex { get => modelIndex; private set => modelIndex = value; }
    public int Score { get => score; private set => score = value; }
}
