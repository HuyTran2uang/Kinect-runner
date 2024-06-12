using System.Linq;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject[] models;
    public ElementSO[] baseElements;
    private ElementSO ele;

    public void Init()
    {
        models.ToList().ForEach(i => i.gameObject.SetActive(false));

        ele = baseElements[Random.Range(0, baseElements.Length)];

        models[ele.ModelIndex].SetActive(true);

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
        receiver.IncreaseScore(ele.Score);
        gameObject.SetActive(false);
    }
}
