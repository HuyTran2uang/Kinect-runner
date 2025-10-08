using System.Linq;
using UnityEngine;
using Windows.Kinect;

public class KinectController : MonoBehaviourSingleton<KinectController>
{
    public KinectManager kinectManager;
    private Body nearestBody;
    [SerializeField] private PlayerController playerController;
    public GameObject targetCube;
    public Animator animator;

    // --- Lane detection ---
    public float leftThreshold = -0.3f;
    public float rightThreshold = 0.3f;

    // --- Jump detection ---
    private float baseLegLength = 0f;
    private bool hasLegBase = false;
    private bool isJumping = false;
    public float jumpThreshold = 0.15f;

    // --- Roll detection ---
    private float baseTorsoLength = 0f;
    private bool hasTorsoBase = false;
    private bool isRolling = false;
    public float rollThreshold = 0.2f;

    public Color color = Color.black;

    // --- Calibration ---
    private float detectStartTime = -1f;
    private float calibrationDuration = 7f; // 10 giây calibrate
    private bool isCalibrated =>
        detectStartTime > 0 && Time.time - detectStartTime >= calibrationDuration;

    private void Awake()
    {
        if (animator == null)
            //animator.SetTrigger("TrigRun");
            Debug.Log("Where the heck is Animator????");
    }

    private void Update()
    {
        nearestBody = GetBodyNearest();
        if (nearestBody == null) return;

        // Bắt đầu tính thời gian calibrate khi phát hiện body lần đầu
        if (detectStartTime < 0)
        {
            detectStartTime = Time.time;
            Debug.Log("Body detected! Starting 10s calibration...");
        }

        if (!GameManager.Instance.isGameRunning)
            DetectRaiseHandRight(nearestBody); // 👈 thêm hàm này

        Vector3 spineBase = GetVector3FromJoint(nearestBody.Joints[JointType.SpineBase]);
        SetCubeColor(color, targetCube);

        DetectLane(spineBase);

        // Nếu chưa calibrate xong thì chỉ set baseline
        if (!isCalibrated)
        {
            Calibrate(nearestBody);
        }
        else
        {
            //DetectJump(nearestBody);
            //DetectRoll(nearestBody);
            //DetectJumpAndRoll(nearestBody);
            if (!GameManager.Instance.isGameRunning) { 
                DetectRaiseHandRight(nearestBody); // 👈 thêm hàm này
                
            }

            if (GameManager.Instance.isGameRunning)
            {
                DetectJumpAndRoll(nearestBody);
            }
        }
    }

    private void Calibrate(Body body)
    {
        Vector3 spineBase = GetPosOfPart(body, JointType.SpineBase);
        Vector3 head = GetPosOfPart(body, JointType.Head);

        if (!hasLegBase)
        {
            baseLegLength = spineBase.y;
            hasLegBase = true;
            //Debug.Log($"[Calibrate] Base SpineBase Y = {baseLegLength:F3}");
        }

        if (!hasTorsoBase)
        {
            baseTorsoLength = head.y - spineBase.y;
            hasTorsoBase = true;
            //Debug.Log($"[Calibrate] Base TorsoLength = {baseTorsoLength:F3}");
        }
    }

    private void DetectLane(Vector3 spineBase)
    {
        if (spineBase.x < leftThreshold)
            playerController.SetLane(-1);
        else if (spineBase.x > rightThreshold)
            playerController.SetLane(1);
        else
            playerController.SetLane(0);
    }

    private void DetectJump(Body body)
    {
        Vector3 spineBase = GetPosOfPart(body, JointType.SpineBase);
        float deltaY = spineBase.y - baseLegLength;

        if (!isJumping && deltaY > jumpThreshold)
        {
            isJumping = true;
            Debug.Log("JUMP DETECTED!");
            playerController.Jump();
            color = Color.blue;
        }

        if (isJumping && deltaY < jumpThreshold * 0.3f)
        {
            isJumping = false;
            Debug.Log("Landed");
        }
    }

    private void DetectRoll(Body body)
    {
        Vector3 head = GetPosOfPart(body, JointType.Head);
        Vector3 spineBase = GetPosOfPart(body, JointType.SpineBase);

        float torsoLength = head.y - spineBase.y;
        float delta = baseTorsoLength - torsoLength;

        if (!isRolling && delta > rollThreshold)
        {
            isRolling = true;
            Debug.Log("ROLL DETECTED!");
            playerController.Roll();
            color = Color.red;
        }

        if (isRolling && delta < rollThreshold * 0.3f)
        {
            isRolling = false;
            Debug.Log("Finished Rolling");
            animator.SetTrigger("TrigRun");
        }
    }

    private void SetCubeColor(Color color, GameObject cube)
    {
        if (cube != null)
        {
            Renderer rend = cube.GetComponent<Renderer>();
            if (rend != null)
                rend.material.color = color;
        }
    }

    public Body GetBodyNearest()
    {
        var data = kinectManager.GetData();
        if (data == null) return null;

        return data.Where(b => b.IsTracked)
                   .OrderBy(b => GetVector3FromJoint(b.Joints[JointType.Head]).magnitude)
                   .FirstOrDefault();
    }

    public Vector3 GetPosOfPart(Body body, JointType jointType)
    {
        return GetVector3FromJoint(body.Joints[jointType]);
    }

    private static Vector3 GetVector3FromJoint(Windows.Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X, joint.Position.Y, joint.Position.Z);
    }

    private void DetectRaiseHandRight(Body body)
    {
        Vector3 handRight = GetPosOfPart(body, JointType.HandRight);
        Vector3 shoulderRight = GetPosOfPart(body, JointType.ShoulderRight);

        // Điều kiện: tay phải cao hơn vai phải 20cm
        if (handRight.y >= shoulderRight.y + 0.15f)
        {
            Debug.Log("RIGHT HAND RAISED -> Start Game!");
            GameManager.Instance.StartGameFromButton();
            //GameManager.Instance.LoadSceneByName("Main Art 1");
            UIController.Instance.HideMainMenu();
            UIController.Instance.ShowGamePage();
        }
    }

    private void DetectJumpAndRoll(Body body)
    {
        Vector3 spineBase = GetPosOfPart(body, JointType.SpineBase);
        float deltaY = spineBase.y - baseLegLength;

        // Jump: spineBase cao hơn baseline
        if (!isJumping && deltaY > jumpThreshold)
        {
            isJumping = true;
            Debug.Log("JUMP DETECTED!");
            playerController.Jump();
            color = Color.blue;
        }
        if (isJumping && deltaY < jumpThreshold * 0.3f)
        {
            isJumping = false;
            Debug.Log("Landed");
        }

        // Roll: spineBase thấp hơn baseline
        if (!isRolling && deltaY < -rollThreshold)
        {
            isRolling = true;
            Debug.Log("ROLL DETECTED!");
            playerController.Roll();
            color = Color.red;
        }
        if (isRolling && deltaY > -rollThreshold * 0.3f)
        {
            isRolling = false;
            Debug.Log("Finished Rolling");
            animator.SetTrigger("TrigRun");
        }
    }

}

