using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int CurrentLane;
    public int LeftestLane = -1;
    public int RightestLane = 1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveToLeft();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveToRight();
        }
    }

    public void MoveToLeft()
    {
        if(CurrentLane == LeftestLane)
        {
            return;
        }
        CurrentLane--;
        transform.position += Vector3.left * GameManager.Instance.ScaleSpace;
    }

    public void MoveToRight()
    {
        if (CurrentLane == RightestLane)
        {
            return;
        }
        CurrentLane++;
        transform.position += Vector3.right * GameManager.Instance.ScaleSpace;
    }

    private void Sensor()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();
        if (item != null)
        {
            item.Receive();
        }
    }
}
