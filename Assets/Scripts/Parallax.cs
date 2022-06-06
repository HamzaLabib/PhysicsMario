using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxFactor;

    void Update()
    {
        transform.position = Camera.main.transform.position * parallaxFactor; // // moving all world with camera moving feels like 3D faking perspective
    }
}
