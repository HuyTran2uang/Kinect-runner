using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public float MoveSpeed = 20;
    [HideInInspector] public float Space;
    [HideInInspector] public int OldSpace;
    public float ScaleSpace = 2;
    public int Score;
    public int SpaceSpawnEle = 15;
    public Transform PointSpawnEle;

    private void Start()
    {
        Load();
        StartGame();
    }

    private void Update()
    {
        SpawnOneItemOrEnemyEachSpace();
    }

    private void FixedUpdate()
    {
        Space += MoveSpeed * Time.deltaTime;
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey("HighestScore"))
        {
            int highestScore = PlayerPrefs.GetInt("HighesetScore");
            UIController.Instance.SetHighestScoreText(highestScore);
        }
    }

    private void StartGame()
    {
        SetScore(0);
    }

    private void SpawnOneItemOrEnemyEachSpace()
    {
        if ((int)Space % SpaceSpawnEle == 0 && (int)Space != OldSpace)
        {
            OldSpace = (int)(Space);
            bool isItem = UnityEngine.Random.Range(0, 2) == 1 ? true : false;
            if (isItem)
            {
                var item = ItemSpawner.Instance.Spawn();
                item.transform.position = Vector3.right * Random.Range(-1, 2) * ScaleSpace + Vector3.forward * PointSpawnEle.position.z;
                item.Init();
            }
            else
            {
                var enemy = EnemySpawner.Instance.Spawn();
                enemy.transform.position = Vector3.right * Random.Range(-1, 2) * ScaleSpace + Vector3.forward * PointSpawnEle.position.z;
                enemy.Init();
            }
        }
    }

    public void SetScore(int amount)
    {
        Score = amount;
        UIController.Instance.SetScoreText(Score);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void EndGame()
    {
        // save highest score
        if (PlayerPrefs.HasKey("HighestScore"))
        {
            int highestScore = PlayerPrefs.GetInt("HighesetScore");
            if (highestScore < Score)
            {
                PlayerPrefs.SetInt("HighestScore", Score);
            }
        }
        else
        {
            PlayerPrefs.SetInt("HighestScore", Score);
        }
        //

        // clear scene
        ItemSpawner.Instance.Clear();
    }
}