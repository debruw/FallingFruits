using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seperator : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Collectable"))
        {
            collision.gameObject.GetComponent<Rigidbody>().useGravity = true;
            collision.gameObject.transform.parent = transform;
        }
    }
}
