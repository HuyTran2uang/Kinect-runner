using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public Vector3 reverseOffset;
    private Quaternion fixedRotation;

    private bool flagStart;

    void Start()
    {
        fixedRotation = transform.rotation; // lưu hướng ban đầu
    }

    void LateUpdate()
    {
        flagStart = GameManager.Instance.isGameRunning;
        if (flagStart)
        {
            // camera đi theo player
            transform.position = player.position + offset;

            // giữ nguyên hướng nhìn, không bị lật theo player
            transform.rotation = fixedRotation;
        }
        else
        {
            transform.position = player.position + reverseOffset;
            // giữ nguyên hướng nhìn, không bị lật theo player
            transform.rotation = Quaternion.Euler(15,180, 0);
        }
    }
}
