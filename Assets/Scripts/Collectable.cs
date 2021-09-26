using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectableType
    {
        Apple,
        Orange,
        Pear,
        Grape,
        Banana,
        Coin,

        Last
    }

    public CollectableType myType;
}
