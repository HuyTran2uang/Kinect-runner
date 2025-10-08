using DG.Tweening;
using DG.Tweening; // nhớ import DOTween
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public float MoveSpeed = 10;
    [HideInInspector] public float Space;
    [HideInInspector] public int OldSpace;
    public float ScaleSpace = 1.5f;
    public int Score;
    public int SpaceSpawnEle = 15;
    public Transform PointSpawnEle;
    public PlayerController playerController;
    
    public float gameDuration = 60f; // Thời gian đếm ngược ban đầu (giây)
    private float timeRemaining; // Thời gian còn lại
    [HideInInspector]
    public bool isGameRunning = false; // Trạng thái trò chơi
    public int defaultSpeed = 10;
    //public GameObject mainCamera; 
    
    public bool isStunned = false;
    [SerializeField] private float itemProbability = 0.35f;
    [SerializeField] private float[] enemyTypeWeights = new float[5] { 0.2f, 0.2f, 0.2f, 0.2f, 0.2f };


    private void Awake()
    {
        if (playerController != null)
        {
            Debug.Log("Here is the Player?");
        }
        
       
        KinectController.Instance.animator.SetTrigger("TrigIdle");
    }

    private void Start()
    {
        
        Load();
        KinectController.Instance.animator.SetTrigger("TrigIdle");
        //StartGame();
    }

    private void Update()
    {
        Debug.Log("isGameRunning: " + isGameRunning);
        if (isGameRunning)
        {
            //MoveSpeed = defaultSpeed;
            Debug.Log("isStunned: " + isStunned);
            // Nếu đang lock speed thì giảm timer
            if (isStunned)
            {
                MoveSpeed = 0;
            }
            else
            {
                if (timeRemaining <= 20)
                    MoveSpeed = defaultSpeed * 1.6f;
                else if (timeRemaining <= 40)
                    MoveSpeed = defaultSpeed * 1.3f;
                else
                    MoveSpeed = defaultSpeed;
            }

            UpdateTimer();
            SpawnOneItemOrEnemyEachSpace();
        } else
        if (!isGameRunning)
        {
            MoveSpeed = 0;
            
        }
    }

    private void FixedUpdate()
    {
        if (isGameRunning)
        {
            Space += MoveSpeed * Time.deltaTime;
        }
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey("HighestScore"))
        {
            int highestScore = PlayerPrefs.GetInt("HighestScore");
            UIController.Instance.SetHighestScoreText(highestScore);
        }

        if (PlayerPrefs.HasKey("LatestScore"))
        {
            int latestScore = PlayerPrefs.GetInt("LatestScore");
            UIController.Instance.SetScoreTextBegin(latestScore);
        }

        
    }

    private void StartGame()
    {
        SetScore(0);
        isGameRunning = true;
        AudioManager.StopBGM();
        AudioManager.PlayBGM(SoundType.BGM);
        //playerController.transform.rotation = Quaternion.Euler(0, 0, 0);
        //mainCamera.transform.position = Vector3.up * 2.25f + Vector3.back * 2.7f;
        timeRemaining = gameDuration; // Khởi tạo thời gian
        
        MoveSpeed = defaultSpeed;
        //GroundController.Instance.ResetGround();
        UIController.Instance.SetTimerText(timeRemaining); // Cập nhật UI ban đầu
        KinectController.Instance.animator.SetTrigger("TrigRun");
        // Tiếp tục âm thanh nền nếu bị tạm dừng
        
    }

    private void UpdateTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0) timeRemaining = 0; // Đảm bảo không âm
            UIController.Instance.SetTimerText(timeRemaining); // Cập nhật UI

            if (timeRemaining <= 20)
                dropDuration = 1.5f;
            else if (timeRemaining <= 40)
                dropDuration = 1.75f;
            else
                dropDuration = 2f;

        }
        else
        {
            EndGame();
        }
    }


    // Spawn đơn lẻ
    //private void SpawnOneItemOrEnemyEachSpace()
    //{
    //    if ((int)Space % SpaceSpawnEle == 0 && (int)Space != OldSpace)
    //    {
    //        OldSpace = (int)(Space);
    //        bool isItem = UnityEngine.Random.Range(0, 2) == 1;
    //        if (isItem)
    //        {
    //            var item = ItemSpawner.Instance.Spawn();
    //            item.transform.position = Vector3.right * Random.Range(-1, 2) * ScaleSpace + Vector3.forward * PointSpawnEle.position.z + Vector3.up * 1.0f;
    //            item.Init();
    //        }
    //        else
    //        {
    //            var enemy = EnemySpawner.Instance.Spawn();
    //            int flag = enemy.enemyIndex;
    //            switch (flag)
    //            {
    //                case 1:
    //                    {
    //                        enemy.transform.position = Vector3.right * Random.Range(-1, 2) * ScaleSpace + Vector3.forward * PointSpawnEle.position.z + Vector3.up * 1.6f;

    //                        // nếu có EnemyMover thì xóa để đứng yên
    //                        var mover = enemy.GetComponent<EnemyMover>();
    //                        if (mover != null) Destroy(mover);
    //                        break;
    //                    }

    //                case 0:
    //                    {
    //                        enemy.transform.position = Vector3.right * Random.Range(-1, 2) * ScaleSpace + Vector3.forward * PointSpawnEle.position.z;

    //                        // Thêm EnemyMover nếu chưa có
    //                        if (enemy.GetComponent<EnemyMover>() == null)
    //                        {
    //                            var mover = enemy.gameObject.AddComponent<EnemyMover>();
    //                            // Tùy chỉnh thông số EnemyMover nếu cần
    //                            mover.moveSpeed = 2f;
    //                            mover.moveRange = 1.5f;
    //                        }

    //                        break;
    //                    }

    //                case 2:
    //                    {
    //                        enemy.transform.position = Vector3.forward * PointSpawnEle.position.z + Vector3.up * 5f;

    //                        var mover = enemy.GetComponent<EnemyMover>();
    //                        if (mover != null) Destroy(mover);
    //                        break;
    //                    }

    //                case 3:
    //                    {
    //                        enemy.transform.position = Vector3.right * -1.0f * ScaleSpace + Vector3.forward * PointSpawnEle.position.z + Vector3.up * 5f;

    //                        var mover = enemy.GetComponent<EnemyMover>();
    //                        if (mover != null) Destroy(mover);
    //                        break;
    //                    }

    //                case 4:
    //                    {
    //                        enemy.transform.position = Vector3.right * ScaleSpace + Vector3.forward * PointSpawnEle.position.z + Vector3.up * 5f;

    //                        var mover = enemy.GetComponent<EnemyMover>();
    //                        if (mover != null) Destroy(mover);
    //                        break;
    //                    }
    //            }

    //            enemy.Init();
    //        }
    //    }
    //}

    //Spawn 1 
    //private void SpawnOneItemOrEnemyEachSpace()
    //{
    //    if ((int)Space % SpaceSpawnEle == 0 && (int)Space != OldSpace)
    //    {
    //        OldSpace = (int)(Space);

    //        // --- Tần suất xuất hiện (chỉnh trong Inspector) ---
    //        float itemProbability = 0.4f; // 40% Item, 60% Enemy
    //        int objectCount = UnityEngine.Random.Range(1, 3); // 1 hoặc 2 object

    //        // --- Lane để dành riêng cho Item ---
    //        int[] lanes = { -1, 0, 1 };
    //        // Shuffle lanes chỉ dùng cho Item
    //        for (int i = 0; i < lanes.Length; i++)
    //        {
    //            int j = UnityEngine.Random.Range(i, lanes.Length);
    //            (lanes[i], lanes[j]) = (lanes[j], lanes[i]);
    //        }

    //        int laneIndexForItem = 0; // con trỏ chạy lane cho Item

    //        for (int i = 0; i < objectCount; i++)
    //        {
    //            bool isItem = UnityEngine.Random.value < itemProbability;

    //            if (isItem)
    //            {
    //                if (laneIndexForItem >= lanes.Length) break; // hết lane trống

    //                int lane = lanes[laneIndexForItem++];
    //                var item = ItemSpawner.Instance.Spawn();
    //                item.transform.position = Vector3.right * lane * ScaleSpace
    //                                        + Vector3.forward * PointSpawnEle.position.z
    //                                        + Vector3.up * 1.0f;
    //                item.Init();
    //            }
    //            else
    //            {
    //                // Enemy: chọn lane ngẫu nhiên (không shuffle)
    //                int lane = UnityEngine.Random.Range(-1, 2);
    //                var enemy = EnemySpawner.Instance.Spawn();
    //                int flag = enemy.enemyIndex;
    //                Vector3 basePos = Vector3.right * lane * ScaleSpace
    //                                + Vector3.forward * PointSpawnEle.position.z;

    //                switch (flag)
    //                {
    //                    case 1:
    //                        enemy.transform.position = basePos + Vector3.up * 1.6f;
    //                        var mover1 = enemy.GetComponent<EnemyMover>();
    //                        if (mover1 != null) Destroy(mover1);
    //                        break;

    //                    case 0:
    //                        enemy.transform.position = basePos;
    //                        if (enemy.GetComponent<EnemyMover>() == null)
    //                        {
    //                            var mover = enemy.gameObject.AddComponent<EnemyMover>();
    //                            mover.moveSpeed = 2f;
    //                            mover.moveRange = 1.5f;
    //                        }
    //                        break;

    //                    case 2:
    //                        enemy.transform.position = basePos + Vector3.up * 5f;
    //                        var mover2 = enemy.GetComponent<EnemyMover>();
    //                        if (mover2 != null) Destroy(mover2);
    //                        break;

    //                    case 3:
    //                        enemy.transform.position = Vector3.right * -1.0f * ScaleSpace
    //                                                + Vector3.forward * PointSpawnEle.position.z
    //                                                + Vector3.up * 5f;
    //                        var mover3 = enemy.GetComponent<EnemyMover>();
    //                        if (mover3 != null) Destroy(mover3);
    //                        break;

    //                    case 4:
    //                        enemy.transform.position = Vector3.right * ScaleSpace
    //                                                + Vector3.forward * PointSpawnEle.position.z
    //                                                + Vector3.up * 5f;
    //                        var mover4 = enemy.GetComponent<EnemyMover>();
    //                        if (mover4 != null) Destroy(mover4);
    //                        break;
    //                }

    //                enemy.Init();
    //            }
    //        }
    //    }
    //}

    //private void SpawnOneItemOrEnemyEachSpace()
    //{
    //    if ((int)Space % SpaceSpawnEle == 0 && (int)Space != OldSpace)
    //    {
    //        OldSpace = (int)(Space);

    //        float itemProbability = 0.35f;
    //        bool spawnItemThisRound = UnityEngine.Random.value < itemProbability;

    //        int objectCount = UnityEngine.Random.Range(1, 3); // 1 hoặc 2

    //        // shuffle lanes và dùng lần lượt (đảm bảo không spawn 2object cùng lane)
    //        List<int> lanes = new List<int> { -1, 0, 1 };
    //        for (int i = 0; i < lanes.Count; i++)
    //        {
    //            int j = UnityEngine.Random.Range(i, lanes.Count);
    //            (lanes[i], lanes[j]) = (lanes[j], lanes[i]);
    //        }

    //        if (spawnItemThisRound)
    //        {
    //            for (int i = 0; i < objectCount && i < lanes.Count; i++)
    //            {
    //                int lane = lanes[i];
    //                var item = ItemSpawner.Instance.Spawn();
    //                item.transform.position = new Vector3(lane * ScaleSpace, 1.0f, PointSpawnEle.position.z);
    //                item.Init();
    //            }
    //        }
    //        else // spawn enemies
    //        {
    //            HashSet<int> usedFlags = new HashSet<int>();
    //            bool spawnedCase0 = false;
    //            bool spawnedCase1 = false;

    //            for (int i = 0; i < objectCount && i < lanes.Count; i++)
    //            {
    //                int lane = lanes[i];

    //                // spawn inactive enemy from pool
    //                var enemy = EnemySpawner.Instance.Spawn(); // spawn returns enemy instance (still inactive ideally)

    //                // chọn flag hợp lệ (không trùng, không cho 0 và 1 cùng lúc)
    //                int flag = enemy.enemyIndex; // fallback
    //                int tries = 0;
    //                // thử chọn ngẫu nhiên trong 0..4 nhưng tránh trùng và tránh 0/1 conflict
    //                do
    //                {
    //                    flag = UnityEngine.Random.Range(0, 5); // 0..4
    //                    tries++;
    //                    if (tries > 20) break;
    //                }
    //                while (usedFlags.Contains(flag) || (flag == 0 && spawnedCase1) || (flag == 1 && spawnedCase0));

    //                // nếu vẫn conflict (hiếm), ép chọn 2..4
    //                if (usedFlags.Contains(flag) || (flag == 0 && spawnedCase1) || (flag == 1 && spawnedCase0))
    //                {
    //                    int c = 0;
    //                    do
    //                    {
    //                        flag = UnityEngine.Random.Range(2, 5);
    //                        c++;
    //                    } while (usedFlags.Contains(flag) && c < 10);
    //                }

    //                // thiết lập model trước khi bật
    //                enemy.Setup(flag);

    //                // đánh dấu
    //                usedFlags.Add(flag);
    //                if (flag == 0) spawnedCase0 = true;
    //                if (flag == 1) spawnedCase1 = true;

    //                // đặt vị trí tùy flag
    //                Vector3 basePos = new Vector3(lane * ScaleSpace, PointSpawnEle.position.y, PointSpawnEle.position.z);
    //                switch (flag)
    //                {
    //                    case 1:
    //                        enemy.transform.position = basePos + Vector3.up * 1.55f;
    //                        var mover1 = enemy.GetComponent<EnemyMover>();
    //                        if (mover1 != null) Destroy(mover1);
    //                        break;
    //                    case 0:
    //                        enemy.transform.position = basePos;
    //                        if (enemy.GetComponent<EnemyMover>() == null)
    //                        {
    //                            var mover = enemy.gameObject.AddComponent<EnemyMover>();
    //                            mover.moveSpeed = 2f;
    //                            mover.moveRange = 1.5f;
    //                        }
    //                        break;
    //                    case 2:
    //                        enemy.transform.position = basePos + Vector3.up * 6f;
    //                        var mover2 = enemy.GetComponent<EnemyMover>();
    //                        if (mover2 != null) Destroy(mover2);
    //                        break;
    //                    case 3:
    //                        enemy.transform.position = new Vector3(-1.0f * ScaleSpace, PointSpawnEle.position.y + 6f, PointSpawnEle.position.z);
    //                        var mover3 = enemy.GetComponent<EnemyMover>();
    //                        if (mover3 != null) Destroy(mover3);
    //                        break;
    //                    case 4:
    //                        enemy.transform.position = new Vector3(ScaleSpace, PointSpawnEle.position.y + 6f, PointSpawnEle.position.z);
    //                        var mover4 = enemy.GetComponent<EnemyMover>();
    //                        if (mover4 != null) Destroy(mover4);
    //                        break;
    //                }

    //                // bật/khởi tạo enemy (Init sẽ SetActive(true))
    //                enemy.Init();
    //            }
    //        }
    //    }
    //}


    private float dropDuration = 2f; 
    

    private void SpawnOneItemOrEnemyEachSpace()
    {
        if ((int)Space % SpaceSpawnEle == 0 && (int)Space != OldSpace)
        {
            OldSpace = (int)(Space);

            float itemProbability = 0.35f;
            bool spawnItemThisRound = UnityEngine.Random.value < itemProbability;

            int objectCount = UnityEngine.Random.Range(1, 3); // 1 hoặc 2

            // shuffle lanes và dùng lần lượt
            List<int> lanes = new List<int> { -1, 0, 1 };
            for (int i = 0; i < lanes.Count; i++)
            {
                int j = UnityEngine.Random.Range(i, lanes.Count);
                (lanes[i], lanes[j]) = (lanes[j], lanes[i]);
            }

            if (spawnItemThisRound)
            {
                for (int i = 0; i < objectCount && i < lanes.Count; i++)
                {
                    int lane = lanes[i];
                    var item = ItemSpawner.Instance.Spawn();
                    item.transform.position = new Vector3(lane * ScaleSpace, 1.0f, PointSpawnEle.position.z);
                    item.Init();
                }
            }
            else // spawn enemies
            {
                HashSet<int> usedFlags = new HashSet<int>();
                bool spawnedCase0 = false;
                bool spawnedCase1 = false;

                for (int i = 0; i < objectCount && i < lanes.Count; i++)
                {
                    int lane = lanes[i];
                    var enemy = EnemySpawner.Instance.Spawn();

                    // chọn flag hợp lệ
                    int flag;
                    int tries = 0;
                    do
                    {
                        flag = UnityEngine.Random.Range(0, 5);
                        tries++;
                        if (tries > 20) break;
                    }
                    while (usedFlags.Contains(flag) || (flag == 0 && spawnedCase1) || (flag == 1 && spawnedCase0));

                    if (usedFlags.Contains(flag) || (flag == 0 && spawnedCase1) || (flag == 1 && spawnedCase0))
                    {
                        int c = 0;
                        do
                        {
                            flag = UnityEngine.Random.Range(2, 5);
                            c++;
                        } while (usedFlags.Contains(flag) && c < 10);
                    }

                    // setup model
                    enemy.Setup(flag);

                    usedFlags.Add(flag);
                    if (flag == 0) spawnedCase0 = true;
                    if (flag == 1) spawnedCase1 = true;

                    // vị trí ban đầu
                    Vector3 basePos = new Vector3(lane * ScaleSpace, PointSpawnEle.position.y, PointSpawnEle.position.z);

                    switch (flag)
                    {
                        case 1:
                            enemy.transform.position = basePos + Vector3.up * 1.55f;
                            var mover1 = enemy.GetComponent<EnemyMover>();
                            if (mover1 != null) Destroy(mover1);
                            break;
                        case 0:
                            enemy.transform.position = basePos;
                            if (enemy.GetComponent<EnemyMover>() == null)
                            {
                                var mover = enemy.gameObject.AddComponent<EnemyMover>();
                                mover.moveSpeed = 2f;
                                mover.moveRange = 1.5f;
                            }
                            break;
                        case 2:
                            enemy.transform.position = basePos + Vector3.up * 6f;
                            var mover2 = enemy.GetComponent<EnemyMover>();
                            if (mover2 != null) Destroy(mover2);

                            // 🔹 chỉnh collider
                            BoxCollider col2 = enemy.GetComponent<BoxCollider>();
                            if (col2 != null)
                            {
                                col2.center = new Vector3(0f, 1.34f, 0f);     // thay đổi kích thước collider
                                col2.size = new Vector3(1.34f, 1.834807f, 0.66f);  // di chuyển tâm collider
                            }

                            break;
                        case 3:
                            enemy.transform.position = new Vector3(-1.0f * ScaleSpace, PointSpawnEle.position.y + 6f, PointSpawnEle.position.z);
                            var mover3 = enemy.GetComponent<EnemyMover>();
                            if (mover3 != null) Destroy(mover3);

                            // 🔹 chỉnh collider
                            BoxCollider col3 = enemy.GetComponent<BoxCollider>();
                            if (col3 != null)
                            {
                                col3.center = new Vector3(0f, 1.34f, 0f);     // thay đổi kích thước collider
                                col3.size = new Vector3(1.34f, 1.834807f, 0.66f);  // di chuyển tâm collider
                            }

                            break;
                        case 4:
                            enemy.transform.position = new Vector3(ScaleSpace, PointSpawnEle.position.y + 6f, PointSpawnEle.position.z);
                            var mover4 = enemy.GetComponent<EnemyMover>();
                            if (mover4 != null) Destroy(mover4);

                            BoxCollider col4 = enemy.GetComponent<BoxCollider>();
                            if (col4 != null)
                            {
                                col4.center = new Vector3(0f, 1.34f, 0f);     // thay đổi kích thước collider
                                col4.size = new Vector3(1.34f, 1.834807f, 0.66f);  // di chuyển tâm collider
                            }

                            break;
                    }

                    // bật enemy
                    enemy.Init();

                    //// 🔹 Nếu flag = 2,3,4 → có tỉ lệ rơi xuống
                    //if (flag >= 2 && flag <= 4)
                    //{
                    //    if (Random.value < 0.9f) // 50% tỉ lệ rơi
                    //    {
                    //        enemy.transform
                    //             .DOMoveY(-6.17f, 7f) // rơi xuống trong 3 giây
                    //             .SetEase(Ease.Linear)
                    //             .OnComplete(() =>
                    //             {
                    //                 if (enemy != null && enemy.gameObject.activeSelf)
                    //                 {
                    //                     enemy.gameObject.SetActive(false);
                    //                 }
                    //             });
                    //    }
                    //}

                    // 🔹 Nếu flag = 2,3,4 → có tỉ lệ rơi khi player lại gần
                    if (flag >= 2 && flag <= 4)
                    {
                        Debug.Log("cmm1");
                        float dropChance = 0.6f; // 50% tỉ lệ rơi
                        float flagChance = Random.value;
                        if ( flagChance < dropChance)
                        {
                            Debug.Log("rate: " + flagChance);
                            // Bắt đầu coroutine để chờ player lại gần
                            StartCoroutine(WaitAndDrop(enemy.transform));
                        }
                    }


                }
            }
        }
    }


    private IEnumerator WaitAndDrop(Transform enemy)
    {
        // Lặp cho đến khi enemy bị disable hoặc player lại gần
        while (enemy != null && enemy.gameObject.activeSelf)
        {
            // Khoảng cách giữa player và enemy
            float distance = Vector3.Distance(playerController.transform.position, enemy.position);

            //Debug.Log("Diss: " + distance);

            if (distance <= 10f) // ~2cm (vì đơn vị Unity = mét)
            {
                // Khi lại gần → cho rơi xuống
                enemy.DOMoveY(-6.17f, dropDuration) // rơi trong 3 giây
                     .SetEase(Ease.Linear)
                     .OnComplete(() =>
                     {
                         if (enemy != null && enemy.gameObject.activeSelf)
                             enemy.gameObject.SetActive(false);
                     });
                Debug.Log("cmm");
                yield break; // thoát vòng lặp
            }

            yield return null; // chờ frame tiếp theo
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
        isGameRunning = false; // Dừng bộ đếm khi tạm dừng
    }

    public void Resume()
    {
        Time.timeScale = 1;
        isGameRunning = true; // Tiếp tục bộ đếm
    }

    public void EndGame()
    {
        AudioManager.StopBGM();
        Debug.Log("End");
        isGameRunning = false; // Dừng trò chơi
        // Lưu điểm cao nhất
        if (PlayerPrefs.HasKey("HighestScore"))
        {
            int highestScore = PlayerPrefs.GetInt("HighestScore");
            if (highestScore < Score)
            {
                PlayerPrefs.SetInt("HighestScore", Score);
                UIController.Instance.SetHighestScoreText(Score);
            }
        }
        else
        {
            Debug.Log("End1");
            PlayerPrefs.SetInt("HighestScore", Score);
            UIController.Instance.SetHighestScoreText(Score);
        }

        //if (PlayerPrefs.HasKey("LatestScore"))
        ////{
        ////    int Score = PlayerPrefs.GetInt("LatestScore");
        ////    if (highestScore < Score)
        ////    {
        ////        PlayerPrefs.SetInt("LatestScore", Score);
        ////        UIController.Instance.SetHighestScoreText(Score);
        ////    }
        ////}
        ////else
        ////{
        ////    Debug.Log("End1");
        ////    PlayerPrefs.SetInt("LatestScore", Score);
        ////    UIController.Instance.SetHighestScoreText(Score);
        ////}
        
        PlayerPrefs.SetInt("LatestScore", Score);


        //UIController.Instance.ShowMainMenu();
        

        bool isVicToCum = true;
        if (Score >= 250)
        {
            playerController.VictoCumPose();
            isVicToCum = true;
        }
        else
        {
            playerController.LoserPose();
            isVicToCum = false;
        }
        UIController.Instance.ShowGameOver(isVicToCum);
        Debug.Log("End2");
        // Xóa scene
        ItemSpawner.Instance.Clear();
        Debug.Log("End3");
        EnemySpawner.Instance.Clear();
        Debug.Log("End4");
        // Ngắt hết tham chiếu đến enemy còn sót
        StopAllCoroutines();
        //UIController.Instance.ShowGameOver(); // Hiển thị màn hình game over
        Debug.Log("End5");
        // 🔹 Gọi coroutine chờ 7 giây rồi load scene
        StartCoroutine(WaitAndLoadScene("Main Art 1", 7f));
    }

    private IEnumerator WaitAndLoadScene(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadSceneByName(sceneName);
    }

    [ContextMenu("Reset Highest Score")]
    public void ResetHighestScore()
    {
        PlayerPrefs.DeleteKey("HighestScore");
        PlayerPrefs.Save();
        Debug.Log("HighestScore reset!");
    }

    // ✅ Hàm này để gắn vào Button
    public void StartGameFromButton()
    {
        Debug.Log("Game started from button!");
        StartGame();
        //UIController.Instance.HideGameOver(); // ẩn màn hình game over nếu có
    }

    public void AddToTimeRemain()
    {
        timeRemaining += 5;
    }

    // Chuyển scene theo tên
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}