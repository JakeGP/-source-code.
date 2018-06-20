using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForeGround_Scroll : MonoBehaviour
{ 
    GameObject foreground1, foreground2;
    GameManager manager;

    float m_scrollSpeed;
    public float m_tileSizeX;

    private Vector3 startPosition;

    private void Awake()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start()
    {
        foreground1 = GameObject.Find("Foreground1");
        foreground2 = GameObject.Find("Foreground2");
        startPosition = transform.localPosition;
    }

    void Update()
    {
        if (transform.position.x < Camera.main.transform.position.x - (m_tileSizeX * 2))
        {
            if (this.name == "Foreground1")
            {
                transform.localPosition = new Vector3(foreground2.transform.localPosition.x + m_tileSizeX, startPosition.y, 0);
            }
            else
            {
                transform.localPosition = new Vector3(foreground1 .transform.localPosition.x + m_tileSizeX, startPosition.y, 0);
            }
        }
        Transform foreground = GameObject.Find("Foreground").transform;
        foreground.position = new Vector3(foreground.position.x, Camera.main.transform.position.y, foreground.position.z);
    }
}
