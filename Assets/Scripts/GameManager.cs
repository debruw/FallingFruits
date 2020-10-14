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

    int currentLevel = 0;
    int maxLevelNumber = 19;
    [HideInInspector]
    GameObject currentLevelObject;
    [HideInInspector]
    public LevelProperties currentLevelProperties;
    public GameObject soundManager;
    public bool isGameStarted, isGameOver;

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
        if (PlayerPrefs.HasKey("LevelId"))
        {
            currentLevel = PlayerPrefs.GetInt("LevelId");
        }
        else
        {
            PlayerPrefs.SetInt("LevelId", currentLevel);
        }
        if (PlayerPrefs.GetInt("UseMenu").Equals(1) || !PlayerPrefs.HasKey("UseMenu"))
        {
            StartPanel.SetActive(true);
            PlayerPrefs.SetInt("UseMenu", 1);
        }
        else if (PlayerPrefs.GetInt("UseMenu").Equals(0))
        {
            InGamePanel.SetActive(true);
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
        if (SoundManager.Instance == null)
        {
            Instantiate(soundManager);
        }
        InitializeLevel();
    }
    public void InitializeLevel()
    {
        //TODO Test için konuldu kaldırılacak
        //currentLevel = 2;
        if (currentLevelObject != null)
        {
            Destroy(currentLevelObject);
        }

        if (currentLevel > maxLevelNumber)
        {
            int rand = Random.Range(0, maxLevelNumber);
            if (rand == PlayerPrefs.GetInt("LastLevel"))
            {
                rand = Random.Range(0, maxLevelNumber);
            }
            PlayerPrefs.SetInt("LastLevel", rand);
            currentLevelObject = Instantiate(Resources.Load("Level" + rand), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        }
        else
        {
            Debug.Log("Level : " + currentLevel);
            currentLevelObject = Instantiate(Resources.Load("Level" + currentLevel), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        }
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
        isGameStarted = false;
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
        isGameStarted = false;
        SoundManager.Instance.playSound(SoundManager.GameSounds.Lose);
        if (PlayerPrefs.GetInt("VIBRATION") == 1)
            TapticManager.Impact(ImpactFeedback.Light);
    }

    public void RetryButtonClick()
    {
        SceneManager.LoadScene("Scene_Game");
    }

    public void TapToStartButtonClick()
    {
        isGameStarted = true;
        StartPanel.SetActive(false);
        InGamePanel.SetActive(true);
    }

    public void TapToNextButtonClick()
    {
        if (PlayerPrefs.GetInt("VIBRATION") == 1)
            TapticManager.Impact(ImpactFeedback.Light);
        PlayerPrefs.SetInt("UseMenu", 0);
        InitializeLevel();
        GameWinPanel.SetActive(false);
        InGamePanel.SetActive(true);
        isGameStarted = true;
        isGameOver = false;
    }

    private void OnApplicationPause(bool pause)
    {
        PlayerPrefs.SetInt("UseMenu", 1);
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
