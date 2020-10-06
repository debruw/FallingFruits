using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private bool dragging = false;
    private float distance;

    void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        dragging = true;
    }

    void OnMouseUp()
    {
        dragging = false;
    }

    RaycastHit hit;
    void Update()
    {
        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = rayPoint;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Place"))
                {
                    Debug.Log("Place");
                    transform.position = hit.transform.position;
                }                
            }
        }
    }
}
