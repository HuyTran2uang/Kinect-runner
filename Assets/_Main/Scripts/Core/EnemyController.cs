using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Item
{
    public List<Transform> models;

    private void OnEnable()
    {
        int randIndex = UnityEngine.Random.Range(0, models.Count);
        models.ForEach(i => i.gameObject.SetActive(false));
        models[randIndex].gameObject.SetActive(true);
    }

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
