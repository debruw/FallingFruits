using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public GameObject[] particles;
    public GameObject CoinParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.isGameOver)
        {
            return;
        }
        if (other.CompareTag("Collectable"))
        {
            GameManager.Instance.currentLevelProperties.RemoveFruit(other.gameObject);
            if (GameManager.Instance.currentLevelProperties.fruits.Count <= 0)
            {
                GameManager.Instance.CheckGameEnd();
            }
            //Add score for fruit
            if (other.GetComponent<Collectable>().myType == GameManager.Instance.targetCollectable)
            {
                GameManager.Instance.AddPoint();
            }
            else
            {
                GameManager.Instance.LosePoint();
            }
            Destroy(Instantiate(particles[(int)other.GetComponent<Collectable>().myType], other.transform.position, Quaternion.identity), 2f);
            other.gameObject.SetActive(false);
            Destroy(other.gameObject, 2);
        }
        else if (other.CompareTag("Coin"))
        {
            GameManager.Instance.currentLevelProperties.RemoveFruit(other.gameObject);
            if (GameManager.Instance.currentLevelProperties.fruits.Count <= 0)
            {
                GameManager.Instance.CheckGameEnd();
            }
            GameManager.Instance.AddPoint();
            SoundManager.Instance.playSound(SoundManager.GameSounds.CoinPick);
            Destroy(Instantiate(CoinParticle, other.transform.position, Quaternion.identity), 2f);
            other.gameObject.SetActive(false);
            Destroy(other.gameObject, 2);
        }
        //if (GameManager.Instance.currentLevelProperties.splines.Count == 0)
        //{
        //    // There is no other spline
        //    if (GameManager.Instance.currentLevel == 9 ||
        //        GameManager.Instance.currentLevel == 14 ||
        //        GameManager.Instance.currentLevel == 19)
        //    {//Check bonus levels
        //        if (!GameManager.Instance.isGameOver)
        //        {
        //            GameManager.Instance.GameWin();
        //        }
        //    }
        //    else
        //    {
        //        GameManager.Instance.CheckGameEnd();
        //    }

        //}
    }
}
