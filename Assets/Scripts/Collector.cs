using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public GameObject[] particles;

    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.Instance.isGameStarted || GameManager.Instance.isGameOver)
        {
            return;
        }
        if (other.CompareTag("Collectable"))
        {
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
    }
}
