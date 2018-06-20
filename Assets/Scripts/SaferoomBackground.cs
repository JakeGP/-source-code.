using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaferoomBackground : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void Update()
    {
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 15f);
        transform.localScale = new Vector3(Camera.main.orthographicSize, Camera.main.orthographicSize);
    }
}
