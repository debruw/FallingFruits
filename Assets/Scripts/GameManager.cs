using System.Collections;
using System.Collections.Generic;
using TapticPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    public int currentLevel = 0;
    int maxLevelNumber = 19;
    public GameObject currentLevelObject;
    public LevelProperties currentLevelProperties;
    //public GameObject soundManager;
    public bool isGameOver;

    public GameObject Shuriken;
    int currentPoint, targetPoint;
    public Collectable.CollectableType targetCollectable;

    #region UI Elements
    [Header("UI Elements")]
    public Text currentPointText;
    public Text targetPointText, LevelText, WinText;
    public GameObject VibrationButton;
    public GameObject GameWinPanel, StartPanel, InGamePanel, GameLosePanel;
    public Image FruitHelpImage;
    public Sprite on, off;
    public Sprite[] fruitIcons;
    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        if (!PlayerPrefs.HasKey("VIBRATION"))
        {
            PlayerPrefs.SetInt("VIBRATION", 1);
            VibrationButton.GetComponent<Image>().sprite = on;
        }
        else
        {
            if (PlayerPrefs.GetInt("VIBRATION") == 1)
            {
                VibrationButton.GetComponent<Image>().sprite = on;
            }
            else
            {
                VibrationButton.GetComponent<Image>().sprite = off;
            }
        }
        InitializeLevel();
    }
    public void InitializeLevel()
    {
        currentLevelProperties = currentLevelObject.GetComponent<LevelProperties>();
        targetPoint = currentLevelProperties.levelTargetPoint;
        targetCollectable = currentLevelProperties.targetCollectable;
        FruitHelpImage.sprite = fruitIcons[(int)targetCollectable];
        LevelText.text = "Level " + (currentLevel + 1).ToString();
        currentPointText.text = currentPoint.ToString();
        targetPointText.text = targetPoint.ToString();
    }

    public void ClearAllGhostColors()
    {
        foreach (SplineControl sc in currentLevelObject.GetComponent<LevelProperties>().splines)
        {
            sc.ClearGhostColor();
        }
    }

    public void AddPoint()
    {
        currentPoint++;
        currentPointText.text = currentPoint.ToString();
        targetPointText.text = targetPoint.ToString();
        currentPointText.GetComponent<Animator>().SetTrigger("Bounce");
        if (currentPoint >= targetPoint && !Shuriken.GetComponent<Shuriken>().isMoving)
        {
            GameWin();
        }
    }

    public void LosePoint()
    {
        currentPoint--;
        if (currentPoint < 0)
        {
            currentPoint = 0;
        }
        currentPointText.text = currentPoint.ToString();
        targetPointText.text = targetPoint.ToString();
    }

    public void GameWin()
    {
        isGameOver = true;
        Debug.Log("<color=green>Game Win!</color>");
        SoundManager.Instance.StopAllSounds();
        SoundManager.Instance.playSound(SoundManager.GameSounds.Win);
        currentPoint = 0;
        //InGamePanel.SetActive(false);
        GameWinPanel.SetActive(true);
        //Destroy(currentLevelObject);
        currentLevel++;
        PlayerPrefs.SetInt("LevelId", currentLevel);
        SoundManager.Instance.playSound(SoundManager.GameSounds.Win);
        if (PlayerPrefs.GetInt("VIBRATION") == 1)
            TapticManager.Impact(ImpactFeedback.Light);
    }

    public void CheckGameLose()
    {
        if (!isGameOver)
        {
            if (currentLevelProperties.splines.Count == 0)
            {
                GameLose();
            }
        }
    }

    public void GameLose()
    {
        Debug.Log("<color=red>Game Lose!</color>");
        //InGamePanel.SetActive(false);
        SoundManager.Instance.StopAllSounds();
        SoundManager.Instance.playSound(SoundManager.GameSounds.Lose);
        currentPoint = 0;
        GameLosePanel.SetActive(true);
        isGameOver = true;
        SoundManager.Instance.playSound(SoundManager.GameSounds.Lose);
        if (PlayerPrefs.GetInt("VIBRATION") == 1)
            TapticManager.Impact(ImpactFeedback.Light);
    }

    public void RetryButtonClick()
    {
        SceneManager.LoadScene("Level" + currentLevel);
    }

    public void TapToStartButtonClick()
    {
        StartPanel.SetActive(false);
        InGamePanel.SetActive(true);
    }

    public void TapToNextButtonClick()
    {
        if (PlayerPrefs.GetInt("VIBRATION") == 1)
            TapticManager.Impact(ImpactFeedback.Light);

        GameWinPanel.SetActive(false);
        isGameOver = false;

        if (currentLevel > 2)
        {
            SceneManager.LoadScene("Level0");
        }
        else
        {
            SceneManager.LoadScene("Level" + currentLevel);
        }
    }

    public void VibrateButtonClick()
    {
        if (PlayerPrefs.GetInt("VIBRATION").Equals(1))
        {//Vibration is on
            PlayerPrefs.SetInt("VIBRATION", 0);
            VibrationButton.GetComponent<Image>().sprite = off;
        }
        else
        {//Vibration is off
            PlayerPrefs.SetInt("VIBRATION", 1);
            VibrationButton.GetComponent<Image>().sprite = on;
        }

        if (PlayerPrefs.GetInt("VIBRATION") == 1)
            TapticManager.Impact(ImpactFeedback.Light);
    }
}
