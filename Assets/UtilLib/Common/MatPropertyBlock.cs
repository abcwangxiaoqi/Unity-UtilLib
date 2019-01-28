using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MatPropertyBlock : MonoBehaviour
{
    public MaterialPropertyBlock block { get; private set; }

    private void Awake()
    {
        block = new MaterialPropertyBlock();
    }
}
