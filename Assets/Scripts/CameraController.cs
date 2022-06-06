using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform mario;
    public Transform luigi;
    public Vector2 camSizaRange = new Vector2(6, 40);
    public Vector2 arenaDimensions = new Vector2(125, 52);

    void Update()
    {
        Vector3 targetPos = (mario.position + luigi.position) / 2;
        targetPos.z = -10;
        transform.position = targetPos;

        float xDistance = Mathf.Abs(mario.position.x - luigi.position.x);
        float yDistance = Mathf.Abs(mario.position.y - luigi.position.y);
        float xInterpolator = Mathf.Clamp01(xDistance / arenaDimensions.x);
        float yInterpolator = Mathf.Clamp01(yDistance / arenaDimensions.y);
        Camera.main.orthographicSize = Mathf.Lerp(camSizaRange.x, camSizaRange.y,
                                        (xInterpolator > yInterpolator ? xInterpolator : yInterpolator));

        // another way to add parallax but another scripts better to add it to another object
        //bg.position = transform.position * parallaxFactor; // moving all world with camera moving feels like 3D faking perspective 
    }
}
