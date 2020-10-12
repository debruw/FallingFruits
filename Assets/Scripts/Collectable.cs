using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectableType
    {
        RedSphere,
        GreenSphere,

        Last
    }

    public CollectableType myType;
}
