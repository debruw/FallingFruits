using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public int currentLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("LevelId"))
        {
            currentLevel = PlayerPrefs.GetInt("LevelId");
        }
        else
        {
            PlayerPrefs.SetInt("LevelId", currentLevel);
        }
    }

    public void TapToStartClick()
    {
        SceneManager.LoadScene("Level" + currentLevel);
    }
}
