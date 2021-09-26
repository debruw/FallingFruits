using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProperties : MonoBehaviour
{
    public List<SplineControl> splines;
    public int levelTargetPoint;
    public Collectable.CollectableType targetCollectable;

    public List<GameObject> fruits;

    public void RemoveFruit(GameObject go)
    {
        fruits.Remove(go);
    }
}
