//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemyController : Item
//{
//    public List<Transform> enemyModels;
//    public GameObject particleHit;
//    public int enemyIndex { get; set; }
//    private void OnEnable()
//    {
//        int randIndex = UnityEngine.Random.Range(0, enemyModels.Count);
//        enemyModels.ForEach(i => i.gameObject.SetActive(false));
//        var _enemyModel = enemyModels[randIndex];
//        _enemyModel.gameObject.SetActive(true);
//        enemyIndex = randIndex;
//    }

//    public override void Receive(PlayerController receiver)
//    {
//        int score = 0;
//        receiver.IncreaseScore(score);
//        gameObject.SetActive(false);
//        GameObject particle = Instantiate(particleHit, transform.position + Vector3.up * 2.8f, Quaternion.identity);
//        Destroy(particle, 0.7f); // tự xóa sau 2 giây
//    }

//    protected override void OnDisable()
//    {
//        EnemySpawner.Instance.AddToPool(this);
//    }
//}

//using System.Collections.Generic;
//using UnityEngine;

//public class EnemyController : Item
//{
//    [SerializeField] private int damageScore = 50;
//    public List<GameObject> models;  // Đổi từ Transform sang GameObject

//    private void OnEnable()
//    {
//        if (models == null || models.Count == 0) return;

//        int randIndex = Random.Range(0, models.Count);

//        // Tắt tất cả models
//        models.ForEach(m => m.SetActive(false));

//        // Bật 1 model ngẫu nhiên
//        models[randIndex].SetActive(true);
//    }

//    public override void Receive(PlayerController receiver)
//    {
//        if (receiver != null)
//        {
//            receiver.DecreaseScore(damageScore);
//        }

//        // Có thể spawn particle tại đây
//        // Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);

//        gameObject.SetActive(false);
//    }

//    protected override void OnDisable()
//    {
//        // Enemy chỉ về EnemySpawner, không dùng ItemSpawner
//        if (EnemySpawner.Instance != null)
//        {
//            EnemySpawner.Instance.AddToPool(this);
//        }
//    }
//}

using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Item
{
    public List<Transform> enemyModels;
    public GameObject particleHit;

    // public getter private setter để chỉ set qua Setup()
    public int enemyIndex { get; private set; } = -1;

    // OnEnable: bỏ random model ở đây (nếu cần làm animation khi active, vẫn có thể dùng)
    private void OnEnable()
    {
        // giữ trống hoặc làm animation start — không random model ở đây
    }

    // Thiết lập model theo index (gọi trước khi SetActive(true)/Init())
    public void Setup(int index = -1)
    {
        if (enemyModels == null || enemyModels.Count == 0) return;

        int idx = index;
        if (idx < 0 || idx >= enemyModels.Count) idx = Random.Range(0, enemyModels.Count);
        enemyIndex = idx;

        // bật model tương ứng, tắt các model khác
        for (int i = 0; i < enemyModels.Count; i++)
        {
            if (enemyModels[i] != null)
                enemyModels[i].gameObject.SetActive(i == idx);
        }
    }

    public override void Receive(PlayerController receiver)
    {
        int score = 0;
        receiver.IncreaseScore(score);

        // lưu vị trí trước khi disable
        Vector3 hitPos = transform.position;
        gameObject.SetActive(false);

        if (particleHit != null)
        {
            GameObject particle = Instantiate(particleHit, hitPos + Vector3.up * 2.8f, Quaternion.identity);
            Destroy(particle, 0.7f);
        }
    }

    protected override void OnDisable()
    {
        // trả về pool (Item.OnDisable() không được gọi nếu bạn override, nên gọi pool riêng)
        EnemySpawner.Instance.AddToPool(this);
    }
}
