using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    public Transform Ground1, Ground2;
    public float Length;

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
}
