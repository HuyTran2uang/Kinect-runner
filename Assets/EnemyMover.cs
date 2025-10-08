using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public float moveSpeed = 2f; // Tốc độ di chuyển ngang
    public float moveRange = 1.5f; // Khoảng di chuyển (±1.5 đơn vị từ vị trí ban đầu)
    private Vector3 initialPosition; // Vị trí ban đầu của enemy
    private float timeOffset; // Offset thời gian để tạo hiệu ứng dao động ngẫu nhiên

    private void Awake()
    {
        // Lưu vị trí ban đầu
        initialPosition = transform.position;
        // Tạo offset ngẫu nhiên để các enemy không di chuyển đồng bộ
        timeOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    private void Update()
    {
        // Tính vị trí X mới dựa trên hàm sin để tạo chuyển động dao động
        float newX = initialPosition.x + Mathf.Sin(Time.time * moveSpeed + timeOffset) * moveRange;
        // Giữ nguyên Y và Z
        Vector3 targetPosition = new Vector3(newX, transform.position.y, transform.position.z);
        // Di chuyển mượt mà đến vị trí mới
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }
}