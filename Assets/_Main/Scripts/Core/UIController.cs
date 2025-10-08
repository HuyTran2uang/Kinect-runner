using TMPro;
using UnityEngine;
using DG.Tweening; // DOTween

public class UIController : MonoBehaviourSingleton<UIController>
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highestScoreText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject mainMenuPanel;

    [SerializeField] private TMP_Text scoreTextBegin;
    [SerializeField] private TMP_Text highestScoreTextBegin;
    [SerializeField] private RectTransform gamePage;

    [Header("Swing Settings")]
    [SerializeField] private GameObject[] swingObjects;
    [SerializeField] private float swingAngle = 15f;
    [SerializeField] private float swingDuration = 1f;

    [Header("Rotate Settings")]
    [SerializeField] private GameObject rotatingObject; // Object cần xoay vòng
    [SerializeField] private float rotateDuration = 2f; // Thời gian xoay hết 360°

    //[Header("Scale Settings")]
    //[SerializeField] private GameObject scalingObject;
    //[SerializeField] private float scaleMultiplier = 1.6f; // tỉ lệ phóng to
    //[SerializeField] private float scaleDuration = 1.5f;  // thời gian phóng/thu
    private void Awake()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void Start()
    {
        StartSwing();
        StartRotate();
        //StartScale();
    }

    public void SetScoreText(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Điểm Của Bạn: " + score.ToString();
        }
    }

    public void SetScoreTextBegin(int score)
    {
        if (scoreTextBegin != null)
        {
            scoreTextBegin.text = "Điểm Của Bạn: " + score.ToString();
        }
    }

    public void SetHighestScoreText(int highestScore)
    {
        if (highestScoreText != null)
        {
            highestScoreText.text = "Điểm cao nhất: " + highestScore.ToString();
        }

        if (highestScoreTextBegin != null)
        {
            highestScoreTextBegin.text = "Điểm cao nhất: " + highestScore.ToString();
        }
    }

    public void SetTimerText(float time)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timerText.text = string.Format("Thời gian: {0:00}:{1:00}", minutes, seconds);
        }
    }

    public void ShowGameOver(bool flag)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (flag)
        {
            gameOverPanel.GetComponentInChildren<TMP_Text>().text = "Bạn đã vô địch Trung Thu!!!";
        } else
        {
            gameOverPanel.GetComponentInChildren<TMP_Text>().text = "Chơi lại để thử sức!!!";
        }

        gamePage.localPosition = new Vector3(0, -600);
    }

    public void HideGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void HideMainMenu()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false);
        }
    }

    public void ShowMainMenu()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }
    }

    public void ShowGamePage()
    {
        if (gamePage != null)
        {
            gamePage.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Làm các object trong mảng đung đưa trên trục Z
    /// </summary>
    private void StartSwing()
    {
        foreach (var obj in swingObjects)
        {
            if (obj != null)
            {
                obj.transform.DOLocalRotate(
                    new Vector3(0, 0, swingAngle),
                    swingDuration
                )
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
            }
        }
    }

    private void StartRotate()
    {
        if (rotatingObject != null)
        {
            rotatingObject.transform
                .DORotate(new Vector3(0, 0, 360), rotateDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }
    }

    //private void StartScale()
    //{
    //    if (scalingObject != null)
    //    {
    //        Vector3 originalScale = scalingObject.transform.localScale;
    //        Vector3 targetScale = originalScale * scaleMultiplier;

    //        scalingObject.transform
    //            .DOScale(targetScale, scaleDuration)
    //            .SetEase(Ease.InOutSine)
    //            .SetLoops(-1, LoopType.Yoyo);
    //    }
    //}
}
