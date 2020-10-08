using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Fruit"))
        {
            //Add score for fruit
            Debug.Log("Fruit");
            GameManager.Instance.AddPoint();
            Destroy(other.gameObject);
        }
    }
}
