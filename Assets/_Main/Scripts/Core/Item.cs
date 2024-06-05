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

    protected virtual void OnDisable()
    {
        ItemSpawner.Instance.AddToPool(this);
    }

    public virtual void Receive(PlayerController receiver)
    {
        int score = 10;
        receiver.IncreaseScore(score);
        gameObject.SetActive(false);
    }
}
