using UnityEngine;

public class Item : MonoBehaviour
{
    public void Init()
    {
        gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        transform.position += GameManager.Instance.MoveSpeed * Vector3.back * Time.deltaTime;
    }

    private void Update()
    {
        if (transform.position.z < -7)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        ItemSpawner.Instance.AddToPool(this);
    }

    public void Receive()
    {
        int score = 10;
        GameManager.Instance.IncreaseScore(score);
        gameObject.SetActive(false);
    }
}
