using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public int CurrentLane = 0;        // 0 = giữa, -1 = trái, 1 = phải
    public int LeftestLane = -1;
    public int RightestLane = 1;

    public float moveSpeed = 10f;      // tốc độ dịch chuyển giữa các lane
    public float jumpForce = 5f;       // lực nhảy
    public float gravity = -20f;       // trọng lực cơ bản
    public float fallMultiplier = 2.5f; // tăng tốc độ rơi
    public float rollDuration = 0.5f;  // thời gian lăn (giây)

    public KinectController kinectController;
    //private Animator _animator;
    public Animator _animator { get; set; }
    private CharacterController controller;
    private Vector3 velocity;          // vận tốc y (nhảy/rơi)
    private bool isGrounded = true;
    private bool isRolling = false;
    private float rollTimer = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }
  

    private void Update()
    {
        // Kiểm tra ground
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // reset nhỏ để dính ground
        }

        // Bắt phím điều khiển
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetLane(CurrentLane - 1); // sang trái
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SetLane(CurrentLane + 1); // sang phải
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump(); // nhảy
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Roll(); // lăn
        }

        MoveToLane();

        // Gravity + Fall Multiplier
        if (velocity.y < 0) // đang rơi
            velocity.y += gravity * fallMultiplier * Time.deltaTime;
        else
            velocity.y += gravity * Time.deltaTime;

        // Di chuyển theo velocity
        controller.Move(velocity * Time.deltaTime);

        // Xử lý roll
        if (isRolling)
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0f)
            {
                isRolling = false;
                //kinectController.triggFlag = "TrigRoll";
                Debug.Log("Roll Ended");
            }
        }
    }

    public void SetLane(int lane)
    {
        CurrentLane = Mathf.Clamp(lane, LeftestLane, RightestLane);
    }

    private void MoveToLane()
    {
        Vector3 targetPosition = new Vector3(
            CurrentLane * GameManager.Instance.ScaleSpace * 1.2f,
            transform.position.y,
            transform.position.z
        );

        // Dịch chuyển ngang (x)
        Vector3 moveDirection = new Vector3(
            (targetPosition.x - transform.position.x),
            0,
            0
        );
        
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            AudioManager.PlaySound(SoundType.RICK_ROLL);

            KinectController.Instance.animator.SetTrigger("TrigJump");
            velocity.y = jumpForce;
            
            Debug.Log("Player Jumped!");
        }
        else
        {
            Debug.Log("Cannot jump: Player is not grounded!");
        }
    }

    public void Roll()
    {
        if (isGrounded && !isRolling)
        {
            isRolling = true;
            AudioManager.PlaySound(SoundType.RICK_ROLL);
            KinectController.Instance.animator.ResetTrigger("TrigJump");
            KinectController.Instance.animator.SetTrigger("TrigRoll");
            rollTimer = rollDuration;
            Debug.Log("Player Rolled!");
            //kinectController.triggFlag = "TrigRun";
            
            // 👉 Ở đây bạn có thể thay đổi Collider height để mô phỏng nằm xuống
        }
        else
        {
            Debug.Log("Cannot roll: Player is not grounded or already rolling!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();
        if (item != null)
        {
            item.Receive(this);

            if (other.GetComponent<EnemyController>() != null)
            {

                AudioManager.PlaySound(SoundType.INNOVAR);
                _animator.SetTrigger("TrigCollide"); // <-- animation bạn muốn
                
                StartCoroutine(StopMovementTemporarily(0.9f)); // dừng 1 giây
            }
            else
            {
                AudioManager.PlaySound(SoundType.KIRBY);
            }
        }
    }
    private IEnumerator StopMovementTemporarily(float duration)
    {
        GameManager.Instance.isStunned = true;
        yield return new WaitForSeconds(duration);
        //GameManager.Instance.MoveSpeed = 10;
        GameManager.Instance.isStunned = false;
    }

    public void IncreaseScore(int amount)
    {
        GameManager.Instance.SetScore(GameManager.Instance.Score + amount);
    }

    public void VictoCumPose()
    {
        _animator.ResetTrigger("TrigRun");
        _animator.SetTrigger("TrigCum");
        AudioManager.PlaySound(SoundType.VICTOCUM);
        
        //this.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void LoserPose()
    {
        _animator.ResetTrigger("TrigRun");
        _animator.SetTrigger("TrigLose");
        AudioManager.PlaySound(SoundType.LOSER);
        
        //this.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}
