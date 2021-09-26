using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            GameManager.Instance.currentLevelProperties.RemoveFruit(other.gameObject);
            if (GameManager.Instance.currentLevelProperties.fruits.Count <= 0)
            {
                GameManager.Instance.CheckGameEnd();
            }
            Destroy(other.gameObject);
        }
        else if(other.CompareTag("Coin"))
        {
            GameManager.Instance.currentLevelProperties.RemoveFruit(other.gameObject);
            if (GameManager.Instance.currentLevelProperties.fruits.Count <= 0)
            {
                GameManager.Instance.CheckGameEnd();
            }
            Destroy(other.gameObject);
        }
    }
}
