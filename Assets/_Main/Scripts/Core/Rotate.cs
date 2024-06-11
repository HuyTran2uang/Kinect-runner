using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        transform.eulerAngles += Vector3.up * speed * Time.deltaTime;
    }
}
