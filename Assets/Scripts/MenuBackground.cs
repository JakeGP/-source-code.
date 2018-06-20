using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackground : MonoBehaviour {

    public float m_scrollSpeed;
    public float m_tileSizeX;

    private Vector3 startPosition;

    void Start ()
    {
        startPosition = transform.position;
	}
	
	void Update ()
    {
        float newPosition = Mathf.Repeat(Time.time * m_scrollSpeed, (m_tileSizeX * 4.00f));
        transform.position = startPosition + Vector3.left * newPosition;		

    }
}
