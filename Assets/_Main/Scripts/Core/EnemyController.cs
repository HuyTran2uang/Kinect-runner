using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Item
{
    public override void Receive(PlayerController receiver)
    {
        int score = 50;
        receiver.DecreaseScore(score);
        gameObject.SetActive(false);
    }

    protected override void OnDisable()
    {
        EnemySpawner.Instance.AddToPool(this);
    }
}
