using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;
public class Shuriken : MonoBehaviour
{
    public Spline currentSpline;

    public Spline spline;
    private float rate = 0;

    public float DurationInSecond;
    bool isMouseUp, isMoving = true;
    private Vector3 target;
    public float speed = 1.0f;

    private void Start()
    {
        target = transform.position;
    }

    private void Update()
    {
        if (isMouseUp && isMoving)
        {
            rate += Time.deltaTime / DurationInSecond;
            if (rate > spline.nodes.Count - 1)
            {
                //rate -= spline.nodes.Count - 1;
                isMoving = false;
                gameObject.SetActive(false);
            }
            else
            {
                PlaceFollower();
            }
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 touchWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 12));

            target = new Vector3(touchWorldPosition.x, 10f, 0);
        }
        if (Input.GetMouseButtonDown(0))
        {
            isMouseUp = false;
            GetComponent<Rigidbody>().useGravity = false;
        }
        if (Input.GetMouseButtonUp(0))
        {
            target = spline.nodes[0].Position;
            isMouseUp = true;
            GetComponent<Rigidbody>().useGravity = true;
            GameManager.Instance.ClearAllGhostColors();
        }

        if (Vector3.Distance(transform.position, target) > .1f && !isMouseUp)
        {
            //Move our position a step closer to the target.
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target, step);

            //Check if the position of the cube and sphere are approximately equal.
            if (Vector3.Distance(transform.position, target) < 0.001f)
            {
                //Swap the position of the cylinder.
                target *= -1.0f;
            }
        }
    }

    private void PlaceFollower()
    {
        CurveSample sample = spline.GetSample(rate);
        transform.localPosition = sample.location;
        transform.localRotation = new Quaternion(sample.Rotation.x + 0, transform.localRotation.y, sample.Rotation.z + 90, sample.Rotation.w + 0);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Place"))
        {
            other.GetComponentInParent<SplineControl>().SetGhostColor();
            spline = other.GetComponentInParent<Spline>();
        }
        else if (other.CompareTag("Fruit"))
        {
            other.GetComponent<Rigidbody>().useGravity = true;
        }
    }

}
