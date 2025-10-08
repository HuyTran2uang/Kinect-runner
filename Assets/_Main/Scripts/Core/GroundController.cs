using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviourSingleton<GroundController>
{
    public Transform Ground1, Ground2;
    public float Length;
    private Vector3 initialGround1Pos;
    private Vector3 initialGround2Pos;
    private void Start()
    {
        initialGround1Pos = Ground1.position;
        initialGround2Pos = Ground2.position;
    }

    private void FixedUpdate()
    {
        Ground1.position += Vector3.back * GameManager.Instance.MoveSpeed * Time.deltaTime;
        Ground2.position += Vector3.back * GameManager.Instance.MoveSpeed * Time.deltaTime;
    }

    private void Update()
    {
        if (Ground1.position.z < -Length)
        {
            Ground1.position = Ground2.position + Vector3.forward * Length;
        }
        if (Ground2.position.z < -Length)
        {
            Ground2.position = Ground1.position + Vector3.forward * Length;
        }
    }
    public void ResetGround()
    {
        // Đặt lại vị trí ban đầu
        Ground1.localPosition = initialGround1Pos;
        Ground2.localPosition = initialGround2Pos;
    }
}
