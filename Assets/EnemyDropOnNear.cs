using UnityEngine;
using DG.Tweening;

public class EnemyDropOnNear : MonoBehaviour
{
    private Transform player;
    private bool hasDropped = false;

    public float dropDistance = 0.02f; // 2cm
    public float dropChance = 0.5f;    // 50%
    public float dropHeight = -6.17f;
    public float dropDuration = 7f;

    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.playerController != null)
        {
            player = GameManager.Instance.playerController.transform;
        }
    }

    private void Update()
    {
        if (player == null || hasDropped) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= dropDistance)
        {
            if (Random.value < dropChance)
            {
                hasDropped = true;
                transform
                    .DOMoveY(dropHeight, dropDuration)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        gameObject.SetActive(false);
                    });
            }
        }
    }
}
