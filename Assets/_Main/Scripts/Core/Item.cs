//using System.Linq;
//using UnityEngine;

//public class Item : MonoBehaviour
//{
//    public GameObject[] models;
//    public ElementSO[] baseElements;
//    private ElementSO ele;

//    public void Init()
//    {
//        models.ToList().ForEach(i => i.gameObject.SetActive(false));

//        ele = baseElements[Random.Range(0, baseElements.Length)];

//        models[ele.ModelIndex].SetActive(true);

//        gameObject.SetActive(true);
//    }

//    private void FixedUpdate()
//    {
//        transform.position += GameManager.Instance.MoveSpeed * Vector3.back * Time.deltaTime;
//    }

//    private void Update()
//    {
//        if (transform.position.z < -7)
//        {
//            gameObject.SetActive(false);
//        }
//    }

//    protected virtual void OnDisable()
//    {
//        ItemSpawner.Instance.AddToPool(this);
//    }

//    public virtual void Receive(PlayerController receiver)
//    {
//        receiver.IncreaseScore(ele.Score);
//        gameObject.SetActive(false);
//    }
//}

using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject[] models;
    public ElementSO[] baseElements;
    private ElementSO ele;

    //public void Init()
    //{
    //    // Tắt toàn bộ model
    //    foreach (var model in models)
    //    {
    //        if (model != null)
    //            model.SetActive(false);
    //    }

    //    // Kiểm tra mảng baseElements có rỗng không
    //    if (baseElements == null || baseElements.Length == 0)
    //    {
    //        Debug.LogWarning("baseElements is empty!");
    //        return;
    //    }

    //    // Random element
    //    ele = baseElements[Random.Range(0, baseElements.Length)];

    //    // Bảo vệ index
    //    if (ele.ModelIndex < 0 || ele.ModelIndex >= models.Length)
    //    {
    //        Debug.LogWarning($"Invalid ModelIndex {ele.ModelIndex} for element {ele.name}");
    //        return;
    //    }

    //    // Kích hoạt model tương ứng
    //    models[ele.ModelIndex].SetActive(true);

    //    gameObject.SetActive(true);
    //}

    private ElementSO GetRandomElement()
    {
        float totalWeight = 0;
        foreach (var e in baseElements)
        {
            totalWeight += e.spawnWeight;
        }

        float r = Random.Range(0, totalWeight);
        float sum = 0;

        foreach (var e in baseElements)
        {
            sum += e.spawnWeight;
            if (r <= sum)
                return e;
        }

        return baseElements[0]; // fallback
    }

    public void Init()
    {
        foreach (var model in models)
            if (model != null) model.SetActive(false);

        if (baseElements == null || baseElements.Length == 0)
        {
            Debug.LogWarning("baseElements is empty!");
            return;
        }

        // Chọn element có trọng số
        ele = GetRandomElement();

        if (ele.ModelIndex < 0 || ele.ModelIndex >= models.Length)
        {
            Debug.LogWarning($"Invalid ModelIndex {ele.ModelIndex} for element {ele.name}");
            return;
        }

        models[ele.ModelIndex].SetActive(true);
        gameObject.SetActive(true);
    }

    private void Update()
    {
        // Di chuyển lùi
        transform.position += GameManager.Instance.MoveSpeed * Vector3.back * Time.deltaTime;

        // Nếu vượt khỏi màn hình thì ẩn
        if (transform.position.z < -2.5f)
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void OnDisable()
    {
        if (ItemSpawner.Instance != null)
        {
            ItemSpawner.Instance.AddToPool(this);
        }
    }

    public virtual void Receive(PlayerController receiver)
    {
        if (ele != null && receiver != null)
        {   
            int _score = ele.Score;
            if (_score == 99999)
            {
                Debug.Log("I need 5 more bullets!!!");
                GameManager.Instance.AddToTimeRemain();
            }
            else
                receiver.IncreaseScore(_score);
        }

        // Spawn particle theo ElementSO
        if (ele != null && ele.PickupParticlePrefab != null)
        {
            GameObject particle = Instantiate(ele.PickupParticlePrefab, transform.position + Vector3.up * 2.8f, Quaternion.identity);
            Destroy(particle, 0.7f); // tự xóa sau 2 giây
        }

        gameObject.SetActive(false);
    }
}
