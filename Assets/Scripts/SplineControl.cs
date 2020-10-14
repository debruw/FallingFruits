using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineControl : MonoBehaviour
{
    public Material DefaultColor;
    public Material GhostColor;

    public MeshRenderer[] meshRenderers;

    private void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    public void SetGhostColor()
    {
        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.material = GhostColor;
        }
    }
    
    public void ClearGhostColor()
    {
        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.material = DefaultColor;
        }
    }
}
