using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }


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
    }

    public GameObject Shuriken;
    public SplineControl[] SplineControls;
    int point, targetPoint;

    #region UI Elements
    [Header("UI Elements")]
    public Text pointCounterText;
    #endregion

    private void Start()
    {
        targetPoint = 10;
        pointCounterText.text = point + " / " + targetPoint;
    }

    public void ClearAllGhostColors()
    {
        foreach (SplineControl sc in SplineControls)
        {
            sc.ClearGhostColor();
        }
    }

    public void AddPoint()
    {
        point++;
        pointCounterText.text = point + " / " + targetPoint;
        if (point > targetPoint)
        {
            GameWin();
        }
    }

    public void GameWin()
    {
        Debug.Log("<color=green>Game Win!</color>");
    }

    public void GameLose()
    {
        Debug.Log("<color=red>Game Lose!</color>");
    }
}
