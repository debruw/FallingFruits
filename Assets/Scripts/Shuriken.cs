using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;
public class Shuriken : MonoBehaviour
{
    public Spline spline;
    private float rate = 0;

    public float DurationInSecond;
    bool isMouseUp, isMouseDown;
    public bool isMoving = true;
    private Vector3 target;
    public float speed = 1.0f;
    public GameObject seperator;
    public Vector3 localStartPosition;

    private void Start()
    {
        target = transform.position;
        localStartPosition = transform.localPosition;
    }

    private void Update()
    {
        if (!GameManager.Instance.isGameStarted || GameManager.Instance.isGameOver)
        {
            return;
        }
        if (isMouseUp && isMoving)
        {
            rate += Time.deltaTime / DurationInSecond;
            if (rate > spline.nodes.Count - 1)
            {
                isMoving = false;
                GameManager.Instance.currentLevelProperties.splines.Remove(spline.gameObject.GetComponent<SplineControl>());
                spline.gameObject.GetComponent<SplineControl>().enabled = false;
                spline.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.localPosition = localStartPosition;
                StartCoroutine(WaitAndCheck());
            }
            else
            {
                PlaceFollower();
            }
            if (Input.GetMouseButtonDown(0))
            {
                seperator.GetComponent<Animator>().SetTrigger("hit");
            }
            if (Input.GetMouseButtonUp(0))
            {
                seperator.GetComponent<Animator>().SetTrigger("back");
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 touchWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 12));
                target = new Vector3(touchWorldPosition.x, 9f, 0);
                transform.position = target;
            }
            if (Input.GetMouseButtonDown(0))
            {
                isMouseUp = false;
                isMouseDown = true;
            }
            if (Input.GetMouseButtonUp(0) && spline != null && isMouseDown == true)
            {
                isMouseUp = true;
                isMoving = true;
                rate = 0;
            }
        }
    }

    IEnumerator WaitAndCheck()
    {
        yield return new WaitForSeconds(5f);
        GameManager.Instance.CheckGameLose();
    }

    private void PlaceFollower()
    {
        CurveSample sample = spline.GetSample(rate);
        transform.localPosition = sample.location;
        transform.localRotation = new Quaternion(sample.Rotation.x + 0, transform.localRotation.y, sample.Rotation.z + 90, sample.Rotation.w + 0);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Place"))
        {
            if (GameManager.Instance.currentLevelProperties.splines.Count == 1)
            {
                other.GetComponentInParent<SplineControl>().SetGhostColor();
                spline = other.GetComponentInParent<Spline>();
            }
            else
            {
                if (spline != other.GetComponentInParent<Spline>())
                {
                    Debug.Log("1");
                    GameManager.Instance.ClearAllGhostColors();
                    other.GetComponentInParent<SplineControl>().SetGhostColor();
                    spline = other.GetComponentInParent<Spline>();
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!GameManager.Instance.isGameStarted || GameManager.Instance.isGameOver)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Collectable"))
        {
            SoundManager.Instance.playSound(SoundManager.GameSounds.Collect);
            collision.gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }

}
