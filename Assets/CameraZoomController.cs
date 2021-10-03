using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    // Start is called before the first frame update
    float velocity;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, WorldManager.instance.WorldRadius*1.1f, ref velocity, 0.8f);
    }
}
