using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public float MoveSpeed = 20;
    [HideInInspector] public float Space;
    [HideInInspector] public int OldSpace;
    public float ScaleSpace = 2;
    public int Score;

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        SpawnOneItemEachSpace();
    }

    private void FixedUpdate()
    {
        Space += MoveSpeed * Time.deltaTime;
    }

    private void StartGame()
    {
        SetScore(0);
    }

    private void SpawnOneItemEachSpace()
    {
        if ((int)Space % 15 == 0 && (int)Space != OldSpace)
        {
            OldSpace = (int)(Space);
            var item = ItemSpawner.Instance.Spawn();
            item.transform.position = Vector3.right * Random.Range(-1, 2) * ScaleSpace + Vector3.forward * 100;
            item.Init();
        }
    }

    public void SetScore(int amount)
    {
        Score = amount;
    }

    public void IncreaseScore(int amount)
    {
        SetScore(Score + amount);
    }
}